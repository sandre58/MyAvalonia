// -----------------------------------------------------------------------
// <copyright file="ColorPicker.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using MyNet.Avalonia.Controls.Resources;
using MyNet.Avalonia.Extensions;
using MyNet.Utilities;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// A color selection control that allows the user to select dates from a drop down color view.
/// </summary>
[TemplatePart(ElementButton, typeof(Button))]
[TemplatePart(ElementColorView, typeof(ColorView))]
[TemplatePart(ElementPopup, typeof(Popup))]
[TemplatePart(ElementTextBox, typeof(TextBox))]
[PseudoClasses(PseudoClassName.FlyoutOpen, PseudoClassName.Pressed)]
public class ColorPicker : ColorView, IDisposable
{
    private const string ElementTextBox = "PART_TextBox";
    private const string ElementButton = "PART_Button";
    private const string ElementPopup = "PART_Popup";
    private const string ElementColorView = "PART_ColorView";

    private ColorView? _colorView;
    private Button? _dropDownButton;
    private Popup? _popUp;
    private TextBox? _textBox;
    private CompositeDisposable? _buttonPointerPressedSubscription;

    private bool _colorIsUpdating;
    private Color? _onOpenColor;
    private bool _isPopupClosing;
    private bool _ignoreButtonClick;
    private bool _isFlyoutOpen;
    private bool _isPressed;

    /// <summary>
    /// Event for when the selected color changes within the slider.
    /// </summary>
    public event EventHandler<ColorTextChangedEventArgs>? TextChanged;

    /// <summary>
    /// Occurs when the drop-down
    /// </summary>
    public event EventHandler? ColorViewClosed;

    /// <summary>
    /// Occurs when the drop-down
    /// </summary>
    public event EventHandler? ColorViewOpened;

    #region IsDropDownOpen

    /// <summary>
    /// Provides IsDropDownOpen Property.
    /// </summary>
    public static readonly StyledProperty<bool> IsDropDownOpenProperty = AvaloniaProperty.Register<ColorPicker, bool>(nameof(IsDropDownOpen));

    /// <summary>
    /// Gets or sets a value indicating whether gets or sets the IsDropDownOpen property.
    /// </summary>
    public bool IsDropDownOpen
    {
        get => GetValue(IsDropDownOpenProperty);
        set => SetValue(IsDropDownOpenProperty, value);
    }

    #endregion

    #region Text

    /// <summary>
    /// Provides Text Property.
    /// </summary>
    public static readonly StyledProperty<string?> TextProperty = AvaloniaProperty.Register<ColorPicker, string?>(nameof(Text));

    /// <summary>
    /// Gets or sets the Text property.
    /// </summary>
    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    #endregion

    #region TextMode

    /// <summary>
    /// Provides TextMode Property.
    /// </summary>
    public static readonly StyledProperty<ColorDisplayNameMode> TextModeProperty = AvaloniaProperty.Register<ColorPicker, ColorDisplayNameMode>(nameof(TextMode));

    /// <summary>
    /// Gets or sets the TextMode property.
    /// </summary>
    public ColorDisplayNameMode TextMode
    {
        get => GetValue(TextModeProperty);
        set => SetValue(TextModeProperty, value);
    }

    #endregion

    #region Hexa

    private bool _disposedValue;

    /// <summary>
    /// Hexa DirectProperty definition.
    /// </summary>
    public static readonly DirectProperty<ColorPicker, string?> HexaProperty = AvaloniaProperty.RegisterDirect<ColorPicker, string?>(nameof(Hexa), o => o.Hexa);

    /// <summary>
    /// Gets the Hexa.
    /// </summary>
    public string? Hexa
    {
        get;
        private set => SetAndRaise(HexaProperty, ref field, value);
    }

    #endregion

    #region Watermark

    /// <summary>
    /// Defines the <see cref="Watermark"/> property.
    /// </summary>
    public static readonly StyledProperty<string?> WatermarkProperty = TextBox.WatermarkProperty.AddOwner<ColorPicker>();

    /// <summary>
    /// Gets or sets the Watermark property.
    /// </summary>
    public string? Watermark
    {
        get => GetValue(WatermarkProperty);
        set => SetValue(WatermarkProperty, value);
    }

    #endregion

    #region UseFloatingWatermark

    /// <summary>
    /// Defines the <see cref="UseFloatingWatermark"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> UseFloatingWatermarkProperty = TextBox.UseFloatingWatermarkProperty.AddOwner<ColorPicker>();

    /// <summary>
    /// Gets or sets a value indicating whether gets or sets the UseFloatingWatermark property.
    /// </summary>
    public bool UseFloatingWatermark
    {
        get => GetValue(UseFloatingWatermarkProperty);
        set => SetValue(UseFloatingWatermarkProperty, value);
    }

    #endregion

    /// <summary>
    /// Updates the visual state of the control by applying latest PseudoClasses.
    /// </summary>
    protected void UpdatePseudoClasses()
    {
        PseudoClasses.Set(PseudoClassName.FlyoutOpen, _isFlyoutOpen);
        PseudoClasses.Set(PseudoClassName.Pressed, _isPressed);
    }

