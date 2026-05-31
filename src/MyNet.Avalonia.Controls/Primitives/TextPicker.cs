// -----------------------------------------------------------------------
// <copyright file="TextPicker.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using MyNet.Avalonia.Controls.Behaviors;
using MyNet.Avalonia.Controls.Resources;
using MyNet.Utilities.Suspending;

namespace MyNet.Avalonia.Controls.Primitives;

#pragma warning disable RCS1158 // Static member in generic type should use a type parameter
#pragma warning disable AVP1002 // AvaloniaProperty objects should not be owned by a generic type
[TemplatePart(PartTextBox, typeof(TextBox))]
[TemplatePart(PartPreviewer, typeof(Control))]
public abstract class TextPicker<T, TPreviewer> : DropDownControl, ITextPicker, IValueSelector<T>, IIncrementableControl
    where TPreviewer : Control
{
    public const string PartTextBox = "PART_TextBox";
    public const string PartPreviewer = "PART_Previewer";

    private static readonly CompositeFormat InvalidFormat = CompositeFormat.Parse(MessagesResources.InvalidFormatError);

    private readonly Suspender _previewValueChangedSuspender = new();
    private readonly Suspender _textBoxTextChangedSuspender = new();

    private T? _oldSelectedValue;
    private IDisposable? _textBoxTextChangedSubscription;

    protected TPreviewer? Previewer { get; private set; }

    protected TextBox? TextBox { get; private set; }

    static TextPicker() => FocusableProperty.OverrideDefaultValue<TextPicker<T, TPreviewer>>(true);

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        RemoveHandlers();

        TextBox = e.NameScope.Find<TextBox>(PartTextBox);
        Previewer = e.NameScope.Find<TPreviewer>(PartPreviewer);

        AddHandlers();

        if (TextBox != null)
        {
            using (_textBoxTextChangedSuspender.Suspend())
            {
                TextBox.Text = ConvertValueToString(SelectedValue);
            }
        }
    }

    protected virtual void RemoveHandlers()
    {
        KeyDownEvent.RemoveHandler(OnTextBoxKeyDown, TextBox);
        _textBoxTextChangedSubscription?.Dispose();
        RemovePreviewerHandlers();
    }

    protected virtual void AddHandlers()
    {
        TextBox?.AddHandler(KeyDownEvent, OnTextBoxKeyDown, RoutingStrategies.Tunnel, handledEventsToo: true);
        _textBoxTextChangedSubscription = TextBox?.GetObservable(TextBox.TextProperty).Subscribe(_ => OnTextBoxTextChanged());
        AddPreviewerHandlers();
    }

    protected virtual void AddPreviewerHandlers() { }

    protected virtual void RemovePreviewerHandlers() { }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        // IsDropDownOpen
        if (change.Property == IsDropDownOpenProperty)
        {
            if (change.GetNewValue<bool>())
            {
                CommitFromTextBox();

                _oldSelectedValue = SelectedValue;
                UpdatePreviewer(SelectedValue);
            }
        }

        // DisplayFormat
        else if (change.Property == DisplayFormatProperty)
        {
            OnDisplayFormat();
        }

        // Value
        else if (change.Property == SelectedValueProperty)
        {
            var (removedValue, addedValue) = change.GetOldAndNewValue<T?>();

            using (_previewValueChangedSuspender.Suspend())
                UpdatePreviewer(addedValue);

            SetCurrentValue(TextProperty, ConvertValueToString(addedValue));

            OnValueSelected(addedValue, removedValue);
        }

        // Text
        else if (change.Property == TextProperty)
        {
            var (_, newValue) = change.GetOldAndNewValue<string?>();

            if (TextBox != null && TextBox.Text != newValue)
            {
                using (_textBoxTextChangedSuspender.Suspend())
                    TextBox.Text = newValue;
            }

            TextChanged?.Invoke(this, new(TextBox.TextChangedEvent));
        }

        base.OnPropertyChanged(change);
    }

    #region PlaceholderText

    /// <summary>
    /// Provides PlaceholderText Property.
    /// </summary>
    public static readonly StyledProperty<string?> PlaceholderTextProperty = AvaloniaProperty.Register<TextPicker<T, TPreviewer>, string?>(nameof(PlaceholderText));

    /// <summary>
    /// Gets or sets the PlaceholderText property.
    /// </summary>
    public string? PlaceholderText
    {
        get => GetValue(PlaceholderTextProperty);
        set => SetValue(PlaceholderTextProperty, value);
    }

    #endregion

    #region DisplayFormat

    /// <summary>
    /// Provides DisplayFormat Property.
    /// </summary>
    public static readonly StyledProperty<string?> DisplayFormatProperty = AvaloniaProperty.Register<TextPicker<T, TPreviewer>, string?>(nameof(DisplayFormat));

    /// <summary>
    /// Gets or sets the DisplayFormat property.
    /// </summary>
    public string? DisplayFormat
    {
        get => GetValue(DisplayFormatProperty);
        set => SetValue(DisplayFormatProperty, value);
    }

    private void OnDisplayFormat() => SetCurrentValue(TextProperty, ConvertValueToString(SelectedValue));

    #endregion

    #region AllowSpin

    /// <summary>
    /// Provides AllowSpin Property.
    /// </summary>
    public static readonly StyledProperty<bool> AllowSpinProperty = AvaloniaProperty.Register<TextPicker<T, TPreviewer>, bool>(nameof(AllowSpin), true);

    /// <summary>
    /// Gets or sets a value indicating whether gets or sets the AllowSpin property.
    /// </summary>
    public bool AllowSpin
    {
        get => GetValue(AllowSpinProperty);
        set => SetValue(AllowSpinProperty, value);
    }

    #endregion

    #region SelectedValue

    public event EventHandler<SelectionChangedEventArgs>? SelectedValueChanged;

    public static readonly StyledProperty<T?> SelectedValueProperty = AvaloniaProperty.Register<TextPicker<T, TPreviewer>, T?>(nameof(SelectedValue), defaultBindingMode: BindingMode.TwoWay);

    public T? SelectedValue
    {
        get => GetValue(SelectedValueProperty);
        set => SetValue(SelectedValueProperty, value);
    }

    private void OnValueSelected(T? addedValue, T? removedValue)
    {
        var handler = SelectedValueChanged;
        if (handler != null)
        {
            var addedItems = new Collection<T>();
            var removedItems = new Collection<T>();

            if (addedValue is not null)
                addedItems.Add(addedValue);

            if (removedValue is not null)
                removedItems.Add(removedValue);

            handler(this, new(SelectingItemsControl.SelectionChangedEvent, removedItems, addedItems));
        }
    }

    #endregion

    #region Text

    public event EventHandler<TextChangedEventArgs>? TextChanged;

    public static readonly StyledProperty<string?> TextProperty = AvaloniaProperty.Register<TextPicker<T, TPreviewer>, string?>(nameof(Text));

    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    #endregion

    #region AutoCommit

    /// <summary>
    /// Provides AutoCommit Property.
    /// </summary>
    public static readonly StyledProperty<bool> AutoCommitProperty = AvaloniaProperty.Register<TextPicker<T, TPreviewer>, bool>(nameof(AutoCommit), true);

    /// <summary>
    /// Gets or sets a value indicating whether gets or sets the AutoCommit property.
    /// </summary>
    public bool AutoCommit
    {
        get => GetValue(AutoCommitProperty);
        set => SetValue(AutoCommitProperty, value);
    }

    #endregion

    #region CloseOnCommit

    /// <summary>
    /// Provides CloseOnCommit Property.
    /// </summary>
    public static readonly StyledProperty<bool> CloseOnCommitProperty = AvaloniaProperty.Register<TextPicker<T, TPreviewer>, bool>(nameof(CloseOnCommit), true);

    /// <summary>
    /// Gets or sets a value indicating whether gets or sets the CloseOnCommit property.
    /// </summary>
    public bool CloseOnCommit
    {
        get => GetValue(CloseOnCommitProperty);
        set => SetValue(CloseOnCommitProperty, value);
    }

    #endregion

    #region Validation

    public event EventHandler<PickerValueValidationErrorEventArgs>? ValidationError;

    protected override void UpdateDataValidation(AvaloniaProperty property, BindingValueType state, Exception? error)
    {
        if (property == SelectedValueProperty)
            DataValidationErrors.SetError(this, error);

        base.UpdateDataValidation(property, state, error);
    }

    protected virtual void OnValueValidationError(PickerValueValidationErrorEventArgs e) => ValidationError?.Invoke(this, e);

    #endregion

    #region Mouse Handlers

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);

        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            e.Handled = true;
    }

    /// <inheritdoc/>
    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);

        if (e.InitialPressMouseButton == MouseButton.Left)
        {
            e.Handled = true;

            if (!InputBehavior.GetIsTextEditable(this))
                TogglePopup();
        }
    }

    protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
    {
        base.OnPointerWheelChanged(e);
        if (!e.Handled && SelectedValue is not null && AllowSpin && IsKeyboardFocusWithin)
        {
            var newValue = IncrementValue(e.Delta.Y > 0 ? 1 : -1);

            if (newValue is null) return;

            SetCurrentValue(SelectedValueProperty, newValue);

            e.Handled = true;
        }
    }

    #endregion

    #region Keyboard handlers

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (e.Handled) return;

        var handled = ProcessKey(e);

        base.OnKeyDown(e);

        e.Handled = handled;
    }

    private bool ProcessKey(KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Enter:
                if (IsDropDownOpen)
                {
                    CommitFromPreview();
                    return true;
                }

                break;

            case Key.Escape:
                if (IsDropDownOpen)
                {
                    Rollback();
                    return true;
                }

                break;

            case Key.Down:
                if (!IsDropDownOpen)
                {
                    if (e.KeyModifiers == KeyModifiers.None && SelectedValue is not null)
                        SetCurrentValue(SelectedValueProperty, IncrementValue(-1));
                    return true;
                }

                break;

            case Key.Up:
                if (!IsDropDownOpen)
                {
                    if (e.KeyModifiers == KeyModifiers.None && SelectedValue is not null)
                        SetCurrentValue(SelectedValueProperty, IncrementValue(1));
                    return true;
                }

                break;

            case Key.PageDown:
                if (!IsDropDownOpen)
                {
                    if (e.KeyModifiers == KeyModifiers.None && SelectedValue is not null)
                        SetCurrentValue(SelectedValueProperty, IncrementLargeValue(-1));
                    return true;
                }

                break;

            case Key.PageUp:
                if (!IsDropDownOpen)
                {
                    if (e.KeyModifiers == KeyModifiers.None && SelectedValue is not null)
                        SetCurrentValue(SelectedValueProperty, IncrementLargeValue(1));
                    return true;
                }

                break;
        }

        return false;
    }

    #endregion

    #region Focus

    protected override void OnGotFocus(FocusChangedEventArgs e)
    {
        base.OnGotFocus(e);

        if (IsDropDownOpen)
            return;

        if (IsEnabled && InputBehavior.GetIsTextEditable(this) && TextBox is not null && e.NavigationMethod == NavigationMethod.Tab)
        {
            TextBox.Focus();
            var text = TextBox.Text;
            if (!string.IsNullOrEmpty(text))
            {
                TextBox.SelectionStart = 0;
                TextBox.SelectionEnd = text.Length;
            }
        }
    }

    protected override void OnLostFocus(FocusChangedEventArgs e)
    {
        if (TopLevel.GetTopLevel(this)?.FocusManager.GetFocusedElement() is Visual v && ReferenceEquals(v.FindAncestorOfType<TPreviewer>(true), Previewer)) return;
        if (e.Source is Visual v1 && ReferenceEquals(v1.FindAncestorOfType<TPreviewer>(true), Previewer)) return;

        CommitFromTextBox();

        base.OnLostFocus(e);
    }

    #endregion

    #region TextBox

    private void OnTextBoxKeyDown(object? sender, KeyEventArgs e) => OnKeyDown(e);

    private void OnTextBoxTextChanged()
    {
        if (_textBoxTextChangedSuspender.IsSuspended) return;

        using (_textBoxTextChangedSuspender.Suspend())
            SetCurrentValue(TextProperty, TextBox?.Text);
    }

    #endregion

    private T? TryParseText(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return default;
        }

        try
        {
            var newSelectedValue = ConvertValueFromString(text);

            if (IsValidValue(newSelectedValue))
            {
                return newSelectedValue;
            }

            var errorMessage = MessagesResources.InvalidValueError;
            var valueValidationError = new PickerValueValidationErrorEventArgs(new ArgumentOutOfRangeException(nameof(text), errorMessage), text);
            OnValueValidationError(valueValidationError);

            DataValidationErrors.SetError(this, valueValidationError.Exception);

            if (valueValidationError.ThrowException)
                throw valueValidationError.Exception;
        }
        catch (FormatException e)
        {
            var ex = new FormatException(string.Format(CultureInfo.CurrentCulture, InvalidFormat, text), e);
            var textParseError = new PickerValueValidationErrorEventArgs(ex, text);
            OnValueValidationError(textParseError);

            DataValidationErrors.SetError(this, textParseError.Exception);

            if (textParseError.ThrowException)
                throw textParseError.Exception;
        }

        return default;
    }

    private void CommitFromTextBox()
    {
        DataValidationErrors.ClearErrors(this);

        if (TextBox == null) return;

        var text = TextBox.Text;

        if (string.IsNullOrWhiteSpace(text))
        {
            if (SelectedValue is not null)
                SetCurrentValue(SelectedValueProperty, null);

            return;
        }

        if (SelectedValue is not null)
        {
            var selectedValueText = ConvertValueToString(SelectedValue);
            if (selectedValueText == text)
                return;
        }

        var parsedValue = TryParseText(text);
        if (parsedValue?.Equals(SelectedValue) == false)
            SetCurrentValue(SelectedValueProperty, parsedValue);
    }

    public virtual void CommitFromPreview()
    {
        SetCurrentValue(SelectedValueProperty, GetPreviewValue());

        if (CloseOnCommit)
        {
            ClosePopup();
            Focus();
        }
    }

    public virtual void Rollback() => SetCurrentValue(SelectedValueProperty, _oldSelectedValue);

    private void UpdatePreviewer(T? value) => SetPreviewValue(value);

    protected virtual void OnPreviewValueChanged()
    {
        if (_previewValueChangedSuspender.IsSuspended) return;

        if (AutoCommit)
            CommitFromPreview();
    }

    public virtual void Clear()
    {
        TextBox?.Clear();
        SetCurrentValue(SelectedValueProperty, null);
        TextBox?.Focus();
    }

    public virtual bool IsEmpty() => string.IsNullOrWhiteSpace(TextBox?.Text);

    public bool Increment(int value)
    {
        if (SelectedValue is null) return false;

        SetCurrentValue(SelectedValueProperty, IncrementValue(value));

        return true;
    }

    public bool IncrementLarge(int value)
    {
        if (SelectedValue is null) return false;

        SetCurrentValue(SelectedValueProperty, IncrementLargeValue(value));

        return true;
    }

    protected abstract T? IncrementValue(int offset);

    protected abstract T? IncrementLargeValue(int offset);

    protected virtual string? ConvertValueToString(T? value) => value?.ToString();

    protected abstract T? ConvertValueFromString(string text);

    protected virtual bool IsValidValue(T? value) => true;

    protected abstract void SetPreviewValue(T? value);

    protected abstract T? GetPreviewValue();
}

#pragma warning restore AVP1002 // AvaloniaProperty objects should not be owned by a generic type
#pragma warning restore RCS1158 // Static member in generic type should use a type parameter
