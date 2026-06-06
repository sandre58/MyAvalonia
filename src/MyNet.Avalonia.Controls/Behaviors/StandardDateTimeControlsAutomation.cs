// -----------------------------------------------------------------------
// <copyright file="StandardDateTimeControlsAutomation.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Automation;
using Avalonia.Automation.Peers;
using Avalonia.Controls;
using AvaloniaCalendar = Avalonia.Controls.Calendar;

namespace MyNet.Avalonia.Controls.Behaviors;

internal static class StandardDateTimeControlsAutomation
{
    [ModuleInitializer]
    [SuppressMessage("Usage", "CA2255:The 'ModuleInitializer' attribute should only be used in application code or advanced source generator scenarios", Justification = "Registers automation defaults for Avalonia date/time pickers when the controls assembly loads.")]
    internal static void Initialize()
    {
        AutomationProperties.ControlTypeOverrideProperty.OverrideDefaultValue<CalendarDatePicker>(AutomationControlType.Custom);
        AutomationProperties.ControlTypeOverrideProperty.OverrideDefaultValue<DatePicker>(AutomationControlType.Custom);
        AutomationProperties.ControlTypeOverrideProperty.OverrideDefaultValue<TimePicker>(AutomationControlType.Custom);
        AutomationProperties.ControlTypeOverrideProperty.OverrideDefaultValue<AvaloniaCalendar>(AutomationControlType.Calendar);

        _ = CalendarDatePicker.SelectedDateProperty.Changed.AddClassHandler<CalendarDatePicker>((picker, _) => UpdateCalendarDatePickerName(picker));
        _ = CalendarDatePicker.PlaceholderTextProperty.Changed.AddClassHandler<CalendarDatePicker>((picker, _) => UpdateCalendarDatePickerName(picker));

        _ = DatePicker.SelectedDateProperty.Changed.AddClassHandler<DatePicker>((picker, _) => UpdateDatePickerName(picker));

        _ = TimePicker.SelectedTimeProperty.Changed.AddClassHandler<TimePicker>((picker, _) => UpdateTimePickerName(picker));

        _ = AvaloniaCalendar.SelectedDateProperty.Changed.AddClassHandler<AvaloniaCalendar>((calendar, _) => UpdateAvaloniaCalendarName(calendar));
        _ = AvaloniaCalendar.DisplayDateProperty.Changed.AddClassHandler<AvaloniaCalendar>((calendar, _) => UpdateAvaloniaCalendarName(calendar));
    }

    private static void UpdateCalendarDatePickerName(CalendarDatePicker picker) =>
        AutomationProperties.SetName(
            picker,
            picker.SelectedDate?.ToString(CultureInfo.CurrentCulture) ?? picker.PlaceholderText ?? string.Empty);

    private static void UpdateDatePickerName(DatePicker picker) =>
        AutomationProperties.SetName(picker, picker.SelectedDate?.ToString(CultureInfo.CurrentCulture) ?? string.Empty);

    private static void UpdateTimePickerName(TimePicker picker) =>
        AutomationProperties.SetName(picker, picker.SelectedTime?.ToString() ?? string.Empty);

    private static void UpdateAvaloniaCalendarName(AvaloniaCalendar calendar)
    {
        var name = calendar.SelectedDate?.ToString(CultureInfo.CurrentCulture) ?? calendar.DisplayDate.ToString(CultureInfo.CurrentCulture);
        AutomationProperties.SetName(calendar, name);
    }
}
