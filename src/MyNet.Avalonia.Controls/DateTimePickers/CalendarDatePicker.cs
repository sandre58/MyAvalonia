// -----------------------------------------------------------------------
// <copyright file="CalendarDatePicker.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using MyNet.Avalonia.Controls.Assists;
using MyNet.Avalonia.Controls.DateTimePickers;
using MyNet.Avalonia.Controls.Primitives;
using MyNet.Avalonia.Extensions;
using MyNet.Utilities.Helpers;
using MyNet.Utilities.Suspending;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

[TemplatePart(PartButton, typeof(Button))]
[TemplatePart(PartPopup, typeof(Popup))]
[TemplatePart(PartTextBox, typeof(TextBox))]
[TemplatePart(PartCalendar, typeof(Calendar))]
[PseudoClasses(PseudoClassName.FlyoutOpen)]
public class CalendarDatePicker : DatePickerBase
{
    public const string PartButton = "PART_Button";
    public const string PartPopup = "PART_Popup";
    public const string PartTextBox = "PART_TextBox";
    public const string PartCalendar = "PART_Calendar";

    private readonly Suspender _changeSelectedDateSuspender = new();
    private readonly Suspender _changeTextSuspender = new();

    private Button? _button;
    private TextBox? _textBox;
    private Calendar? _calendar;
    private Popup? _popup;
    private DateTime? _oldSelectedDate;
    private IDisposable? _textBoxTextChangedSubscription;

    static CalendarDatePicker()
    {
        FocusableProperty.OverrideDefaultValue<CalendarDatePicker>(true);
        AllowSpinProperty.OverrideDefaultValue<CalendarDatePicker>(true);
    }

    public CalendarDatePicker()
    {
        SetCurrentValue(FirstDayOfWeekProperty, DateTimeHelper.GetCurrentDateTimeFormatInfo().FirstDayOfWeek);
        SetCurrentValue(DisplayDateProperty, DateTime.Today);
    }

    protected void UpdatePseudoClasses() => PseudoClasses.Set(PseudoClassName.FlyoutOpen, IsDropDownOpen);

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        // IsDropDownOpen
        if (change.Property == IsDropDownOpenProperty)
        {
            SetSelectedDateFromText();

            if (change.GetNewValue<bool>())
            {
                _oldSelectedDate = SelectedDate;
                _calendar?.MoveToDate(SelectedDate ?? DisplayDate);
            }

            UpdatePseudoClasses();
        }

        // DisplayFormat
        else if (change.Property == DisplayFormatProperty)
        {
            OnDateFormatChanged();
        }

        // SelectedDate
        else if (change.Property == SelectedDateProperty)
        {
            var (removedDate, addedDate) = change.GetOldAndNewValue<DateTime?>();

            if (_changeSelectedDateSuspender.IsSuspended) return;

            using (_changeSelectedDateSuspender.Suspend())
            {
                SetCurrentValue(TextProperty, DateToString(addedDate));
                OnDateSelected(addedDate, removedDate);
            }
        }

        // Text
        else if (change.Property == TextProperty)
        {
            var (_, newValue) = change.GetOldAndNewValue<string?>();

            using (_changeTextSuspender.Suspend())
            {
                if (_textBox != null && _textBox.Text != newValue)
                    _textBox.Text = newValue;

                TextChanged?.Invoke(this, new TextChangedEventArgs(TextBox.TextChangedEvent));
            }
        }

