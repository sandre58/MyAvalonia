// -----------------------------------------------------------------------
// <copyright file="DateTimeView.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Avalonia;
using Avalonia.Automation;
using Avalonia.Automation.Peers;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using MyNet.Avalonia.Controls.Primitives;
using MyNet.Primitives.Intervals;
using MyNet.Primitives.Temporal;
using MyNet.Utilities.Suspending;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

[TemplatePart(PartCalendar, typeof(Calendar))]
[TemplatePart(PartTimeView, typeof(TimeView))]
[TemplatePart(PartSectionSelector, typeof(TabControl))]
public class DateTimeView : TemplatedControl, IValueSelector<DateTime?>
{
    public const string PartCalendar = "PART_Calendar";
    public const string PartTimeView = "PART_TimeView";
    public const string PartSectionSelector = "PART_SectionSelector";

    private readonly Suspender _syncSuspender = new();

    private Calendar? _calendar;
    private TimeView? _timeView;
    private TabControl? _sectionSelector;
    private bool _suppressSectionSelectionChanged;

    public DateTimeViewSection ActiveSection { get; private set; } = DateTimeViewSection.Calendar;

    static DateTimeView()
    {
        FocusableProperty.OverrideDefaultValue<DateTimeView>(true);
        KeyboardNavigation.TabNavigationProperty.OverrideDefaultValue<DateTimeView>(KeyboardNavigationMode.Continue);
        AutomationProperties.ControlTypeOverrideProperty.OverrideDefaultValue<DateTimeView>(AutomationControlType.Custom);
        SelectedValueProperty.Changed.AddClassHandler<DateTimeView>((view, _) => view.UpdateAutomationName());
        KeyDownEvent.AddClassHandler<DateTimeView>((view, e) => view.OnDateTimeViewKeyDown(e), RoutingStrategies.Tunnel);
    }

    public DateTimeView()
    {
        SetCurrentValue(FirstDayOfWeekProperty, DateTimeHelper.GetCurrentDateTimeFormatInfo().FirstDayOfWeek);
        SetCurrentValue(DisplayDateProperty, DateTime.Today);
    }

    private void UpdateAutomationName()
    {
        var value = SelectedValue?.ToString(CultureInfo.CurrentCulture) ?? string.Empty;
        var section = ActiveSection == DateTimeViewSection.Calendar ? "Calendrier" : "Heure";
        AutomationProperties.SetName(this, string.IsNullOrEmpty(value) ? section : $"{value}, {section}");
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        RemoveHandlers();

        _calendar = e.NameScope.Find<Calendar>(PartCalendar);
        _timeView = e.NameScope.Find<TimeView>(PartTimeView);
        _sectionSelector = e.NameScope.Find<TabControl>(PartSectionSelector);

        AddHandlers();
        SyncSectionSelectorFromActiveSection();
        SyncPartsFromSelectedValue();
    }

    public void FocusSection(DateTimeViewSection section)
    {
        ActiveSection = section;
        SyncSectionSelectorFromActiveSection();

        switch (section)
        {
            case DateTimeViewSection.Calendar:
                _calendar?.FocusSelectedDay();
                break;

            case DateTimeViewSection.Time:
                _timeView?.FocusComponent(TimeComponent.Hour);
                break;
        }

        UpdateAutomationName();
    }

    private void SyncSectionSelectorFromActiveSection()
    {
        if (_sectionSelector is null)
            return;

        var index = ActiveSection == DateTimeViewSection.Calendar ? 0 : 1;
        if (_sectionSelector.SelectedIndex == index)
            return;

        _suppressSectionSelectionChanged = true;
        try
        {
            _sectionSelector.SelectedIndex = index;
        }
        finally
        {
            _suppressSectionSelectionChanged = false;
        }
    }

    internal Calendar? CalendarPart => _calendar;

    internal TimeView? TimeViewPart => _timeView;

    internal bool IsSourceInCalendarSection(object? source)
    {
        if (source is not Visual visual || _calendar is null)
            return false;

        return visual.FindAncestorOfType<Calendar>(includeSelf: true) == _calendar;
    }

    internal bool IsSourceInTimeSection(object? source)
    {
        if (source is not Visual visual || _timeView is null)
            return false;

        return visual.FindAncestorOfType<TimeView>(includeSelf: true) == _timeView;
    }

    protected override void OnGotFocus(FocusChangedEventArgs e)
    {
        base.OnGotFocus(e);

        if (ReferenceEquals(e.Source, this))
            FocusSection(DateTimeViewSection.Calendar);
        else
            UpdateActiveSectionFromFocus(e.Source);
    }

