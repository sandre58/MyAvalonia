// -----------------------------------------------------------------------
// <copyright file="DateRangePicker.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using MyNet.Avalonia.Controls.DateTimePickers;
using MyNet.Avalonia.Controls.Primitives;
using MyNet.Avalonia.Extensions;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

[TemplatePart(PartButton, typeof(Button))]
[TemplatePart(PartPopup, typeof(Popup))]
[TemplatePart(PartStartCalendar, typeof(CalendarView))]
[TemplatePart(PartEndCalendar, typeof(CalendarView))]
[TemplatePart(PartStartTextBox, typeof(TextBox))]
[TemplatePart(PartEndTextBox, typeof(TextBox))]
[PseudoClasses(PseudoClassName.Empty)]
public class DateRangePicker : DatePickerBase
{
    public const string PartButton = "PART_Button";
    public const string PartPopup = "PART_Popup";
    public const string PartStartCalendar = "PART_StartCalendar";
    public const string PartEndCalendar = "PART_EndCalendar";
    public const string PartStartTextBox = "PART_StartTextBox";
    public const string PartEndTextBox = "PART_EndTextBox";

    public static readonly StyledProperty<DateTime?> SelectedStartDateProperty =
        AvaloniaProperty.Register<DateRangePicker, DateTime?>(
            nameof(SelectedStartDate), defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<DateTime?> SelectedEndDateProperty =
        AvaloniaProperty.Register<DateRangePicker, DateTime?>(
            nameof(SelectedEndDate), defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<bool> EnableMonthSyncProperty =
        AvaloniaProperty.Register<DateRangePicker, bool>(
            nameof(EnableMonthSync));

    private Button? _button;
    private CalendarView? _endCalendar;
    private TextBox? _endTextBox;
    private DateTime? _previewEnd;

    private DateTime? _previewStart;
    private bool? _start;
    private bool _isFocused;
    private CalendarView? _startCalendar;
    private TextBox? _startTextBox;
    private Popup? _popup;

    static DateRangePicker()
    {
        FocusableProperty.OverrideDefaultValue<DateRangePicker>(true);
        _ = SelectedStartDateProperty.Changed.AddClassHandler<DateRangePicker, DateTime?>((picker, args) =>
            picker.OnSelectionChanged(args));
        _ = SelectedEndDateProperty.Changed.AddClassHandler<DateRangePicker, DateTime?>((picker, args) =>
            picker.OnSelectionChanged(args));
    }

    #region SeparatorText

    /// <summary>
    /// Provides SeparatorText Property.
    /// </summary>
    public static readonly StyledProperty<string?> SeparatorTextProperty = AvaloniaProperty.Register<DateRangePicker, string?>(nameof(SeparatorText), "~");

    /// <summary>
    /// Gets or sets the SeparatorText property.
    /// </summary>
    public string? SeparatorText
    {
        get => GetValue(SeparatorTextProperty);
        set => SetValue(SeparatorTextProperty, value);
    }

    #endregion

    public bool EnableMonthSync
    {
        get => GetValue(EnableMonthSyncProperty);
        set => SetValue(EnableMonthSyncProperty, value);
    }

    public DateTime? SelectedStartDate
    {
        get => GetValue(SelectedStartDateProperty);
        set => SetValue(SelectedStartDateProperty, value);
    }

    public DateTime? SelectedEndDate
    {
        get => GetValue(SelectedEndDateProperty);
        set => SetValue(SelectedEndDateProperty, value);
    }

    public void Clear()
    {
        SetCurrentValue(SelectedStartDateProperty, null);
        SetCurrentValue(SelectedEndDateProperty, null);
    }

    private void OnSelectionChanged(AvaloniaPropertyChangedEventArgs<DateTime?> args)
    {
        if (args.Property == SelectedStartDateProperty)
        {
            if (args.NewValue.Value is null)
            {
                _startCalendar?.ClearSelection();
                _startTextBox?.Clear();
            }
            else
            {
                _startCalendar?.MarkDates(args.NewValue.Value, args.NewValue.Value);
                _ = _startTextBox?.SetValue(TextBox.TextProperty, args.NewValue.Value.Value.ToString(DisplayFormat ?? CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern, CultureInfo.CurrentCulture));
            }
        }
        else if (args.Property == SelectedEndDateProperty)
        {
            if (args.NewValue.Value is null)
            {
                _endCalendar?.ClearSelection();
                _endTextBox?.Clear();
            }
            else
            {
                _endCalendar?.MarkDates(args.NewValue.Value, args.NewValue.Value);
                _ = _endTextBox?.SetValue(TextBox.TextProperty, args.NewValue.Value.Value.ToString(DisplayFormat ?? CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern, CultureInfo.CurrentCulture));
            }
        }

        PseudoClasses.Set(PseudoClassName.Empty, SelectedStartDate is null && SelectedEndDate is null);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        GotFocusEvent.RemoveHandler(OnTextBoxGetFocus, _startTextBox);
        TextBox.TextChangedEvent.RemoveHandler(OnTextChanged, _startTextBox, _endTextBox);
        PointerPressedEvent.RemoveHandler(OnTextBoxPointerPressed, _startTextBox, _endTextBox);
        Button.ClickEvent.RemoveHandler(OnButtonClick, _button);
        LostFocusEvent.RemoveHandler(OnTextBoxLostFocus, _startTextBox, _endTextBox);
        if (_startCalendar != null)
        {
            _startCalendar.DateSelected -= OnDateSelected;
            _startCalendar.DatePreviewed -= OnDatePreviewed;
            _startCalendar.ContextDateChanged -= OnContextDateChanged;
        }

        if (_endCalendar != null)
        {
            _endCalendar.DateSelected -= OnDateSelected;
            _endCalendar.DatePreviewed -= OnDatePreviewed;
            _endCalendar.ContextDateChanged -= OnContextDateChanged;
        }

        _button = e.NameScope.Find<Button>(PartButton);
        _popup = e.NameScope.Find<Popup>(PartPopup);
        _startCalendar = e.NameScope.Find<CalendarView>(PartStartCalendar);
        _endCalendar = e.NameScope.Find<CalendarView>(PartEndCalendar);
        _startTextBox = e.NameScope.Find<TextBox>(PartStartTextBox);
        _endTextBox = e.NameScope.Find<TextBox>(PartEndTextBox);

        Button.ClickEvent.AddHandler(OnButtonClick, RoutingStrategies.Bubble, true, _button);
        GotFocusEvent.AddHandler(OnTextBoxGetFocus, _startTextBox, _endTextBox);
        TextBox.TextChangedEvent.AddHandler(OnTextChanged, _startTextBox, _endTextBox);
        PointerPressedEvent.AddHandler(OnTextBoxPointerPressed, RoutingStrategies.Tunnel, false, _startTextBox, _endTextBox);
        LostFocusEvent.AddHandler(OnTextBoxLostFocus, _startTextBox, _endTextBox);

        if (_startCalendar != null)
        {
            _startCalendar.DateSelected += OnDateSelected;
            _startCalendar.DatePreviewed += OnDatePreviewed;
            _startCalendar.ContextDateChanged += OnContextDateChanged;
        }

        if (_endCalendar != null)
        {
            _endCalendar.DateSelected += OnDateSelected;
            _endCalendar.DatePreviewed += OnDatePreviewed;
            _endCalendar.ContextDateChanged += OnContextDateChanged;
        }

        SyncDateToText();
    }

    private void SyncDateToText()
    {
        if (SelectedStartDate is not null)
            _ = _startTextBox?.SetValue(TextBox.TextProperty, SelectedStartDate.Value.ToString(DisplayFormat ?? CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern, CultureInfo.CurrentCulture));
        if (SelectedEndDate is not null)
            _ = _endTextBox?.SetValue(TextBox.TextProperty, SelectedEndDate.Value.ToString(DisplayFormat ?? CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern, CultureInfo.CurrentCulture));

        if (SelectedStartDate is null)
            _startCalendar?.ClearSelection();
        if (SelectedEndDate is null)
            _endCalendar?.ClearSelection();
        if (SelectedStartDate is not null && SelectedEndDate is not null)
        {
            _startCalendar?.MarkDates(SelectedStartDate, SelectedEndDate);
            _endCalendar?.MarkDates(SelectedStartDate, SelectedEndDate);
        }
        else if (SelectedStartDate is not null)
        {
            _startCalendar?.MarkDates(SelectedStartDate, SelectedStartDate);
        }
        else if (SelectedEndDate is not null)
        {
            _endCalendar?.MarkDates(SelectedEndDate, SelectedEndDate);
        }

        PseudoClasses.Set(PseudoClassName.Empty, SelectedStartDate is null && SelectedEndDate is null);
    }

    private void OnTextBoxLostFocus(object? sender, RoutedEventArgs e)
    {
        if (Equals(sender, _startTextBox))
            OnTextChangedInternal(_startTextBox, SelectedStartDateProperty);
        else if (Equals(sender, _endTextBox)) OnTextChangedInternal(_endTextBox, SelectedEndDateProperty);
    }

    private void OnContextDateChanged(object? sender, CalendarContext e)
    {
        if (Equals(sender, _startCalendar) && _startCalendar?.Mode == CalendarViewMode.Month)
        {
            var needsUpdate = EnableMonthSync || _startCalendar?.ContextDate.CompareTo(_endCalendar?.ContextDate) >= 0;
            if (needsUpdate) _endCalendar?.SyncContextDate(_startCalendar?.ContextDate.NextMonth());
        }
        else if (Equals(sender, _endCalendar) && _endCalendar?.Mode == CalendarViewMode.Month)
        {
            var needsUpdate = EnableMonthSync || _endCalendar?.ContextDate.CompareTo(_startCalendar?.ContextDate) <= 0;
            if (needsUpdate) _startCalendar?.SyncContextDate(_endCalendar?.ContextDate.PreviousMonth());
        }
    }

    private void OnDatePreviewed(object? sender, CalendarDayButtonEventArgs e)
    {
        if (_start == true)
        {
            _previewStart = e.Date;
            _startCalendar?.MarkDates(SelectedStartDate, SelectedEndDate, _previewStart, _previewEnd);
            _endCalendar?.MarkDates(SelectedStartDate, SelectedEndDate, _previewStart, _previewEnd);
        }
        else if (_start == false)
        {
            _previewEnd = e.Date;
            _startCalendar?.MarkDates(SelectedStartDate, SelectedEndDate, _previewStart, _previewEnd);
            _endCalendar?.MarkDates(SelectedStartDate, SelectedEndDate, _previewStart, _previewEnd);
        }
    }

    private void OnDateSelected(object? sender, CalendarDayButtonEventArgs e)
    {
        if (_start == true)
        {
            if (SelectedEndDate < e.Date) SelectedEndDate = null;
            SetCurrentValue(SelectedStartDateProperty, e.Date);
            _ = _startTextBox?.SetValue(TextBox.TextProperty, e.Date?.ToString(DisplayFormat ?? CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern, CultureInfo.CurrentCulture));

            _previewStart = null;
            _previewEnd = null;
            _startCalendar?.MarkDates(SelectedStartDate, SelectedEndDate, _previewStart, _previewEnd);
            _endCalendar?.MarkDates(SelectedStartDate, SelectedEndDate, _previewStart, _previewEnd);
            _ = _endTextBox?.Focus();
        }
        else if (_start == false)
        {
            if (SelectedStartDate > e.Date) SelectedStartDate = null;
            SetCurrentValue(SelectedEndDateProperty, e.Date);
            _ = _endTextBox?.SetValue(TextBox.TextProperty, e.Date?.ToString(DisplayFormat ?? CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern, CultureInfo.CurrentCulture));
            _start = null;
            _previewStart = null;
            _previewEnd = null;
            _startCalendar?.MarkDates(SelectedStartDate, SelectedEndDate, _previewStart, _previewEnd);
            _endCalendar?.MarkDates(SelectedStartDate, SelectedEndDate, _previewStart, _previewEnd);
            if (SelectedStartDate is null)
            {
                _ = _startTextBox?.Focus();
            }
            else
            {
                SetCurrentValue(IsDropDownOpenProperty, false);
            }
        }
    }

    private void OnButtonClick(object? sender, RoutedEventArgs e)
    {
        if (IsFocused)
        {
            SetCurrentValue(IsDropDownOpenProperty, !IsDropDownOpen);
        }
    }

    private void OnTextBoxPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (Equals(sender, _startTextBox))
        {
            if (_startCalendar is not null)
            {
                var date = SelectedStartDate ?? DateTime.Today;
                _startCalendar.ContextDate = new CalendarContext(date.Year, date.Month);
                _startCalendar.UpdateDayButtons();
            }

            if (_endCalendar is not null)
            {
                var date2 = SelectedEndDate;
                if (date2 is null || (date2.Value.Year == SelectedStartDate?.Year && date2.Value.Month == SelectedStartDate?.Month))
                {
                    date2 = SelectedStartDate ?? DateTime.Today;
                    date2 = date2.Value.AddMonths(1);
                }

                _endCalendar.ContextDate = new CalendarContext(date2.Value.Year, date2.Value.Month);
                _endCalendar.UpdateDayButtons();
            }
        }
        else if (Equals(sender, _endTextBox))
        {
            if (_endCalendar is not null)
            {
                var date = SelectedEndDate ?? DateTime.Today;
                _endCalendar.ContextDate = new CalendarContext(date.Year, date.Month);
                _endCalendar.UpdateDayButtons();
            }

            if (_startCalendar is not null)
            {
                var date2 = SelectedStartDate;
                if (date2 is null || (date2.Value.Year == SelectedEndDate?.Year && date2.Value.Month == SelectedEndDate?.Month))
                {
                    date2 = SelectedStartDate ?? DateTime.Today;
                    date2 = date2.Value.AddMonths(-1);
                }

                _startCalendar.ContextDate = new CalendarContext(date2.Value.Year, date2.Value.Month);
                _startCalendar.UpdateDayButtons();
            }
        }

        SetCurrentValue(IsDropDownOpenProperty, true);
    }

    private void OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (Equals(sender, _startTextBox))
            OnTextChangedInternal(_startTextBox, SelectedStartDateProperty, true);
        else if (Equals(sender, _endTextBox)) OnTextChangedInternal(_endTextBox, SelectedEndDateProperty, true);
    }

