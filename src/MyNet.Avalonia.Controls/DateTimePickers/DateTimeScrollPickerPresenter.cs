// -----------------------------------------------------------------------
// <copyright file="DateTimeScrollPickerPresenter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// Defines the presenter used for selecting a date and a time with scrollable
/// selectors. It merges the behavior of Avalonia's <c>DatePickerPresenter</c> and
/// <c>TimePickerPresenter</c> into a single presenter exposing a <see cref="SelectedDateTime"/>.
/// </summary>
[TemplatePart(TemplateItems.PickerContainerName, typeof(Grid), IsRequired = true)]
[TemplatePart(TemplateItems.AcceptButtonName, typeof(Button), IsRequired = true)]
[TemplatePart(TemplateItems.DismissButtonName, typeof(Button))]
[TemplatePart(TemplateItems.DayHostName, typeof(Panel))]
[TemplatePart(TemplateItems.MonthHostName, typeof(Panel))]
[TemplatePart(TemplateItems.YearHostName, typeof(Panel))]
[TemplatePart(TemplateItems.HourHostName, typeof(Panel))]
[TemplatePart(TemplateItems.MinuteHostName, typeof(Panel))]
[TemplatePart(TemplateItems.SecondHostName, typeof(Panel))]
[TemplatePart(TemplateItems.PeriodHostName, typeof(Panel))]
[TemplatePart(TemplateItems.DaySelectorName, typeof(DateTimePickerPanel))]
[TemplatePart(TemplateItems.MonthSelectorName, typeof(DateTimePickerPanel))]
[TemplatePart(TemplateItems.YearSelectorName, typeof(DateTimePickerPanel))]
[TemplatePart(TemplateItems.HourSelectorName, typeof(DateTimePickerPanel))]
[TemplatePart(TemplateItems.MinuteSelectorName, typeof(DateTimePickerPanel))]
[TemplatePart(TemplateItems.SecondSelectorName, typeof(DateTimePickerPanel))]
[TemplatePart(TemplateItems.PeriodSelectorName, typeof(DateTimePickerPanel))]
public class DateTimeScrollPickerPresenter : PickerPresenterBase
{
    /// <summary>
    /// Defines the <see cref="SelectedDateTime"/> property.
    /// </summary>
    public static readonly StyledProperty<DateTime> SelectedDateTimeProperty =
        AvaloniaProperty.Register<DateTimeScrollPickerPresenter, DateTime>(nameof(SelectedDateTime));

    /// <summary>
    /// Defines the <see cref="DayVisible"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> DayVisibleProperty =
        AvaloniaProperty.Register<DateTimeScrollPickerPresenter, bool>(nameof(DayVisible), true);

    /// <summary>
    /// Defines the <see cref="MonthVisible"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> MonthVisibleProperty =
        AvaloniaProperty.Register<DateTimeScrollPickerPresenter, bool>(nameof(MonthVisible), true);

    /// <summary>
    /// Defines the <see cref="YearVisible"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> YearVisibleProperty =
        AvaloniaProperty.Register<DateTimeScrollPickerPresenter, bool>(nameof(YearVisible), true);

    /// <summary>
    /// Defines the <see cref="DayFormat"/> property.
    /// </summary>
    public static readonly StyledProperty<string> DayFormatProperty =
        AvaloniaProperty.Register<DateTimeScrollPickerPresenter, string>(nameof(DayFormat), "%d");

    /// <summary>
    /// Defines the <see cref="MonthFormat"/> property.
    /// </summary>
    public static readonly StyledProperty<string> MonthFormatProperty =
        AvaloniaProperty.Register<DateTimeScrollPickerPresenter, string>(nameof(MonthFormat), "MMMM");

    /// <summary>
    /// Defines the <see cref="YearFormat"/> property.
    /// </summary>
    public static readonly StyledProperty<string> YearFormatProperty =
        AvaloniaProperty.Register<DateTimeScrollPickerPresenter, string>(nameof(YearFormat), "yyyy");