    private void OnDateTimeViewKeyDown(KeyEventArgs e)
    {
        if (e.Handled || !IsEnabled)
            return;

        switch (e.Key)
        {
            case Key.F6:
                FocusSection(ActiveSection == DateTimeViewSection.Calendar
                    ? DateTimeViewSection.Time
                    : DateTimeViewSection.Calendar);
                e.Handled = true;
                break;

            case Key.Right when (e.KeyModifiers & KeyModifiers.Control) == KeyModifiers.Control
                                && (e.KeyModifiers & KeyModifiers.Shift) == 0:
                FocusSection(DateTimeViewSection.Time);
                e.Handled = true;
                break;

            case Key.Left when (e.KeyModifiers & KeyModifiers.Control) == KeyModifiers.Control
                               && (e.KeyModifiers & KeyModifiers.Shift) == 0:
                FocusSection(DateTimeViewSection.Calendar);
                e.Handled = true;
                break;
        }
    }

    private void UpdateActiveSectionFromFocus(object? source)
    {
        if (source is not Visual visual)
            return;

        if (_timeView != null && visual.FindAncestorOfType<TimeView>(includeSelf: true) == _timeView)
            ActiveSection = DateTimeViewSection.Time;
        else if (_calendar != null && visual.FindAncestorOfType<Calendar>(includeSelf: true) == _calendar)
            ActiveSection = DateTimeViewSection.Calendar;

        SyncSectionSelectorFromActiveSection();
        UpdateAutomationName();
    }

    private void RemoveHandlers()
    {
        if (_calendar != null)
        {
            _calendar.DayButtonClick -= OnCalendarDayButtonClick;
            _calendar.PropertyChanged -= OnCalendarPropertyChanged;
        }

        if (_timeView != null)
            _timeView.SelectedValueChanged -= OnTimeViewSelectedValueChanged;

        if (_sectionSelector != null)
            _sectionSelector.SelectionChanged -= OnSectionSelectionChanged;
    }

    private void AddHandlers()
    {
        if (_calendar != null)
        {
            _calendar.SelectionMode = CalendarSelectionMode.SingleDate;
            _calendar.DayButtonClick += OnCalendarDayButtonClick;
            _calendar.PropertyChanged += OnCalendarPropertyChanged;
        }

        _timeView?.SelectedValueChanged += OnTimeViewSelectedValueChanged;

        if (_sectionSelector != null)
            _sectionSelector.SelectionChanged += OnSectionSelectionChanged;
    }

    private void OnSectionSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (_suppressSectionSelectionChanged || _sectionSelector is null)
            return;

        ActiveSection = _sectionSelector.SelectedIndex == 0
            ? DateTimeViewSection.Calendar
            : DateTimeViewSection.Time;

