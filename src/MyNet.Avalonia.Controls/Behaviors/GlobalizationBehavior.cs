// -----------------------------------------------------------------------
// <copyright file="GlobalizationBehavior.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls;
using MyNet.Globalization.Culture;
using MyNet.Globalization.Facade;
using MyNet.Primitives;
using MyNet.Primitives.Temporal;

namespace MyNet.Avalonia.Controls.Behaviors;

public static class GlobalizationBehavior
{
    private sealed class State
    {
        public EventHandler<CultureChangedEventArgs>? CultureChangedHandler { get; set; }
    }

    private static readonly ConditionalWeakTable<Control, State> States = [];

    static GlobalizationBehavior() => UpdateOnCultureChangedProperty.Changed.Subscribe(OnUpdateOnCultureChangedCallback);

    #region UpdateOnCultureChanged

    /// <summary>
    /// Provides UpdateOnCultureChanged Property for attached GlobalizationBehavior element.
    /// </summary>
    public static readonly AttachedProperty<bool> UpdateOnCultureChangedProperty = AvaloniaProperty.RegisterAttached<StyledElement, bool>("UpdateOnCultureChanged", typeof(GlobalizationBehavior));

    /// <summary>
    /// Accessor for Attached  <see cref="UpdateOnCultureChangedProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="UpdateOnCultureChangedProperty"/>.</param>
    public static void SetUpdateOnCultureChanged(StyledElement element, bool value) => element.SetValue(UpdateOnCultureChangedProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="UpdateOnCultureChangedProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static bool GetUpdateOnCultureChanged(StyledElement element) => element.GetValue(UpdateOnCultureChangedProperty);

    private static void OnUpdateOnCultureChangedCallback(AvaloniaPropertyChangedEventArgs args)
    {
        if (args.Sender is not Control element) return;

        var state = States.GetOrCreateValue(element);

        if (((bool?)args.NewValue).IsTrue())
        {
            element.OnLoading<Control>(
                x =>
                {
                    Subscribe(element, state);
                    UpdateControl(x);
                },
                _ => Unsubscribe(state));
        }
        else
        {
            Unsubscribe(state);
        }
    }

    private static void Subscribe(Control element, State state)
    {
        if (state.CultureChangedHandler is not null) return;

        state.CultureChangedHandler = (_, _) => UpdateControl(element);
        GlobalizationServices.Current.CultureChanged += state.CultureChangedHandler;
    }

    private static void Unsubscribe(State state)
    {
        if (state.CultureChangedHandler is null) return;

        GlobalizationServices.Current.CultureChanged -= state.CultureChangedHandler;
        state.CultureChangedHandler = null;
    }

    private static void UpdateControl(Control? element)
    {
        switch (element)
        {
            case TimePicker tp:
                UpdateTimePicker(tp);
                break;
            case TimePickerEx tpEx:
                UpdateTimePickerEx(tpEx);
                break;
            case CalendarDatePicker calendarDatePicker:
                UpdateCalendarDatePicker(calendarDatePicker);
                break;
            case CalendarDatePickerEx calendarDatePickerEx:
                UpdateCalendarDatePickerEx(calendarDatePickerEx);
                break;
            case DateTimePickerEx dateTimePickerEx:
                UpdateDateTimePickerEx(dateTimePickerEx);
                break;
            case DatePicker datepicker:
                UpdateDatePicker(datepicker);
                break;
        }
    }

    private static void UpdateTimePicker(TimePicker timePicker) => timePicker.ClockIdentifier = GlobalizationServices.Current.CurrentCulture.DateTimeFormat.ShortTimePattern.Contains("HH", StringComparison.InvariantCulture) ? "24HourClock" : "12HourClock";

    private static void UpdateTimePickerEx(TimePickerEx timePicker)
    {
        timePicker.TimeFormat = GlobalizationServices.Current.CurrentCulture.DateTimeFormat.ShortTimePattern.Contains("HH", StringComparison.InvariantCulture) ? TimeFormat.TwentyFourHour : TimeFormat.TwelveHour;
        timePicker.DisplayFormat = timePicker.ShowSeconds ? GlobalizationServices.Current.CurrentCulture.DateTimeFormat.LongTimePattern : GlobalizationServices.Current.CurrentCulture.DateTimeFormat.ShortTimePattern;
    }

    private static void UpdateCalendarDatePicker(CalendarDatePicker calendarDatePicker)
    {
        calendarDatePicker.SelectedDateFormat = CalendarDatePickerFormat.Custom;
        calendarDatePicker.CustomDateFormatString = GlobalizationServices.Current.CurrentCulture.DateTimeFormat.ShortDatePattern;
    }

    private static void UpdateCalendarDatePickerEx(CalendarDatePickerEx calendarDatePicker) => calendarDatePicker.DisplayFormat = GlobalizationServices.Current.CurrentCulture.DateTimeFormat.ShortDatePattern;

    private static void UpdateDateTimePickerEx(DateTimePickerEx dateTimePicker)
    {
        dateTimePicker.TimeFormat = GlobalizationServices.Current.CurrentCulture.DateTimeFormat.ShortTimePattern.Contains("HH", StringComparison.InvariantCulture) ? TimeFormat.TwentyFourHour : TimeFormat.TwelveHour;
        dateTimePicker.DisplayFormat = dateTimePicker.ShowSeconds
            ? $"{GlobalizationServices.Current.CurrentCulture.DateTimeFormat.ShortDatePattern} {GlobalizationServices.Current.CurrentCulture.DateTimeFormat.LongTimePattern}"
            : $"{GlobalizationServices.Current.CurrentCulture.DateTimeFormat.ShortDatePattern} {GlobalizationServices.Current.CurrentCulture.DateTimeFormat.ShortTimePattern}";
    }

    private static void UpdateDatePicker(DatePicker datePicker)
    {
        var oldValue = datePicker.MonthFormat;
        datePicker.MonthFormat = string.Empty;
        datePicker.MonthFormat = oldValue;

        datePicker.DayVisible = !datePicker.DayVisible;
        datePicker.DayVisible = !datePicker.DayVisible;
    }

    #endregion
}