    private void OnTextChangedInternal(TextBox? textBox, AvaloniaProperty property, bool fromText = false)
    {
        if (textBox?.Text is null || string.IsNullOrEmpty(textBox.Text))
        {
            SetCurrentValue(property, null);
            _startCalendar?.ClearSelection();
            _endCalendar?.ClearSelection(end: true);
        }
        else if (string.IsNullOrEmpty(DisplayFormat))
        {
            if (DateTime.TryParse(textBox.Text, out var defaultTime))
            {
                SetCurrentValue(property, defaultTime);
                _startCalendar?.MarkDates(defaultTime, defaultTime);
                _endCalendar?.MarkDates(defaultTime, defaultTime);
            }
        }
        else
        {
            if (DateTime.TryParseExact(textBox.Text, DisplayFormat, CultureInfo.CurrentCulture, DateTimeStyles.None, out var date))
            {
                SetCurrentValue(property, date);
                if (_startCalendar is not null)
                {
                    var date1 = SelectedStartDate ?? DateTime.Today;
                    _startCalendar.ContextDate = new CalendarContext(date1.Year, date.Month);
                    _startCalendar.UpdateDayButtons();
                    _startCalendar.MarkDates(SelectedStartDate, SelectedEndDate, _previewStart, _previewEnd);
                }

                if (_endCalendar is not null)
                {
                    var date2 = SelectedEndDate ?? SelectedStartDate ?? DateTime.Today;
                    if (SelectedEndDate is null) date2 = date2.AddMonths(1);
                    _endCalendar.ContextDate = new CalendarContext(date2.Year, date2.Month);
                    _endCalendar.UpdateDayButtons();
                    _endCalendar.MarkDates(SelectedStartDate, SelectedEndDate, _previewStart, _previewEnd);
                }
            }
            else
            {
                if (!fromText)
                {
                    SetCurrentValue(property, null);
                    _ = textBox.SetValue(TextBox.TextProperty, null);
                    _startCalendar?.ClearSelection();
                    _endCalendar?.ClearSelection(end: true);
                }
            }
        }
    }

