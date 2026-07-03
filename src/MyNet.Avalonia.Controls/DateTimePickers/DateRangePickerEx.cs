// -----------------------------------------------------------------------
// <copyright file="DateRangePickerEx.cs" company="Stéphane ANDRE">
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
using Avalonia.Interactivity;
using MyNet.Avalonia.Controls.DateTimePickers.Internal;
using MyNet.Avalonia.Controls.Internals.Calendar;
using MyNet.Avalonia.Controls.Primitives;
using MyNet.Primitives;
using MyNet.Primitives.Intervals;
using MyNet.Primitives.Temporal;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

[TemplatePart(PartButton, typeof(Button))]
[TemplatePart(PartPopup, typeof(Popup))]
[TemplatePart(PartTextBox, typeof(TextBox))]
[TemplatePart(PartPreviewer, typeof(Calendar))]
[TemplatePart(PartClearButton, typeof(Button))]
[PseudoClasses(PseudoClassName.FlyoutOpen)]
[SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix", Justification = "Improve Avalonia control")]
public partial class DateRangePickerEx : TextPicker<Period?, Calendar>
{
    public const string PartClearButton = "PART_ClearButton";

    private Button? _clearButton;
    private DateTime? _pendingRangeStart;
    private DateTime? _partialStart;
    private DateTime? _partialEnd;

    static DateRangePickerEx()
    {
        AutomationProperties.ControlTypeOverrideProperty.OverrideDefaultValue<DateRangePickerEx>(AutomationControlType.Custom);
        CloseOnCommitProperty.OverrideDefaultValue<DateRangePickerEx>(false);
        CloseOnSingleSelectionProperty.OverrideDefaultValue<DateRangePickerEx>(true);
    }

    public DateRangePickerEx()
    {
        SetCurrentValue(FirstDayOfWeekProperty, DateTimeHelper.GetCurrentDateTimeFormatInfo().FirstDayOfWeek);
        SetCurrentValue(DisplayDateProperty, DateTime.Today);
        SetCurrentValue(DisplayFormatProperty, DateTimeHelper.GetCurrentDateTimeFormatInfo().ShortDatePattern);
    }

    #region BlackoutDates

    public static readonly StyledProperty<AvaloniaList<Period>?> BlackoutDatesProperty = AvaloniaProperty.Register<DateRangePickerEx, AvaloniaList<Period>?>(nameof(BlackoutDates));

    [SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "Used for binding")]
    public AvaloniaList<Period>? BlackoutDates
    {
        get => GetValue(BlackoutDatesProperty);
        set => SetValue(BlackoutDatesProperty, value);
    }

    #endregion

    #region BlackoutDateRule

    public static readonly StyledProperty<IDateSelector?> BlackoutDateRuleProperty = AvaloniaProperty.Register<DateRangePickerEx, IDateSelector?>(nameof(BlackoutDateRule));

    public IDateSelector? BlackoutDateRule
    {
        get => GetValue(BlackoutDateRuleProperty);
        set => SetValue(BlackoutDateRuleProperty, value);
    }

    #endregion

    #region FirstDayOfWeek

    public static readonly StyledProperty<DayOfWeek> FirstDayOfWeekProperty = AvaloniaProperty.Register<DateRangePickerEx, DayOfWeek>(nameof(FirstDayOfWeek), CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek);

    public DayOfWeek FirstDayOfWeek
    {
        get => GetValue(FirstDayOfWeekProperty);
        set => SetValue(FirstDayOfWeekProperty, value);
    }

    #endregion

    #region IsTodayHighlighted

    public static readonly StyledProperty<bool> IsTodayHighlightedProperty = AvaloniaProperty.Register<DateRangePickerEx, bool>(nameof(IsTodayHighlighted), true);

    public bool IsTodayHighlighted
    {
        get => GetValue(IsTodayHighlightedProperty);
        set => SetValue(IsTodayHighlightedProperty, value);
    }

    #endregion

    #region DisplayDate

    public static readonly StyledProperty<DateTime> DisplayDateProperty = AvaloniaProperty.Register<DateRangePickerEx, DateTime>(nameof(DisplayDate), DateTime.Today);

    public DateTime DisplayDate
    {
        get => GetValue(DisplayDateProperty);
        set => SetValue(DisplayDateProperty, value);
    }

    #endregion

    #region DisplayDateStart

    public static readonly StyledProperty<DateTime?> DisplayDateStartProperty = AvaloniaProperty.Register<DateRangePickerEx, DateTime?>(nameof(DisplayDateStart));

    public DateTime? DisplayDateStart
    {
        get => GetValue(DisplayDateStartProperty);
        set => SetValue(DisplayDateStartProperty, value);
    }

    #endregion

    #region DisplayDateEnd

    public static readonly StyledProperty<DateTime?> DisplayDateEndProperty = AvaloniaProperty.Register<DateRangePickerEx, DateTime?>(nameof(DisplayDateEnd));

    public DateTime? DisplayDateEnd
    {
        get => GetValue(DisplayDateEndProperty);
        set => SetValue(DisplayDateEndProperty, value);
    }

    #endregion

    #region AllowTapRangeSelection

    public static readonly StyledProperty<bool> AllowTapRangeSelectionProperty = AvaloniaProperty.Register<DateRangePickerEx, bool>(nameof(AllowTapRangeSelection), true);

    public bool AllowTapRangeSelection
    {
        get => GetValue(AllowTapRangeSelectionProperty);
        set => SetValue(AllowTapRangeSelectionProperty, value);
    }

    #endregion

    #region RangeSeparator

    public static readonly StyledProperty<string> RangeSeparatorProperty = AvaloniaProperty.Register<DateRangePickerEx, string>(nameof(RangeSeparator), " – ");

    public string RangeSeparator
    {
        get => GetValue(RangeSeparatorProperty);
        set => SetValue(RangeSeparatorProperty, value);
    }

    #endregion

    private static DateTime GetPeriodStart(Period period) => period.Start!.Value.Value.DiscardTime();

    private static DateTime GetPeriodEnd(Period period) => period.End!.Value.Value.DiscardTime();

    #region StartDate / EndDate

    public DateTime? StartDate
    {
        get => SelectedValue is { } period ? GetPeriodStart(period) : _partialStart;
        set => SetBoundaryDate(value, isStart: true);
    }

    public DateTime? EndDate
    {
        get => SelectedValue is { } period ? GetPeriodEnd(period) : _partialEnd;
        set => SetBoundaryDate(value, isStart: false);
    }

    private void SetBoundaryDate(DateTime? value, bool isStart)
    {
        if (!value.HasValue)
        {
            if (isStart)
            {
                _partialStart = null;
                SetCurrentValue(SelectedValueProperty, null);
            }
            else
            {
                _partialEnd = null;
                if (SelectedValue is { } period)
                {
                    _partialStart = GetPeriodStart(period);
                    SetCurrentValue(SelectedValueProperty, null);
                }
            }

            return;
        }

        var date = value.Value.DiscardTime();
        if (isStart)
            _partialStart = date;
        else
            _partialEnd = date;

        TryCommitPartialRange();
    }

    private void TryCommitPartialRange()
    {
        if (_partialStart is not { } start || _partialEnd is not { } end)
            return;

        if (start.IsAfter(end))
            (start, end) = (end, start);

        _partialStart = null;
        _partialEnd = null;
        SetCurrentValue(SelectedValueProperty, CalendarDateRangeHelper.ToDateRangePeriod(start, end));
    }

    #endregion

    #region Calendar

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        if (_clearButton is not null)
            _clearButton.Click -= OnClearButtonClick;

        _clearButton = e.NameScope.Find<Button>(PartClearButton);
        if (_clearButton is not null)
            _clearButton.Click += OnClearButtonClick;
    }

    private void OnClearButtonClick(object? sender, RoutedEventArgs e) => Clear();

    protected override void RemovePreviewerHandlers()
    {
        base.RemovePreviewerHandlers();
        Previewer?.DayButtonClick -= OnCalendarDayButtonClick;
    }

    protected override void AddPreviewerHandlers()
    {
        base.AddPreviewerHandlers();

        if (Previewer is null)
            return;

        Previewer.SelectionMode = CalendarSelectionMode.SingleRange;
        Previewer.AllowTapRangeSelection = AllowTapRangeSelection;
        Previewer.DayButtonClick += OnCalendarDayButtonClick;
        SyncBlackoutDates();
    }

    protected override void OnDropDownClosing()
    {
        if (ShouldRollbackOnClose())
            _pendingRangeStart = null;

        base.OnDropDownClosing();
    }

    protected override bool ShouldCloseAfterSingleSelection() =>
        CloseOnSingleSelection && IsDropDownOpen && GetPreviewValue() is not null;

    protected override bool ShouldRollbackOnClose() => HasUncommittedRangePreview();

    private void SyncBlackoutDates()
    {
        if (Previewer is null || BlackoutDates is null)
            return;

        Previewer.BlackoutDates.Clear();
        foreach (var period in BlackoutDates)
            Previewer.BlackoutDates.Add(period);
    }

    private void OnCalendarDayButtonClick(object? sender, RoutedEventArgs e)
    {
        if (AllowTapRangeSelection)
        {
            if (_pendingRangeStart is null)
            {
                _pendingRangeStart = CalendarDateRangeHelper.GetSelectedMin(Previewer!.SelectedDates);
                return;
            }

            _pendingRangeStart = null;
        }

        OnPreviewValueChanged();
    }

    #endregion

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == IsDropDownOpenProperty && change.GetNewValue<bool>())
        {
            _pendingRangeStart = null;
        }
        else if (change.Property == AllowTapRangeSelectionProperty && Previewer is not null)
        {
            Previewer.AllowTapRangeSelection = change.GetNewValue<bool>();
        }
        else if (change.Property == BlackoutDatesProperty)
        {
            SyncBlackoutDates();
        }
        else if (change.Property == SelectedValueProperty)
        {
            _partialStart = null;
            _partialEnd = null;
        }
        else if (change.Property == RangeSeparatorProperty && SelectedValue is not null)
        {
            SetCurrentValue(TextProperty, ConvertValueToString(SelectedValue));
        }
    }

    protected override void TryFocusPopupContent()
    {
        if (Previewer is { } calendar)
        {
            calendar.FocusSelectedDay();
            return;
        }

        base.TryFocusPopupContent();
    }

    protected override Period? IncrementValue(int offset)
    {
        if (SelectedValue is not { } period)
            return null;

        return period.Shift(TimeSpan.FromDays(offset));
    }

    protected override Period? IncrementLargeValue(int offset)
    {
        if (SelectedValue is not { } period)
            return null;

        var start = GetPeriodStart(period).AddMonths(offset);
        var end = GetPeriodEnd(period).AddMonths(offset);
        return CalendarDateRangeHelper.ToDateRangePeriod(start, end);
    }

    protected override string? ConvertValueToString(Period? value)
    {
        if (value is null)
            return null;

        var format = DisplayFormat ?? DateTimeHelper.GetCurrentDateTimeFormatInfo().ShortDatePattern;
        var culture = CultureInfo.CurrentCulture;
        var start = GetPeriodStart(value).ToString(format, culture);
        var end = GetPeriodEnd(value).ToString(format, culture);

        return $"{start}{RangeSeparator}{end}";
    }

    protected override Period ConvertValueFromString(string text)
    {
        var separator = RangeSeparator;
        var index = text.IndexOf(separator, StringComparison.Ordinal);
        if (index < 0)
            throw new FormatException($"Expected range separator '{separator}'.");

        var format = DisplayFormat ?? DateTimeHelper.GetCurrentDateTimeFormatInfo().ShortDatePattern;
        var formatInfo = DateTimeHelper.GetCurrentDateTimeFormatInfo();
        var start = DateTime.ParseExact(text[..index].Trim(), format, formatInfo).DiscardTime();
        var end = DateTime.ParseExact(text[(index + separator.Length)..].Trim(), format, formatInfo).DiscardTime();

        if (start.IsAfter(end))
            (start, end) = (end, start);

        return CalendarDateRangeHelper.ToDateRangePeriod(start, end);
    }

    protected override bool IsValidValue(Period? value) =>
        value is null
        || (Previewer?.IsValidSelection(GetPeriodStart(value)) == true
            && Previewer.IsValidSelection(GetPeriodEnd(value)));

    protected override void SetPreviewValue(Period? value)
    {
        if (Previewer is null)
            return;

        Previewer.CancelPendingRangeSelection();

        if (value is null)
        {
            Previewer.SelectedDates.Clear();
            Previewer.MoveToDate(DisplayDate);
            return;
        }

        var start = GetPeriodStart(value);
        var end = GetPeriodEnd(value);
        Previewer.SelectedDates.Set(start, end);
        Previewer.MoveToDate(start);
    }

    private bool HasUncommittedRangePreview() =>
        _pendingRangeStart is not null
        || Previewer?.HasPendingRangeSelection == true;

    protected override Period? GetPreviewValue()
    {
        if (Previewer is null)
            return null;

        var min = CalendarDateRangeHelper.GetSelectedMin(Previewer.SelectedDates);
        var max = CalendarDateRangeHelper.GetSelectedMax(Previewer.SelectedDates);

        if (!min.HasValue || !max.HasValue)
            return null;

        return CalendarDateRangeHelper.ToDateRangePeriod(min.Value, max.Value);
    }
}
