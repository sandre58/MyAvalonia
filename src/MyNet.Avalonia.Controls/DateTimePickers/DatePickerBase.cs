// -----------------------------------------------------------------------
// <copyright file="DatePickerBase.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using MyNet.Avalonia.Controls.DateTimePickers;
using MyNet.Utilities.DateTimes;
using MyNet.Utilities.Localization;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls.Primitives;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public class DatePickerBase : TemplatedControl
{
    protected DatePickerBase() => GlobalizationService.Current.CultureChanged += (_, _) => DisplayFormat = GlobalizationService.Current.Culture.DateTimeFormat.ShortDatePattern;

    public static readonly StyledProperty<string?> DisplayFormatProperty = AvaloniaProperty.Register<DatePickerBase, string?>(nameof(DisplayFormat), CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);

    public static readonly StyledProperty<AvaloniaList<Period>> BlackoutDatesProperty = AvaloniaProperty.Register<DatePickerBase, AvaloniaList<Period>>(nameof(BlackoutDates));

    public static readonly StyledProperty<IDateSelector?> BlackoutDateRuleProperty = AvaloniaProperty.Register<DatePickerBase, IDateSelector?>(nameof(BlackoutDateRule));

    public static readonly StyledProperty<DayOfWeek> FirstDayOfWeekProperty = AvaloniaProperty.Register<DatePickerBase, DayOfWeek>(nameof(FirstDayOfWeek), CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek);

    public static readonly StyledProperty<bool> IsTodayHighlightedProperty = AvaloniaProperty.Register<DatePickerBase, bool>(nameof(IsTodayHighlighted), true);

    public static readonly StyledProperty<bool> IsDropDownOpenProperty = AvaloniaProperty.Register<DatePickerBase, bool>(nameof(IsDropDownOpen), defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<bool> AllowSpinProperty = AvaloniaProperty.Register<DatePickerBase, bool>(nameof(AllowSpin));

    public static readonly StyledProperty<string?> WatermarkProperty = AvaloniaProperty.Register<DatePickerBase, string?>(nameof(Watermark));

    public static readonly StyledProperty<DateTime> DisplayDateProperty = AvaloniaProperty.Register<CalendarDatePicker, DateTime>(nameof(DisplayDate), DateTime.Today);

    public static readonly StyledProperty<DateTime?> DisplayDateStartProperty = AvaloniaProperty.Register<CalendarDatePicker, DateTime?>(nameof(DisplayDateStart));

    public static readonly StyledProperty<DateTime?> DisplayDateEndProperty = AvaloniaProperty.Register<CalendarDatePicker, DateTime?>(nameof(DisplayDateEnd));

    public string? Watermark
    {
        get => GetValue(WatermarkProperty);
        set => SetValue(WatermarkProperty, value);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "Used for binding")]
    public AvaloniaList<Period> BlackoutDates
    {
        get => GetValue(BlackoutDatesProperty);
        set => SetValue(BlackoutDatesProperty, value);
    }

    public IDateSelector? BlackoutDateRule
    {
        get => GetValue(BlackoutDateRuleProperty);
        set => SetValue(BlackoutDateRuleProperty, value);
    }

    public DayOfWeek FirstDayOfWeek
    {
        get => GetValue(FirstDayOfWeekProperty);
        set => SetValue(FirstDayOfWeekProperty, value);
    }

    public bool IsTodayHighlighted
    {
        get => GetValue(IsTodayHighlightedProperty);
        set => SetValue(IsTodayHighlightedProperty, value);
    }

    public bool IsDropDownOpen
    {
        get => GetValue(IsDropDownOpenProperty);
        set => SetValue(IsDropDownOpenProperty, value);
    }

    public string? DisplayFormat
    {
        get => GetValue(DisplayFormatProperty);
        set => SetValue(DisplayFormatProperty, value);
    }

    public DateTime DisplayDate
    {
        get => GetValue(DisplayDateProperty);
        set => SetValue(DisplayDateProperty, value);
    }

    public DateTime? DisplayDateStart
    {
        get => GetValue(DisplayDateStartProperty);
        set => SetValue(DisplayDateStartProperty, value);
    }

    public DateTime? DisplayDateEnd
    {
        get => GetValue(DisplayDateEndProperty);
        set => SetValue(DisplayDateEndProperty, value);
    }

    public bool AllowSpin
    {
        get => GetValue(AllowSpinProperty);
        set => SetValue(AllowSpinProperty, value);
    }
}