    /// <inheritdoc/>
    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        if (!e.Handled && e.Source is Visual source)
        {
            if (_popup?.IsInsidePopup(source) == true)
                e.Handled = true;
        }
    }

    private void OnTextBoxGetFocus(object? sender, GotFocusEventArgs e)
    {
        if (sender == _startTextBox)
        {
            _start = true;
        }
        else if (sender == _endTextBox)
        {
            _start = false;
        }

        if (!IsDropDownOpen)
        {
            SetCurrentValue(IsDropDownOpenProperty, true);
        }
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (e.Key is Key.Escape)
        {
            SetCurrentValue(IsDropDownOpenProperty, false);
            e.Handled = true;
        }
        else if (e.Key is Key.Down)
        {
            SetCurrentValue(IsDropDownOpenProperty, true);
            e.Handled = true;
        }
        else if (e.Key is Key.Tab)
        {
            if (_endTextBox?.IsFocused == true && e.KeyModifiers == KeyModifiers.None)
            {
                SetCurrentValue(IsDropDownOpenProperty, false);
            }
            else if (_startTextBox?.IsFocused == true && e.KeyModifiers == KeyModifiers.Shift)
            {
                SetCurrentValue(IsDropDownOpenProperty, false);
            }
        }
        else if (e.Key is Key.Enter)
        {
            SetCurrentValue(IsDropDownOpenProperty, false);
            CommitInput(true);
            e.Handled = true;
        }
        else
        {
            base.OnKeyDown(e);
        }
    }

    protected override void OnGotFocus(GotFocusEventArgs e)
    {
        base.OnGotFocus(e);
        FocusChanged(IsKeyboardFocusWithin);
    }

    protected override void OnLostFocus(RoutedEventArgs e)
    {
        base.OnLostFocus(e);
        FocusChanged(IsKeyboardFocusWithin);
        var top = TopLevel.GetTopLevel(this);
        var element = top?.FocusManager?.GetFocusedElement();
        if (element is Visual v && _popup?.IsInsidePopup(v) == true)
        {
            return;
        }

        if (element == _startTextBox || element == _endTextBox)
        {
            return;
        }

        CommitInput(true);
        SetCurrentValue(IsDropDownOpenProperty, false);
    }

    private void FocusChanged(bool hasFocus)
    {
        var wasFocused = _isFocused;
        _isFocused = hasFocus;

        if (hasFocus)
        {
            if (!wasFocused && _startTextBox != null && _endTextBox?.IsFocused == false)
            {
                _ = _startTextBox.Focus();
                _start = true;
            }
        }
    }

    private void CommitInput(bool clearWhenInvalid)
    {
        DateTime? startDate = null;
        DateTime? endDate = null;
        if (DateTime.TryParseExact(_startTextBox?.Text, DisplayFormat, CultureInfo.CurrentCulture, DateTimeStyles.None, out var localStartDate))
        {
            startDate = localStartDate;
            SetCurrentValue(SelectedStartDateProperty, localStartDate);
            if (_startCalendar is not null)
            {
                _startCalendar.ContextDate = _startCalendar.ContextDate.With(year: localStartDate.Year, month: localStartDate.Month);
                _startCalendar.UpdateDayButtons();
            }
        }
        else
        {
            if (clearWhenInvalid)
            {
                SetCurrentValue(SelectedStartDateProperty, null);
            }
        }

        if (DateTime.TryParseExact(_endTextBox?.Text, DisplayFormat, CultureInfo.CurrentCulture, DateTimeStyles.None, out var localEndDate))
        {
            endDate = localEndDate;
            SetCurrentValue(SelectedEndDateProperty, localEndDate);
            if (_endCalendar is not null)
            {
                _endCalendar.ContextDate = _endCalendar.ContextDate.With(year: localEndDate.Year, month: localEndDate.Month);
                _endCalendar.UpdateDayButtons();
            }
        }
        else
        {
            if (clearWhenInvalid)
            {
                SetCurrentValue(SelectedEndDateProperty, null);
            }
        }

        if (startDate is null || endDate is null)
        {
            _startCalendar?.ClearSelection();
            _endCalendar?.ClearSelection();
        }
        else
        {
            _startCalendar?.MarkDates(startDate: startDate, endDate: endDate);
            _endCalendar?.MarkDates(startDate: startDate, endDate: endDate);
        }
    }
}
