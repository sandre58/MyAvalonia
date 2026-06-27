// -----------------------------------------------------------------------
// <copyright file="DateTimePickerEx.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Avalonia;
using Avalonia.Automation;
using Avalonia.Automation.Peers;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using MyNet.Avalonia.Controls.Primitives;
using MyNet.Primitives.Intervals;
using MyNet.Primitives.Temporal;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

[TemplatePart(PartButton, typeof(Button))]
[TemplatePart(PartPopup, typeof(Popup))]
[TemplatePart(PartTextBox, typeof(TextBox))]
[TemplatePart(PartPreviewer, typeof(Control))]
[PseudoClasses(PseudoClassName.FlyoutOpen, PseudoClassName.Pressed)]
[SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix", Justification = "Improve Avalonia control")]
public partial class DateTimePickerEx : TextPicker<DateTime?, DateTimeView>
{
    static DateTimePickerEx()
    {
        AutomationProperties.ControlTypeOverrideProperty.OverrideDefaultValue<DateTimePickerEx>(AutomationControlType.Custom);
        CloseOnCommitProperty.OverrideDefaultValue<DateTimePickerEx>(false);
        ShowSecondsProperty.Changed.AddClassHandler<DateTimePickerEx>((o, _) => o.DisplayFormat = o.ComputeDisplayFormat());
        TimeFormatProperty.Changed.AddClassHandler<DateTimePickerEx>((o, _) => o.DisplayFormat = o.ComputeDisplayFormat());
    }

    public DateTimePickerEx()
    {
        SetCurrentValue(FirstDayOfWeekProperty, DateTimeHelper.GetCurrentDateTimeFormatInfo().FirstDayOfWeek);
        SetCurrentValue(DisplayDateProperty, DateTime.Today);
        SetCurrentValue(DisplayFormatProperty, ComputeDisplayFormat());
    }

    #region BlackoutDates

    public static readonly StyledProperty<AvaloniaList<Period>> BlackoutDatesProperty =
        DateTimeView.BlackoutDatesProperty.AddOwner<DateTimePickerEx>();

    [SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "Used for binding")]
    public AvaloniaList<Period> BlackoutDates
    {
        get => GetValue(BlackoutDatesProperty);
        set => SetValue(BlackoutDatesProperty, value);
    }

    #endregion

    #region BlackoutDateRule

    public static readonly StyledProperty<IDateSelector?> BlackoutDateRuleProperty =
        DateTimeView.BlackoutDateRuleProperty.AddOwner<DateTimePickerEx>();

    public IDateSelector? BlackoutDateRule
    {
        get => GetValue(BlackoutDateRuleProperty);
        set => SetValue(BlackoutDateRuleProperty, value);
    }

    #endregion

    #region FirstDayOfWeek

    public static readonly StyledProperty<DayOfWeek> FirstDayOfWeekProperty =
        DateTimeView.FirstDayOfWeekProperty.AddOwner<DateTimePickerEx>();

    public DayOfWeek FirstDayOfWeek
    {
        get => GetValue(FirstDayOfWeekProperty);
        set => SetValue(FirstDayOfWeekProperty, value);
    }

    #endregion

    #region IsTodayHighlighted

    public static readonly StyledProperty<bool> IsTodayHighlightedProperty =
        DateTimeView.IsTodayHighlightedProperty.AddOwner<DateTimePickerEx>();

    public bool IsTodayHighlighted
    {
        get => GetValue(IsTodayHighlightedProperty);
        set => SetValue(IsTodayHighlightedProperty, value);
    }

    #endregion

    #region DisplayDate

    public static readonly StyledProperty<DateTime> DisplayDateProperty =
        DateTimeView.DisplayDateProperty.AddOwner<DateTimePickerEx>();

    public DateTime DisplayDate
    {
        get => GetValue(DisplayDateProperty);
        set => SetValue(DisplayDateProperty, value);
    }

    #endregion

    #region DisplayDateStart

    public static readonly StyledProperty<DateTime?> DisplayDateStartProperty =
        DateTimeView.DisplayDateStartProperty.AddOwner<DateTimePickerEx>();

    public DateTime? DisplayDateStart
    {
        get => GetValue(DisplayDateStartProperty);
        set => SetValue(DisplayDateStartProperty, value);
    }

    #endregion

    #region DisplayDateEnd

    public static readonly StyledProperty<DateTime?> DisplayDateEndProperty =
        DateTimeView.DisplayDateEndProperty.AddOwner<DateTimePickerEx>();

    public DateTime? DisplayDateEnd
    {
        get => GetValue(DisplayDateEndProperty);
        set => SetValue(DisplayDateEndProperty, value);
    }

    #endregion

    #region NumberFormat

    public static readonly StyledProperty<string> NumberFormatProperty = TimeView.NumberFormatProperty.AddOwner<DateTimePickerEx>();

    public string NumberFormat
    {
        get => GetValue(NumberFormatProperty);
        set => SetValue(NumberFormatProperty, value);
    }

    #endregion

    #region ShowSeconds

    public static readonly StyledProperty<bool> ShowSecondsProperty = TimeSelectorBase.ShowSecondsProperty.AddOwner<DateTimePickerEx>();

    public bool ShowSeconds
    {
        get => GetValue(ShowSecondsProperty);
        set => SetValue(ShowSecondsProperty, value);
    }

    #endregion

    #region TimeFormat

    public static readonly StyledProperty<TimeFormat> TimeFormatProperty = TimeSelectorBase.TimeFormatProperty.AddOwner<DateTimePickerEx>();

    public TimeFormat TimeFormat
    {
        get => GetValue(TimeFormatProperty);
        set => SetValue(TimeFormatProperty, value);
    }

    #endregion

    private void OnDateTimeChanged(object? sender, SelectionChangedEventArgs e) => OnPreviewValueChanged();

    protected override DateTime? IncrementValue(int offset) => SelectedValue?.AddMinutes(offset);

    protected override DateTime? IncrementLargeValue(int offset) => SelectedValue?.AddHours(offset);

    protected override string? ConvertValueToString(DateTime? value) =>
        value?.ToString(DisplayFormat ?? ComputeDisplayFormat(), CultureInfo.CurrentCulture);

    protected override DateTime? ConvertValueFromString(string text) => string.IsNullOrWhiteSpace(text)
        ? null
        : DateTime.TryParse(text, CultureInfo.CurrentCulture, DateTimeStyles.None, out var dateTime)
            ? dateTime
            : DateTime.TryParseExact(text, DisplayFormat ?? ComputeDisplayFormat(), CultureInfo.CurrentCulture, DateTimeStyles.None, out dateTime)
                ? dateTime
                : null;

    protected override bool IsValidValue(DateTime? value) => !value.HasValue || Previewer?.IsValidSelection(value.Value) == true;

    protected override void SetPreviewValue(DateTime? value) => Previewer?.SelectedValue = value;

    protected override DateTime? GetPreviewValue() => Previewer?.SelectedValue;

    private string ComputeDisplayFormat()
    {
        var culture = CultureInfo.CurrentCulture;
        var dateFormat = culture.DateTimeFormat.ShortDatePattern;
        var baseTimeFormat = TimeFormat == TimeFormat.TwelveHour ? "h:mm" : "HH:mm";
        var timeFormat = ShowSeconds ? $"{baseTimeFormat}:ss" : baseTimeFormat;

        if (TimeFormat == TimeFormat.TwelveHour)
            timeFormat += " tt";

        return $"{dateFormat} {timeFormat}";
    }
}
