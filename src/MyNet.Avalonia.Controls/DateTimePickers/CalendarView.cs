// -----------------------------------------------------------------------
// <copyright file="CalendarView.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using MyNet.Avalonia.Controls.DateTimePickers;
using MyNet.Avalonia.Controls.Primitives;
using MyNet.Avalonia.Extensions;
using MyNet.Humanizer;
using MyNet.Utilities;
using MyNet.Utilities.Helpers;
using MyNet.Utilities.Localization;
using Calendar = System.Globalization.Calendar;

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
[PseudoClasses(PseudoClassName.Month)]
public class CalendarView : TemplatedControl
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

    private const string ShortestDayName = "ShortestDayName";

    internal static readonly DirectProperty<CalendarView, CalendarViewMode> ModeProperty =
        AvaloniaProperty.RegisterDirect<CalendarView, CalendarViewMode>(
            nameof(Mode), o => o.Mode, (o, v) => o.Mode = v);

    public static readonly StyledProperty<bool> IsTodayHighlightedProperty =
        DatePickerBase.IsTodayHighlightedProperty.AddOwner<CalendarView>();

    public static readonly StyledProperty<DayOfWeek> FirstDayOfWeekProperty =
        DatePickerBase.FirstDayOfWeekProperty.AddOwner<CalendarView>();

    private readonly Calendar _calendar = new GregorianCalendar();
    private Button? _fastNextButton;

    private Button? _fastPreviousButton;

    private Button? _headerButton;
    private Button? _monthButton;
    private Grid? _monthGrid;
    private Button? _nextButton;
    private Button? _previousButton;
    private Button? _yearButton;
    private Grid? _yearGrid;

    private DateTime? _start;
    private DateTime? _end;
    private DateTime? _previewStart;
    private DateTime? _previewEnd;

    static CalendarView()
    {
        _ = FirstDayOfWeekProperty.Changed.AddClassHandler<CalendarView, DayOfWeek>((view, args) =>
            view.OnFirstDayOfWeekChanged(args));
        _ = ModeProperty.Changed.AddClassHandler<CalendarView, CalendarViewMode>((view, args) => view.PseudoClasses.Set(PseudoClassName.Month, args.NewValue.Value == CalendarViewMode.Month));
        _ = ContextDateProperty.Changed.AddClassHandler<CalendarView, CalendarContext>((view, args) =>
            view.OnContextDateChanged(args));
    }

    public CalendarView() => GlobalizationService.Current.CultureChanged += (_, _) =>
    {
        UpdateHeaderButtons();
        UpdateDayButtons();
        UpdateYearButtons();
        SetDayTitles();
    };

    private void OnContextDateChanged(AvaloniaPropertyChangedEventArgs<CalendarContext> args)
    {
        if (!_dateContextSyncing)
            ContextDateChanged?.Invoke(this, args.NewValue.Value);
    }

    internal CalendarViewMode Mode
    {
        get;
        set => SetAndRaise(ModeProperty, ref field, value);
    }

    public static readonly DirectProperty<CalendarView, CalendarContext> ContextDateProperty = AvaloniaProperty.RegisterDirect<CalendarView, CalendarContext>(
        nameof(ContextDate), o => o.ContextDate, (o, v) => o.ContextDate = v);

    public CalendarContext ContextDate
    {
        get;
        internal set => SetAndRaise(ContextDateProperty, ref field, value);
    }

        = new();

    public bool IsTodayHighlighted
    {
        get => GetValue(IsTodayHighlightedProperty);
        set => SetValue(IsTodayHighlightedProperty, value);
    }

    public DayOfWeek FirstDayOfWeek
    {
        get => GetValue(FirstDayOfWeekProperty);
        set => SetValue(FirstDayOfWeekProperty, value);
    }

    public static readonly StyledProperty<ITemplate<Control>?> DayTitleTemplateProperty =
        AvaloniaProperty.Register<CalendarView, ITemplate<Control>?>(
            nameof(DayTitleTemplate),
            defaultBindingMode: BindingMode.OneTime);

    public ITemplate<Control>? DayTitleTemplate
    {
        get => GetValue(DayTitleTemplateProperty);
        set => SetValue(DayTitleTemplateProperty, value);
    }

    public static readonly RoutedEvent<CalendarDayButtonEventArgs> DateSelectedEvent =
        RoutedEvent.Register<TimePickerPresenter, CalendarDayButtonEventArgs>(
            nameof(DateSelected), RoutingStrategies.Bubble);

    public event EventHandler<CalendarDayButtonEventArgs> DateSelected
    {
        add => AddHandler(DateSelectedEvent, value);
        remove => RemoveHandler(DateSelectedEvent, value);
    }

    public event EventHandler<CalendarDayButtonEventArgs>? DatePreviewed;

    internal event EventHandler<CalendarContext>? ContextDateChanged;

    private void OnFirstDayOfWeekChanged(AvaloniaPropertyChangedEventArgs<DayOfWeek> args)
    {
        UpdateMonthViewHeader(args.NewValue.Value);
        UpdateDayButtons();
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        Button.ClickEvent.RemoveHandler(OnHeaderYearButtonClick, _yearButton);
        Button.ClickEvent.RemoveHandler(OnHeaderMonthButtonClick, _monthButton);
        Button.ClickEvent.RemoveHandler(OnHeaderButtonClick, _headerButton);
        Button.ClickEvent.RemoveHandler(OnFastPrevious, _fastPreviousButton);
        Button.ClickEvent.RemoveHandler(OnPrevious, _previousButton);
        Button.ClickEvent.RemoveHandler(OnNext, _nextButton);
        Button.ClickEvent.RemoveHandler(OnFastNext, _fastNextButton);

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
        Button.ClickEvent.AddHandler(OnFastPrevious, _fastPreviousButton);
        Button.ClickEvent.AddHandler(OnPrevious, _previousButton);
        Button.ClickEvent.AddHandler(OnNext, _nextButton);
        Button.ClickEvent.AddHandler(OnFastNext, _fastNextButton);

        ContextDate = new CalendarContext(DateTime.Today.Year, DateTime.Today.Month);
        PseudoClasses.Set(PseudoClassName.Month, Mode == CalendarViewMode.Month);
        InitializeGridButtons();
        UpdateDayButtons();
        UpdateYearButtons();
    }

    private void OnFastNext(object? sender, RoutedEventArgs e)
    {
        if (Mode == CalendarViewMode.Month)
        {
            ContextDate = ContextDate.With(year: ContextDate.Year + 1);
            UpdateDayButtons();
        }
    }

    private void OnNext(object? sender, RoutedEventArgs e)
    {
        if (Mode == CalendarViewMode.Month)
        {
            ContextDate = ContextDate.NextMonth();
            UpdateDayButtons();
        }
        else if (Mode == CalendarViewMode.Year)
        {
            ContextDate = ContextDate.NextYear();
            UpdateYearButtons();
        }
        else if (Mode == CalendarViewMode.Decade)
        {
            ContextDate = ContextDate.With(startYear: ContextDate.StartYear + 10, endYear: ContextDate.EndYear + 10);
            UpdateYearButtons();
        }
        else if (Mode == CalendarViewMode.Century)
        {
            ContextDate = ContextDate.With(startYear: ContextDate.StartYear + 100, endYear: ContextDate.EndYear + 100);
            UpdateYearButtons();
        }
    }

    private void OnPrevious(object? sender, RoutedEventArgs e)
    {
        if (Mode == CalendarViewMode.Month)
        {
            ContextDate = ContextDate.PreviousMonth();
            UpdateDayButtons();
        }
        else if (Mode == CalendarViewMode.Year)
        {
            ContextDate = ContextDate.With(year: ContextDate.Year - 1);
            UpdateYearButtons();
        }
        else if (Mode == CalendarViewMode.Decade)
        {
            ContextDate = ContextDate.With(startYear: ContextDate.StartYear - 10, endYear: ContextDate.EndYear - 10);
            UpdateYearButtons();
        }
        else if (Mode == CalendarViewMode.Century)
        {
            ContextDate = ContextDate.With(startYear: ContextDate.StartYear - 100, endYear: ContextDate.EndYear - 100);
            UpdateYearButtons();
        }
    }

    private void OnFastPrevious(object? sender, RoutedEventArgs e)
    {
        if (Mode == CalendarViewMode.Month)
        {
            ContextDate = ContextDate.PreviousYear();
            UpdateDayButtons();
        }
    }

    private void OnHeaderButtonClick(object? sender, RoutedEventArgs e)
    {
        // Header button should be hidden in Month mode.
        if (Mode == CalendarViewMode.Month) return;
        if (Mode == CalendarViewMode.Year)
        {
            Mode = CalendarViewMode.Decade;
            var range = DateTimeHelper.GetDecade(ContextDate.Year!.Value);
            _dateContextSyncing = true;
            ContextDate = ContextDate.With(startYear: range.Start, endYear: range.End);
            _dateContextSyncing = false;
            UpdateYearButtons();
            return;
        }

        if (Mode == CalendarViewMode.Decade)
        {
            Mode = CalendarViewMode.Century;
            var range = DateTimeHelper.GetCentury(ContextDate.StartYear!.Value);
            _dateContextSyncing = true;
            ContextDate = ContextDate.With(startYear: range.Start, endYear: range.End);
            _dateContextSyncing = false;
            UpdateYearButtons();
        }
    }

    /// <summary>
    ///     Generate Buttons and labels for MonthView.
    ///     Generate Buttons for YearView.
    ///     This method should be called only once.
    /// </summary>
    private void InitializeGridButtons()
    {
        // Generate Day titles (Sun, Mon, Tue, Wed, Thu, Fri, Sat) based on FirstDayOfWeek and culture.
        const int count = 7 + (7 * 7);
        var children = new List<Control>(count);
        var dayOfWeek = (int)FirstDayOfWeek;
        var info = DateTimeHelper.GetCurrentDateTimeFormatInfo();
        for (var i = 0; i < 7; i++)
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

        // Generate day buttons.
        for (var i = 2; i < DateTimeHelper.MaxNumberOfWeeksPerMonth() + 2; i++)
        {
            for (var j = 0; j < DateTimeHelper.NumberOfDaysInWeek(); j++)
            {
                var cell = new CalendarDayButton();
                _ = cell.SetValue(Grid.RowProperty, i);
                _ = cell.SetValue(Grid.ColumnProperty, j);
                cell.AddHandler(CalendarDayButton.DateSelectedEvent, OnCellDateSelected);
                cell.AddHandler(CalendarDayButton.DatePreviewedEvent, OnCellDatePreviewed);
                children.Add(cell);
            }
        }

        _monthGrid?.Children.AddRange(children);

        // Generate month/year buttons.
        for (var i = 0; i < 12; i++)
        {
            var button = new CalendarYearButton();
            Grid.SetRow(button, i / 3);
            Grid.SetColumn(button, i % 3);
            button.AddHandler(CalendarYearButton.ItemSelectedEvent, OnYearItemSelected);
            _yearGrid?.Children.Add(button);
        }
    }

    private void SetDayTitles()
    {
        if (_monthGrid is null || Mode != CalendarViewMode.Month) return;

        for (var childIndex = 0; childIndex < DateTimeHelper.NumberOfDaysInWeek(); childIndex++)
        {
            var daytitle = _monthGrid.Children[childIndex];
            daytitle.DataContext = DateTimeHelper.GetCurrentDateTimeFormatInfo().ShortestDayNames[(childIndex + (int)FirstDayOfWeek) % DateTimeHelper.NumberOfDaysInWeek()];
        }
    }

    internal void UpdateDayButtons()
    {
        if (_monthGrid is null || Mode != CalendarViewMode.Month) return;
        var children = _monthGrid.Children;
        var info = DateTimeHelper.GetCurrentDateTimeFormatInfo();
        var date = new DateTime(ContextDate.Year ?? ContextDate.StartYear!.Value, ContextDate.Month!.Value, 1);
        var dayBefore = PreviousMonthDays(date);
        var dateToSet = date.FirstDayOfMonth().AddDays(-dayBefore);
        for (var i = 7; i < children.Count; i++)
        {
            var day = dateToSet;
            if (children[i] is not CalendarDayButton cell) continue;
            cell.DataContext = day;
            if (IsTodayHighlighted) cell.IsToday = day == DateTime.Today;
            cell.Content = day.Day.ToString(info);
            dateToSet = dateToSet.AddDays(1);
        }

        FadeOutDayButtons();
        MarkDates(_start, _end, _previewStart, _previewEnd);
        UpdateHeaderButtons();
        SetDayTitles();
    }

    private void UpdateYearButtons()
    {
        if (_yearGrid is null) return;
        var mode = Mode;
        var contextDate = ContextDate;
        if (mode == CalendarViewMode.Century && contextDate.StartYear.HasValue)
        {
            var range = DateTimeHelper.GetCentury(contextDate.StartYear.Value);
            var start = range.Start - 10;
            for (var i = 0; i < 12; i++)
            {
                var child = _yearGrid.Children[i] as CalendarYearButton;
                child?.SetContext(CalendarViewMode.Century,
                    new CalendarContext(startYear: start, endYear: start + 10));
                start += 10;
            }
        }
        else if (mode == CalendarViewMode.Decade && contextDate.StartYear.HasValue)
        {
            var range = DateTimeHelper.GetDecade(contextDate.StartYear.Value);
            var year = range.Start - 1;
            for (var i = 0; i < 12; i++)
            {
                var child = _yearGrid.Children[i] as CalendarYearButton;
                child?.SetContext(CalendarViewMode.Decade,
                    new CalendarContext(year: year));
                year++;
            }
        }
        else if (mode == CalendarViewMode.Year)
        {
            for (var i = 0; i < 12; i++)
            {
                var child = _yearGrid.Children[i] as CalendarYearButton;
                child?.SetContext(CalendarViewMode.Year, new CalendarContext(month: i + 1));
            }
        }

        UpdateHeaderButtons();
    }

    private void FadeOutDayButtons()
    {
        if (_monthGrid is null) return;
        var children = _monthGrid.Children;
        for (var i = 7; i < children.Count; i++)
        {
            if (children[i] is CalendarDayButton { DataContext: DateTime d } button)
                button.IsNotCurrentMonth = d.Month != ContextDate.Month;
        }
    }

    private void UpdateMonthViewHeader(DayOfWeek day)
    {
        var dayOfWeek = (int)day;
        var info = DateTimeHelper.GetCurrentDateTimeFormatInfo();
        var texts = _monthGrid?.Children.Where(a => a is TextBlock { Tag: ShortestDayName }).ToList();
        if (texts is not null)
        {
            for (var i = 0; i < 7; i++)
            {
                var d = (dayOfWeek + i) % DateTimeHelper.NumberOfDaysInWeek();
                _ = texts[i].SetValue(TextBlock.TextProperty, info.ShortestDayNames[d]);
                _ = texts[i].SetValue(HorizontalAlignmentProperty, HorizontalAlignment.Center);
                _ = texts[i].SetValue(Grid.RowProperty, 0);
                _ = texts[i].SetValue(Grid.ColumnProperty, i);
            }
        }
    }

    private int PreviousMonthDays(DateTime date)
    {
        var firstDay = date.FirstDayOfMonth();
        var dayOfWeek = _calendar.GetDayOfWeek(firstDay);
        var firstDayOfWeek = FirstDayOfWeek;
        var i = (dayOfWeek - firstDayOfWeek + DateTimeHelper.NumberOfDaysInWeek()) % DateTimeHelper.NumberOfDaysInWeek();
        return i == 0 ? DateTimeHelper.NumberOfDaysInWeek() : i;
    }

    private void OnCellDatePreviewed(object? sender, CalendarDayButtonEventArgs e) => DatePreviewed?.Invoke(this, e);

    private void OnCellDateSelected(object? sender, CalendarDayButtonEventArgs e)
    {
        if (e.Date.HasValue && e.Date.Value.Month != ContextDate.Month)
        {
            ContextDate = ContextDate.With(year: e.Date.Value.Year, month: e.Date.Value.Month);
            UpdateDayButtons();
        }

        RaiseEvent(new CalendarDayButtonEventArgs(e.Date) { RoutedEvent = DateSelectedEvent, Source = this });
    }

    private void OnHeaderMonthButtonClick(object? sender, RoutedEventArgs e)
    {
        SetCurrentValue(ModeProperty, CalendarViewMode.Year);
        UpdateYearButtons();
    }

    private void OnHeaderYearButtonClick(object? sender, RoutedEventArgs e)
    {
        if (_yearGrid is null) return;
        SetCurrentValue(ModeProperty, CalendarViewMode.Decade);
        var range = DateTimeHelper.GetDecade(ContextDate.Year!.Value);
        _dateContextSyncing = true;
        ContextDate = ContextDate.With(startYear: range.Start, endYear: range.End);
        _dateContextSyncing = false;
        UpdateYearButtons();
    }

    private void OnYearItemSelected(object? sender, CalendarYearButtonEventArgs e)
    {
        if (_yearGrid is null) return;
        if (Mode == CalendarViewMode.Century)
        {
            Mode = CalendarViewMode.Decade;
            ContextDate = e.Context.With(year: null);
        }
        else if (Mode == CalendarViewMode.Decade)
        {
            Mode = CalendarViewMode.Year;
            ContextDate = e.Context.Clone();
        }
        else if (Mode == CalendarViewMode.Year)
        {
            Mode = CalendarViewMode.Month;
            ContextDate = ContextDate.With(null, e.Context.Month);
            UpdateDayButtons();
        }
        else if (Mode == CalendarViewMode.Month)
        {
            return;
        }

        UpdateHeaderButtons();
        UpdateYearButtons();
    }

    private void UpdateHeaderButtons()
    {
        if (Mode == CalendarViewMode.Century)
        {
            IsVisibleProperty.SetValue(true, _headerButton, _yearGrid);
            IsVisibleProperty.SetValue(false, _yearButton, _monthButton, _monthGrid, _fastPreviousButton, _fastNextButton);
            _ = _headerButton?.SetValue(ContentControl.ContentProperty, ContextDate.StartYear + "-" + ContextDate.EndYear);
        }
        else if (Mode == CalendarViewMode.Decade)
        {
            IsVisibleProperty.SetValue(true, _headerButton, _yearGrid);
            IsVisibleProperty.SetValue(false, _yearButton, _monthButton, _monthGrid, _fastPreviousButton, _fastNextButton);
            _ = _headerButton?.SetValue(ContentControl.ContentProperty,
                ContextDate.StartYear + "-" + ContextDate.EndYear);
        }
        else if (Mode == CalendarViewMode.Year)
        {
            IsVisibleProperty.SetValue(true, _headerButton, _yearGrid);
            IsVisibleProperty.SetValue(false, _yearButton, _monthButton, _monthGrid, _fastPreviousButton, _fastNextButton);
            _ = _headerButton?.SetValue(ContentControl.ContentProperty, ContextDate.Year);
        }
        else if (Mode == CalendarViewMode.Month)
        {
            IsVisibleProperty.SetValue(false, _headerButton, _yearGrid);
            IsVisibleProperty.SetValue(true, _yearButton, _monthButton, _monthGrid, _fastPreviousButton, _fastNextButton);
            _ = _yearButton?.SetValue(ContentControl.ContentProperty, ContextDate.Year);
            _ = _monthButton?.SetValue(ContentControl.ContentProperty, DateTimeHelper.GetCurrentDateTimeFormatInfo().AbbreviatedMonthNames[ContextDate.Month - 1 ?? 0].ToTitle());
        }

        var canForward = !(ContextDate.EndYear <= 0) && !(ContextDate.Year <= 0);
        var canNext = !(ContextDate.StartYear > 9999) && !(ContextDate.EndYear > 9999);
        IsEnabledProperty.SetValue(canForward, _previousButton, _fastPreviousButton);
        IsEnabledProperty.SetValue(canNext, _nextButton, _fastNextButton);
    }

    public void MarkDates(DateTime? startDate = null, DateTime? endDate = null, DateTime? previewStartDate = null, DateTime? previewEndDate = null)
    {
        _start = startDate;
        _end = endDate;
        _previewStart = previewStartDate;
        _previewEnd = previewEndDate;
        if (_monthGrid?.Children is null) return;
        var start = startDate ?? DateTime.MaxValue;
        var end = endDate ?? DateTime.MinValue;
        var previewStart = previewStartDate ?? DateTime.MaxValue;
        var previewEnd = previewEndDate ?? DateTime.MinValue;
        var rangeStart = DateTimeHelper.Min(start, previewStart);
        var rangeEnd = DateTimeHelper.Max(end, previewEnd);
        foreach (var child in _monthGrid.Children)
        {
            if (child is not CalendarDayButton { DataContext: DateTime d } button) continue;
            button.ResetSelection();
            if (d.Month != ContextDate.Month) continue;
            if (d < rangeEnd && d > rangeStart) button.IsInRange = true;
            if (d == previewStart) button.IsPreviewStartDate = true;
            if (d == previewEnd) button.IsPreviewEndDate = true;
            if (d == startDate) button.IsStartDate = true;
            if (d == endDate) button.IsEndDate = true;
            if (d == startDate && d == endDate) button.IsSelected = true;
        }
    }

    public void ClearSelection(bool start = true, bool end = true)
    {
        if (start)
        {
            _previewStart = null;
            _start = null;
        }

        if (end)
        {
            _previewEnd = null;
            _end = null;
        }

        if (_monthGrid?.Children is null) return;
        foreach (var child in _monthGrid.Children)
        {
            if (child is not CalendarDayButton button) continue;
            if (start)
            {
                button.IsPreviewStartDate = false;
                button.IsStartDate = false;
            }

            if (end)
            {
                button.IsEndDate = false;
                button.IsInRange = false;
            }

            button.IsPreviewEndDate = false;
        }

        UpdateDayButtons();
    }

    protected override void OnPointerExited(PointerEventArgs e)
    {
        base.OnPointerExited(e);
        DatePreviewed?.Invoke(this, new CalendarDayButtonEventArgs(null));
    }

    private bool _dateContextSyncing;

    internal void SyncContextDate(CalendarContext? context)
    {
        if (context is null) return;
        _dateContextSyncing = true;
        ContextDate = context;
        _dateContextSyncing = false;
        UpdateDayButtons();
        UpdateYearButtons();
    }
}