    /// <summary>
    /// Defines the <see cref="MinYear"/> property.
    /// </summary>
    public static readonly StyledProperty<DateTimeOffset> MinYearProperty =
        AvaloniaProperty.Register<DateTimeScrollPickerPresenter, DateTimeOffset>(nameof(MinYear));

    /// <summary>
    /// Defines the <see cref="MaxYear"/> property.
    /// </summary>
    public static readonly StyledProperty<DateTimeOffset> MaxYearProperty =
        AvaloniaProperty.Register<DateTimeScrollPickerPresenter, DateTimeOffset>(nameof(MaxYear));

    /// <summary>
    /// Defines the <see cref="MinuteIncrement"/> property.
    /// </summary>
    public static readonly StyledProperty<int> MinuteIncrementProperty =
        AvaloniaProperty.Register<DateTimeScrollPickerPresenter, int>(nameof(MinuteIncrement), 1);

    /// <summary>
    /// Defines the <see cref="SecondIncrement"/> property.
    /// </summary>
    public static readonly StyledProperty<int> SecondIncrementProperty =
        AvaloniaProperty.Register<DateTimeScrollPickerPresenter, int>(nameof(SecondIncrement), 1);

    /// <summary>
    /// Defines the <see cref="ClockIdentifier"/> property.
    /// </summary>
    public static readonly StyledProperty<string> ClockIdentifierProperty =
        AvaloniaProperty.Register<DateTimeScrollPickerPresenter, string>(nameof(ClockIdentifier), "12HourClock");

    /// <summary>
    /// Defines the <see cref="UseSeconds"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> UseSecondsProperty =
        AvaloniaProperty.Register<DateTimeScrollPickerPresenter, bool>(nameof(UseSeconds));

    private struct TemplateItems
    {
        public const string PickerContainerName = "PART_PickerContainer";
        public const string AcceptButtonName = "PART_AcceptButton";
        public const string DismissButtonName = "PART_DismissButton";

        public const string DayHostName = "PART_DayHost";
        public const string MonthHostName = "PART_MonthHost";
        public const string YearHostName = "PART_YearHost";
        public const string HourHostName = "PART_HourHost";
        public const string MinuteHostName = "PART_MinuteHost";
        public const string SecondHostName = "PART_SecondHost";
        public const string PeriodHostName = "PART_PeriodHost";

        public const string DaySelectorName = "PART_DaySelector";
        public const string MonthSelectorName = "PART_MonthSelector";
        public const string YearSelectorName = "PART_YearSelector";
        public const string HourSelectorName = "PART_HourSelector";
        public const string MinuteSelectorName = "PART_MinuteSelector";
        public const string SecondSelectorName = "PART_SecondSelector";
        public const string PeriodSelectorName = "PART_PeriodSelector";

        public const string DayUpButtonName = "PART_DayUpButton";
        public const string MonthUpButtonName = "PART_MonthUpButton";
        public const string YearUpButtonName = "PART_YearUpButton";
        public const string HourUpButtonName = "PART_HourUpButton";
        public const string MinuteUpButtonName = "PART_MinuteUpButton";
        public const string SecondUpButtonName = "PART_SecondUpButton";
        public const string PeriodUpButtonName = "PART_PeriodUpButton";

        public const string DayDownButtonName = "PART_DayDownButton";
        public const string MonthDownButtonName = "PART_MonthDownButton";
        public const string YearDownButtonName = "PART_YearDownButton";
        public const string HourDownButtonName = "PART_HourDownButton";
        public const string MinuteDownButtonName = "PART_MinuteDownButton";
        public const string SecondDownButtonName = "PART_SecondDownButton";
        public const string PeriodDownButtonName = "PART_PeriodDownButton";

        public Grid PickerContainer { get; init; }

        public Button AcceptButton { get; init; }

        public Button? DismissButton { get; init; }

