// -----------------------------------------------------------------------
// <copyright file="TextPicker.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Automation;
using Avalonia.Automation.Peers;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Interactivity;
using MyNet.Utilities.Suspending;

namespace MyNet.Avalonia.Controls.Primitives;

#pragma warning disable RCS1158 // Static member in generic type should use a type parameter
#pragma warning disable AVP1002 // AvaloniaProperty objects should not be owned by a generic type
[TemplatePart(PartTextBox, typeof(TextBox))]
[TemplatePart(PartPreviewer, typeof(Control))]
public abstract partial class TextPicker<T, TPreviewer> : DropDownControl, ITextPicker, IValueSelector<T>, IIncrementableControl
    where TPreviewer : Control
{
    public const string PartTextBox = "PART_TextBox";
    public const string PartPreviewer = "PART_Previewer";

    private readonly Suspender _previewValueChangedSuspender = new();
    private readonly Suspender _textBoxTextChangedSuspender = new();

    private T? _oldSelectedValue;
    private IDisposable? _textBoxTextChangedSubscription;

    protected TPreviewer? Previewer { get; private set; }

    protected TextBox? TextBox { get; private set; }

    static TextPicker()
    {
        FocusableProperty.OverrideDefaultValue<TextPicker<T, TPreviewer>>(true);
        AutomationProperties.ControlTypeOverrideProperty.OverrideDefaultValue<TextPicker<T, TPreviewer>>(AutomationControlType.ComboBox);
        _ = PlaceholderTextProperty.Changed.AddClassHandler<TextPicker<T, TPreviewer>>((picker, _) => picker.UpdateAutomationName());
        _ = TextProperty.Changed.AddClassHandler<TextPicker<T, TPreviewer>>((picker, _) => picker.UpdateAutomationName());
        _ = SelectedValueProperty.Changed.AddClassHandler<TextPicker<T, TPreviewer>>((picker, _) => picker.UpdateAutomationName());
    }

    protected TextPicker() => UpdateAutomationName();

    private void UpdateAutomationName()
    {
        var name = !string.IsNullOrWhiteSpace(Text) ? Text
            : !string.IsNullOrWhiteSpace(PlaceholderText) ? PlaceholderText
            : SelectedValue?.ToString() ?? string.Empty;

        AutomationProperties.SetName(this, name);
    }

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

    protected virtual void AddPreviewerHandlers() => Previewer?.AddHandler(KeyDownEvent, OnPreviewerKeyDown, RoutingStrategies.Tunnel, handledEventsToo: true);

    protected virtual void RemovePreviewerHandlers() =>
        Previewer?.RemoveHandler(KeyDownEvent, OnPreviewerKeyDown);

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        if (change.Property == IsDropDownOpenProperty)
        {
            if (change.GetNewValue<bool>())
            {
                CommitFromTextBox();

                _oldSelectedValue = SelectedValue;
                UpdatePreviewer(SelectedValue);
            }
            else if (change.GetOldValue<bool>())
            {
                OnDropDownClosing();
            }
        }
        else if (change.Property == DisplayFormatProperty)
        {
            OnDisplayFormat();
        }
        else if (change.Property == SelectedValueProperty)
        {
            var (removedValue, addedValue) = change.GetOldAndNewValue<T?>();

            using (_previewValueChangedSuspender.Suspend())
                UpdatePreviewer(addedValue);

            SetCurrentValue(TextProperty, ConvertValueToString(addedValue));

            OnValueSelected(addedValue, removedValue);
        }
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

    public static readonly StyledProperty<string?> PlaceholderTextProperty = AvaloniaProperty.Register<TextPicker<T, TPreviewer>, string?>(nameof(PlaceholderText));

    public string? PlaceholderText
    {
        get => GetValue(PlaceholderTextProperty);
        set => SetValue(PlaceholderTextProperty, value);
    }

    #endregion

    #region DisplayFormat

    public static readonly StyledProperty<string?> DisplayFormatProperty = AvaloniaProperty.Register<TextPicker<T, TPreviewer>, string?>(nameof(DisplayFormat));

    public string? DisplayFormat
    {
        get => GetValue(DisplayFormatProperty);
        set => SetValue(DisplayFormatProperty, value);
    }

    private void OnDisplayFormat() => SetCurrentValue(TextProperty, ConvertValueToString(SelectedValue));

    #endregion

    #region AllowSpin

    public static readonly StyledProperty<bool> AllowSpinProperty = AvaloniaProperty.Register<TextPicker<T, TPreviewer>, bool>(nameof(AllowSpin), true);

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

    public static readonly StyledProperty<bool> AutoCommitProperty = AvaloniaProperty.Register<TextPicker<T, TPreviewer>, bool>(nameof(AutoCommit), true);

    public bool AutoCommit
    {
        get => GetValue(AutoCommitProperty);
        set => SetValue(AutoCommitProperty, value);
    }

    #endregion

    #region CloseOnCommit

    public static readonly StyledProperty<bool> CloseOnCommitProperty = AvaloniaProperty.Register<TextPicker<T, TPreviewer>, bool>(nameof(CloseOnCommit), true);

    public bool CloseOnCommit
    {
        get => GetValue(CloseOnCommitProperty);
        set => SetValue(CloseOnCommitProperty, value);
    }

    #endregion

    #region CloseOnSingleSelection

    public static readonly StyledProperty<bool> CloseOnSingleSelectionProperty = AvaloniaProperty.Register<TextPicker<T, TPreviewer>, bool>(nameof(CloseOnSingleSelection));

    public bool CloseOnSingleSelection
    {
        get => GetValue(CloseOnSingleSelectionProperty);
        set => SetValue(CloseOnSingleSelectionProperty, value);
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
