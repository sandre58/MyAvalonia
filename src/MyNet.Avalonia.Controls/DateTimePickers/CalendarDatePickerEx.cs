// -----------------------------------------------------------------------
// <copyright file="CalendarDatePickerEx.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using MyNet.Avalonia.Controls.Primitives;
using MyNet.Utilities.DateTimes;
using MyNet.Utilities.Helpers;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

[TemplatePart(PartButton, typeof(Button))]
[TemplatePart(PartPopup, typeof(Popup))]
[TemplatePart(PartTextBox, typeof(TextBox))]
[TemplatePart(PartPreviewer, typeof(Calendar))]
[PseudoClasses(PseudoClassName.FlyoutOpen)]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix", Justification = "Improve Avalonia control")]
public class CalendarDatePickerEx : TextPicker<DateTime?, Calendar>
{
    public CalendarDatePickerEx()
    {
        SetCurrentValue(FirstDayOfWeekProperty, DateTimeHelper.GetCurrentDateTimeFormatInfo().FirstDayOfWeek);
        SetCurrentValue(DisplayDateProperty, DateTime.Today);
    }

    #region BlackoutDates

    public static readonly StyledProperty<AvaloniaList<Period>> BlackoutDatesProperty = AvaloniaProperty.Register<CalendarDatePickerEx, AvaloniaList<Period>>(nameof(BlackoutDates));

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "Used for binding")]
    public AvaloniaList<Period> BlackoutDates
    {
        get => GetValue(BlackoutDatesProperty);
        set => SetValue(BlackoutDatesProperty, value);
    }

    #endregion

    #region BlackoutDateRule

    public static readonly StyledProperty<IDateSelector?> BlackoutDateRuleProperty = AvaloniaProperty.Register<CalendarDatePickerEx, IDateSelector?>(nameof(BlackoutDateRule));

    public IDateSelector? BlackoutDateRule
    {
        get => GetValue(BlackoutDateRuleProperty);
        set => SetValue(BlackoutDateRuleProperty, value);
    }

    #endregion

    #region FirstDayOfWeek

    public static readonly StyledProperty<DayOfWeek> FirstDayOfWeekProperty = AvaloniaProperty.Register<CalendarDatePickerEx, DayOfWeek>(nameof(FirstDayOfWeek), CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek);

    public DayOfWeek FirstDayOfWeek
    {
        get => GetValue(FirstDayOfWeekProperty);
        set => SetValue(FirstDayOfWeekProperty, value);
    }

    #endregion

    #region IsTodayHighlighted

    public static readonly StyledProperty<bool> IsTodayHighlightedProperty = AvaloniaProperty.Register<CalendarDatePickerEx, bool>(nameof(IsTodayHighlighted), true);

    public bool IsTodayHighlighted
    {
        get => GetValue(IsTodayHighlightedProperty);
        set => SetValue(IsTodayHighlightedProperty, value);
    }

    #endregion

    #region DisplayDate

    public static readonly StyledProperty<DateTime> DisplayDateProperty = AvaloniaProperty.Register<CalendarDatePickerEx, DateTime>(nameof(DisplayDate), DateTime.Today);

    public DateTime DisplayDate
    {
        get => GetValue(DisplayDateProperty);
        set => SetValue(DisplayDateProperty, value);
    }

    #endregion

    #region DisplayDateStart

    public static readonly StyledProperty<DateTime?> DisplayDateStartProperty = AvaloniaProperty.Register<CalendarDatePickerEx, DateTime?>(nameof(DisplayDateStart));

    public DateTime? DisplayDateStart
    {
        get => GetValue(DisplayDateStartProperty);
        set => SetValue(DisplayDateStartProperty, value);
    }

    #endregion

    #region DisplayDateEnd

    public static readonly StyledProperty<DateTime?> DisplayDateEndProperty = AvaloniaProperty.Register<CalendarDatePickerEx, DateTime?>(nameof(DisplayDateEnd));

    public DateTime? DisplayDateEnd
    {
        get => GetValue(DisplayDateEndProperty);
        set => SetValue(DisplayDateEndProperty, value);
    }

    #endregion

    #region Calendar

    protected override void RemovePreviewerHandlers()
    {
        if (Previewer != null)
        {
            Previewer.DayButtonClick -= OnCalendarDayButtonClick;
        }
    }

    protected override void AddPreviewerHandlers()
    {
        if (Previewer != null)
        {
            Previewer.SelectionMode = CalendarSelectionMode.SingleDate;
            Previewer.DayButtonClick += OnCalendarDayButtonClick;
        }
    }

    private void OnCalendarDayButtonClick(object? sender, RoutedEventArgs e) => OnPreviewValueChanged();

    #endregion

    protected override DateTime? IncrementValue(int offset) => SelectedValue?.AddDays(offset);

    protected override DateTime? IncrementLargeValue(int offset) => SelectedValue?.AddMonths(offset);

    protected override string? ConvertValueToString(DateTime? value) => value?.ToString(DisplayFormat ?? DateTimeHelper.GetCurrentDateTimeFormatInfo().ShortDatePattern, CultureInfo.CurrentCulture);

    protected override DateTime? ConvertValueFromString(string text) => DateTime.ParseExact(text, DisplayFormat ?? DateTimeHelper.GetCurrentDateTimeFormatInfo().ShortDatePattern, DateTimeHelper.GetCurrentDateTimeFormatInfo());

    protected override bool IsValidValue(DateTime? value) => !value.HasValue || Previewer?.IsValidSelection(value.Value) == true;

    protected override void SetPreviewValue(DateTime? value) => Previewer?.MoveToDate(value ?? DisplayDate);

    protected override DateTime? GetPreviewValue() => Previewer?.SelectedDate;
}
