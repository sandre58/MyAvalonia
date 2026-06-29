// -----------------------------------------------------------------------
// <copyright file="Calendar.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using Avalonia;
using Avalonia.Automation;
using Avalonia.Automation.Peers;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using MyNet.Avalonia.Controls.Internals;
using MyNet.Avalonia.Controls.Primitives;
using MyNet.Collections;
using MyNet.Globalization.Facade;
using MyNet.Primitives;
using MyNet.Primitives.Intervals;
using MyNet.Primitives.Temporal;
using MyNet.Utilities.Suspending;
using CalendarBlackoutDatesCollection = MyNet.Avalonia.Controls.Primitives.CalendarBlackoutDatesCollection;
using CalendarDateChangedEventArgs = MyNet.Avalonia.Controls.Primitives.CalendarDateChangedEventArgs;
using CalendarDayButton = MyNet.Avalonia.Controls.Primitives.CalendarDayButton;
using SelectedDatesCollection = MyNet.Avalonia.Controls.Primitives.SelectedDatesCollection;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

[TemplatePart(PartFastNextButton, typeof(Button))]
[TemplatePart(PartFastPreviousButton, typeof(Button))]
[TemplatePart(PartNextButton, typeof(Button))]
[TemplatePart(PartPreviousButton, typeof(Button))]
[TemplatePart(PartYearButton, typeof(Button))]
[TemplatePart(PartMonthButton, typeof(Button))]
[TemplatePart(PartHeaderButton, typeof(Button))]
[TemplatePart(PartMonthGrid, typeof(Grid))]
[TemplatePart(PartYearGrid, typeof(Grid))]
[PseudoClasses(PseudoClassName.Month, PseudoClassName.Year, PseudoClassName.Decade, PseudoClassName.Century)]
public class Calendar : TemplatedControl
{
    public const string PartFastNextButton = "PART_FastNextButton";
    public const string PartFastPreviousButton = "PART_FastPreviousButton";
    public const string PartNextButton = "PART_NextButton";
    public const string PartPreviousButton = "PART_PreviousButton";
    public const string PartYearButton = "PART_YearButton";
    public const string PartMonthButton = "PART_MonthButton";
    public const string PartHeaderButton = "PART_HeaderButton";
    public const string PartMonthGrid = "PART_MonthGrid";
    public const string PartYearGrid = "PART_YearGrid";

    private const int NumberOfColumnInYearGrid = CalendarKeyboardNavigationHelper.YearGridColumns;

    private readonly Suspender _changeDisplayDate = new();
    private readonly Dictionary<DateTime, CalendarDateButton> _cells = [];
    private readonly CalendarSelectionCoordinator _selectionCoordinator;

    private Button? _fastNextButton;
    private Button? _fastPreviousButton;
    private Button? _headerButton;
    private Button? _monthButton;
    private Button? _nextButton;
    private Button? _previousButton;
    private Button? _yearButton;
    private Grid? _monthGrid;
    private Grid? _yearGrid;

    private DateTime? _lastSelectedDate;
    private KeyModifiers _lastKeyModifiers;
    private DateTime? _previewEndDate;
    private DateTime? _pointerOverDate;
    private DateTime? _cachedPreviewAnchor;
    private DateTime? _cachedPreviewEnd;
    private readonly HashSet<DateTime> _previewHighlightDates = [];
    private readonly HashSet<DateTime> _committedHighlightDates = [];
    private bool _isPointerSelecting;
    private bool _intervalPreviewActive;
    private PreviewController _previewController;
    private KeyModifiers _pointerPressModifiers;

    private enum PreviewController
    {
        None,
        Keyboard,
        PointerShift,
        Drag
    }

    static Calendar()
    {
        FocusableProperty.OverrideDefaultValue<Calendar>(true);
        AutomationProperties.ControlTypeOverrideProperty.OverrideDefaultValue<Calendar>(AutomationControlType.Calendar);
        _ = SelectedDateProperty.Changed.AddClassHandler<Calendar>((calendar, _) => calendar.UpdateAutomationName());
        _ = DisplayDateProperty.Changed.AddClassHandler<Calendar>((calendar, _) => calendar.UpdateAutomationName());
        FirstDayOfWeekProperty.Changed.AddClassHandler<Calendar>((x, e) => x.OnFirstDayOfWeekChanged(e));
        IsTodayHighlightedProperty.Changed.AddClassHandler<Calendar>((x, e) => x.OnIsTodayHighlightedChanged(e));
        DisplayDateContextProperty.Changed.AddClassHandler<Calendar>((x, e) => x.OnDisplayDateContextPropertyChanged(e));
        SelectionModeProperty.Changed.AddClassHandler<Calendar>((x, e) => x.OnSelectionModeChanged(e));
        SelectedDateProperty.Changed.AddClassHandler<Calendar>((x, e) => x.OnSelectedDateChanged(e));
        DisplayDateProperty.Changed.AddClassHandler<Calendar>((x, e) => x.OnDisplayDateChanged(e));
        DisplayDateStartProperty.Changed.AddClassHandler<Calendar>((x, e) => x.OnDisplayDateStartChanged(e));
        DisplayDateEndProperty.Changed.AddClassHandler<Calendar>((x, e) => x.OnDisplayDateEndChanged(e));
    }

    public Calendar()
    {
        AddHandler(KeyDownEvent, OnCalendarKeyDownHandler, RoutingStrategies.Tunnel, handledEventsToo: true);
        AddHandler(KeyUpEvent, OnCalendarKeyUpHandler, RoutingStrategies.Tunnel, handledEventsToo: true);
        AddHandler(PointerExitedEvent, OnCalendarPointerExited, RoutingStrategies.Direct | RoutingStrategies.Tunnel, handledEventsToo: true);
        DisplayDateContext = new MonthContext(DateTime.Today.Month, DateTime.Today.Year);
        SetCurrentValue(DisplayDateProperty, DateTime.Today);
        UpdateDisplayDate(DisplayDate, DateTime.MinValue);
        BlackoutDates = new(this);
        SelectedDates = new(this);
        GlobalizationServices.Current.CultureChanged += (_, _) => Refresh();
        SelectedDates.CollectionChanged += OnSelectedDatesCollectionChanged;
        BlackoutDates.CollectionChanged += OnBlackoutDatesCollectionChanged;
        _selectionCoordinator = new(
            () => SelectionMode,
            () => AllowTapRangeSelection,
            () => DisplayDate,
            IsValidSelection,
            new SelectionCommands(this));

        UpdateAutomationName();
    }

    private void UpdateAutomationName()
    {
        var name = SelectedDate?.ToString(CultureInfo.CurrentCulture) ?? DisplayDate.ToString(CultureInfo.CurrentCulture);
        AutomationProperties.SetName(this, name);
    }

    private sealed class SelectionCommands(Calendar owner) : ICalendarSelectionCommands
    {
        public void SetSelection(DateTime date) => owner.SetSelection(date);

        public void SetSelection(DateTime start, DateTime end) => owner.SetSelection(start, end);

        public void AddSelection(DateTime date) => owner.AddSelection(date);

        public void AddSelection(DateTime start, DateTime end) => owner.AddSelection(start, end);

        public void ToggleSelection(DateTime date) => owner.ToggleSelection(date);

        public void ChangeSelection(DateTime start, DateTime end, bool isSelected) => owner.ChangeSelection(start, end, isSelected);

        public bool Contains(DateTime date) => owner.SelectedDates.Contains(date.DiscardTime());

        public void MoveToDate(DateTime date) => owner.MoveToDate(date);
    }

    internal event EventHandler<RoutedEventArgs>? DayButtonClick;

    #region IsTodayHighlighted

    public static readonly StyledProperty<bool> IsTodayHighlightedProperty = AvaloniaProperty.Register<Calendar, bool>(nameof(IsTodayHighlighted), defaultValue: true);