        base.OnPropertyChanged(change);
    }

    protected override void UpdateDataValidation(AvaloniaProperty property, BindingValueType state, Exception? error)
    {
        if (property == SelectedDateProperty)
            DataValidationErrors.SetError(this, error);

        base.UpdateDataValidation(property, state, error);
    }

    protected virtual void OnDateValidationError(CalendarDatePickerDateValidationErrorEventArgs e) => DateValidationError?.Invoke(this, e);

    private void OnDateFormatChanged() => SetCurrentValue(TextProperty, DateToString(SelectedDate));

    #region SelectedDate

    public event EventHandler<SelectionChangedEventArgs>? SelectedDateChanged;

    public event EventHandler<CalendarDatePickerDateValidationErrorEventArgs>? DateValidationError;

    public static readonly StyledProperty<DateTime?> SelectedDateProperty = AvaloniaProperty.Register<CalendarDatePicker, DateTime?>(nameof(SelectedDate), defaultBindingMode: BindingMode.TwoWay);

    public DateTime? SelectedDate
    {
        get => GetValue(SelectedDateProperty);
        set => SetValue(SelectedDateProperty, value);
    }

    private void OnDateSelected(DateTime? addedDate, DateTime? removedDate)
    {
        var handler = SelectedDateChanged;
        if (handler != null)
        {
            var addedItems = new Collection<DateTime>();
            var removedItems = new Collection<DateTime>();

            if (addedDate.HasValue)
                addedItems.Add(addedDate.Value);

            if (removedDate.HasValue)
                removedItems.Add(removedDate.Value);

            handler(this, new SelectionChangedEventArgs(SelectingItemsControl.SelectionChangedEvent, removedItems, addedItems));
        }
    }

    #endregion

    #region Text

    public event EventHandler<TextChangedEventArgs>? TextChanged;

    public static readonly StyledProperty<string?> TextProperty = AvaloniaProperty.Register<CalendarDatePicker, string?>(nameof(Text));

    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    #endregion

    #region Mouse Handlers

    private void OnButtonClick(object? sender, RoutedEventArgs e)
    {
        _ = Focus(NavigationMethod.Pointer);
        TogglePopUp();
    }

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

            if (!TextFieldAssist.GetIsTextEditable(this))
                TogglePopUp();
        }
    }

    protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
    {
        base.OnPointerWheelChanged(e);
        if (!e.Handled && SelectedDate.HasValue && AllowSpin && IsKeyboardFocusWithin)
        {
            var newDate = SelectedDate.Value.AddDays(e.Delta.Y > 0 ? -1 : 1);
            SetCurrentValue(SelectedDateProperty, newDate);
            e.Handled = true;
        }
    }

    #endregion

    #region Keyboard handlers

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (!e.Handled)
            e.Handled = ProcessDatePickerKey(e);

        base.OnKeyDown(e);
    }

    #endregion

    #region Focus

    protected override void OnGotFocus(GotFocusEventArgs e)
    {
        base.OnGotFocus(e);

        if (IsDropDownOpen)
            return;

        if (IsEnabled && TextFieldAssist.GetIsTextEditable(this) && _textBox is not null && e.NavigationMethod == NavigationMethod.Tab)
        {
            _textBox.Focus();
            var text = _textBox.Text;
            if (!string.IsNullOrEmpty(text))
            {
                _textBox.SelectionStart = 0;
                _textBox.SelectionEnd = text.Length;
            }
        }
    }

    protected override void OnLostFocus(RoutedEventArgs e)
    {
        base.OnLostFocus(e);

        if (TopLevel.GetTopLevel(this)?.FocusManager?.GetFocusedElement() is Visual v && v.FindAncestorOfType<Calendar>(true) == _calendar) return;
        if (e.Source is Visual v1 && v1.FindAncestorOfType<Calendar>(true) == _calendar) return;

        if (IsDropDownOpen)
        {
            SetSelectedDateFromText();
            SetCurrentValue(IsDropDownOpenProperty, false);
        }
        else
        {
            SetSelectedDateFromText();
        }
    }

    #endregion

    #region Calendar

    private void OnCalendarDayButtonMouseUp(object? sender, PointerReleasedEventArgs e)
    {
        SetCurrentValue(IsDropDownOpenProperty, false);
        Focus();
    }

    private void OnCalendarKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Handled || _calendar?.DisplayDateContext is not MonthContext) return;

        switch (e.Key)
        {
            case Key.Escape:
                SetCurrentValue(SelectedDateProperty, _oldSelectedDate);
                SetCurrentValue(IsDropDownOpenProperty, false);
                Focus();
                e.Handled = true;
                break;

            case Key.Space:
            case Key.Enter:
                SetCurrentValue(SelectedDateProperty, _calendar.GetFocusedDayButton()?.DataContext);
                SetCurrentValue(IsDropDownOpenProperty, false);
                Focus();
                e.Handled = true;
                break;
        }
    }

    #endregion

    #region TextBox

    private void OnTextBoxKeyDown(object? sender, KeyEventArgs e)
    {
        if (!e.Handled)
            e.Handled = ProcessDatePickerKey(e);
    }

    private void OnTextBoxTextChanged()
    {
        if (_changeTextSuspender.IsSuspended) return;

        using (_changeTextSuspender.Suspend())
        {
            SetCurrentValue(TextProperty, _textBox?.Text);
        }
    }

    private bool ProcessDatePickerKey(KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Enter:
                if (IsDropDownOpen)
                {
                    SetSelectedDateFromText();
                    SetCurrentValue(IsDropDownOpenProperty, false);
                    return true;
                }

                break;

            case Key.Escape:
                if (IsDropDownOpen)
                {
                    SetCurrentValue(SelectedDateProperty, _oldSelectedDate);
                    SetCurrentValue(IsDropDownOpenProperty, false);
                    return true;
                }

                break;

            case Key.Down:
                if (!IsDropDownOpen)
                {
                    if (e.KeyModifiers.HasFlag(KeyModifiers.Alt))
                        SetCurrentValue(IsDropDownOpenProperty, true);
                    else if (SelectedDate.HasValue)
                        SetCurrentValue(SelectedDateProperty, SelectedDate.Value.AddDays(1));
                    return true;
                }

                break;

            case Key.Up:
                if (!IsDropDownOpen)
                {
                    if (e.KeyModifiers.HasFlag(KeyModifiers.Alt))
                        SetCurrentValue(IsDropDownOpenProperty, true);
                    else if (SelectedDate.HasValue)
                        SetCurrentValue(SelectedDateProperty, SelectedDate.Value.AddDays(-1));
                    return true;
                }

                break;
        }

        return false;
    }

    #endregion

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        Button.ClickEvent.RemoveHandler(OnButtonClick, _button);

        if (_calendar != null)
        {
            _calendar.DayButtonMouseUp -= OnCalendarDayButtonMouseUp;
            KeyDownEvent.RemoveHandler(OnCalendarKeyDown, _calendar);
        }

        if (_popup != null)
        {
            _popup.Opened -= OnPopupOpened;
        }

        KeyDownEvent.RemoveHandler(OnTextBoxKeyDown, _textBox);
        _textBoxTextChangedSubscription?.Dispose();

        _calendar = e.NameScope.Find<Calendar>(PartCalendar);
        _textBox = e.NameScope.Find<TextBox>(PartTextBox);
        _button = e.NameScope.Find<Button>(PartButton);
        _popup = e.NameScope.Find<Popup>(PartPopup);

        if (_calendar != null)
        {
            _calendar.SelectionMode = CalendarSelectionMode.SingleDate;

            _calendar.DayButtonMouseUp += OnCalendarDayButtonMouseUp;
            _calendar.AddHandler(KeyDownEvent, OnCalendarKeyDown, RoutingStrategies.Tunnel, handledEventsToo: true);
        }

        if (_popup != null)
        {
            _popup.Opened += OnPopupOpened;
        }

        _textBox?.AddHandler(KeyDownEvent, OnTextBoxKeyDown, RoutingStrategies.Tunnel, handledEventsToo: true);
        _textBoxTextChangedSubscription = _textBox?.GetObservable(TextBox.TextProperty).Subscribe(_ => OnTextBoxTextChanged());

        if (_textBox != null)
        {
            using (_changeTextSuspender.Suspend())
            {
                _textBox.Text = DateToString(SelectedDate);
            }
        }

        Button.ClickEvent.AddHandler(OnButtonClick, RoutingStrategies.Bubble, true, _button);
    }

    private void OnPopupOpened(object? sender, EventArgs e) =>
        global::Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
        {
            if (_calendar != null && IsDropDownOpen)
                _calendar.Focus();
        },
            global::Avalonia.Threading.DispatcherPriority.Input);

    private void TogglePopUp()
    {
        if (IsDropDownOpen)
        {
            Focus();
            SetCurrentValue(IsDropDownOpenProperty, false);
        }
        else
        {
            SetCurrentValue(IsDropDownOpenProperty, true);
        }
    }

    private string? DateToString(DateTime? date) => date?.ToString(DisplayFormat ?? DateTimeHelper.GetCurrentDateTimeFormatInfo().ShortDatePattern, CultureInfo.CurrentCulture);

    private DateTime? DateFromString(string? text)
    {
        if (string.IsNullOrWhiteSpace(text)) return null;

        try
        {
            var newSelectedDate = DateTime.ParseExact(text, DisplayFormat ?? DateTimeHelper.GetCurrentDateTimeFormatInfo().ShortDatePattern, DateTimeHelper.GetCurrentDateTimeFormatInfo());

            if (_calendar?.IsValidSelection(newSelectedDate) == true || _calendar == null)
            {
                return newSelectedDate;
            }
            else
            {
                var dateValidationError = new CalendarDatePickerDateValidationErrorEventArgs(new ArgumentOutOfRangeException(nameof(text), "SelectedDate value is not valid."), text);
                OnDateValidationError(dateValidationError);

                if (dateValidationError.ThrowException)
                    throw dateValidationError.Exception;
            }
        }
        catch (FormatException ex)
        {
            var textParseError = new CalendarDatePickerDateValidationErrorEventArgs(ex, text);
            OnDateValidationError(textParseError);

            if (textParseError.ThrowException)
                throw textParseError.Exception;
        }

        return null;
    }

    private void SetSelectedDateFromText()
    {
        if (_textBox == null) return;

        var text = _textBox.Text;

        if (string.IsNullOrWhiteSpace(text))
        {
            if (SelectedDate.HasValue)
            {
                using (_changeSelectedDateSuspender.Suspend())
                using (_changeTextSuspender.Suspend())
                {
                    SetCurrentValue(SelectedDateProperty, null);
                }
            }

            return;
        }

        if (SelectedDate.HasValue)
        {
            var selectedDateText = DateToString(SelectedDate.Value);
            if (selectedDateText == text)
                return;
        }

        var parsedDate = DateFromString(text);
        if (parsedDate != SelectedDate)
        {
            using (_changeSelectedDateSuspender.Suspend())
            using (_changeTextSuspender.Suspend())
            {
                SetCurrentValue(SelectedDateProperty, parsedDate);

                if (parsedDate.HasValue && _textBox != null)
                    _textBox.Text = DateToString(parsedDate.Value);
            }
        }
    }

    public void Clear()
    {
        _textBox?.Clear();
        SetCurrentValue(SelectedDateProperty, null);
        _textBox?.Focus();
    }

    public bool IsEmpty() => string.IsNullOrWhiteSpace(_textBox?.Text);
}