        public Panel? DayHost { get; init; }

        public Panel? MonthHost { get; init; }

        public Panel? YearHost { get; init; }

        public Panel? HourHost { get; init; }

        public Panel? MinuteHost { get; init; }

        public Panel? SecondHost { get; init; }

        public Panel? PeriodHost { get; init; }

        public DateTimePickerPanel? DaySelector { get; init; }

        public DateTimePickerPanel? MonthSelector { get; init; }

        public DateTimePickerPanel? YearSelector { get; init; }

        public DateTimePickerPanel? HourSelector { get; init; }

        public DateTimePickerPanel? MinuteSelector { get; init; }

        public DateTimePickerPanel? SecondSelector { get; init; }

        public DateTimePickerPanel? PeriodSelector { get; init; }
    }

    private TemplateItems? _templateItems;
    private readonly List<Control> _spacers = [];
    private Control? _dateTimeSeparator;
    private readonly GregorianCalendar _calendar = new();
    private DateTime _syncDateTime;
    private bool _suppressUpdateSelection;

    public DateTimeScrollPickerPresenter()
    {
        var now = DateTimeOffset.Now;
        SetCurrentValue(MinYearProperty, new(now.Year - 100, 1, 1, 0, 0, 0, now.Offset));
        SetCurrentValue(MaxYearProperty, new(now.Year + 100, 12, 31, 0, 0, 0, now.Offset));
        SetCurrentValue(SelectedDateTimeProperty, DateTime.Now);

        var timePattern = CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;
        if (timePattern.Contains('H', StringComparison.Ordinal))
            SetCurrentValue(ClockIdentifierProperty, "24HourClock");
    }

    static DateTimeScrollPickerPresenter() => KeyboardNavigation.TabNavigationProperty.OverrideDefaultValue<DateTimeScrollPickerPresenter>(KeyboardNavigationMode.Cycle);

