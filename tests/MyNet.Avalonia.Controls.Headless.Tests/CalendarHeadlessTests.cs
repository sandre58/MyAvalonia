// -----------------------------------------------------------------------
// <copyright file="CalendarHeadlessTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.Threading;
using FluentAssertions;
using MyNet.Avalonia.Controls.Primitives;
using MyNet.Primitives.Temporal;

namespace MyNet.Avalonia.Controls.Headless.Tests;

public class CalendarHeadlessTests
{
    [AvaloniaFact]
    public void ApplyTemplate_CreatesMonthGrid()
    {
        var calendar = CreateCalendar(new(2026, 5, 15));
        HeadlessControlHost.Show(calendar, new(420, 360));

        HeadlessControlHost.FindByName<Grid>(calendar, Calendar.PartMonthGrid).Should().NotBeNull();
    }

    [AvaloniaFact]
    public void NextButton_AdvancesDisplayDateContext()
    {
        var calendar = CreateCalendar(new(2026, 5, 15));
        HeadlessControlHost.Show(calendar, new(420, 360));

        var nextButton = HeadlessControlHost.FindByName<Button>(calendar, Calendar.PartNextButton);
        nextButton.Should().NotBeNull();

        HeadlessControlHost.Click(nextButton);

        calendar.DisplayDateContext.Should().Be(new MonthContext(6, 2026));
    }

    [AvaloniaFact]
    public void SettingSelectedDate_UpdatesSelection()
    {
        var calendar = CreateCalendar(new(2026, 5, 15));
        HeadlessControlHost.Show(calendar, new(420, 360));

        var selectedDate = new DateTime(2026, 5, 20);
        calendar.SelectedDate = selectedDate;

        calendar.SelectedDate.Should().Be(selectedDate);
        calendar.SelectedDates.Should().Contain(selectedDate);
    }

    [AvaloniaFact]
    public void ReapplyTemplate_DoesNotDuplicateMonthGridChildren()
    {
        var calendar = CreateCalendar(new(2026, 5, 15));
        HeadlessControlHost.Show(calendar, new(420, 360));

        var grid = HeadlessControlHost.FindByName<Grid>(calendar, Calendar.PartMonthGrid);
        grid.Should().NotBeNull();
        var initialCount = grid!.Children.Count;
        initialCount.Should().BeGreaterThan(0);

        var theme = calendar.Theme;
        calendar.Theme = null;
        calendar.Theme = theme;
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Loaded);