    /// <inheritdoc/>
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        if (_colorView != null)
            _colorView.KeyDown -= ColorView_KeyDown;

        _colorView = e.NameScope.Find<ColorView>(ElementColorView);
        if (_colorView != null)
            _colorView.KeyDown += ColorView_KeyDown;

        if (_popUp != null)
        {
            _popUp.Child = null;
            _popUp.Closed -= PopUp_Closed;
        }

        _popUp = e.NameScope.Find<Popup>(ElementPopup);
        if (_popUp != null)
        {
            _popUp.Closed += PopUp_Closed;
            if (IsDropDownOpen)
                OpenDropDown();
        }

        if (_dropDownButton != null)
        {
            _dropDownButton.Click -= DropDownButton_Click;
            _buttonPointerPressedSubscription?.Dispose();
        }

        _dropDownButton = e.NameScope.Find<Button>(ElementButton);
        if (_dropDownButton != null)
        {
            _dropDownButton.Click += DropDownButton_Click;
            _buttonPointerPressedSubscription = new CompositeDisposable(
                _dropDownButton.AddDisposableHandler(PointerPressedEvent, DropDownButton_PointerPressed, handledEventsToo: true),
                _dropDownButton.AddDisposableHandler(PointerReleasedEvent, DropDownButton_PointerReleased, handledEventsToo: true));
        }

        if (_textBox != null)
        {
            _textBox.KeyDown -= TextBox_KeyDown;
            _textBox.GotFocus -= TextBox_GotFocus;
        }

        _textBox = e.NameScope.Find<TextBox>(ElementTextBox);

        UpdatePseudoClasses();