    /// <summary>
    /// Gets or sets the working date and time of the presenter.
    /// </summary>
    public DateTime SelectedDateTime
    {
        get => GetValue(SelectedDateTimeProperty);
        set => SetValue(SelectedDateTimeProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the day selector is visible.
    /// </summary>
    public bool DayVisible
    {
        get => GetValue(DayVisibleProperty);
        set => SetValue(DayVisibleProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the month selector is visible.
    /// </summary>
    public bool MonthVisible
    {
        get => GetValue(MonthVisibleProperty);
        set => SetValue(MonthVisibleProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the year selector is visible.
    /// </summary>
    public bool YearVisible
    {
        get => GetValue(YearVisibleProperty);
        set => SetValue(YearVisibleProperty, value);
    }

    /// <summary>
    /// Gets or sets the day format.
    /// </summary>
    public string DayFormat
    {
        get => GetValue(DayFormatProperty);
        set => SetValue(DayFormatProperty, value);
    }

    /// <summary>
    /// Gets or sets the month format.
    /// </summary>
    public string MonthFormat
    {
        get => GetValue(MonthFormatProperty);
        set => SetValue(MonthFormatProperty, value);
    }

    /// <summary>
    /// Gets or sets the year format.
    /// </summary>
    public string YearFormat
    {
        get => GetValue(YearFormatProperty);
        set => SetValue(YearFormatProperty, value);
    }

    /// <summary>
    /// Gets or sets the minimum pickable year.
    /// </summary>
    public DateTimeOffset MinYear
    {
        get => GetValue(MinYearProperty);
        set => SetValue(MinYearProperty, value);
    }

    /// <summary>
    /// Gets or sets the maximum pickable year.
    /// </summary>
    public DateTimeOffset MaxYear
    {
        get => GetValue(MaxYearProperty);
        set => SetValue(MaxYearProperty, value);
    }

    /// <summary>
    /// Gets or sets the minute increment in the selector.
    /// </summary>
    public int MinuteIncrement
    {
        get => GetValue(MinuteIncrementProperty);
        set => SetValue(MinuteIncrementProperty, value);
    }

    /// <summary>
    /// Gets or sets the second increment in the selector.
    /// </summary>
    public int SecondIncrement
    {
        get => GetValue(SecondIncrementProperty);
        set => SetValue(SecondIncrementProperty, value);
    }

    /// <summary>
    /// Gets or sets the clock identifier, either 12HourClock or 24HourClock.
    /// </summary>
    public string ClockIdentifier
    {
        get => GetValue(ClockIdentifierProperty);
        set => SetValue(ClockIdentifierProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the selector uses seconds.
    /// </summary>
    public bool UseSeconds
    {
        get => GetValue(UseSecondsProperty);
        set => SetValue(UseSecondsProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _templateItems = new TemplateItems
        {
            PickerContainer = e.NameScope.Get<Grid>(TemplateItems.PickerContainerName),
            AcceptButton = e.NameScope.Get<Button>(TemplateItems.AcceptButtonName),
            DismissButton = e.NameScope.Find<Button>(TemplateItems.DismissButtonName),

            DayHost = e.NameScope.Find<Panel>(TemplateItems.DayHostName),
            MonthHost = e.NameScope.Find<Panel>(TemplateItems.MonthHostName),
            YearHost = e.NameScope.Find<Panel>(TemplateItems.YearHostName),
            HourHost = e.NameScope.Find<Panel>(TemplateItems.HourHostName),
            MinuteHost = e.NameScope.Find<Panel>(TemplateItems.MinuteHostName),
            SecondHost = e.NameScope.Find<Panel>(TemplateItems.SecondHostName),
            PeriodHost = e.NameScope.Find<Panel>(TemplateItems.PeriodHostName),

            DaySelector = e.NameScope.Find<DateTimePickerPanel>(TemplateItems.DaySelectorName),
            MonthSelector = e.NameScope.Find<DateTimePickerPanel>(TemplateItems.MonthSelectorName),
            YearSelector = e.NameScope.Find<DateTimePickerPanel>(TemplateItems.YearSelectorName),
            HourSelector = e.NameScope.Find<DateTimePickerPanel>(TemplateItems.HourSelectorName),
            MinuteSelector = e.NameScope.Find<DateTimePickerPanel>(TemplateItems.MinuteSelectorName),
            SecondSelector = e.NameScope.Find<DateTimePickerPanel>(TemplateItems.SecondSelectorName),
            PeriodSelector = e.NameScope.Find<DateTimePickerPanel>(TemplateItems.PeriodSelectorName),
        };

        var items = _templateItems.Value;

        _spacers.Clear();
        foreach (var name in new[] { "PART_FirstSpacer", "PART_SecondSpacer", "PART_ThirdSpacer", "PART_FourthSpacer", "PART_FifthSpacer" })
        {
            if (e.NameScope.Find<Control>(name) is { } spacer)
                _spacers.Add(spacer);
        }

        _dateTimeSeparator = e.NameScope.Find<Control>("PART_DateTimeSeparator");

        items.AcceptButton.Click += OnAcceptButtonClicked;
        if (items.DismissButton is { } dismissButton)
            dismissButton.Click += OnDismissButtonClicked;

        if (items.DaySelector is { } daySelector)
            daySelector.SelectionChanged += OnDayChanged;
        if (items.MonthSelector is { } monthSelector)
            monthSelector.SelectionChanged += OnMonthChanged;
        if (items.YearSelector is { } yearSelector)
            yearSelector.SelectionChanged += OnYearChanged;
        if (items.HourSelector is { } hourSelector)
            hourSelector.SelectionChanged += OnTimeChanged;
        if (items.MinuteSelector is { } minuteSelector)
            minuteSelector.SelectionChanged += OnTimeChanged;
        if (items.SecondSelector is { } secondSelector)
            secondSelector.SelectionChanged += OnTimeChanged;
        if (items.PeriodSelector is { } periodSelector)
            periodSelector.SelectionChanged += OnTimeChanged;

        WireSelectorButton(e, TemplateItems.DayUpButtonName, DateTimePickerPanelType.Day, SpinDirection.Decrease);
        WireSelectorButton(e, TemplateItems.DayDownButtonName, DateTimePickerPanelType.Day, SpinDirection.Increase);
        WireSelectorButton(e, TemplateItems.MonthUpButtonName, DateTimePickerPanelType.Month, SpinDirection.Decrease);
        WireSelectorButton(e, TemplateItems.MonthDownButtonName, DateTimePickerPanelType.Month, SpinDirection.Increase);
        WireSelectorButton(e, TemplateItems.YearUpButtonName, DateTimePickerPanelType.Year, SpinDirection.Decrease);
        WireSelectorButton(e, TemplateItems.YearDownButtonName, DateTimePickerPanelType.Year, SpinDirection.Increase);
        WireSelectorButton(e, TemplateItems.HourUpButtonName, DateTimePickerPanelType.Hour, SpinDirection.Decrease);
        WireSelectorButton(e, TemplateItems.HourDownButtonName, DateTimePickerPanelType.Hour, SpinDirection.Increase);
        WireSelectorButton(e, TemplateItems.MinuteUpButtonName, DateTimePickerPanelType.Minute, SpinDirection.Decrease);
        WireSelectorButton(e, TemplateItems.MinuteDownButtonName, DateTimePickerPanelType.Minute, SpinDirection.Increase);
        WireSelectorButton(e, TemplateItems.SecondUpButtonName, DateTimePickerPanelType.Second, SpinDirection.Decrease);
        WireSelectorButton(e, TemplateItems.SecondDownButtonName, DateTimePickerPanelType.Second, SpinDirection.Increase);
        WireSelectorButton(e, TemplateItems.PeriodUpButtonName, DateTimePickerPanelType.TimePeriod, SpinDirection.Decrease);
        WireSelectorButton(e, TemplateItems.PeriodDownButtonName, DateTimePickerPanelType.TimePeriod, SpinDirection.Increase);

        InitPicker();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == SelectedDateTimeProperty ||
            change.Property == DayVisibleProperty ||
            change.Property == MonthVisibleProperty ||
            change.Property == YearVisibleProperty ||
            change.Property == DayFormatProperty ||
            change.Property == MonthFormatProperty ||
            change.Property == YearFormatProperty ||
            change.Property == MinYearProperty ||
            change.Property == MaxYearProperty ||
            change.Property == MinuteIncrementProperty ||
            change.Property == SecondIncrementProperty ||
            change.Property == ClockIdentifierProperty ||
            change.Property == UseSecondsProperty)
        {
            InitPicker();
        }
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Escape:
                OnDismiss();
                e.Handled = true;
                break;
            case Key.Enter:
                OnConfirmed();
                e.Handled = true;
                break;
        }

        base.OnKeyDown(e);
    }

    protected override void OnConfirmed()
    {
        SetCurrentValue(SelectedDateTimeProperty, _syncDateTime);
        base.OnConfirmed();
    }

    private bool Is12HourClock => ClockIdentifier == "12HourClock";

    private void InitPicker()
    {
        if (_templateItems is not { } items)
            return;

        _suppressUpdateSelection = true;

        _syncDateTime = CoerceDateTime(SelectedDateTime);

        if (items.MonthSelector is { } monthSelector)
        {
            monthSelector.MaximumValue = 12;
            monthSelector.MinimumValue = 1;
            monthSelector.ItemFormat = MonthFormat;
        }

        if (items.DaySelector is { } daySelector)
            daySelector.ItemFormat = DayFormat;

        if (items.YearSelector is { } yearSelector)
        {
            yearSelector.MaximumValue = MaxYear.Year;
            yearSelector.MinimumValue = MinYear.Year;
            yearSelector.ItemFormat = YearFormat;
        }

        var clock12 = Is12HourClock;
        if (items.HourSelector is { } hourSelector)
        {
            hourSelector.MaximumValue = clock12 ? 12 : 23;
            hourSelector.MinimumValue = clock12 ? 1 : 0;
            hourSelector.ItemFormat = "%h";
        }

        if (items.MinuteSelector is { } minuteSelector)
        {
            minuteSelector.MaximumValue = 59;
            minuteSelector.MinimumValue = 0;
            minuteSelector.Increment = MinuteIncrement;
            minuteSelector.ItemFormat = "mm";
        }

        if (items.SecondSelector is { } secondSelector)
        {
            secondSelector.MaximumValue = 59;
            secondSelector.MinimumValue = 0;
            secondSelector.Increment = SecondIncrement;
            secondSelector.ItemFormat = "ss";
        }

        if (items.PeriodSelector is { } periodSelector)
        {
            periodSelector.MaximumValue = 1;
            periodSelector.MinimumValue = 0;
        }

        SetGrid(items);

        var dt = _syncDateTime;

        if (DayVisible && items.DaySelector is { } day)
        {
            day.MaximumValue = _calendar.GetDaysInMonth(dt.Year, dt.Month);
            day.MinimumValue = 1;
            day.SelectedValue = dt.Day;
        }

        if (MonthVisible && items.MonthSelector is { } month)
            month.SelectedValue = dt.Month;

        if (YearVisible && items.YearSelector is { } year)
            year.SelectedValue = dt.Year;

        if (items.HourSelector is { } hour)
        {
            var hr = dt.Hour;
            hour.SelectedValue = !clock12 ? hr : hr > 12 ? hr - 12 : hr == 0 ? 12 : hr;
        }

        if (items.MinuteSelector is { } minute)
            minute.SelectedValue = dt.Minute;

        if (items.SecondSelector is { } second)
            second.SelectedValue = dt.Second;

        if (items.PeriodSelector is { } period)
            period.SelectedValue = dt.Hour >= 12 ? 1 : 0;

        _suppressUpdateSelection = false;

        SetInitialFocus(items);
    }

    private DateTime CoerceDateTime(DateTime value) => value < MinYear.DateTime ? MinYear.DateTime : value > MaxYear.DateTime ? MaxYear.DateTime : value;

    private void SetGrid(TemplateItems items)
    {
        ResetGridChildrenColumns(items);

        var fmt = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;

        // Date parts ordered following the current culture short date pattern.
        var orderedDateParts = new List<(Panel? Host, int Index)>
        {
            (items.MonthHost, MonthVisible ? fmt.IndexOf('M', StringComparison.OrdinalIgnoreCase) : -1),
            (items.YearHost, YearVisible ? fmt.IndexOf('Y', StringComparison.OrdinalIgnoreCase) : -1),
            (items.DayHost, DayVisible ? fmt.IndexOf('D', StringComparison.OrdinalIgnoreCase) : -1)
        };
        orderedDateParts.Sort((x, y) => x.Index - y.Index);

        var use24HourClock = ClockIdentifier == "24HourClock";

        // Build the ordered list of visible hosts: date parts first, then time parts.
        var orderedDateHosts = new List<Panel>();
        foreach (var (host, index) in orderedDateParts)
        {
            if (host is null)
                continue;
            host.IsVisible = index != -1;
            if (index != -1)
                orderedDateHosts.Add(host);
        }

        var orderedTimeHosts = new List<Panel>();
        addTimeHost(items.HourHost, true);
        addTimeHost(items.MinuteHost, true);
        addTimeHost(items.SecondHost, UseSeconds);
        addTimeHost(items.PeriodHost, !use24HourClock);

        void addTimeHost(Panel? host, bool visible)
        {
            if (host is null)
                return;
            host.IsVisible = visible;
            if (visible)
                orderedTimeHosts.Add(host);
        }

        var orderedHosts = new List<Panel>(orderedDateHosts.Count + orderedTimeHosts.Count);
        orderedHosts.AddRange(orderedDateHosts);
        orderedHosts.AddRange(orderedTimeHosts);
        var datePartCount = orderedDateHosts.Count;

        // Build columns: star column per host, auto column for the spacer in-between.
        var columns = new ColumnDefinitions();
        var spacerIndex = 0;

        foreach (var spacer in _spacers)
            spacer.IsVisible = false;
        if (_dateTimeSeparator is not null)
            _dateTimeSeparator.IsVisible = false;

        for (var i = 0; i < orderedHosts.Count; i++)
        {
            if (i > 0)
            {
                columns.Add(new(0, GridUnitType.Auto));
                var useDateTimeSeparator = datePartCount > 0 && orderedTimeHosts.Count > 0 && i == datePartCount;
                if (useDateTimeSeparator && _dateTimeSeparator is not null)
                {
                    Grid.SetColumn(_dateTimeSeparator, columns.Count - 1);
                    _dateTimeSeparator.IsVisible = true;
                }
                else if (spacerIndex < _spacers.Count)
                {
                    var spacer = _spacers[spacerIndex++];
                    Grid.SetColumn(spacer, columns.Count - 1);
                    spacer.IsVisible = true;
                }
            }

            var isMonth = ReferenceEquals(orderedHosts[i], items.MonthHost);
            columns.Add(new(isMonth ? 2 : 1, GridUnitType.Star));
            Grid.SetColumn(orderedHosts[i], columns.Count - 1);
        }

        if (columns.Count == 0)
            columns.Add(new(GridLength.Star));

        items.PickerContainer.ColumnDefinitions = columns;
    }

    private void ResetGridChildrenColumns(TemplateItems items)
    {
        foreach (var host in new[]
                 {
                     items.DayHost, items.MonthHost, items.YearHost,
                     items.HourHost, items.MinuteHost, items.SecondHost, items.PeriodHost
                 })
        {
            if (host is not null)
                Grid.SetColumn(host, 0);
        }

        foreach (var spacer in _spacers)
            Grid.SetColumn(spacer, 0);

        if (_dateTimeSeparator is not null)
            Grid.SetColumn(_dateTimeSeparator, 0);
    }

    private void SetInitialFocus(TemplateItems items)
    {
        ReadOnlySpan<(bool visible, Panel? host, DateTimePickerPanel? selector)> candidates =
        [
            (MonthVisible, items.MonthHost, items.MonthSelector),
            (DayVisible, items.DayHost, items.DaySelector),
            (YearVisible, items.YearHost, items.YearSelector),
            (true, items.HourHost, items.HourSelector),
        ];

        DateTimePickerPanel? leftmost = null;
        var minCol = int.MaxValue;

        foreach (var (visible, host, selector) in candidates)
        {
            if (!visible || host is null || selector is null)
                continue;

            var col = Grid.GetColumn(host);
            if (col < minCol)
            {
                minCol = col;
                leftmost = selector;
            }
        }

        leftmost?.Focus(NavigationMethod.Pointer);
    }

    private void OnYearChanged(object? sender, EventArgs e)
    {
        if (_suppressUpdateSelection || _templateItems is not { } items || items.YearSelector is null)
            return;

        var year = items.YearSelector.SelectedValue;
        var maxDays = _calendar.GetDaysInMonth(year, _syncDateTime.Month);
        var day = _syncDateTime.Day > maxDays ? maxDays : _syncDateTime.Day;
        _syncDateTime = new(year, _syncDateTime.Month, day, _syncDateTime.Hour, _syncDateTime.Minute, _syncDateTime.Second);

        RefreshDaysIfNeeded(items, maxDays);
    }

    private void OnMonthChanged(object? sender, EventArgs e)
    {
        if (_suppressUpdateSelection || _templateItems is not { } items || items.MonthSelector is null)
            return;

        var month = items.MonthSelector.SelectedValue;
        var maxDays = _calendar.GetDaysInMonth(_syncDateTime.Year, month);
        var day = _syncDateTime.Day > maxDays ? maxDays : _syncDateTime.Day;
        _syncDateTime = new(_syncDateTime.Year, month, day, _syncDateTime.Hour, _syncDateTime.Minute, _syncDateTime.Second);

        RefreshDaysIfNeeded(items, maxDays);
    }

    private void OnDayChanged(object? sender, EventArgs e)
    {
        if (_suppressUpdateSelection || _templateItems is not { } items || items.DaySelector is null)
            return;

        _syncDateTime = new(_syncDateTime.Year, _syncDateTime.Month, items.DaySelector.SelectedValue, _syncDateTime.Hour, _syncDateTime.Minute, _syncDateTime.Second);
    }

    private void OnTimeChanged(object? sender, EventArgs e)
    {
        if (_suppressUpdateSelection || _templateItems is not { } items)
            return;

        var time = BuildTime(items);
        _syncDateTime = _syncDateTime.Date.Add(time);
    }

    private TimeSpan BuildTime(TemplateItems items)
    {
        var hr = items.HourSelector?.SelectedValue ?? _syncDateTime.Hour;
        var min = items.MinuteSelector?.SelectedValue ?? _syncDateTime.Minute;
        var sec = UseSeconds ? items.SecondSelector?.SelectedValue ?? 0 : 0;
        var per = items.PeriodSelector?.SelectedValue ?? (hr >= 12 ? 1 : 0);

        if (Is12HourClock)
            hr = per == 1 ? hr == 12 ? 12 : hr + 12 : per == 0 && hr == 12 ? 0 : hr;

        return new(hr, min, sec);
    }

    private void RefreshDaysIfNeeded(TemplateItems items, int maxDays)
    {
        if (!DayVisible || items.DaySelector is not { } daySelector)
            return;

        _suppressUpdateSelection = true;

        if (daySelector.MaximumValue != maxDays)
            daySelector.MaximumValue = maxDays;
        else
            daySelector.RefreshItems();

        _suppressUpdateSelection = false;
    }

    private void OnDismissButtonClicked(object? sender, RoutedEventArgs e) => OnDismiss();

    private void OnAcceptButtonClicked(object? sender, RoutedEventArgs e) => OnConfirmed();

    private void OnSelectorButtonClick(DateTimePickerPanelType type, SpinDirection direction)
    {
        var target = type switch
        {
            DateTimePickerPanelType.Day => _templateItems?.DaySelector,
            DateTimePickerPanelType.Month => _templateItems?.MonthSelector,
            DateTimePickerPanelType.Year => _templateItems?.YearSelector,
            DateTimePickerPanelType.Hour => _templateItems?.HourSelector,
            DateTimePickerPanelType.Minute => _templateItems?.MinuteSelector,
            DateTimePickerPanelType.Second => _templateItems?.SecondSelector,
            DateTimePickerPanelType.TimePeriod => _templateItems?.PeriodSelector,
            _ => null,
        };

        switch (direction)
        {
            case SpinDirection.Increase:
                target?.ScrollDown();
                break;
            case SpinDirection.Decrease:
                target?.ScrollUp();
                break;
        }
    }

    private void WireSelectorButton(TemplateAppliedEventArgs e, string name, DateTimePickerPanelType type, SpinDirection direction)
    {
        if (e.NameScope.Find<Button>(name) is { } button)
            button.Click += (_, _) => OnSelectorButtonClick(type, direction);
    }

    internal double GetOffsetForPopup()
    {
        if (_templateItems is not { } items)
            return 0;

        var acceptDismissButtonHeight = items.AcceptButton.Bounds.Height;
        var itemHeight = items.HourSelector?.ItemHeight ?? items.DaySelector?.ItemHeight ?? 0;
        return (-(MaxHeight - acceptDismissButtonHeight) / 2) - (itemHeight / 2);
    }
}
