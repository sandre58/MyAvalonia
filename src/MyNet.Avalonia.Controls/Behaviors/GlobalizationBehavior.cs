// -----------------------------------------------------------------------
// <copyright file="GlobalizationBehavior.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using Avalonia.Controls;
using MyNet.Avalonia.Controls.Extensions;
using MyNet.Globalization.Facade;
using MyNet.Primitives;
using MyNet.Primitives.Temporal;

namespace MyNet.Avalonia.Controls.Behaviors;

public static class GlobalizationBehavior
{
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

        if (((bool?)args.NewValue).IsTrue())
        {
            args.Sender.OnLoading<Control>(x =>
                {
                    UpdateControl(x);
                    GlobalizationServices.Current.CultureChanged += onCultureChanged;
                },
                _ => GlobalizationServices.Current.CultureChanged -= onCultureChanged);
        }
        else
        {
            GlobalizationServices.Current.CultureChanged -= onCultureChanged;
        }

        void onCultureChanged(object? sender, EventArgs e) => UpdateControl(element);
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