        base.OnApplyTemplate(e);
    }

    /// <inheritdoc/>
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        // IsDropDownOpen
        if (change.Property == IsDropDownOpenProperty)
        {
            var (oldValue, newValue) = change.GetOldAndNewValue<bool>();

            if (_popUp is { Child: not null } && newValue != oldValue)
            {
                if (newValue)
                {
                    OpenDropDown();
                }
                else
                {
                    _popUp.IsOpen = false;
                    _isFlyoutOpen = _popUp.IsOpen;
                    _isPressed = false;

                    UpdatePseudoClasses();
                    OnColorViewClosed(new RoutedEventArgs());
                }
            }
        }

        // Color
        else if (change.Property == ColorProperty && !_colorIsUpdating)
        {
            _colorIsUpdating = true;

            var (_, newValue) = change.GetOldAndNewValue<Color>();
            try
            {
                UpdateColorName(newValue);
                Hexa = newValue.ToHex();
            }
            finally
            {
                _colorIsUpdating = false;
            }
        }

        // Text
        else if (change.Property == TextProperty && !_colorIsUpdating)
        {
            var (_, newValue) = change.GetOldAndNewValue<string?>();

            if (string.IsNullOrEmpty(newValue))
            {
                SetCurrentValue(ColorProperty, default);
            }
            else if (newValue.ToColor() is { } color)
            {
                if (Color != color)
                {
                    SetCurrentValue(ColorProperty, color);
                }

                // if the color stayed the same we still have to update the displayed name
                else
                {
                    _colorIsUpdating = true;
                    try
                    {
                        UpdateColorName(color);
                        Hexa = color.ToHex();
                    }
                    finally
                    {
                        _colorIsUpdating = false;
                    }
                }
            }
            else
            {
                throw new FormatException(ColorPickerResources.XIsNotAValidColorError.FormatWith(newValue));
            }

            TextChanged?.Invoke(this, new ColorTextChangedEventArgs(newValue));
        }

        base.OnPropertyChanged(change);
    }

    /// <inheritdoc/>
    protected override void UpdateDataValidation(AvaloniaProperty property, BindingValueType state, Exception? error)
    {
        if (property == ColorProperty || property == TextProperty)
            DataValidationErrors.SetError(this, error);

        base.UpdateDataValidation(property, state, error);
    }

    /// <inheritdoc/>
    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);

        if (!e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            return;
        e.Handled = true;

        _ignoreButtonClick = _isPopupClosing;

        _isPressed = true;
        UpdatePseudoClasses();
    }

    /// <inheritdoc/>
    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);

        if (!_isPressed || e.InitialPressMouseButton != MouseButton.Left)
            return;
        e.Handled = true;

        if (!_ignoreButtonClick)
        {
            TogglePopUp();
        }
        else
        {
            _ignoreButtonClick = false;
        }

        _isPressed = false;
        UpdatePseudoClasses();
    }

    /// <inheritdoc/>
    protected override void OnPointerCaptureLost(PointerCaptureLostEventArgs e)
    {
        base.OnPointerCaptureLost(e);

        _isPressed = false;
        UpdatePseudoClasses();
    }

    /// <inheritdoc/>
    protected override void OnGotFocus(GotFocusEventArgs e)
    {
        base.OnGotFocus(e);
        if (!IsEnabled || _textBox == null)
            return;
        _ = _textBox.Focus();
        _textBox.SelectAll();
    }

    /// <inheritdoc/>
    protected override void OnLostFocus(RoutedEventArgs e)
    {
        base.OnLostFocus(e);

        _isPressed = false;
        UpdatePseudoClasses();
    }

    /// <inheritdoc/>
    protected override void OnKeyUp(KeyEventArgs e)
    {
        var key = e.Key;

        if (key is Key.Space or Key.Enter && IsEffectivelyEnabled)
        {
            // Since the TextBox is used for direct date entry,
            // it isn't supported to open the popup/flyout using these keys.
            // Other controls open the popup/flyout here.
        }
        else if (key == Key.Down && e.KeyModifiers.HasFlag(KeyModifiers.Alt) && IsEffectivelyEnabled && !IsDropDownOpen)
        {
            e.Handled = true;

            if (!_ignoreButtonClick)
            {
                TogglePopUp();
            }
            else
            {
                _ignoreButtonClick = false;
            }

            UpdatePseudoClasses();
        }
        else
        {
            // No action for other keys.
        }

        base.OnKeyUp(e);
    }

    private void OnColorViewClosed(EventArgs e) => ColorViewClosed?.Invoke(this, e);

    private void OnColorViewOpened(EventArgs e) => ColorViewOpened?.Invoke(this, e);

    private void ColorView_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Handled || sender is not ColorView || e.Key is not (Key.Enter or Key.Space or Key.Escape))
            return;

        _ = Focus();
        SetCurrentValue(IsDropDownOpenProperty, false);

        if (e.Key == Key.Escape)
            SetCurrentValue(ColorProperty, _onOpenColor);
    }

    private void TextBox_GotFocus(object? sender, RoutedEventArgs e) => SetCurrentValue(IsDropDownOpenProperty, false);

    private void TextBox_KeyDown(object? sender, KeyEventArgs e)
    {
        if (!e.Handled)
            e.Handled = ProcessDatePickerKey(e);
    }

    private void DropDownButton_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (_isFlyoutOpen && _dropDownButton?.IsEffectivelyEnabled == true && e.GetCurrentPoint(_dropDownButton).Properties.IsLeftButtonPressed)
        {
            // When a flyout is open with OverlayDismissEventPassThrough enabled and the drop-down button
            // is pressed, close the flyout
            _ignoreButtonClick = true;

            e.Handled = true;
            TogglePopUp();
        }
        else
        {
            _ignoreButtonClick = _isPopupClosing;

            _isPressed = true;
            UpdatePseudoClasses();
        }
    }

    private void DropDownButton_PointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        _isPressed = false;
        UpdatePseudoClasses();
    }

    private void DropDownButton_Click(object? sender, RoutedEventArgs e)
    {
        if (!_ignoreButtonClick)
        {
            TogglePopUp();
        }
        else
        {
            _ignoreButtonClick = false;
        }
    }

    private void PopUp_Closed(object? sender, EventArgs e)
    {
        SetCurrentValue(IsDropDownOpenProperty, false);

        if (_isPopupClosing)
            return;
        _isPopupClosing = true;
        _ = global::Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() => _isPopupClosing = false);
    }

    /// <summary>
    /// Toggles the <see cref="IsDropDownOpen"/> property to open/close the calendar popup.
    /// This will automatically adjust control focus as well.
    /// </summary>
    private void TogglePopUp()
    {
        if (IsDropDownOpen)
        {
            _ = Focus();
            SetCurrentValue(IsDropDownOpenProperty, false);
        }
        else
        {
            SetCurrentValue(IsDropDownOpenProperty, true);
            _ = _colorView?.Focus();
        }
    }

    private void OpenDropDown()
    {
        if (_colorView == null)
            return;
        _ = _colorView.Focus();

        // Open the PopUp
        _onOpenColor = Color;
        _popUp!.IsOpen = true;
        _isFlyoutOpen = _popUp!.IsOpen;

        UpdatePseudoClasses();
        OnColorViewOpened(new RoutedEventArgs());
    }

    private bool ProcessDatePickerKey(KeyEventArgs e)
    {
        if (e.Key is Key.Enter)
        {
            TogglePopUp();
            return true;
        }
        else if (e.Key is Key.Down)
        {
            if ((e.KeyModifiers & KeyModifiers.Control) == KeyModifiers.Control)
            {
                TogglePopUp();
                return true;
            }
        }

        return false;
    }

    protected virtual void UpdateColorName(Color color)
    {
        if (color == default)
        {
            SetCurrentValue(TextProperty, null);
            return;
        }

        if (TextMode == ColorDisplayNameMode.Hexa)
        {
            SetCurrentValue(TextProperty, color.ToHex());
        }
        else
        {
            var name = color.ToName();

            if (TextMode == ColorDisplayNameMode.Name)
            {
                SetCurrentValue(TextProperty, name);
            }
            else
            {
                var hexa = color.ToHex();
                SetCurrentValue(TextProperty, name == hexa ? hexa : $"{name} ({hexa})");
            }
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposedValue)
            return;
        if (disposing)
            _buttonPointerPressedSubscription?.Dispose();

        _disposedValue = true;
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