    public bool IsTodayHighlighted
    {
        get => GetValue(IsTodayHighlightedProperty);
        set => SetValue(IsTodayHighlightedProperty, value);
    }

    [SuppressMessage("Roslynator", "RCS1163:Unused parameter", Justification = "Used by handler")]
    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Used by handler")]
    private void OnIsTodayHighlightedChanged(AvaloniaPropertyChangedEventArgs e)
    {
        if (_cells.GetValueOrDefault(DateTime.Today) is not CalendarDayButton cell) return;

        cell.IsToday = e.NewValue is true;
    }

    #endregion

    #region FirstDayOfWeek

    public static readonly StyledProperty<DayOfWeek> FirstDayOfWeekProperty = AvaloniaProperty.Register<Calendar, DayOfWeek>(nameof(FirstDayOfWeek), defaultValue: DateTimeHelper.GetCurrentDateTimeFormatInfo().FirstDayOfWeek);

    public DayOfWeek FirstDayOfWeek
    {
        get => GetValue(FirstDayOfWeekProperty);
        set => SetValue(FirstDayOfWeekProperty, value);
    }

    private static bool IsValidFirstDayOfWeek(object value) => CalendarValidationHelper.IsValidFirstDayOfWeek((DayOfWeek)value);

    private void OnFirstDayOfWeekChanged(AvaloniaPropertyChangedEventArgs e)
    {
        if (IsValidFirstDayOfWeek(e.NewValue!))
            Refresh();
        else
            throw new ArgumentOutOfRangeException(nameof(e));
    }

    #endregion

    #region DayTitleTemplate

    public static readonly StyledProperty<ITemplate<Control>?> DayTitleTemplateProperty = AvaloniaProperty.Register<Calendar, ITemplate<Control>?>(nameof(DayTitleTemplate), defaultBindingMode: BindingMode.OneTime);

    public ITemplate<Control>? DayTitleTemplate
    {
        get => GetValue(DayTitleTemplateProperty);
        set => SetValue(DayTitleTemplateProperty, value);
    }

    #endregion

    #region SelectionMode

    public static readonly StyledProperty<CalendarSelectionMode> SelectionModeProperty = AvaloniaProperty.Register<Calendar, CalendarSelectionMode>(nameof(SelectionMode), defaultValue: CalendarSelectionMode.SingleDate);

    public CalendarSelectionMode SelectionMode
    {
        get => GetValue(SelectionModeProperty);
        set => SetValue(SelectionModeProperty, value);
    }

    private static bool IsValidSelectionMode(object value) => CalendarValidationHelper.IsValidSelectionMode((CalendarSelectionMode)value);