        UpdateAutomationName();
    }

    private void OnCalendarDayButtonClick(object? sender, RoutedEventArgs e)
    {
        UpdateSelectedValueFromParts();
        FocusSection(DateTimeViewSection.Time);
    }

    private void OnCalendarPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property == Calendar.SelectedDateProperty)
            UpdateSelectedValueFromParts();
    }

    private void OnTimeViewSelectedValueChanged(object? sender, SelectionChangedEventArgs e) => UpdateSelectedValueFromParts();

    private void UpdateSelectedValueFromParts()
    {
        if (_syncSuspender.IsSuspended) return;

        var date = _calendar?.SelectedDate?.Date ?? SelectedValue?.Date ?? DisplayDate.Date;
        var time = _timeView?.SelectedValue ?? SelectedValue?.TimeOfDay ?? TimeSpan.Zero;
        var merged = date + time;

        if (SelectedValue == merged)
            return;

        using (_syncSuspender.Suspend())
            SetCurrentValue(SelectedValueProperty, merged);
    }

    private void SyncPartsFromSelectedValue()
    {
        if (_syncSuspender.IsSuspended)
            return;

        using (_syncSuspender.Suspend())
        {
            if (_calendar != null)
            {
                var date = SelectedValue?.Date ?? DisplayDate.Date;
                if (_calendar.SelectedDate?.Date != date)
                {
                    _calendar.MoveToDate(date);
                    _calendar.SelectedDate = date;
                }
            }

            var time = SelectedValue?.TimeOfDay;
            if (_timeView is not null && _timeView.SelectedValue != time)
                _timeView.SelectedValue = time;
        }
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        if (change.Property == SelectedValueProperty)
        {
            var (oldValue, newValue) = change.GetOldAndNewValue<DateTime?>();
            OnValueSelected(oldValue, newValue);
            SyncPartsFromSelectedValue();
        }

        base.OnPropertyChanged(change);
    }

    internal bool IsValidSelection(DateTime dateTime) => _calendar?.IsValidSelection(dateTime.Date) == true;

    #region SelectedValue

    public event EventHandler<SelectionChangedEventArgs>? SelectedValueChanged;

    public static readonly StyledProperty<DateTime?> SelectedValueProperty =
        AvaloniaProperty.Register<DateTimeView, DateTime?>(nameof(SelectedValue), defaultBindingMode: BindingMode.TwoWay);

    public DateTime? SelectedValue
    {
        get => GetValue(SelectedValueProperty);
        set => SetValue(SelectedValueProperty, value);
    }

    private void OnValueSelected(DateTime? removedValue, DateTime? addedValue)
    {
        var handler = SelectedValueChanged;
        if (handler == null) return;

        var addedItems = new Collection<DateTime?>();
        var removedItems = new Collection<DateTime?>();

        if (addedValue.HasValue)
            addedItems.Add(addedValue);

        if (removedValue.HasValue)
            removedItems.Add(removedValue);

        handler(this, new(SelectingItemsControl.SelectionChangedEvent, removedItems, addedItems));
    }

    #endregion

    #region BlackoutDates

    public static readonly StyledProperty<AvaloniaList<Period>> BlackoutDatesProperty =
        AvaloniaProperty.Register<DateTimeView, AvaloniaList<Period>>(nameof(BlackoutDates));

    [SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "Used for binding")]
    public AvaloniaList<Period> BlackoutDates
    {
        get => GetValue(BlackoutDatesProperty);
        set => SetValue(BlackoutDatesProperty, value);
    }

    #endregion

    #region BlackoutDateRule

    public static readonly StyledProperty<IDateSelector?> BlackoutDateRuleProperty =
        AvaloniaProperty.Register<DateTimeView, IDateSelector?>(nameof(BlackoutDateRule));

    public IDateSelector? BlackoutDateRule
    {
        get => GetValue(BlackoutDateRuleProperty);
        set => SetValue(BlackoutDateRuleProperty, value);
    }

    #endregion

    #region FirstDayOfWeek

    public static readonly StyledProperty<DayOfWeek> FirstDayOfWeekProperty =
        AvaloniaProperty.Register<DateTimeView, DayOfWeek>(nameof(FirstDayOfWeek), CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek);

    public DayOfWeek FirstDayOfWeek
    {
        get => GetValue(FirstDayOfWeekProperty);
        set => SetValue(FirstDayOfWeekProperty, value);
    }

    #endregion

    #region IsTodayHighlighted

    public static readonly StyledProperty<bool> IsTodayHighlightedProperty =
        AvaloniaProperty.Register<DateTimeView, bool>(nameof(IsTodayHighlighted), true);

    public bool IsTodayHighlighted
    {
        get => GetValue(IsTodayHighlightedProperty);
        set => SetValue(IsTodayHighlightedProperty, value);
    }

    #endregion

    #region DisplayDate

    public static readonly StyledProperty<DateTime> DisplayDateProperty =
        AvaloniaProperty.Register<DateTimeView, DateTime>(nameof(DisplayDate), DateTime.Today);

    public DateTime DisplayDate
    {
        get => GetValue(DisplayDateProperty);
        set => SetValue(DisplayDateProperty, value);
    }

    #endregion

    #region DisplayDateStart

    public static readonly StyledProperty<DateTime?> DisplayDateStartProperty =
        AvaloniaProperty.Register<DateTimeView, DateTime?>(nameof(DisplayDateStart));

    public DateTime? DisplayDateStart
    {
        get => GetValue(DisplayDateStartProperty);
        set => SetValue(DisplayDateStartProperty, value);
    }

    #endregion

    #region DisplayDateEnd

    public static readonly StyledProperty<DateTime?> DisplayDateEndProperty =
        AvaloniaProperty.Register<DateTimeView, DateTime?>(nameof(DisplayDateEnd));

    public DateTime? DisplayDateEnd
    {
        get => GetValue(DisplayDateEndProperty);
        set => SetValue(DisplayDateEndProperty, value);
    }

    #endregion

    #region NumberFormat

    public static readonly StyledProperty<string> NumberFormatProperty = TimeView.NumberFormatProperty.AddOwner<DateTimeView>();

    public string NumberFormat
    {
        get => GetValue(NumberFormatProperty);
        set => SetValue(NumberFormatProperty, value);
    }

    #endregion

    #region ShowSeconds

    public static readonly StyledProperty<bool> ShowSecondsProperty = TimeSelectorBase.ShowSecondsProperty.AddOwner<DateTimeView>();

    public bool ShowSeconds
    {
        get => GetValue(ShowSecondsProperty);
        set => SetValue(ShowSecondsProperty, value);
    }

    #endregion

    #region TimeFormat

    public static readonly StyledProperty<TimeFormat> TimeFormatProperty = TimeSelectorBase.TimeFormatProperty.AddOwner<DateTimeView>();

    public TimeFormat TimeFormat
    {
        get => GetValue(TimeFormatProperty);
        set => SetValue(TimeFormatProperty, value);
    }

    #endregion

    public bool IsEmpty() => !SelectedValue.HasValue;

    public void Clear() => SetCurrentValue(SelectedValueProperty, null);
}
