// -----------------------------------------------------------------------
// <copyright file="Calendar.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using MyNet.Avalonia.Controls.DateTimePickers;
using MyNet.Avalonia.Extensions;
using MyNet.Utilities;
using MyNet.Utilities.DateTimes;
using MyNet.Utilities.Helpers;
using MyNet.Utilities.Localization;
using MyNet.Utilities.Suspending;

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

    private const int NumberOfColumnInYearGrid = 3;

    private readonly GregorianCalendar _calendar = new();
    private readonly Suspender _changeDisplayDate = new();
    private readonly Dictionary<DateTime, CalendarDateButton> _cells = [];

    private Button? _fastNextButton;
    private Button? _fastPreviousButton;
    private Button? _headerButton;
    private Button? _monthButton;
    private Button? _nextButton;
    private Button? _previousButton;
    private Button? _yearButton;
    private Grid? _monthGrid;
    private Grid? _yearGrid;

    private DateTime? _hoverStart;
    private DateTime? _lastSelectedDate;

    static Calendar()
    {
        FocusableProperty.OverrideDefaultValue<Calendar>(true);
        FirstDayOfWeekProperty.Changed.AddClassHandler<Calendar>((x, e) => x.OnFirstDayOfWeekChanged(e));
        IsTodayHighlightedProperty.Changed.AddClassHandler<Calendar>((x, e) => x.OnIsTodayHighlightedChanged(e));
        DisplayDateContextProperty.Changed.AddClassHandler<Calendar>((x, e) => x.OnDisplayDateContextPropertyChanged(e));
        SelectionModeProperty.Changed.AddClassHandler<Calendar>((x, e) => x.OnSelectionModeChanged(e));
        SelectedDateProperty.Changed.AddClassHandler<Calendar>((x, e) => x.OnSelectedDateChanged(e));
        DisplayDateProperty.Changed.AddClassHandler<Calendar>((x, e) => x.OnDisplayDateChanged(e));
        DisplayDateStartProperty.Changed.AddClassHandler<Calendar>((x, e) => x.OnDisplayDateStartChanged(e));
        DisplayDateEndProperty.Changed.AddClassHandler<Calendar>((x, e) => x.OnDisplayDateEndChanged(e));
        KeyDownEvent.AddClassHandler<Calendar>((x, e) => x.OnCalendarKeyDown(e), handledEventsToo: true);
    }

    public Calendar()
    {
        DisplayDateContext = new MonthContext(DateTime.Today.Month, DateTime.Today.Year);
        SetCurrentValue(DisplayDateProperty, DateTime.Today);
        UpdateDisplayDate(DisplayDate, DateTime.MinValue);
        BlackoutDates = new CalendarBlackoutDatesCollection(this);
        SelectedDates = new SelectedDatesCollection(this);
        GlobalizationService.Current.CultureChanged += (_, _) => Refresh();
        SelectedDates.CollectionChanged += OnSelectedDatesCollectionChanged;
        BlackoutDates.CollectionChanged += OnBlackoutDatesCollectionChanged;
    }

    internal event EventHandler<PointerReleasedEventArgs>? DayButtonMouseUp;

    #region IsTodayHighlighted

    public static readonly StyledProperty<bool> IsTodayHighlightedProperty = AvaloniaProperty.Register<Calendar, bool>(nameof(IsTodayHighlighted), defaultValue: true);

    public bool IsTodayHighlighted
    {
        get => GetValue(IsTodayHighlightedProperty);
        set => SetValue(IsTodayHighlightedProperty, value);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Roslynator", "RCS1163:Unused parameter", Justification = "Used by handler")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Used by handler")]
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

    private static bool IsValidFirstDayOfWeek(object value)
    {
        var day = (DayOfWeek)value;

        return day == DayOfWeek.Sunday
            || day == DayOfWeek.Monday
            || day == DayOfWeek.Tuesday
            || day == DayOfWeek.Wednesday
            || day == DayOfWeek.Thursday
            || day == DayOfWeek.Friday
            || day == DayOfWeek.Saturday;
    }

    private void OnFirstDayOfWeekChanged(AvaloniaPropertyChangedEventArgs e)
    {
        if (IsValidFirstDayOfWeek(e.NewValue!))
            Refresh();
        else
            throw new ArgumentOutOfRangeException(nameof(e), "Invalid DayOfWeek");
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

    private static bool IsValidSelectionMode(object value)
    {
        var mode = (CalendarSelectionMode)value;

        return mode == CalendarSelectionMode.SingleDate
            || mode == CalendarSelectionMode.SingleRange
            || mode == CalendarSelectionMode.MultipleRange
            || mode == CalendarSelectionMode.None;
    }

    private void OnSelectionModeChanged(AvaloniaPropertyChangedEventArgs e)
    {
        if (IsValidSelectionMode(e.NewValue!))
            ClearSelection();
        else
            throw new ArgumentOutOfRangeException(nameof(e), "Invalid SelectionMode");
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

    public static readonly RoutedEvent<CalendarDateButtonEventArgs> DateSelectedEvent = RoutedEvent.Register<TimePickerPresenter, CalendarDateButtonEventArgs>(nameof(DateSelected), RoutingStrategies.Bubble);

    public event EventHandler<CalendarDateButtonEventArgs> DateSelected
    {
        add => AddHandler(DateSelectedEvent, value);
        remove => RemoveHandler(DateSelectedEvent, value);
    }

    public event EventHandler<CalendarDateButtonEventArgs>? DatePreviewed;

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

    internal bool IsValidSelection(DateTime date) => !BlackoutDates.Contains(date) && date.IsBetween(GetDisplayDateRangeStart(), GetDisplayDateRangeEnd());

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
                else
                {
                    if (!(SelectedDates.Count > 0 && SelectedDates[0] == addedDate.Value))
                    {
                        SetSelection(addedDate.Value);
                        MoveToDate(addedDate.Value);
                    }
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(e), "SelectedDate value is not valid.");
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
        foreach (var date in oldItems)
        {
            ChangeSelectedState(date, false);
        }

        var newItems = e.NewItems?.OfType<DateTime>().ToList() ?? [];
        foreach (var date in newItems)
        {
            ChangeSelectedState(date, true);
        }

        if (e.Action == NotifyCollectionChangedAction.Reset)
        {
            _cells.ForEach(x => ChangeSelectedState(x.Key, false));
        }

        SelectedDatesChanged?.Invoke(this, new SelectionChangedEventArgs(SelectingItemsControl.SelectionChangedEvent, oldItems, newItems)
        {
            Source = this
        });
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
        foreach (var date in period.ToDates())
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

    private static DateContext CoerceDisplayDateContext(AvaloniaObject sender, DateContext value) => value switch
    {
        DecadeContext decadeContext => decadeContext.StartYear % 10 == 0 ? decadeContext : new DecadeContext(DateTimeHelper.GetDecade(decadeContext.StartYear).Start),
        CenturyContext centuryContaxt => centuryContaxt.StartYear % 100 == 0 ? centuryContaxt : new CenturyContext(DateTimeHelper.GetCentury(centuryContaxt.StartYear).Start),
        _ => value,
    };

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
        // If DisplayDate < DisplayDateStart, DisplayDate = DisplayDateStart
        if (addedDate.IsBefore(GetDisplayDateRangeStart()))
        {
            DisplayDate = GetDisplayDateRangeStart();
            return;
        }

        // If DisplayDate > DisplayDateEnd, DisplayDate = DisplayDateEnd
        if (addedDate.IsAfter(GetDisplayDateRangeEnd()))
        {
            DisplayDate = GetDisplayDateRangeEnd();
            return;
        }

        DisplayDateContext = DisplayDateContext.FromDate(addedDate);

        OnDisplayDate(new CalendarDateChangedEventArgs(removedDate, addedDate));
    }

    private void OnDisplayDateChanged(AvaloniaPropertyChangedEventArgs e) => UpdateDisplayDate((DateTime)e.NewValue!, (DateTime)e.OldValue!);

    private void OnDisplayDate(CalendarDateChangedEventArgs e) => DisplayDateChanged?.Invoke(this, e);

    private DateTime GetFocusedDate() => _lastSelectedDate.HasValue && DisplayDateContext.IsSimilar(_lastSelectedDate.Value)
            ? _lastSelectedDate.Value
            : DisplayDateContext.IsSimilar(DateTime.Today) ? DateTime.Today : DisplayDateContext.ToDate();

    #endregion

    #region DisplayDateStart

    public static readonly StyledProperty<DateTime?> DisplayDateStartProperty = AvaloniaProperty.Register<Calendar, DateTime?>(nameof(DisplayDateStart), defaultBindingMode: BindingMode.TwoWay);

    public DateTime? DisplayDateStart
    {
        get => GetValue(DisplayDateStartProperty);
        set => SetValue(DisplayDateStartProperty, value);
    }

    private DateTime GetDisplayDateRangeStart() => DisplayDateStart ?? DateTime.MinValue;

    private void OnDisplayDateStartChanged(AvaloniaPropertyChangedEventArgs e)
    {
        if (_changeDisplayDate.IsSuspended) return;

        var newValue = e.NewValue as DateTime?;

        if (newValue.HasValue)
        {
            // DisplayDateStart coerces to the date of the SelectedDateMin if SelectedDateMin < DisplayDateStart
            var selectedDateMin = SelectedDateMin();

            if (selectedDateMin.HasValue && selectedDateMin.Value.IsBefore(newValue.Value))
            {
                SetCurrentValue(DisplayDateStartProperty, selectedDateMin.Value);
                return;
            }

            // if DisplayDateStart > DisplayDateEnd, DisplayDateEnd = DisplayDateStart
            if (newValue.Value.IsAfter(GetDisplayDateRangeEnd()))
            {
                SetCurrentValue(DisplayDateEndProperty, DisplayDateStart);
                return;
            }

            // If DisplayDateStart > DisplayDate , DisplayDate = DisplayDateStart
            if (newValue.Value.IsAfter(DisplayDate))
                SetCurrentValue(DisplayDateProperty, newValue.Value);
        }

        Refresh();
    }

    private DateTime? SelectedDateMin()
    {
        DateTime selectedDateMin;

        if (SelectedDates.Count > 0)
        {
            selectedDateMin = SelectedDates[0];
            Debug.Assert(SelectedDate!.Value == selectedDateMin, "The SelectedDate should be the minimum selected date!");
        }
        else
        {
            return null;
        }

        foreach (var selectedDate in SelectedDates)
        {
            if (selectedDate.IsBefore(selectedDateMin))
                selectedDateMin = selectedDate;
        }

        return selectedDateMin;
    }

    #endregion

    #region DisplayDateEnd

    public static readonly StyledProperty<DateTime?> DisplayDateEndProperty = AvaloniaProperty.Register<Calendar, DateTime?>(nameof(DisplayDateEnd), defaultBindingMode: BindingMode.TwoWay);

    public DateTime? DisplayDateEnd
    {
        get => GetValue(DisplayDateEndProperty);
        set => SetValue(DisplayDateEndProperty, value);
    }

    private DateTime GetDisplayDateRangeEnd() => DisplayDateEnd ?? DateTime.MaxValue;

    private void OnDisplayDateEndChanged(AvaloniaPropertyChangedEventArgs e)
    {
        if (_changeDisplayDate.IsSuspended) return;

        var newValue = e.NewValue as DateTime?;

        if (newValue.HasValue)
        {
            // DisplayDateEnd coerces to the date of the SelectedDateMax if SelectedDateMax > DisplayDateEnd
            var selectedDateMax = SelectedDateMax();

            if (selectedDateMax.HasValue && selectedDateMax.Value.IsAfter(newValue.Value))
            {
                SetCurrentValue(DisplayDateEndProperty, selectedDateMax.Value);
                return;
            }

            // if DisplayDateEnd < DisplayDateStart, DisplayDateEnd = DisplayDateStart
            if (newValue.Value.IsBefore(GetDisplayDateRangeStart()))
            {
                SetCurrentValue(DisplayDateEndProperty, GetDisplayDateRangeStart());
                return;
            }

            // If DisplayDate > DisplayDateEnd, DisplayDate = DisplayDateEnd
            if (newValue.Value.IsBefore(DisplayDate))
                SetCurrentValue(DisplayDateProperty, newValue.Value);
        }

        Refresh();
    }

    private DateTime? SelectedDateMax()
    {
        DateTime selectedDateMax;

        if (SelectedDates.Count > 0)
        {
            selectedDateMax = SelectedDates[0];
            Debug.Assert(SelectedDate!.Value == selectedDateMax, "The SelectedDate should be the maximum SelectedDate!");
        }
        else
        {
            return null;
        }

        foreach (var selectedDate in SelectedDates)
        {
            if (selectedDate.IsAfter(selectedDateMax))
                selectedDateMax = selectedDate;
        }

        return selectedDateMax;
    }

    #endregion

    #region Build

    private void InitializeGridButtons()
    {
        // Generate Day titles (Sun, Mon, Tue, Wed, Thu, Fri, Sat) based on FirstDayOfWeek and culture.
        var count = DateTimeHelper.NumberOfDaysInWeek() + (DateTimeHelper.NumberOfDaysInWeek() * DateTimeHelper.NumberOfDaysInWeek());
        var children = new List<Control>(count);
        var dayOfWeek = (int)FirstDayOfWeek;
        var info = DateTimeHelper.GetCurrentDateTimeFormatInfo();
        for (var i = 0; i < DateTimeHelper.NumberOfDaysInWeek(); i++)
        {
            var d = (dayOfWeek + i) % DateTimeHelper.NumberOfDaysInWeek();
            if (DayTitleTemplate?.Build() is Control cell)
            {
                cell.DataContext = string.Empty;
                _ = cell.SetValue(Grid.RowProperty, 0);
                _ = cell.SetValue(Grid.ColumnProperty, i);
                children.Add(cell);
            }
        }

        // Generate date buttons.
        for (var i = 2; i < DateTimeHelper.MaxNumberOfWeeksPerMonth() + 2; i++)
        {
            for (var j = 0; j < DateTimeHelper.NumberOfDaysInWeek(); j++)
            {
                var cell = new CalendarDayButton
                {
                    Owner = this
                };
                _ = cell.SetValue(Grid.RowProperty, i);
                _ = cell.SetValue(Grid.ColumnProperty, j);
                cell.AddHandler(PointerReleasedEvent, OnDayPointerReleased, handledEventsToo: true);
                cell.AddHandler(PointerPressedEvent, OnDayPointerPressed, handledEventsToo: true);
                cell.AddHandler(KeyDownEvent, OnDayKeyDown, RoutingStrategies.Tunnel, handledEventsToo: true);

                children.Add(cell);
            }
        }

        _monthGrid?.Children.AddRange(children);

        // Generate month/year buttons.
        for (var i = 0; i < 12; i++)
        {
            var cell = new CalendarYearButton
            {
                Owner = this
            };
            Grid.SetRow(cell, i / NumberOfColumnInYearGrid);
            Grid.SetColumn(cell, i % NumberOfColumnInYearGrid);
            cell.AddHandler(Button.ClickEvent, OnCalendarYearButtonClick);
            cell.AddHandler(KeyDownEvent, OnYearKeyDown, RoutingStrategies.Tunnel, handledEventsToo: true);

            _yearGrid?.Children.Add(cell);
        }
    }

    private void Refresh()
    {
        PseudoClasses.Set(PseudoClassName.Month, DisplayDateContext is MonthContext);
        PseudoClasses.Set(PseudoClassName.Year, DisplayDateContext is YearContext);
        PseudoClasses.Set(PseudoClassName.Decade, DisplayDateContext is DecadeContext);
        PseudoClasses.Set(PseudoClassName.Century, DisplayDateContext is CenturyContext);

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

        for (var childIndex = 0; childIndex < DateTimeHelper.NumberOfDaysInWeek(); childIndex++)
        {
            var daytitle = _monthGrid.Children[childIndex];
            daytitle.DataContext = DateTimeHelper.GetCurrentDateTimeFormatInfo().ShortestDayNames[(childIndex + (int)FirstDayOfWeek) % DateTimeHelper.NumberOfDaysInWeek()];
        }
    }

    private void UpdateMonths()
    {
        if (_monthGrid is null || DisplayDateContext is not MonthContext monthContext) return;

        _cells.Clear();

        var children = _monthGrid.Children;
        var daysBeforeCount = PreviousMonthDays(monthContext);
        var date = monthContext.ToDate().AddDays(-daysBeforeCount);

        for (var i = DateTimeHelper.NumberOfDaysInWeek(); i < children.Count; i++)
        {
            if (children[i] is not CalendarDayButton cell) continue;

            var dateContext = new DayContext(date.Day, date.Month, date.Year);
            cell.Index = i;
            cell.SetContext(dateContext);
            cell.IsInactive = monthContext.Month != dateContext.Month;
            cell.IsSelected = SelectedDates.Contains(date);
            cell.IsBlackout = BlackoutDates.Contains(date);

            _cells.Add(date, cell);

            date = date.AddDays(1);
        }

        SetDayTitles();
    }

    private void UpdateYears()
    {
        if (_yearGrid is null) return;

        _cells.Clear();

        switch (DisplayDateContext)
        {
            case YearContext yearContext:
                {
                    for (var i = 0; i < 12; i++)
                    {
                        if (_yearGrid.Children[i] is not CalendarYearButton cell) continue;

                        var dateContext = new MonthContext(i + 1, yearContext.Year);
                        cell.Index = i;
                        cell.SetContext(dateContext);
                        cell.IsInactive = yearContext.Year != dateContext.Year;
                        cell.IsSelected = dateContext.IsSimilar(CurrentMonthContext.ToDate());

                        _cells.Add(dateContext.ToDate(), cell);
                    }

                    break;
                }

            case DecadeContext decadeContext:
                {
                    for (var i = 0; i < 12; i++)
                    {
                        if (_yearGrid.Children[i] is not CalendarYearButton cell) continue;

                        var dateContext = new YearContext(decadeContext.StartYear - 1 + i);
                        cell.SetContext(dateContext);
                        cell.IsInactive = decadeContext.StartYear != DateTimeHelper.GetDecade(dateContext.Year).Start;
                        cell.IsSelected = dateContext.IsSimilar(CurrentMonthContext.ToDate());

                        _cells.Add(dateContext.ToDate(), cell);
                    }

                    break;
                }

            case CenturyContext centuryContext:
                {
                    for (var i = 0; i < 12; i++)
                    {
                        if (_yearGrid.Children[i] is not CalendarYearButton cell) continue;

                        var dateContext = new DecadeContext(centuryContext.StartYear - 10 + (i * 10));
                        cell.SetContext(dateContext);
                        cell.IsInactive = centuryContext.StartYear != DateTimeHelper.GetCentury(dateContext.StartYear).Start;
                        cell.IsSelected = dateContext.IsSimilar(CurrentMonthContext.ToDate());

                        _cells.Add(dateContext.ToDate(), cell);
                    }

                    break;
                }
        }
    }

    private int PreviousMonthDays(MonthContext context)
    {
        var firstDay = context.ToDate();
        var dayOfWeek = _calendar.GetDayOfWeek(firstDay);
        var firstDayOfWeek = FirstDayOfWeek;
        var i = (dayOfWeek - firstDayOfWeek + DateTimeHelper.NumberOfDaysInWeek()) % DateTimeHelper.NumberOfDaysInWeek();
        return i == 0 ? DateTimeHelper.NumberOfDaysInWeek() : i;
    }

    internal CalendarDayButton? GetFocusedDayButton() => (CalendarDayButton?)_cells.Values.FirstOrDefault(x => x.IsFocused);

    #endregion

    #region Move Display Date

    private void FastNext() => SetCurrentValue(DisplayDateProperty, DisplayDateContext.FastNext().ToDate());

    private void FastPrevious() => SetCurrentValue(DisplayDateProperty, DisplayDateContext.FastPrevious().ToDate());

    private void Next() => SetCurrentValue(DisplayDateProperty, DisplayDateContext.Next().ToDate());

    private void Previous() => SetCurrentValue(DisplayDateProperty, DisplayDateContext.Previous().ToDate());

    public void MoveToDate(DateTime date)
    {
        _lastSelectedDate = date;

        DisplayDateContext = new MonthContext(date.Month, date.Year);
        SetCurrentValue(DisplayDateProperty, date);

        UpdateFocus(date);
    }

    #endregion

    #region Move Display Mode

    private void ShowMonthMode() => DisplayDateContext = new MonthContext(DisplayDate.Month, DisplayDate.Year);

    private void ShowYearMode() => DisplayDateContext = new YearContext(DisplayDate.Year);

    private void ShowDecadeMode() => DisplayDateContext = new DecadeContext(DateTimeHelper.GetDecade(DisplayDate.Year).Start);

    private void ShowCenturyMode() => DisplayDateContext = new CenturyContext(DateTimeHelper.GetCentury(DisplayDate.Year).Start);

    #endregion

    #region Selection

    private void ClearSelection() => SelectedDates.ClearInternal();

    private void SetSelection(DateTime date) => SelectedDates.Set(date);

    private void SetSelection(DateTime start, DateTime end) => SelectedDates.Set(start, end);

    private void AddSelection(DateTime selectedDate) => SelectedDates.Add(selectedDate);

    private void AddSelection(DateTime start, DateTime end) => SelectedDates.AddRange(start, end);

    private void ToggleSelection(DateTime selectedDate)
    {
        if (!SelectedDates.Remove(selectedDate))
            SelectedDates.Add(selectedDate);
    }

    private void ChangeSelection(DateTime start, DateTime end, bool isSelected)
    {
        if (isSelected)
            SelectedDates.AddRange(start, end);
        else
            SelectedDates.RemoveRange(start, end);
    }

    private void ProcessDateSelection(DateTime date, bool shift, bool ctrl)
    {
        if (!IsValidSelection(date)) return;

        if (AllowTapRangeSelection)
        {
            ProcessTapRangeSelection(date, ctrl);
            return;
        }

        switch (SelectionMode)
        {
            case CalendarSelectionMode.SingleDate:
                SetSelection(date);
                break;

            case CalendarSelectionMode.SingleRange:
                if (shift)
                    SetSelection(_hoverStart ?? DisplayDate, date);
                else
                    SetSelection(date);

                break;

            case CalendarSelectionMode.MultipleRange:
                if (ctrl)
                {
                    if (shift)
                    {
                        var startDate = _hoverStart ?? DisplayDate;
                        ChangeSelection(startDate, date, SelectedDates.Contains(startDate));
                    }
                    else
                    {
                        ToggleSelection(date);
                    }
                }
                else
                {
                    if (shift)
                        SetSelection(_hoverStart ?? DisplayDate, date);
                    else
                        SetSelection(date);
                }

                break;
        }

        if (!shift)
            _hoverStart = date;

        MoveToDate(date);
    }

    private void ProcessTapRangeSelection(DateTime date, bool ctrl)
    {
        switch (SelectionMode)
        {
            case CalendarSelectionMode.SingleDate:
                SetSelection(date);
                break;

            case CalendarSelectionMode.SingleRange:
                if (!_hoverStart.HasValue)
                {
                    SetSelection(date);
                    _hoverStart = date;
                }
                else
                {
                    SetSelection(_hoverStart.Value, date);
                    _hoverStart = null;
                }

                break;

            case CalendarSelectionMode.MultipleRange:
                if (ctrl)
                {
                    if (!_hoverStart.HasValue)
                    {
                        AddSelection(date);
                        _hoverStart = date;
                    }
                    else
                    {
                        AddSelection(_hoverStart.Value, date);
                        _hoverStart = null;
                    }
                }
                else
                {
                    if (!_hoverStart.HasValue)
                    {
                        SetSelection(date);
                        _hoverStart = date;
                    }
                    else
                    {
                        SetSelection(_hoverStart.Value, date);
                        _hoverStart = null;
                    }
                }

                break;
        }

        MoveToDate(date);
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
        switch (DisplayDateContext)
        {
            case YearContext:
                ShowDecadeMode();
                break;
            case DecadeContext:
                ShowCenturyMode();
                break;
        }
    }

    private void OnHeaderMonthButtonClick(object? sender, RoutedEventArgs e) => ShowYearMode();

    private void OnHeaderYearButtonClick(object? sender, RoutedEventArgs e) => ShowDecadeMode();

    #endregion

    #region Mouse Events

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);
        if (e.InitialPressMouseButton == MouseButton.Left)
            Focus();
    }

    protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
    {
        base.OnPointerWheelChanged(e);
        if (!e.Handled)
        {
            var ctrl = (e.KeyModifiers & KeyModifiers.Control) == KeyModifiers.Control;

            if (e.Delta.Y > 0)
                ProcessPageUpKey(ctrl, false);
            else
                ProcessPageDownKey(ctrl, false);

            e.Handled = true;
        }
    }

    private void OnDayPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (sender is not CalendarDayButton cell || AllowTapRangeSelection || !e.GetCurrentPoint(cell).Properties.IsLeftButtonPressed) return;

        Focus();

        if (cell.IsBlackout || !cell.IsEnabled || SelectionMode is CalendarSelectionMode.None || cell.DateContext?.ToDate() is not DateTime date)
        {
            _hoverStart = null;
            return;
        }

        var shift = (e.KeyModifiers & KeyModifiers.Shift) == KeyModifiers.Shift;

        if (!shift || !_hoverStart.HasValue)
            _hoverStart = date;
    }

    private void OnDayPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (sender is not CalendarDayButton cell || e.InitialPressMouseButton != MouseButton.Left) return;

        if (cell.DataContext is DateTime selectedDate)
        {
            var shift = (e.KeyModifiers & KeyModifiers.Shift) == KeyModifiers.Shift;
            var ctrl = (e.KeyModifiers & KeyModifiers.Control) == KeyModifiers.Control;

            ProcessDateSelection(selectedDate, shift, ctrl);

            DayButtonMouseUp?.Invoke(this, e);
        }
    }

    private void OnCalendarYearButtonClick(object? sender, RoutedEventArgs e)
    {
        if (sender is not CalendarYearButton cell || cell.DateContext is null) return;

        DisplayDateContext = cell.DateContext;

        Focus();
    }

    #endregion

    #region Keyboard events

    private void OnDayKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Handled || sender is not CalendarDayButton) return;

        if (e.Key == Key.Enter || e.Key == Key.Space)
            OnCalendarKeyDown(e);
    }

    private void OnYearKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Handled || sender is not CalendarYearButton) return;

        if (e.Key == Key.Enter || e.Key == Key.Space)
            OnCalendarKeyDown(e);
    }

    private void OnCalendarKeyDown(KeyEventArgs e)
    {
        if (e.Handled || !IsEnabled) return;

        var ctrl = (e.KeyModifiers & KeyModifiers.Control) == KeyModifiers.Control;
        var shift = (e.KeyModifiers & KeyModifiers.Shift) == KeyModifiers.Shift;

        switch (e.Key)
        {
            case Key.Up:
                ProcessUpKey(ctrl, shift);
                e.Handled = true;
                break;

            case Key.Down:
                ProcessDownKey(ctrl, shift);
                e.Handled = true;
                break;

            case Key.Left:
                ProcessLeftKey(ctrl, shift);
                e.Handled = true;
                break;

            case Key.Right:
                ProcessRightKey(ctrl, shift);
                e.Handled = true;
                break;

            case Key.PageDown:
                ProcessPageDownKey(ctrl, shift);
                e.Handled = true;
                break;

            case Key.PageUp:
                ProcessPageUpKey(ctrl, shift);
                e.Handled = true;
                break;

            case Key.Home:
                ProcessHomeKey(ctrl, shift);
                e.Handled = true;
                break;

            case Key.End:
                ProcessEndKey(ctrl, shift);
                e.Handled = true;
                break;

            case Key.Enter:
            case Key.Space:
                ProcessEnterKey(ctrl, shift);
                break;
        }
    }

    private void ProcessUpKey(bool ctrl, bool shift)
    {
        switch (DisplayDateContext)
        {
            case MonthContext:
                ProcessDateSelection(GetFocusedDate().AddDays(-DateTimeHelper.NumberOfDaysInWeek()), shift, ctrl);
                break;

            case YearContext:
                ProcessContextSelection(CurrentMonthContext.Add(-NumberOfColumnInYearGrid));
                break;

            case DecadeContext:
                ProcessContextSelection(CurrentMonthContext.AddYears(-NumberOfColumnInYearGrid));
                break;

            case CenturyContext:
                ProcessContextSelection(CurrentMonthContext.AddDecades(-NumberOfColumnInYearGrid));
                break;
        }
    }

    private void ProcessDownKey(bool ctrl, bool shift)
    {
        switch (DisplayDateContext)
        {
            case MonthContext:
                ProcessDateSelection(GetFocusedDate().AddDays(DateTimeHelper.NumberOfDaysInWeek()), shift, ctrl);
                break;

            case YearContext:
                ProcessContextSelection(CurrentMonthContext.Add(NumberOfColumnInYearGrid));
                break;

            case DecadeContext:
                ProcessContextSelection(CurrentMonthContext.AddYears(NumberOfColumnInYearGrid));
                break;

            case CenturyContext:
                ProcessContextSelection(CurrentMonthContext.AddDecades(NumberOfColumnInYearGrid));
                break;
        }
    }

    private void ProcessLeftKey(bool ctrl, bool shift)
    {
        switch (DisplayDateContext)
        {
            case MonthContext:
                ProcessDateSelection(GetFocusedDate().AddDays(-1), shift, ctrl);
                break;

            case YearContext:
                ProcessContextSelection(CurrentMonthContext.Add(-1));
                break;

            case DecadeContext:
                ProcessContextSelection(CurrentMonthContext.AddYears(-1));
                break;

            case CenturyContext:
                ProcessContextSelection(CurrentMonthContext.AddDecades(-1));
                break;
        }
    }

    private void ProcessRightKey(bool ctrl, bool shift)
    {
        switch (DisplayDateContext)
        {
            case MonthContext:
                ProcessDateSelection(GetFocusedDate().AddDays(1), shift, ctrl);
                break;

            case YearContext:
                ProcessContextSelection(CurrentMonthContext.Add(1));
                break;

            case DecadeContext:
                ProcessContextSelection(CurrentMonthContext.AddYears(1));
                break;

            case CenturyContext:
                ProcessContextSelection(CurrentMonthContext.AddDecades(1));
                break;
        }
    }

    private void ProcessEnterKey(bool ctrl, bool shift)
    {
        switch (DisplayDateContext)
        {
            case MonthContext:
                var date = (DateTime?)GetFocusedDayButton()?.DataContext;
                if (date.HasValue)
                    ProcessDateSelection(date.Value, shift, ctrl);
                break;

            default:
                var context = _cells.Values.FirstOrDefault(x => x.IsSelected)?.DateContext;
                if (context is not null)
                    DisplayDateContext = context;
                break;
        }
    }

    private void ProcessHomeKey(bool ctrl, bool shift)
    {
        switch (DisplayDateContext)
        {
            case MonthContext:
                ProcessDateSelection(GetFocusedDate().BeginningOfMonth(), shift, ctrl);
                break;

            case YearContext:
                ProcessContextSelection(CurrentMonthContext.BeginningOfYear());
                break;

            case DecadeContext:
                ProcessContextSelection(CurrentMonthContext.BeginningOfDecade());
                break;

            case CenturyContext:
                ProcessContextSelection(CurrentMonthContext.BeginningOfCentury());
                break;
        }
    }

    private void ProcessEndKey(bool ctrl, bool shift)
    {
        switch (DisplayDateContext)
        {
            case MonthContext:
                ProcessDateSelection(GetFocusedDate().EndOfMonth(), shift, ctrl);
                break;

            case YearContext:
                ProcessContextSelection(CurrentMonthContext.EndOfYear());
                break;

            case DecadeContext:
                ProcessContextSelection(CurrentMonthContext.EndOfDecade());
                break;

            case CenturyContext:
                ProcessContextSelection(CurrentMonthContext.EndOfCentury());
                break;
        }
    }

    private void ProcessPageDownKey(bool ctrl, bool shift)
    {
        if (!ctrl && !shift)
        {
            Next();
            return;
        }

        switch (DisplayDateContext)
        {
            case MonthContext:
                if (ctrl)
                    ShowYearMode();
                else if (shift)
                    ProcessDateSelection(GetFocusedDate().AddMonths(1), shift, ctrl);

                break;

            case YearContext:
                if (ctrl)
                    ShowDecadeMode();

                break;

            case DecadeContext:
                if (ctrl)
                    ShowCenturyMode();

                break;
        }
    }

    private void ProcessPageUpKey(bool ctrl, bool shift)
    {
        if (!ctrl && !shift)
        {
            Previous();
            return;
        }

        switch (DisplayDateContext)
        {
            case MonthContext:
                if (shift)
                    ProcessDateSelection(GetFocusedDate().AddMonths(-1), shift, ctrl);

                break;

            case YearContext:
                if (ctrl)
                    ShowMonthMode();

                break;

            case DecadeContext:
                if (ctrl)
                    ShowYearMode();

                break;

            case CenturyContext:
                if (ctrl)
                    ShowDecadeMode();

                break;
        }
    }

    #endregion

    #region Focus

    private void UpdateFocus(DateTime? date = null)
    {
        if (date.HasValue)
        {
            _cells.GetOrDefault(date.Value)?.Focus();
        }
        else
        {
            switch (DisplayDateContext)
            {
                case MonthContext:
                    _cells.GetValueOrDefault(GetFocusedDate())?.Focus();
                    break;

                default:
                    _cells.Values.FirstOrDefault(x => x.IsSelected)?.Focus();
                    break;
            }
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