    private void OnSelectionModeChanged(AvaloniaPropertyChangedEventArgs e)
    {
        if (IsValidSelectionMode(e.NewValue!))
        {
            ClearRangePreview();
            ClearSelection();
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(e));
        }
    }

    #endregion

    #region AllowTapRangeSelection

    public static readonly StyledProperty<bool> AllowTapRangeSelectionProperty = AvaloniaProperty.Register<Calendar, bool>(nameof(AllowTapRangeSelection), defaultValue: true);

    public bool AllowTapRangeSelection
    {
        get => GetValue(AllowTapRangeSelectionProperty);
        set => SetValue(AllowTapRangeSelectionProperty, value);
    }

    #endregion

    #region SelectedDate

    public static readonly RoutedEvent<CalendarDateButtonEventArgs> DateSelectedEvent = RoutedEvent.Register<Calendar, CalendarDateButtonEventArgs>(nameof(DateSelected), RoutingStrategies.Bubble);

    public event EventHandler<CalendarDateButtonEventArgs> DateSelected
    {
        add => AddHandler(DateSelectedEvent, value);
        remove => RemoveHandler(DateSelectedEvent, value);
    }

    public static readonly StyledProperty<DateTime?> SelectedDateProperty = AvaloniaProperty.Register<Calendar, DateTime?>(nameof(SelectedDate), defaultBindingMode: BindingMode.TwoWay);

    public DateTime? SelectedDate
    {
        get => GetValue(SelectedDateProperty);
        set => SetValue(SelectedDateProperty, value);
    }

    private bool ValidateDate(DateTime? value)
    {
        if (!value.HasValue)
            return true;

        if (BlackoutDates.Contains(value.Value))
            return false;

        using (_changeDisplayDate.Suspend())
        {
            if (value.Value.IsBefore(GetDisplayDateRangeStart()))
                DisplayDateStart = value;
            else if (value.Value.IsAfter(GetDisplayDateRangeEnd()))
                DisplayDateEnd = value;
        }

        return true;
    }

    internal bool IsValidSelection(DateTime date) => CalendarValidationHelper.IsValidSelection(date, GetDisplayDateRangeStart(), GetDisplayDateRangeEnd(), BlackoutDates.Contains);

    private void OnSelectedDateChanged(AvaloniaPropertyChangedEventArgs e)
    {
        if (_changeDisplayDate.IsSuspended) return;

        if (SelectionMode != CalendarSelectionMode.None)
        {
            var addedDate = (DateTime?)e.NewValue;

            if (ValidateDate(addedDate))
            {
                if (addedDate is null)
                {
                    ClearSelection();
                }
                else if (SelectionMode == CalendarSelectionMode.MultipleRange)
                {
                    MoveToDate(addedDate.Value);
                }
                else if (!(SelectedDates.Count > 0 && SelectedDates[0] == addedDate.Value.DiscardTime()))
                {
                    SetSelection(addedDate.Value);
                    MoveToDate(addedDate.Value);
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(e));
            }
        }
        else
        {
            throw new InvalidOperationException("The SelectedDate property cannot be set when the selection mode is None.");
        }
    }

    #endregion

    #region SelectedDates

    public event EventHandler<SelectionChangedEventArgs>? SelectedDatesChanged;

    public SelectedDatesCollection SelectedDates { get; }

    private void OnSelectedDatesCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        var oldItems = e.OldItems?.OfType<DateTime>().ToList() ?? [];
        var newItems = e.NewItems?.OfType<DateTime>().ToList() ?? [];

        if (e.Action == NotifyCollectionChangedAction.Reset)
        {
            if (IsRangeSelectionMode())
            {
                UpdateRangeHighlights();
            }
            else
            {
                foreach (var cell in _cells.Values.OfType<CalendarDayButton>())
                {
                    if (cell.DateContext?.ToDate() is { } date)
                        ChangeSelectedState(date, SelectedDates.Contains(date.DiscardTime()));
                }
            }
        }
        else
        {
            foreach (var date in oldItems)
                ChangeSelectedState(date, false);

            foreach (var date in newItems)
                ChangeSelectedState(date, true);

            UpdateRangeHighlights();
        }

        SelectedDatesChanged?.Invoke(this, new(SelectingItemsControl.SelectionChangedEvent, oldItems, newItems) { Source = this });
    }

    internal void SyncSelectedDateFromCollection()
    {
        using (_changeDisplayDate.Suspend())
        {
            if (SelectedDates.Count == 0)
            {
                if (SelectionMode != CalendarSelectionMode.None && SelectedDate != null)
                    SelectedDate = null;

                return;
            }

            var first = SelectedDates[0];
            if (!SelectedDate.HasValue || SelectedDate.Value != first)
                SelectedDate = first;
        }
    }

    internal void SyncSelectedDateAfterInsertAt(int index, DateTime date)
    {
        if (index != 0)
            return;

        using (_changeDisplayDate.Suspend())
        {
            if (!SelectedDate.HasValue || SelectedDate.Value != date)
                SelectedDate = date;
        }
    }

    internal void SyncSelectedDateAfterRemovalAt(int removedIndex)
    {
        if (removedIndex != 0)
            return;

        using (_changeDisplayDate.Suspend())
        {
            SelectedDate = SelectedDates.Count > 0 ? SelectedDates[0] : null;
        }
    }

    private void ChangeSelectedState(DateTime date, bool value)
    {
        if (_cells.GetOrDefault(date) is CalendarDayButton cell && cell.IsSelected != value)
            cell.IsSelected = value;
    }

    #endregion

    #region BlackoutDates

    public CalendarBlackoutDatesCollection BlackoutDates { get; }

    private void OnBlackoutDatesCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        foreach (var item in e.OldItems ?? Array.Empty<object>())
        {
            if (item is Period range)
                ChangeBlackoutState(range, false);
        }

        foreach (var item in e.NewItems ?? Array.Empty<object>())
        {
            if (item is Period range)
                ChangeBlackoutState(range, true);
        }
    }

    private void ChangeBlackoutState(Period period, bool value)
    {
        foreach (var date in period.EnumerateDays())
        {
            if (_cells.GetOrDefault(date) is CalendarDayButton cell && cell.IsBlackout != value)
                cell.IsBlackout = value;
        }
    }

    #endregion

    #region DisplayDateContext

    public static readonly StyledProperty<DateContext> DisplayDateContextProperty = AvaloniaProperty.Register<Calendar, DateContext>(nameof(DisplayDateContext), coerce: CoerceDisplayDateContext);

    public DateContext DisplayDateContext
    {
        get => GetValue(DisplayDateContextProperty);
        private set => SetValue(DisplayDateContextProperty, value);
    }

    private static DateContext CoerceDisplayDateContext(AvaloniaObject sender, DateContext value) => CalendarDisplayContextHelper.CoerceDisplayDateContext(value);

    private void OnDisplayDateContextPropertyChanged(AvaloniaPropertyChangedEventArgs e)
    {
        if (e.NewValue?.Equals(e.OldValue) == true) return;

        Refresh();

        UpdateFocus();
    }

    #endregion

    #region DisplayDate

    public event EventHandler<CalendarDateChangedEventArgs>? DisplayDateChanged;

    public static readonly StyledProperty<DateTime> DisplayDateProperty = AvaloniaProperty.Register<Calendar, DateTime>(nameof(DisplayDate), defaultBindingMode: BindingMode.TwoWay);

    public DateTime DisplayDate
    {
        get => GetValue(DisplayDateProperty);
        set => SetValue(DisplayDateProperty, value);
    }

    public MonthContext CurrentMonthContext => new(DisplayDate.Month, DisplayDate.Year);

    private void UpdateDisplayDate(DateTime addedDate, DateTime removedDate)
    {
        var rangeStart = GetDisplayDateRangeStart();
        var rangeEnd = GetDisplayDateRangeEnd();
        var clampedDate = CalendarDateRangeHelper.ClampToRange(addedDate, rangeStart, rangeEnd);
        if (clampedDate != addedDate)
        {
            DisplayDate = clampedDate;
            return;
        }

        DisplayDateContext = DisplayDateContext.FromDate(addedDate);

        OnDisplayDate(new(removedDate, addedDate));
    }

    private void OnDisplayDateChanged(AvaloniaPropertyChangedEventArgs e) => UpdateDisplayDate((DateTime)e.NewValue!, (DateTime)e.OldValue!);

    private void OnDisplayDate(CalendarDateChangedEventArgs e) => DisplayDateChanged?.Invoke(this, e);

    private DateTime GetFocusedDate()
    {
        if (GetFocusedDayButton()?.DataContext is DateTime focusedDate)
            return focusedDate;

        return CalendarDisplayContextHelper.GetFocusedDate(_lastSelectedDate, DisplayDateContext, DateTime.Today);
    }

    #endregion

    #region DisplayDateStart

    public static readonly StyledProperty<DateTime?> DisplayDateStartProperty = AvaloniaProperty.Register<Calendar, DateTime?>(nameof(DisplayDateStart), defaultBindingMode: BindingMode.TwoWay);

    public DateTime? DisplayDateStart
    {
        get => GetValue(DisplayDateStartProperty);
        set => SetValue(DisplayDateStartProperty, value);
    }

    private DateTime GetDisplayDateRangeStart() => CalendarDateRangeHelper.GetRangeStart(DisplayDateStart);

    private void OnDisplayDateStartChanged(AvaloniaPropertyChangedEventArgs e)
    {
        if (_changeDisplayDate.IsSuspended) return;

        if (e.NewValue is DateTime newValue)
        {
            var adjustment = CalendarDateRangeHelper.ResolveDisplayDateStartChange(
                newValue,
                GetDisplayDateRangeEnd(),
                DisplayDate,
                CalendarDateRangeHelper.GetSelectedMin(SelectedDates),
                DisplayDateStart);

            if (adjustment is { } resolved)
            {
                ApplyDisplayDateRangeAdjustment(resolved);
                return;
            }
        }

        Refresh();
    }

    private void ApplyDisplayDateRangeAdjustment(DisplayDateRangeAdjustment adjustment)
    {
        if (adjustment.DisplayDateStart is { } displayDateStart)
            SetCurrentValue(DisplayDateStartProperty, displayDateStart);

        if (adjustment.DisplayDateEnd is { } displayDateEnd)
            SetCurrentValue(DisplayDateEndProperty, displayDateEnd);

        if (adjustment.DisplayDate is { } displayDate)
            SetCurrentValue(DisplayDateProperty, displayDate);

        if (adjustment.RequiresRefresh)
            Refresh();
    }

    #endregion

    #region DisplayDateEnd

    public static readonly StyledProperty<DateTime?> DisplayDateEndProperty = AvaloniaProperty.Register<Calendar, DateTime?>(nameof(DisplayDateEnd), defaultBindingMode: BindingMode.TwoWay);

    public DateTime? DisplayDateEnd
    {
        get => GetValue(DisplayDateEndProperty);
        set => SetValue(DisplayDateEndProperty, value);
    }

    private DateTime GetDisplayDateRangeEnd() => CalendarDateRangeHelper.GetRangeEnd(DisplayDateEnd);

    private void OnDisplayDateEndChanged(AvaloniaPropertyChangedEventArgs e)
    {
        if (_changeDisplayDate.IsSuspended) return;

        if (e.NewValue is DateTime newValue)
        {
            var adjustment = CalendarDateRangeHelper.ResolveDisplayDateEndChange(
                newValue,
                GetDisplayDateRangeStart(),
                DisplayDate,
                CalendarDateRangeHelper.GetSelectedMax(SelectedDates));

            if (adjustment is { } resolved)
            {
                ApplyDisplayDateRangeAdjustment(resolved);
                return;
            }
        }

        Refresh();
    }

    #endregion

    #region Build

    private void InitializeGridButtons()
    {
        _monthGrid?.Children.Clear();
        _yearGrid?.Children.Clear();

        // Generate Day titles (Sun, Mon, Tue, Wed, Thu, Fri, Sat) based on FirstDayOfWeek and culture.
        const int dayTitleRow = 0;
        const int firstDayRow = 1;
        var weekRowCount = DateTimeHelper.MaxNumberOfWeeksPerMonth;
        var capacity = DateTimeHelper.DaysPerWeek + (DateTimeHelper.DaysPerWeek * weekRowCount);
        var children = new List<Control>(capacity);
        for (var i = 0; i < DateTimeHelper.DaysPerWeek; i++)
        {
            if (DayTitleTemplate?.Build() is { } cell)
            {
                cell.DataContext = string.Empty;
                _ = cell.SetValue(Grid.RowProperty, dayTitleRow);
                _ = cell.SetValue(Grid.ColumnProperty, i);
                children.Add(cell);
            }
        }

        // Generate date buttons.
        for (var i = firstDayRow; i < weekRowCount + firstDayRow; i++)
        {
            for (var j = 0; j < DateTimeHelper.DaysPerWeek; j++)
            {
                var cell = new CalendarDayButton { Owner = this };
                _ = cell.SetValue(Grid.RowProperty, i);
                _ = cell.SetValue(Grid.ColumnProperty, j);
                cell.AddHandler(PointerReleasedEvent, OnDayPointerReleased, handledEventsToo: true);
                cell.AddHandler(PointerPressedEvent, OnDayPointerPressed, handledEventsToo: true);
                cell.AddHandler(PointerEnteredEvent, OnDayPointerEnter, handledEventsToo: true);
                cell.AddHandler(PointerExitedEvent, OnDayPointerExited, handledEventsToo: true);
                cell.AddHandler(PointerMovedEvent, OnDayPointerMove, handledEventsToo: true);

                children.Add(cell);
            }
        }

        _monthGrid?.Children.AddRange(children);
        _monthGrid?.AddHandler(PointerExitedEvent, OnMonthGridPointerLeave, handledEventsToo: true);
        _monthGrid?.AddHandler(PointerMovedEvent, OnMonthGridPointerMove, handledEventsToo: true);

        // Generate month/year buttons.
        for (var i = 0; i < 12; i++)
        {
            var cell = new CalendarYearButton { Owner = this };
            Grid.SetRow(cell, i / NumberOfColumnInYearGrid);
            Grid.SetColumn(cell, i % NumberOfColumnInYearGrid);
            cell.AddHandler(Button.ClickEvent, OnCalendarYearButtonClick);

            _yearGrid?.Children.Add(cell);
        }
    }

    private void Refresh()
    {
        var (month, year, decade, century) = CalendarDisplayModeHelper.GetViewPseudoClasses(DisplayDateContext);
        PseudoClasses.Set(PseudoClassName.Month, month);
        PseudoClasses.Set(PseudoClassName.Year, year);
        PseudoClasses.Set(PseudoClassName.Decade, decade);
        PseudoClasses.Set(PseudoClassName.Century, century);

        switch (DisplayDateContext)
        {
            case MonthContext:
                UpdateMonths();
                break;
            case YearContext:
            case DecadeContext:
            case CenturyContext:
                UpdateYears();
                break;
        }
    }

    private void SetDayTitles()
    {
        if (_monthGrid is null) return;

        for (var childIndex = 0; childIndex < DateTimeHelper.DaysPerWeek; childIndex++)
        {
            var daytitle = _monthGrid.Children[childIndex];
            daytitle.DataContext = DateTimeHelper.GetCurrentDateTimeFormatInfo().ShortestDayNames[
                CalendarMonthGridHelper.GetDayTitleColumnIndex(childIndex, FirstDayOfWeek)];
        }
    }

    private void UpdateMonths()
    {
        if (_monthGrid is null || DisplayDateContext is not MonthContext monthContext) return;

        _cells.Clear();
        _committedHighlightDates.Clear();
        _previewHighlightDates.Clear();

        var children = _monthGrid.Children;
        var dayCellCount = children.Count - DateTimeHelper.DaysPerWeek;
        var cellIndex = DateTimeHelper.DaysPerWeek;

        for (var i = DateTimeHelper.DaysPerWeek; i < children.Count; i++)
        {
            if (children[i] is CalendarDayButton dayCell)
                CalendarDayRangeStateHelper.ClearRangeState(dayCell);
        }

        foreach (var state in CalendarMonthGridHelper.EnumerateDayCells(monthContext, FirstDayOfWeek, dayCellCount))
        {
            if (children[cellIndex] is not CalendarDayButton cell)
            {
                cellIndex++;
                continue;
            }

            cell.Index = cellIndex;
            cell.SetContext(state.DateContext);
            cell.IsInactive = state.IsInactive;
            cell.IsSelected = SelectedDates.Contains(state.Date);
            cell.IsBlackout = BlackoutDates.Contains(state.Date);

            _cells.Add(state.Date, cell);
            cellIndex++;
        }

        SetDayTitles();
        UpdateRangeHighlights();
    }

    private static bool IsRangeSelectionMode(CalendarSelectionMode mode) =>
        mode is CalendarSelectionMode.SingleRange or CalendarSelectionMode.MultipleRange;

    private bool IsRangeSelectionMode() => IsRangeSelectionMode(SelectionMode);

    private void UpdateRangeHighlights()
    {
        UpdateCommittedRangeHighlights();
        UpdatePreviewRangeHighlightsOnly();
        SyncSelectedStateWithRangeRoles();
    }

    private void UpdateCommittedRangeHighlights()
    {
        if (!IsRangeSelectionMode())
        {
            ClearAllCommittedHighlightDates();
            return;
        }

        HashSet<DateTime> newDates = [];
        Dictionary<DateTime, (DateTime Start, DateTime End)> dateSegments = [];

        foreach (var (start, end) in CalendarDayRangeStateHelper.EnumerateConsecutiveRanges(SelectedDates))
        {
            foreach (var date in SelectedDatesHelper.EnumerateDateRange(start, end))
            {
                var normalized = date.DiscardTime();
                newDates.Add(normalized);
                dateSegments[normalized] = (start, end);
            }
        }

        foreach (var date in _committedHighlightDates.ToArray())
        {
            if (newDates.Contains(date))
                continue;

            if (_cells.GetOrDefault(date) is CalendarDayButton cell)
            {
                CalendarDayRangeStateHelper.ClearCommittedRangeState(cell);
                SyncCellSelectedState(cell, date);
            }

            _committedHighlightDates.Remove(date);
        }

        foreach (var date in newDates)
        {
            if (_cells.GetOrDefault(date) is not CalendarDayButton cell)
                continue;

            var (start, end) = dateSegments[date];
            if (!CalendarDayRangeStateHelper.CellMatchesCommittedInterval(cell, date, start, end))
                CalendarDayRangeStateHelper.ApplyRangeSegmentToCell(cell, date, start, end, isPreview: false);

            _committedHighlightDates.Add(date);
        }
    }

    private void ClearAllCommittedHighlightDates()
    {
        foreach (var date in _committedHighlightDates.ToArray())
        {
            if (_cells.GetOrDefault(date) is CalendarDayButton cell)
            {
                CalendarDayRangeStateHelper.ClearCommittedRangeState(cell);
                SyncCellSelectedState(cell, date);
            }
        }

        _committedHighlightDates.Clear();
    }

    private void SyncCellSelectedState(CalendarDayButton cell, DateTime date)
    {
        var shouldSelect = SelectedDates.Contains(date.DiscardTime());
        if (cell.IsSelected != shouldSelect)
            cell.IsSelected = shouldSelect;
    }

    private void InvalidatePreviewHighlightCache()
    {
        _cachedPreviewAnchor = null;
        _cachedPreviewEnd = null;
    }

    private void SyncSelectedStateWithRangeRoles()
    {
        if (!IsRangeSelectionMode())
            return;

        foreach (var date in _committedHighlightDates)
        {
            if (_cells.GetOrDefault(date) is not CalendarDayButton cell)
                continue;

            if (cell.IsSelected)
                cell.IsSelected = false;
        }

        foreach (var date in SelectedDates)
        {
            if (_cells.GetOrDefault(date) is not CalendarDayButton cell)
                continue;

            if (cell.IsStartDate || cell.IsEndDate || cell.IsInRange)
            {
                if (cell.IsSelected)
                    cell.IsSelected = false;
                continue;
            }

            SyncCellSelectedState(cell, date);
        }
    }

    private void UpdatePreviewRangeHighlightsOnly()
    {
        if (!TryGetPreviewInterval(out var anchor, out var end))
        {
            if (AllowTapRangeSelection
                && _selectionCoordinator.HasPendingRangeAnchor
                && (_previewEndDate is not null || _pointerOverDate is not null)
                && _previewController != PreviewController.Keyboard)
            {
                return;
            }

            if (_previewHighlightDates.Count == 0)
            {
                InvalidatePreviewHighlightCache();
                return;
            }

            foreach (var date in _previewHighlightDates.ToArray())
            {
                if (_cells.GetOrDefault(date) is CalendarDayButton cell)
                    CalendarDayRangeStateHelper.ClearPreviewRangeState(cell);
            }

            _previewHighlightDates.Clear();
            InvalidatePreviewHighlightCache();
            UpdateCommittedRangeHighlights();
            return;
        }

        anchor = anchor.DiscardTime();
        end = end.DiscardTime();

        if (_cachedPreviewAnchor == anchor && _cachedPreviewEnd == end)
        {
            ReconcileCommittedCapsDuringPreview();
            return;
        }

        HashSet<DateTime>? newDates = null;
        foreach (var date in SelectedDatesHelper.EnumerateDateRange(anchor, end))
        {
            newDates ??= [];
            newDates.Add(date.DiscardTime());
        }

        newDates ??= [];

        foreach (var date in _previewHighlightDates.ToArray())
        {
            if (!newDates.Contains(date))
            {
                if (_cells.GetOrDefault(date) is CalendarDayButton cell)
                    CalendarDayRangeStateHelper.ClearPreviewRangeState(cell);
                _previewHighlightDates.Remove(date);
            }
        }

        foreach (var date in newDates)
        {
            if (_cells.GetOrDefault(date) is not CalendarDayButton cell)
                continue;

            if (!CalendarDayRangeStateHelper.CellMatchesPreviewInterval(cell, date, anchor, end))
                CalendarDayRangeStateHelper.ApplyPreviewRoleTransition(cell, date, anchor, end);

            _previewHighlightDates.Add(date);
        }

        _cachedPreviewAnchor = anchor;
        _cachedPreviewEnd = end;
        ReconcileCommittedCapsDuringPreview();
    }

    private void ReconcileCommittedCapsDuringPreview()
    {
        if (!ShouldPreviewInterval() || !TryGetPreviewInterval(out var anchor, out var end) || anchor == end)
            return;

        if (_cells.GetOrDefault(anchor) is not CalendarDayButton anchorCell)
            return;

        CalendarDayRangeStateHelper.ReconcileSingleDayCapDuringPreview(anchorCell, anchor, end);
    }

    private bool IsDragRangeSelectionMode() =>
        IsRangeSelectionMode() && !AllowTapRangeSelection;

    private bool TryResolvePreviewStart(out DateTime start)
    {
        start = default;

        if (!IsDragRangeSelectionMode())
        {
            if (_selectionCoordinator.HasPendingRangeAnchor)
            {
                start = _selectionCoordinator.HoverStart!.Value;
                return true;
            }

            return false;
        }

        if (!_selectionCoordinator.HasPendingRangeAnchor)
            return false;

        start = _selectionCoordinator.HoverStart!.Value;
        return true;
    }

    private bool TryGetPreviewInterval(out DateTime anchor, out DateTime end)
    {
        anchor = default;
        end = default;

        if (!ShouldPreviewInterval())
            return false;

        if (!TryResolvePreviewStart(out anchor))
            return false;

        end = _previewController == PreviewController.Keyboard
            ? _previewEndDate ?? default
            : _previewEndDate ?? _pointerOverDate ?? default;
        return end != default;
    }

    private bool ResolveIntervalPreview(bool shiftHeld) =>
        IsRangeSelectionMode()
        && _selectionCoordinator.HasPendingRangeAnchor
        && (AllowTapRangeSelection || shiftHeld);

    private void SchedulePreviewUpdate() => UpdatePreviewRangeHighlightsOnly();

    private KeyModifiers GetCurrentKeyModifiers() => _lastKeyModifiers;

    private bool ShouldPreviewInterval()
    {
        if (!IsRangeSelectionMode())
            return false;

        if (_isPointerSelecting || _previewController == PreviewController.Drag)
            return true;

        if (!_selectionCoordinator.HasPendingRangeAnchor)
            return false;

        if (_previewController == PreviewController.Keyboard && _previewEndDate.HasValue)
            return _intervalPreviewActive;

        if (AllowTapRangeSelection)
            return _previewEndDate.HasValue || _pointerOverDate.HasValue;

        if (_previewController == PreviewController.PointerShift)
            return true;

        return false;
    }

    private KeyModifiers GetEffectivePointerModifiers(KeyModifiers pointerModifiers) =>
        GetCurrentKeyModifiers() | pointerModifiers;

    private bool IsShiftHeld(KeyModifiers pointerModifiers = default) =>
        (GetEffectivePointerModifiers(pointerModifiers | _pointerPressModifiers) & KeyModifiers.Shift) != 0;

    private bool IsCtrlHeld(KeyModifiers pointerModifiers = default) =>
        (GetEffectivePointerModifiers(pointerModifiers | _pointerPressModifiers) & KeyModifiers.Control) != 0;

    private bool ResolvePointerShift(KeyModifiers pointerModifiers) =>
        AllowTapRangeSelection ? false : IsShiftHeld(pointerModifiers);

    private bool ResolvePointerCtrl(KeyModifiers pointerModifiers) =>
        AllowTapRangeSelection && SelectionMode == CalendarSelectionMode.SingleRange
            ? false
            : IsCtrlHeld(pointerModifiers);

    private void ClearPointerPressModifiers() => _pointerPressModifiers = default;

    private static KeyModifiers GetEffectiveModifiers(KeyEventArgs e, bool isKeyDown)
    {
        var modifiers = e.KeyModifiers;
        if (!isKeyDown)
            return modifiers;

        return e.Key switch
        {
            Key.LeftShift or Key.RightShift => modifiers | KeyModifiers.Shift,
            Key.LeftCtrl or Key.RightCtrl => modifiers | KeyModifiers.Control,
            _ => modifiers,
        };
    }

    private void ClearRangePreview()
    {
        _previewEndDate = null;
        _pointerOverDate = null;
        _isPointerSelecting = false;
        _previewController = PreviewController.None;
        _intervalPreviewActive = false;
        _selectionCoordinator.ResetHover();
        _selectionCoordinator.ClearPointerPress();
        ClearPointerPressModifiers();
        UpdateRangeHighlights();
    }

    private void ClearPreviewController()
    {
        _previewController = PreviewController.None;
        _intervalPreviewActive = false;
    }

    private void ClearPreviewVisuals()
    {
        _previewEndDate = null;
        _pointerOverDate = null;
        ClearPreviewController();

        if (_previewHighlightDates.Count == 0)
        {
            InvalidatePreviewHighlightCache();
            return;
        }

        foreach (var date in _previewHighlightDates.ToArray())
        {
            if (_cells.GetOrDefault(date) is CalendarDayButton cell)
                CalendarDayRangeStateHelper.ClearPreviewRangeState(cell);
        }

        _previewHighlightDates.Clear();
        InvalidatePreviewHighlightCache();
    }

    private void OnShiftKeyDown()
    {
        if (!IsRangeSelectionMode())
            return;

        if (AllowTapRangeSelection)
        {
            if (_previewController == PreviewController.Keyboard && _previewEndDate.HasValue)
                _intervalPreviewActive = ResolveIntervalPreview(shiftHeld: true);

            return;
        }

        if (_previewController == PreviewController.Keyboard)
            _intervalPreviewActive = true;
    }

    private void OnShiftKeyUp()
    {
        if (!IsRangeSelectionMode())
            return;

        if (AllowTapRangeSelection)
        {
            if (_previewController == PreviewController.Keyboard && _previewEndDate.HasValue)
                _intervalPreviewActive = true;

            return;
        }

        if (_previewController == PreviewController.Keyboard)
        {
            _intervalPreviewActive = true;
        }
        else if (_previewController == PreviewController.PointerShift)
        {
            _previewController = PreviewController.None;
            _previewEndDate = null;
            _intervalPreviewActive = false;
        }
    }

    private bool TryGetPreviewDateFromCell(object? sender, out DateTime date)
    {
        date = default;

        if (sender is not CalendarDayButton cell)
            return false;

        if (cell.IsBlackout || !cell.IsEnabled || cell.DateContext?.ToDate() is not { } cellDate)
            return false;

        if (!IsRangeSelectionMode())
            return false;

        date = cellDate;
        return true;
    }

    private bool TryHitTestDayButton(Point position, out DateTime date)
    {
        date = default;

        if (_monthGrid is null)
            return false;

        if (_monthGrid.InputHitTest(position) is not CalendarDayButton { IsBlackout: false, IsEnabled: true } cell)
            return false;

        if (cell.DateContext?.ToDate() is not { } cellDate || !IsRangeSelectionMode())
            return false;

        date = cellDate;
        return true;
    }

    private bool IsPointerPreviewTrackingActive()
    {
        if (!IsRangeSelectionMode())
            return false;

        if (_previewController == PreviewController.Keyboard)
            return false;

        if (_isPointerSelecting || _previewController == PreviewController.Drag)
            return true;

        if (_previewController == PreviewController.PointerShift)
            return true;

        return AllowTapRangeSelection && _selectionCoordinator.HasPendingRangeAnchor;
    }

    private void ApplyPointerPreviewUpdate(DateTime date, bool fromMove, KeyModifiers pointerModifiers = default)
    {
        var shiftHeld = IsShiftHeld(pointerModifiers);

        if (IsDragRangeSelectionMode()
            && fromMove
            && !shiftHeld
            && !_isPointerSelecting
            && _previewController == PreviewController.Keyboard)
            return;

        _pointerOverDate = date;

        if (!_selectionCoordinator.HasPendingRangeAnchor && !_isPointerSelecting)
            return;

        if (AllowTapRangeSelection)
        {
            if (_previewController == PreviewController.Keyboard && !fromMove)
                return;

            if (fromMove && _previewController == PreviewController.Keyboard)
            {
                _previewController = PreviewController.None;
                _intervalPreviewActive = true;
            }

            SetPreviewEndDate(date);
            return;
        }

        if (!fromMove && !_isPointerSelecting)
            return;

        if (_previewController == PreviewController.Keyboard)
        {
            if (!fromMove)
                return;

            if (!shiftHeld)
                return;

            _previewController = PreviewController.PointerShift;
            _intervalPreviewActive = true;
        }
        else if (!_isPointerSelecting)
        {
            if (!fromMove || !shiftHeld)
                return;

            _previewController = PreviewController.PointerShift;
            _intervalPreviewActive = true;
        }

        SetPreviewEndDate(date);
    }

    private void SetPreviewEndDate(DateTime date)
    {
        if (!ShouldPreviewInterval())
        {
            if (_previewController != PreviewController.Keyboard
                && _selectionCoordinator.HasPendingRangeAnchor
                && _previewEndDate is not null)
            {
                return;
            }

            if (_previewEndDate is not null && _previewController != PreviewController.Keyboard)
            {
                _previewEndDate = null;
                SchedulePreviewUpdate();
            }

            return;
        }

        if (_previewEndDate == date)
            return;

        _previewEndDate = date;
        SchedulePreviewUpdate();
    }

    private void TrackPointerOverCell(object? sender)
    {
        if (sender is not CalendarDayButton cell)
            return;

        if (cell.IsBlackout || !cell.IsEnabled || cell.DateContext?.ToDate() is not { } date)
            return;

        if (!IsRangeSelectionMode())
            return;

        _pointerOverDate = date;
    }

    private void UpdateYears()
    {
        if (_yearGrid is null) return;

        _cells.Clear();

        foreach (var state in CalendarYearGridHelper.BuildCells(DisplayDateContext, CurrentMonthContext))
        {
            if (_yearGrid.Children[state.Index] is not CalendarYearButton cell) continue;

            cell.Index = state.Index;
            cell.SetContext(state.DateContext);
            cell.IsInactive = state.IsInactive;
            cell.IsSelected = state.IsSelected;

            _cells.Add(state.CellDate, cell);
        }
    }

    #endregion

    #region Move Display Date

    private void FastNext() => SetCurrentValue(DisplayDateProperty, DisplayDateContext.FastNext().ToDate());

    private void FastPrevious() => SetCurrentValue(DisplayDateProperty, DisplayDateContext.FastPrevious().ToDate());

    private void Next() => SetCurrentValue(DisplayDateProperty, DisplayDateContext.Next().ToDate());

    private void Previous() => SetCurrentValue(DisplayDateProperty, DisplayDateContext.Previous().ToDate());

    public void MoveToDate(DateTime date)
    {
        _lastSelectedDate = date;

        DisplayDateContext = CalendarDisplayModeHelper.ToMonthContext(date);
        SetCurrentValue(DisplayDateProperty, date);

        if (DisplayDateContext is MonthContext)
            _cells.GetOrDefault(date)?.Focus();
    }

    private void NavigatePreview(DateTime focusDate, bool shiftHeld)
    {
        if (AllowTapRangeSelection)
            shiftHeld = false;

        MoveToDate(focusDate);

        if (IsDragRangeSelectionMode())
        {
            if (shiftHeld)
            {
                if (!_selectionCoordinator.HasPendingRangeAnchor)
                {
                    _previewController = PreviewController.None;
                    _intervalPreviewActive = false;
                    _previewEndDate = null;
                    UpdatePreviewRangeHighlightsOnly();
                    return;
                }

                _previewEndDate = focusDate;
                _intervalPreviewActive = true;
                _previewController = PreviewController.Keyboard;
            }
            else
            {
                _selectionCoordinator.SetRangeAnchor(focusDate);
                _previewController = PreviewController.None;
                _intervalPreviewActive = false;
                _previewEndDate = null;
            }

            UpdatePreviewRangeHighlightsOnly();
            return;
        }

        _previewEndDate = focusDate;
        _intervalPreviewActive = ResolveIntervalPreview(shiftHeld);
        _previewController = _intervalPreviewActive ? PreviewController.Keyboard : PreviewController.None;
        UpdatePreviewRangeHighlightsOnly();
    }

    private void CommitPreview(RoutedEventArgs? e, DateTime? date = null, bool shift = false, bool ctrl = false)
    {
        var target = date ?? GetFocusedDate();
        var intervalPreview = _intervalPreviewActive;

        ClearPreviewController();

        if (e is KeyEventArgs keyEvent)
        {
            if (SelectionMode == CalendarSelectionMode.SingleDate)
            {
                SetSelection(target);
                MoveToDate(target);
            }
            else
            {
                var keyShift = (keyEvent.KeyModifiers & KeyModifiers.Shift) == KeyModifiers.Shift;
                var keyCtrl = (keyEvent.KeyModifiers & KeyModifiers.Control) == KeyModifiers.Control;
                if (AllowTapRangeSelection)
                {
                    keyShift = false;
                    if (SelectionMode == CalendarSelectionMode.SingleRange)
                        keyCtrl = false;
                }

                _selectionCoordinator.CommitFromKeyboard(target, intervalPreview, keyShift, keyCtrl);
            }
        }
        else
        {
            _selectionCoordinator.Commit(target, shift, ctrl);
        }

        FinishDaySelectionInteraction(e);
    }

    #endregion

    #region Move Display Mode

    private void ShowMonthMode() => DisplayDateContext = CalendarDisplayModeHelper.ToMonthContext(DisplayDate);

    private void ShowYearMode() => DisplayDateContext = CalendarDisplayModeHelper.ToYearContext(DisplayDate);

    private void ShowDecadeMode() => DisplayDateContext = CalendarDisplayModeHelper.ToDecadeContext(DisplayDate);

    private void ShowCenturyMode() => DisplayDateContext = CalendarDisplayModeHelper.ToCenturyContext(DisplayDate);

    #endregion

    #region Selection

    private void ClearSelection() => SelectedDates.ClearInternal();

    private void SetSelection(DateTime date) => SelectedDates.Set(date);

    private void SetSelection(DateTime start, DateTime end) => SelectedDates.Set(start, end);

    private void AddSelection(DateTime selectedDate) => SelectedDates.Add(selectedDate);

    private void AddSelection(DateTime start, DateTime end) => SelectedDates.AddRange(start, end);

    private void ToggleSelection(DateTime selectedDate)
    {
        var date = selectedDate.DiscardTime();
        if (!SelectedDates.Remove(date))
            SelectedDates.Add(date);
    }

    private void ChangeSelection(DateTime start, DateTime end, bool isSelected)
    {
        if (isSelected)
            SelectedDates.AddRange(start, end);
        else
            SelectedDates.RemoveRange(start, end);
    }

    private void ProcessContextSelection(MonthContext context)
    {
        SetCurrentValue(DisplayDateProperty, context.ToDate());

        foreach (var cell in _cells)
            cell.Value.IsSelected = cell.Value.DateContext?.IsSimilar(context.ToDate()) == true;

        UpdateFocus();
    }

    #endregion

    #region Buttons handlers

    private void OnFastNextButtonClick(object? sender, RoutedEventArgs e) => FastNext();

    private void OnFastPreviousButtonClick(object? sender, RoutedEventArgs e) => FastPrevious();

    private void OnNextButtonClick(object? sender, RoutedEventArgs e) => Next();

    private void OnPreviousButtonClick(object? sender, RoutedEventArgs e) => Previous();

    private void OnHeaderButtonClick(object? sender, RoutedEventArgs e)
    {
        if (CalendarDisplayModeHelper.GetHeaderDrillDownAction(DisplayDateContext) is { } action)
            ApplyNavigation(new(action), ctrl: false, shift: false);
    }

    private void OnHeaderMonthButtonClick(object? sender, RoutedEventArgs e) => ShowYearMode();

    private void OnHeaderYearButtonClick(object? sender, RoutedEventArgs e) => ShowDecadeMode();

    #endregion

    #region Mouse Events

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        ReleaseMonthGridPointerCapture(e);
        base.OnPointerReleased(e);
    }

    private void OnCalendarPointerExited(object? sender, PointerEventArgs e)
    {
        var position = e.GetPosition(this);
        var bounds = new Rect(Bounds.Size);

        if (bounds.Contains(position))
            return;

        ClearPointerPreview(fullClear: true);
    }

    protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
    {
        base.OnPointerWheelChanged(e);
        if (!e.Handled)
        {
            var ctrl = (e.KeyModifiers & KeyModifiers.Control) == KeyModifiers.Control;

            ApplyNavigation(e.Delta.Y > 0 ? CalendarKeyboardNavigationHelper.Resolve(Key.PageUp, DisplayDateContext, GetFocusedDate(), CurrentMonthContext, SelectionMode, AllowTapRangeSelection, ctrl, shift: false) : CalendarKeyboardNavigationHelper.Resolve(Key.PageDown, DisplayDateContext, GetFocusedDate(), CurrentMonthContext, SelectionMode, AllowTapRangeSelection, ctrl, shift: false), ctrl, shift: false);

            e.Handled = true;
        }
    }

    private void OnDayPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (sender is not CalendarDayButton cell || !e.GetCurrentPoint(cell).Properties.IsLeftButtonPressed)
            return;

        if (cell.IsBlackout || !cell.IsEnabled || SelectionMode is CalendarSelectionMode.None || cell.DateContext?.ToDate() is not { } date)
        {
            ClearRangePreview();
            return;
        }

        _pointerPressModifiers = GetEffectivePointerModifiers(e.KeyModifiers);

        if (AllowTapRangeSelection)
            return;

        if (!IsRangeSelectionMode())
            return;

        var shift = IsShiftHeld(e.KeyModifiers);
        var ctrl = IsCtrlHeld(e.KeyModifiers);

        if (SelectionMode == CalendarSelectionMode.MultipleRange && ctrl && !shift)
        {
            _selectionCoordinator.RecordPointerPress(date);
            return;
        }

        _isPointerSelecting = false;
        _previewController = PreviewController.None;
        _previewEndDate = null;
        _selectionCoordinator.RecordPointerPress(date);
    }

    private void TryStartPointerDragSelection(DateTime hoverDate, KeyModifiers pointerModifiers, IPointer? pointer = null)
    {
        if (AllowTapRangeSelection || !IsRangeSelectionMode())
            return;

        if (_isPointerSelecting)
            return;

        if (_selectionCoordinator.PointerPressDate is not { } pressDate)
            return;

        var shift = IsShiftHeld(pointerModifiers);
        if (hoverDate == pressDate && !shift)
            return;

        _isPointerSelecting = true;
        _previewController = PreviewController.Drag;
        _selectionCoordinator.BeginPointerSelection(pressDate, shift);
        _previewEndDate = hoverDate;

        if (pointer is not null && _monthGrid is not null)
            pointer.Capture(_monthGrid);

        SchedulePreviewUpdate();
    }

    private void ReleaseMonthGridPointerCapture(PointerEventArgs e)
    {
        if (_monthGrid is not null && e.Pointer.Captured == _monthGrid)
            e.Pointer.Capture(null);
    }

    private void OnDayPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (sender is not CalendarDayButton cell || e.InitialPressMouseButton != MouseButton.Left)
            return;

        if (cell.IsBlackout || !cell.IsEnabled || SelectionMode is CalendarSelectionMode.None || cell.DataContext is not DateTime releaseDate)
            return;

        var shift = ResolvePointerShift(e.KeyModifiers);
        var ctrl = ResolvePointerCtrl(e.KeyModifiers);
        ReleaseMonthGridPointerCapture(e);
        ClearPointerPressModifiers();

        if (AllowTapRangeSelection)
        {
            CommitPreview(e, releaseDate, shift, ctrl);
            return;
        }

        var wasDragging = _isPointerSelecting
            && _selectionCoordinator.PointerPressDate is { } pressDate
            && pressDate != releaseDate
            && IsRangeSelectionMode();

        _isPointerSelecting = false;
        _selectionCoordinator.CompletePointerSelection(releaseDate, shift, ctrl, wasDragging);
        FinishDaySelectionInteraction(e);
    }

    private void OnDayPointerEnter(object? sender, PointerEventArgs e)
    {
        TrackPointerOverCell(sender);

        if (!TryGetPreviewDateFromCell(sender, out var date))
            return;

        if (_isPointerSelecting)
            ApplyPointerPreviewUpdate(date, fromMove: false, e.KeyModifiers);
        else if (AllowTapRangeSelection && _previewController != PreviewController.Keyboard)
            ApplyPointerPreviewUpdate(date, fromMove: false, e.KeyModifiers);
    }

    private void OnDayPointerExited(object? sender, PointerEventArgs e)
    {
        if (sender is not CalendarDayButton)
            return;

        if (_monthGrid is not null && TryHitTestDayButton(e.GetPosition(_monthGrid), out _))
        {
            return;
        }

        ClearPointerPreview(fullClear: false);
    }

    private void OnDayPointerMove(object? sender, PointerEventArgs e)
    {
        if (!TryGetPreviewDateFromCell(sender, out var date))
            return;

        if (sender is CalendarDayButton cell
            && e.GetCurrentPoint(cell).Properties.IsLeftButtonPressed)
        {
            TryStartPointerDragSelection(date, e.KeyModifiers, e.Pointer);
        }

        ApplyPointerPreviewUpdate(date, fromMove: true, e.KeyModifiers);
    }

    private void OnMonthGridPointerMove(object? sender, PointerEventArgs e)
    {
        if (_monthGrid is null)
            return;

        var position = e.GetPosition(_monthGrid);
        var leftButtonPressed = e.GetCurrentPoint(_monthGrid).Properties.IsLeftButtonPressed;

        if (!TryHitTestDayButton(position, out var date))
            return;

        if (leftButtonPressed)
            TryStartPointerDragSelection(date, e.KeyModifiers, e.Pointer);

        if (!IsPointerPreviewTrackingActive())
            return;

        ApplyPointerPreviewUpdate(date, fromMove: true, e.KeyModifiers);
    }

    private void OnMonthGridPointerLeave(object? sender, PointerEventArgs e)
    {
        if (_monthGrid is null)
            return;

        var position = e.GetPosition(_monthGrid);
        var bounds = new Rect(_monthGrid.Bounds.Size);

        if (bounds.Contains(position))
            return;

        ClearPointerPreview(fullClear: true);
    }

    private void ClearPointerHoverOnly()
    {
        if (_previewController == PreviewController.Keyboard)
            return;

        if (_isPointerSelecting || _previewController == PreviewController.Drag)
            return;

        _pointerOverDate = null;
    }

    private void ClearPointerPreview(bool fullClear = true)
    {
        if (_previewController == PreviewController.Keyboard)
            return;

        if (!fullClear)
        {
            ClearPointerHoverOnly();
            return;
        }

        if (_isPointerSelecting || _previewController == PreviewController.Drag)
            return;

        if (_pointerOverDate is null && _previewEndDate is null)
            return;

        _pointerOverDate = null;

        if (_previewController == PreviewController.PointerShift)
        {
            _previewController = PreviewController.None;
            _intervalPreviewActive = false;
            _previewEndDate = null;
            SchedulePreviewUpdate();
            return;
        }

        if (_previewEndDate is null)
            return;

        _previewEndDate = null;
        SchedulePreviewUpdate();
    }

    private void OnCalendarYearButtonClick(object? sender, RoutedEventArgs e)
    {
        if (sender is not CalendarYearButton cell || cell.DateContext is null) return;

        DisplayDateContext = cell.DateContext;

        Focus();
    }

    #endregion

    #region Keyboard events

    private void OnCalendarKeyDownHandler(object? sender, KeyEventArgs e) => OnCalendarKeyDown(e);

    private void OnCalendarKeyUpHandler(object? sender, KeyEventArgs e) => OnCalendarKeyUp(e);

    private void OnCalendarKeyDown(KeyEventArgs e)
    {
        if (!IsEnabled) return;

        _lastKeyModifiers = GetEffectiveModifiers(e, isKeyDown: true);

        if (IsModifierKey(e.Key))
        {
            if (e.Key is Key.LeftShift or Key.RightShift)
            {
                OnShiftKeyDown();
                SchedulePreviewUpdate();
            }

            return;
        }

        if (e.Handled) return;

        var ctrl = (e.KeyModifiers & KeyModifiers.Control) == KeyModifiers.Control;
        var shift = (e.KeyModifiers & KeyModifiers.Shift) == KeyModifiers.Shift;

        if (e.Key is Key.Space or Key.Enter)
        {
            CommitPreview(e);
            e.Handled = true;
            return;
        }

        var result = CalendarKeyboardNavigationHelper.Resolve(
            e.Key,
            DisplayDateContext,
            GetFocusedDate(),
            CurrentMonthContext,
            SelectionMode,
            AllowTapRangeSelection,
            ctrl,
            shift);

        if (result.Kind == CalendarNavigationKind.None)
            return;

        ApplyNavigation(result, ctrl, shift);
        e.Handled = true;
    }

    private void OnCalendarKeyUp(KeyEventArgs e)
    {
        if (!IsEnabled) return;

        _lastKeyModifiers = GetEffectiveModifiers(e, isKeyDown: false);

        if (IsModifierKey(e.Key))
        {
            if (e.Key is Key.LeftShift or Key.RightShift)
            {
                OnShiftKeyUp();
                SchedulePreviewUpdate();
            }
        }
    }

    private static bool IsModifierKey(Key key) =>
        key is Key.LeftShift or Key.RightShift or Key.LeftCtrl or Key.RightCtrl;

    private void FinishDaySelectionInteraction(RoutedEventArgs? e = null)
    {
        ClearPreviewVisuals();
        UpdateRangeHighlights();
        DayButtonClick?.Invoke(this, e ?? new RoutedEventArgs());
    }

    private void ApplyNavigation(CalendarNavigationResult result, bool ctrl, bool shift)
    {
        switch (result.Kind)
        {
            case CalendarNavigationKind.MoveFocus when result.Date is { } focusDate:
                NavigatePreview(focusDate, shift);
                break;

            case CalendarNavigationKind.SelectMonthContext when result.MonthContext is { } context:
                ProcessContextSelection(context);
                break;

            case CalendarNavigationKind.Next:
                Next();
                break;

            case CalendarNavigationKind.Previous:
                Previous();
                break;

            case CalendarNavigationKind.ShowMonthView:
                ShowMonthMode();
                break;

            case CalendarNavigationKind.ShowYearView:
                ShowYearMode();
                break;

            case CalendarNavigationKind.ShowDecadeView:
                ShowDecadeMode();
                break;

            case CalendarNavigationKind.ShowCenturyView:
                ShowCenturyMode();
                break;
        }
    }

    #endregion

    #region Focus

    public void FocusSelectedDay()
    {
        Focus();
        MoveToDate(SelectedDate ?? DisplayDate);
    }

    internal CalendarDayButton? GetFocusedDayButton() =>
        (CalendarDayButton?)_cells.Values.FirstOrDefault(x => x.IsFocused);

    private void UpdateFocus(DateTime? date = null)
    {
        if (DisplayDateContext is MonthContext)
        {
            if (date.HasValue)
                _cells.GetOrDefault(date.Value)?.Focus();
            else
                _cells.GetOrDefault(GetFocusedDate())?.Focus();

            return;
        }

        if (date.HasValue)
        {
            _cells.GetOrDefault(date.Value)?.Focus();
        }
        else
        {
            _cells.Values.FirstOrDefault(x => x.IsSelected)?.Focus();
        }
    }

    #endregion

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        Button.ClickEvent.RemoveHandler(OnHeaderYearButtonClick, _yearButton);
        Button.ClickEvent.RemoveHandler(OnHeaderMonthButtonClick, _monthButton);
        Button.ClickEvent.RemoveHandler(OnHeaderButtonClick, _headerButton);
        Button.ClickEvent.RemoveHandler(OnFastPreviousButtonClick, _fastPreviousButton);
        Button.ClickEvent.RemoveHandler(OnPreviousButtonClick, _previousButton);
        Button.ClickEvent.RemoveHandler(OnNextButtonClick, _nextButton);
        Button.ClickEvent.RemoveHandler(OnFastNextButtonClick, _fastNextButton);

        _monthGrid = e.NameScope.Find<Grid>(PartMonthGrid);
        _yearGrid = e.NameScope.Find<Grid>(PartYearGrid);
        _yearButton = e.NameScope.Find<Button>(PartYearButton);
        _monthButton = e.NameScope.Find<Button>(PartMonthButton);
        _headerButton = e.NameScope.Find<Button>(PartHeaderButton);
        _fastPreviousButton = e.NameScope.Find<Button>(PartFastPreviousButton);
        _previousButton = e.NameScope.Find<Button>(PartPreviousButton);
        _nextButton = e.NameScope.Find<Button>(PartNextButton);
        _fastNextButton = e.NameScope.Find<Button>(PartFastNextButton);

        Button.ClickEvent.AddHandler(OnHeaderYearButtonClick, _yearButton);
        Button.ClickEvent.AddHandler(OnHeaderMonthButtonClick, _monthButton);
        Button.ClickEvent.AddHandler(OnHeaderButtonClick, _headerButton);
        Button.ClickEvent.AddHandler(OnFastPreviousButtonClick, _fastPreviousButton);
        Button.ClickEvent.AddHandler(OnPreviousButtonClick, _previousButton);
        Button.ClickEvent.AddHandler(OnNextButtonClick, _nextButton);
        Button.ClickEvent.AddHandler(OnFastNextButtonClick, _fastNextButton);

        InitializeGridButtons();
        Refresh();
    }
}
