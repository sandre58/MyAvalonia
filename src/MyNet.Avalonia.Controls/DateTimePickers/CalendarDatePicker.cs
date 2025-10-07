// -----------------------------------------------------------------------
// <copyright file="CalendarDatePicker.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using System.Runtime.CompilerServices;
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
using MyNet.Utilities.Helpers;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

[TemplatePart(PartButton, typeof(Button))]
[TemplatePart(PartPopup, typeof(Popup))]
[TemplatePart(PartTextBox, typeof(TextBox))]
[TemplatePart(PartCalendar, typeof(CalendarView))]
public class CalendarDatePicker : DatePickerBase
{
    public const string PartButton = "PART_Button";
    public const string PartPopup = "PART_Popup";
    public const string PartTextBox = "PART_TextBox";
    public const string PartCalendar = "PART_Calendar";
    private Button? _button;
    private TextBox? _textBox;
    private CalendarView? _calendar;
    private Popup? _popup;
    private bool _isFocused;

    public static readonly StyledProperty<DateTime?> SelectedDateProperty = AvaloniaProperty.Register<CalendarDatePicker, DateTime?>(
        nameof(SelectedDate), defaultBindingMode: BindingMode.TwoWay);

    public DateTime? SelectedDate
    {
        get => GetValue(SelectedDateProperty);
        set => SetValue(SelectedDateProperty, value);
    }

    static CalendarDatePicker()
    {
        _ = SelectedDateProperty.Changed.AddClassHandler<CalendarDatePicker, DateTime?>((picker, args) => picker.OnSelectionChanged(args));
        _ = DisplayFormatProperty.Changed.AddClassHandler<CalendarDatePicker, string?>((picker, _) => picker.SyncSelectedDateToText(picker.SelectedDate));
    }

    private void OnSelectionChanged(AvaloniaPropertyChangedEventArgs<DateTime?> args) => SyncSelectedDateToText(args.NewValue.Value);

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        GotFocusEvent.RemoveHandler(OnTextBoxGetFocus, _textBox);
        TextBox.TextChangedEvent.RemoveHandler(OnTextChanged, _textBox);
        PointerPressedEvent.RemoveHandler(OnTextBoxPointerPressed, _textBox);
        Button.ClickEvent.RemoveHandler(OnButtonClick, _button);
        CalendarView.DateSelectedEvent.RemoveHandler(OnDateSelected, _calendar);

        _button = e.NameScope.Find<Button>(PartButton);
        _popup = e.NameScope.Find<Popup>(PartPopup);
        _textBox = e.NameScope.Find<TextBox>(PartTextBox);
        _calendar = e.NameScope.Find<CalendarView>(PartCalendar);

        Button.ClickEvent.AddHandler(OnButtonClick, RoutingStrategies.Bubble, true, _button);
        GotFocusEvent.AddHandler(OnTextBoxGetFocus, _textBox);
        TextBox.TextChangedEvent.AddHandler(OnTextChanged, _textBox);
        PointerPressedEvent.AddHandler(OnTextBoxPointerPressed, RoutingStrategies.Tunnel, false, _textBox);
        CalendarView.DateSelectedEvent.AddHandler(OnDateSelected, RoutingStrategies.Bubble, true, _calendar);
        SyncSelectedDateToText(SelectedDate);
    }

    private void OnDateSelected(object? sender, CalendarDayButtonEventArgs e)
    {
        SetCurrentValue(SelectedDateProperty, e.Date);
        SetCurrentValue(IsDropDownOpenProperty, false);
    }

    private void OnButtonClick(object? sender, RoutedEventArgs e)
    {
        _ = Focus(NavigationMethod.Pointer);
        SetCurrentValue(IsDropDownOpenProperty, !IsDropDownOpen);
    }

    private void OnTextBoxPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (_calendar is not null)
        {
            var date = SelectedDate ?? DateTime.Today;
            _calendar.ContextDate = new CalendarContext(date.Year, date.Month);
            _calendar.UpdateDayButtons();
        }

        SetCurrentValue(IsDropDownOpenProperty, true);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnTextChanged(object? sender, TextChangedEventArgs e) => SetSelectedDate(true);

    private void SyncSelectedDateToText(DateTime? date)
    {
        if (date is null)
        {
            _ = _textBox?.SetValue(TextBox.TextProperty, null);
            _calendar?.ClearSelection();
        }
        else
        {
            _ = _textBox?.SetValue(TextBox.TextProperty, date.Value.ToString(DisplayFormat ?? DateTimeHelper.GetCurrentDateTimeFormatInfo().ShortDatePattern, CultureInfo.CurrentCulture));
            _calendar?.MarkDates(startDate: date.Value, endDate: date.Value);
        }
    }

    private void SetSelectedDate(bool fromText = false)
    {
        if (string.IsNullOrEmpty(_textBox?.Text))
        {
            SetCurrentValue(SelectedDateProperty, null);
            _calendar?.ClearSelection();
        }
        else if (string.IsNullOrEmpty(DisplayFormat))
        {
            if (DateTime.TryParse(_textBox?.Text, out var defaultTime))
            {
                SetCurrentValue(SelectedDateProperty, defaultTime);
                _calendar?.MarkDates(startDate: defaultTime, endDate: defaultTime);
            }
        }
        else
        {
            CommitInput(!fromText);
        }
    }

    private void OnTextBoxGetFocus(object? sender, GotFocusEventArgs e)
    {
        if (_calendar is not null)
        {
            var date = SelectedDate ?? DateTime.Today;
            _calendar.ContextDate = _calendar.ContextDate.With(year: date.Year, month: date.Month);
            _calendar.UpdateDayButtons();
        }

        SetCurrentValue(IsDropDownOpenProperty, true);
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
            SetCurrentValue(IsDropDownOpenProperty, false);
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

    public void Clear() => SetCurrentValue(SelectedDateProperty, null);

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
        if (element is Visual v && _popup?.IsInsidePopup(v) == true) return;
        if (Equals(element, _textBox)) return;
        CommitInput(true);
        SetCurrentValue(IsDropDownOpenProperty, false);
    }

    private void FocusChanged(bool hasFocus)
    {
        var wasFocused = _isFocused;
        _isFocused = hasFocus;

        if (hasFocus)
        {
            if (!wasFocused && _textBox != null)
            {
                _ = _textBox.Focus();
            }
        }
    }

    private void CommitInput(bool clearWhenInvalid)
    {
        if (DateTime.TryParseExact(_textBox?.Text, DisplayFormat, CultureInfo.CurrentCulture, DateTimeStyles.None, out var date))
        {
            SetCurrentValue(SelectedDateProperty, date);
            if (_calendar is not null)
            {
                _calendar.ContextDate = _calendar.ContextDate.With(year: date.Year, month: date.Month);
                _calendar.UpdateDayButtons();
            }

            _calendar?.MarkDates(startDate: date, endDate: date);
        }
        else
        {
            if (clearWhenInvalid)
            {
                SetCurrentValue(SelectedDateProperty, null);
            }

            _calendar?.ClearSelection();
        }
    }
}