        grid.Children.Count.Should().Be(initialCount);
    }

    [AvaloniaFact]
    public void LastWeekDayButtons_AreAssignedToFinalGridRow()
    {
        var calendar = CreateCalendar(new(2026, 5, 15));
        HeadlessControlHost.Show(calendar, new(420, 360));

        var grid = HeadlessControlHost.FindByName<Grid>(calendar, Calendar.PartMonthGrid);
        grid.Should().NotBeNull();

        var dayButtons = grid!.Children.OfType<CalendarDayButton>().ToList();
        dayButtons.Should().NotBeEmpty();
        dayButtons.Should().HaveCount(DateTimeHelper.DaysPerWeek * DateTimeHelper.MaxNumberOfWeeksPerMonth);
        dayButtons.Max(x => Grid.GetRow(x)).Should().Be(DateTimeHelper.MaxNumberOfWeeksPerMonth);
    }

    [AvaloniaFact]
    public void SingleRange_TapSelection_ShowsPreviewStates()
    {
        var calendar = CreateCalendar(new(2026, 5, 15));
        calendar.SelectionMode = CalendarSelectionMode.SingleRange;
        calendar.AllowTapRangeSelection = true;
        HeadlessControlHost.Show(calendar, new(420, 360));

        var grid = HeadlessControlHost.FindByName<Grid>(calendar, Calendar.PartMonthGrid);
        grid.Should().NotBeNull();

        var startDate = new DateTime(2026, 5, 10);
        var endDate = new DateTime(2026, 5, 14);
        var startButton = FindDayButton(grid!, startDate);
        var endButton = FindDayButton(grid!, endDate);
        var middleButton = FindDayButton(grid!, new DateTime(2026, 5, 12));

        HeadlessControlHost.PointerRelease(startButton);
        HeadlessControlHost.PointerEnter(endButton);

        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        startButton.IsPreviewStartDate.Should().BeTrue();
        middleButton.IsPreviewInRange.Should().BeTrue();
        endButton.IsPreviewEndDate.Should().BeTrue();

        HeadlessControlHost.PointerRelease(endButton);

        startButton.IsStartDate.Should().BeTrue();
        middleButton.IsInRange.Should().BeTrue();
        endButton.IsEndDate.Should().BeTrue();
        startButton.IsPreviewStartDate.Should().BeFalse();
        endButton.IsPreviewEndDate.Should().BeFalse();
    }

    [AvaloniaFact]
    public void SingleRange_TapPreview_RemainsStableAcrossFrames()
    {
        var calendar = CreateCalendar(new(2026, 5, 15));
        calendar.SelectionMode = CalendarSelectionMode.SingleRange;
        calendar.AllowTapRangeSelection = true;
        HeadlessControlHost.Show(calendar, new(420, 360));

        var grid = HeadlessControlHost.FindByName<Grid>(calendar, Calendar.PartMonthGrid);
        grid.Should().NotBeNull();

        var startDate = new DateTime(2026, 5, 10);
        var endDate = new DateTime(2026, 5, 14);
        var startButton = FindDayButton(grid!, startDate);
        var endButton = FindDayButton(grid!, endDate);
        var middleButton = FindDayButton(grid!, new DateTime(2026, 5, 12));

        HeadlessControlHost.PointerRelease(startButton);
        HeadlessControlHost.PointerEnter(endButton);

        for (var i = 0; i < 3; i++)
        {
            Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);
            middleButton.IsPreviewInRange.Should().BeTrue("preview should stay visible after frame " + i);
            startButton.IsPreviewStartDate.Should().BeTrue();
            endButton.IsPreviewEndDate.Should().BeTrue();
        }
    }

    [AvaloniaFact]
    public void SingleRange_KeyboardTapSelection_ValidatesOnEnterNotArrows()
    {
        var calendar = CreateCalendar(new(2026, 5, 15));
        calendar.SelectionMode = CalendarSelectionMode.SingleRange;
        calendar.AllowTapRangeSelection = true;
        var dayButtonClicks = 0;
        calendar.DayButtonClick += (_, _) => dayButtonClicks++;

        HeadlessControlHost.Show(calendar, new(420, 360));
        calendar.Focus();

        var grid = HeadlessControlHost.FindByName<Grid>(calendar, Calendar.PartMonthGrid);
        grid.Should().NotBeNull();

        var startDate = new DateTime(2026, 5, 10);
        var endDate = new DateTime(2026, 5, 14);
        calendar.MoveToDate(startDate);
        HeadlessControlHost.KeyDown(calendar, Key.Space);

        dayButtonClicks.Should().Be(1);
        calendar.SelectedDates.Should().Contain(startDate);
        calendar.SelectedDates.Should().NotContain(endDate);

        HeadlessControlHost.KeyDown(calendar, Key.Right);
        HeadlessControlHost.KeyDown(calendar, Key.Right);
        HeadlessControlHost.KeyDown(calendar, Key.Right);
        HeadlessControlHost.KeyDown(calendar, Key.Right);

        dayButtonClicks.Should().Be(1, "arrow keys must only move focus and preview, not validate");
        calendar.SelectedDates.Should().NotContain(endDate);

        HeadlessControlHost.KeyDown(calendar, Key.Space);

        dayButtonClicks.Should().Be(2);
        calendar.SelectedDates.Should().Contain(startDate);
        calendar.SelectedDates.Should().Contain(endDate);
        FindDayButton(grid!, new DateTime(2026, 5, 12)).IsInRange.Should().BeTrue();
    }

    [AvaloniaFact]
    public void SingleRange_KeyboardPreview_RemainsStableAcrossFrames()
    {
        var calendar = CreateCalendar(new(2026, 5, 15));
        calendar.SelectionMode = CalendarSelectionMode.SingleRange;
        calendar.AllowTapRangeSelection = true;
        HeadlessControlHost.Show(calendar, new(420, 360));
        calendar.Focus();

        var grid = HeadlessControlHost.FindByName<Grid>(calendar, Calendar.PartMonthGrid);
        grid.Should().NotBeNull();

        var startDate = new DateTime(2026, 5, 10);
        var endDate = new DateTime(2026, 5, 14);
        var startButton = FindDayButton(grid!, startDate);
        var endButton = FindDayButton(grid!, endDate);
        var middleButton = FindDayButton(grid!, new DateTime(2026, 5, 12));

        calendar.MoveToDate(startDate);
        HeadlessControlHost.KeyDown(calendar, Key.Space);

        for (var i = 0; i < 4; i++)
            HeadlessControlHost.KeyDown(calendar, Key.Right);

        for (var i = 0; i < 3; i++)
        {
            Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);
            startButton.IsPreviewStartDate.Should().BeTrue("preview start should stay visible after frame " + i);
            middleButton.IsPreviewInRange.Should().BeTrue("keyboard preview should stay visible after frame " + i);
            endButton.IsPreviewEndDate.Should().BeTrue("preview end should stay visible after frame " + i);
        }
    }

    [AvaloniaFact]
    public void MultipleRange_DragWithoutShift_SelectsRange()
    {
        var calendar = CreateCalendar(new(2026, 5, 15));
        calendar.SelectionMode = CalendarSelectionMode.MultipleRange;
        calendar.AllowTapRangeSelection = false;
        HeadlessControlHost.Show(calendar, new(420, 360));

        var grid = HeadlessControlHost.FindByName<Grid>(calendar, Calendar.PartMonthGrid);
        grid.Should().NotBeNull();

        var startDate = new DateTime(2026, 5, 5);
        var endDate = new DateTime(2026, 5, 9);
        var startButton = FindDayButton(grid!, startDate);
        var endButton = FindDayButton(grid!, endDate);

        HeadlessControlHost.PointerPress(startButton);
        HeadlessControlHost.PointerEnter(endButton);
        HeadlessControlHost.PointerRelease(endButton);

        calendar.SelectedDates.Should().Contain(startDate);
        calendar.SelectedDates.Should().Contain(endDate);
        calendar.SelectedDates.Should().Contain(new DateTime(2026, 5, 7));
    }

    [AvaloniaFact]
    public void SingleRange_WithoutTap_NoPreviewUntilKeyboardNavigation()
    {
        var calendar = CreateCalendar(new(2026, 5, 15));
        calendar.SelectionMode = CalendarSelectionMode.SingleRange;
        calendar.AllowTapRangeSelection = false;
        HeadlessControlHost.Show(calendar, new(420, 360));
        calendar.Focus();

        var grid = HeadlessControlHost.FindByName<Grid>(calendar, Calendar.PartMonthGrid);
        grid.Should().NotBeNull();

        var startDate = new DateTime(2026, 5, 10);
        var middleDate = new DateTime(2026, 5, 12);
        calendar.MoveToDate(startDate);
        HeadlessControlHost.KeyDown(calendar, Key.Space);

        FindDayButton(grid!, middleDate).IsPreviewInRange.Should().BeFalse("mouse-style preview must not appear without keyboard navigation");

        HeadlessControlHost.KeyDown(calendar, Key.Right);
        HeadlessControlHost.KeyDown(calendar, Key.Right);

        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        var middleButton = FindDayButton(grid!, middleDate);
        middleButton.IsPreviewInRange.Should().BeFalse("non-tap keyboard preview with anchor is a single focused cell");
        middleButton.IsPreviewStartDate.Should().BeFalse("single-cell keyboard navigation uses focus, not preview pseudos");
        middleButton.IsPreviewEndDate.Should().BeFalse();
        middleButton.IsFocused.Should().BeTrue();
    }

    [AvaloniaFact]
    public void SingleRange_WithTap_KeyboardPreviewAfterArrowsWithoutShift()
    {
        var calendar = CreateCalendar(new(2026, 5, 15));
        calendar.SelectionMode = CalendarSelectionMode.SingleRange;
        calendar.AllowTapRangeSelection = true;
        HeadlessControlHost.Show(calendar, new(420, 360));
        calendar.Focus();

        var grid = HeadlessControlHost.FindByName<Grid>(calendar, Calendar.PartMonthGrid);
        grid.Should().NotBeNull();

        var startDate = new DateTime(2026, 5, 10);
        var endDate = new DateTime(2026, 5, 14);
        calendar.MoveToDate(startDate);
        HeadlessControlHost.KeyDown(calendar, Key.Space);

        FindDayButton(grid!, endDate).IsPreviewInRange.Should().BeFalse();

        for (var i = 0; i < 4; i++)
            HeadlessControlHost.KeyDown(calendar, Key.Right);

        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        var middleButton = FindDayButton(grid!, new DateTime(2026, 5, 12));
        middleButton.IsPreviewInRange.Should().BeTrue();
        FindDayButton(grid!, endDate).IsPreviewEndDate.Should().BeTrue();
        calendar.SelectedDates.Should().NotContain(endDate);
    }

    [AvaloniaFact]
    public void SingleRange_KeyboardCommit_WorksAfterReleasingShift()
    {
        var calendar = CreateCalendar(new(2026, 5, 15));
        calendar.SelectionMode = CalendarSelectionMode.SingleRange;
        calendar.AllowTapRangeSelection = false;
        var dayButtonClicks = 0;
        calendar.DayButtonClick += (_, _) => dayButtonClicks++;

        HeadlessControlHost.Show(calendar, new(420, 360));
        calendar.Focus();

        var startDate = new DateTime(2026, 5, 10);
        var endDate = new DateTime(2026, 5, 14);
        calendar.MoveToDate(startDate);
        HeadlessControlHost.KeyDown(calendar, Key.Space);

        for (var i = 0; i < 4; i++)
            HeadlessControlHost.KeyDown(calendar, Key.Right, KeyModifiers.Shift);

        HeadlessControlHost.KeyUp(calendar, Key.LeftShift);
        HeadlessControlHost.KeyDown(calendar, Key.Enter);

        dayButtonClicks.Should().Be(2);
        calendar.SelectedDates.Should().Contain(startDate);
        calendar.SelectedDates.Should().Contain(endDate);
    }

    [AvaloniaFact]
    public void SingleDate_KeyboardArrows_PreviewWithoutCommit()
    {
        var calendar = CreateCalendar(new(2026, 5, 15));
        calendar.SelectedDate = new DateTime(2026, 5, 15);
        HeadlessControlHost.Show(calendar, new(420, 360));
        calendar.Focus();

        var previewDate = new DateTime(2026, 5, 20);
        calendar.MoveToDate(new DateTime(2026, 5, 15));
        HeadlessControlHost.KeyDown(calendar, Key.Right);
        HeadlessControlHost.KeyDown(calendar, Key.Right);
        HeadlessControlHost.KeyDown(calendar, Key.Right);
        HeadlessControlHost.KeyDown(calendar, Key.Right);
        HeadlessControlHost.KeyDown(calendar, Key.Right);

        calendar.SelectedDate.Should().Be(new DateTime(2026, 5, 15));

        HeadlessControlHost.KeyDown(calendar, Key.Enter);
        calendar.SelectedDate.Should().Be(previewDate);
    }

    [AvaloniaFact]
    public void SingleDate_KeyboardPreview_PersistsAfterPointerMove()
    {
        var calendar = CreateCalendar(new(2026, 5, 15));
        calendar.SelectedDate = new DateTime(2026, 5, 15);
        HeadlessControlHost.Show(calendar, new(420, 360));
        calendar.Focus();

        var grid = HeadlessControlHost.FindByName<Grid>(calendar, Calendar.PartMonthGrid);
        grid.Should().NotBeNull();

        var previewDate = new DateTime(2026, 5, 20);
        calendar.MoveToDate(new DateTime(2026, 5, 15));
        HeadlessControlHost.KeyDown(calendar, Key.Right);
        HeadlessControlHost.KeyDown(calendar, Key.Right);
        HeadlessControlHost.KeyDown(calendar, Key.Right);
        HeadlessControlHost.KeyDown(calendar, Key.Right);
        HeadlessControlHost.KeyDown(calendar, Key.Right);

        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        var previewButton = FindDayButton(grid!, previewDate);
        previewButton.IsFocused.Should().BeTrue();
        previewButton.IsPreviewStartDate.Should().BeFalse("single-date keyboard preview uses focus, not preview pseudos");
        previewButton.IsPreviewEndDate.Should().BeFalse();

        HeadlessControlHost.PointerEnter(FindDayButton(grid!, new DateTime(2026, 5, 12)));

        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        previewButton.IsFocused.Should().BeTrue("keyboard focus must survive unrelated pointer movement");
        previewButton.IsPreviewStartDate.Should().BeFalse();
        previewButton.IsPreviewEndDate.Should().BeFalse();
    }

    [AvaloniaFact]
    public void SingleRange_CommittedRange_MiddleCellsAreNotSelected()
    {
        var calendar = CreateCalendar(new(2026, 5, 15));
        calendar.SelectionMode = CalendarSelectionMode.SingleRange;
        calendar.AllowTapRangeSelection = true;
        HeadlessControlHost.Show(calendar, new(420, 360));

        var grid = HeadlessControlHost.FindByName<Grid>(calendar, Calendar.PartMonthGrid);
        grid.Should().NotBeNull();

        var startDate = new DateTime(2026, 5, 10);
        var middleDate = new DateTime(2026, 5, 12);
        var endDate = new DateTime(2026, 5, 14);

        HeadlessControlHost.PointerRelease(FindDayButton(grid!, startDate));
        HeadlessControlHost.PointerRelease(FindDayButton(grid!, endDate));

        var startButton = FindDayButton(grid!, startDate);
        var middleButton = FindDayButton(grid!, middleDate);
        var endButton = FindDayButton(grid!, endDate);

        startButton.IsStartDate.Should().BeTrue();
        endButton.IsEndDate.Should().BeTrue();
        middleButton.IsInRange.Should().BeTrue();

        startButton.IsSelected.Should().BeFalse();
        middleButton.IsSelected.Should().BeFalse();
        endButton.IsSelected.Should().BeFalse();
    }

    [AvaloniaFact]
    public void DayButtons_AreKeyboardFocusable()
    {
        var calendar = CreateCalendar(new(2026, 5, 15));
        calendar.SelectionMode = CalendarSelectionMode.SingleRange;
        HeadlessControlHost.Show(calendar, new(420, 360));
        calendar.Focus();

        var grid = HeadlessControlHost.FindByName<Grid>(calendar, Calendar.PartMonthGrid);
        grid.Should().NotBeNull();

        foreach (var dayButton in grid!.Children.OfType<CalendarDayButton>())
            dayButton.Focusable.Should().BeTrue();
    }

    private static CalendarDayButton FindDayButton(Grid grid, DateTime date) =>
        grid.Children.OfType<CalendarDayButton>().Single(x => x.DataContext is DateTime d && d == date);

    private static Calendar CreateCalendar(DateTime displayDate)
    {
        HeadlessTestApp.EnsureGlobalizationServices();

        return new()
        {
            DisplayDate = displayDate,
            SelectionMode = CalendarSelectionMode.SingleDate
        };
    }
}
