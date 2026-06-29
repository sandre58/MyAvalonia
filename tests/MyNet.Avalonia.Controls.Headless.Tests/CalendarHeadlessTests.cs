// -----------------------------------------------------------------------
// <copyright file="CalendarHeadlessTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using Avalonia;
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

        startButton.IsEndDate.Should().BeFalse("anchor must lose end cap while extending preview to the right");
        startButton.IsStartDate.Should().BeTrue();

        HeadlessControlHost.PointerRelease(endButton);

        startButton.IsStartDate.Should().BeTrue();
        middleButton.IsInRange.Should().BeTrue();
        endButton.IsEndDate.Should().BeTrue();
        startButton.IsPreviewStartDate.Should().BeFalse();
        endButton.IsPreviewEndDate.Should().BeFalse();
    }

    [AvaloniaFact]
    public void SingleRange_TapPreview_AnchorCap_ReversedExtension_ClearsStartDate()
    {
        var calendar = CreateCalendar(new(2026, 5, 15));
        calendar.SelectionMode = CalendarSelectionMode.SingleRange;
        calendar.AllowTapRangeSelection = true;
        HeadlessControlHost.Show(calendar, new(420, 360));

        var grid = HeadlessControlHost.FindByName<Grid>(calendar, Calendar.PartMonthGrid);
        grid.Should().NotBeNull();

        var anchorDate = new DateTime(2026, 5, 14);
        var previewEndDate = new DateTime(2026, 5, 10);
        var anchorButton = FindDayButton(grid!, anchorDate);
        var previewEndButton = FindDayButton(grid!, previewEndDate);

        HeadlessControlHost.PointerRelease(anchorButton);
        HeadlessControlHost.PointerEnter(previewEndButton);
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        anchorButton.IsPreviewEndDate.Should().BeTrue();
        previewEndButton.IsPreviewStartDate.Should().BeTrue();
        anchorButton.IsStartDate.Should().BeFalse("anchor must lose start cap while extending preview to the left");
        anchorButton.IsEndDate.Should().BeTrue();
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
        HeadlessControlHost.PointerMove(endButton, leftButtonPressed: true);
        HeadlessControlHost.PointerRelease(endButton);

        calendar.SelectedDates.Should().Contain(startDate);
        calendar.SelectedDates.Should().Contain(endDate);
        calendar.SelectedDates.Should().Contain(new DateTime(2026, 5, 7));
    }

    [AvaloniaFact]
    public void MultipleRange_WithoutTap_CtrlClickTogglesWhenReleaseLosesModifier()
    {
        var calendar = CreateCalendar(new(2026, 5, 15));
        calendar.SelectionMode = CalendarSelectionMode.MultipleRange;
        calendar.AllowTapRangeSelection = false;
        HeadlessControlHost.Show(calendar, new(420, 360));

        var grid = HeadlessControlHost.FindByName<Grid>(calendar, Calendar.PartMonthGrid);
        grid.Should().NotBeNull();

        var toggleDate = new DateTime(2026, 5, 10);
        var toggleButton = FindDayButton(grid!, toggleDate);

        HeadlessControlHost.PointerPress(toggleButton, KeyModifiers.Control);
        HeadlessControlHost.PointerRelease(toggleButton);
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        calendar.SelectedDates.Should().Contain(toggleDate);

        HeadlessControlHost.PointerPress(toggleButton, KeyModifiers.Control);
        HeadlessControlHost.PointerRelease(toggleButton);
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        calendar.SelectedDates.Should().NotContain(toggleDate);
    }

    [AvaloniaFact]
    public void SingleRange_WithoutTap_ClickThenDragCommit_StartCellKeepsStartCapOnly()
    {
        var calendar = CreateCalendar(new(2026, 5, 15));
        calendar.SelectionMode = CalendarSelectionMode.SingleRange;
        calendar.AllowTapRangeSelection = false;
        HeadlessControlHost.Show(calendar, new(420, 360));

        var grid = HeadlessControlHost.FindByName<Grid>(calendar, Calendar.PartMonthGrid);
        grid.Should().NotBeNull();

        var startDate = new DateTime(2026, 5, 10);
        var endDate = new DateTime(2026, 5, 14);
        var startButton = FindDayButton(grid!, startDate);
        var endButton = FindDayButton(grid!, endDate);

        HeadlessControlHost.PointerPress(startButton);
        HeadlessControlHost.PointerRelease(startButton);
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        startButton.IsStartDate.Should().BeTrue();
        startButton.IsEndDate.Should().BeTrue("first click commits a single-day range cap");

        HeadlessControlHost.PointerPress(startButton);
        HeadlessControlHost.PointerMove(endButton, leftButtonPressed: true);
        HeadlessControlHost.PointerRelease(endButton);
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        startButton.IsStartDate.Should().BeTrue();
        startButton.IsEndDate.Should().BeFalse("committed range start must not keep single-day end cap");
        endButton.IsEndDate.Should().BeTrue();
        FindDayButton(grid!, new DateTime(2026, 5, 12)).IsInRange.Should().BeTrue();
    }

    [AvaloniaFact]
    public void MultipleRange_WithoutTap_CtrlClickToggles()
    {
        var calendar = CreateCalendar(new(2026, 5, 15));
        calendar.SelectionMode = CalendarSelectionMode.MultipleRange;
        calendar.AllowTapRangeSelection = false;
        HeadlessControlHost.Show(calendar, new(420, 360));

        var grid = HeadlessControlHost.FindByName<Grid>(calendar, Calendar.PartMonthGrid);
        grid.Should().NotBeNull();

        var toggleDate = new DateTime(2026, 5, 10);
        var toggleButton = FindDayButton(grid!, toggleDate);

        HeadlessControlHost.PointerPress(toggleButton, KeyModifiers.Control);
        HeadlessControlHost.PointerRelease(toggleButton, KeyModifiers.Control);
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        calendar.SelectedDates.Should().Contain(toggleDate);

        HeadlessControlHost.PointerPress(toggleButton, KeyModifiers.Control);
        HeadlessControlHost.PointerRelease(toggleButton, KeyModifiers.Control);
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        calendar.SelectedDates.Should().NotContain(toggleDate);
    }

    [AvaloniaFact]
    public void MultipleRange_WithoutTap_CtrlClickTogglesMiddleOfRange()
    {
        var calendar = CreateCalendar(new(2026, 5, 15));
        calendar.SelectionMode = CalendarSelectionMode.MultipleRange;
        calendar.AllowTapRangeSelection = false;
        HeadlessControlHost.Show(calendar, new(420, 360));

        var grid = HeadlessControlHost.FindByName<Grid>(calendar, Calendar.PartMonthGrid);
        grid.Should().NotBeNull();

        var startDate = new DateTime(2026, 5, 10);
        var middleDate = new DateTime(2026, 5, 12);
        var endDate = new DateTime(2026, 5, 14);

        HeadlessControlHost.PointerPress(FindDayButton(grid!, startDate));
        HeadlessControlHost.PointerMove(FindDayButton(grid!, endDate), leftButtonPressed: true);
        HeadlessControlHost.PointerRelease(FindDayButton(grid!, endDate));
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        var middleButton = FindDayButton(grid!, middleDate);
        HeadlessControlHost.PointerPress(middleButton, KeyModifiers.Control);
        HeadlessControlHost.PointerRelease(middleButton, KeyModifiers.Control);
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        calendar.SelectedDates.Should().Contain(startDate);
        calendar.SelectedDates.Should().Contain(endDate);
        calendar.SelectedDates.Should().Contain(new DateTime(2026, 5, 11));
        calendar.SelectedDates.Should().Contain(new DateTime(2026, 5, 13));
        calendar.SelectedDates.Should().NotContain(middleDate);
    }

    [AvaloniaFact]
    public void MultipleRange_WithoutTap_PlainClickOnMiddleOfRange_ReplacesWithSingleDate()
    {
        var calendar = CreateCalendar(new(2026, 5, 15));
        calendar.SelectionMode = CalendarSelectionMode.MultipleRange;
        calendar.AllowTapRangeSelection = false;
        HeadlessControlHost.Show(calendar, new(420, 360));

        var grid = HeadlessControlHost.FindByName<Grid>(calendar, Calendar.PartMonthGrid);
        grid.Should().NotBeNull();

        var startDate = new DateTime(2026, 5, 10);
        var middleDate = new DateTime(2026, 5, 12);
        var endDate = new DateTime(2026, 5, 14);

        HeadlessControlHost.PointerPress(FindDayButton(grid!, startDate));
        HeadlessControlHost.PointerMove(FindDayButton(grid!, endDate), leftButtonPressed: true);
        HeadlessControlHost.PointerRelease(FindDayButton(grid!, endDate));
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        var middleButton = FindDayButton(grid!, middleDate);
        HeadlessControlHost.PointerPress(middleButton);
        HeadlessControlHost.PointerRelease(middleButton);
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        calendar.SelectedDates.Should().ContainSingle().Which.Should().Be(middleDate);
        middleButton.IsStartDate.Should().BeTrue("single selected day must show committed range styling");
        middleButton.IsEndDate.Should().BeTrue();
    }

    [AvaloniaFact]
    public void MultipleRange_WithoutTap_CtrlClickNearRangeStart_TogglesOnlyThatDate()
    {
        var calendar = CreateCalendar(new(2026, 5, 15));
        calendar.SelectionMode = CalendarSelectionMode.MultipleRange;
        calendar.AllowTapRangeSelection = false;
        HeadlessControlHost.Show(calendar, new(420, 360));

        var grid = HeadlessControlHost.FindByName<Grid>(calendar, Calendar.PartMonthGrid);
        grid.Should().NotBeNull();

        var startDate = new DateTime(2026, 5, 10);
        var secondDate = new DateTime(2026, 5, 11);
        var endDate = new DateTime(2026, 5, 14);

        HeadlessControlHost.PointerPress(FindDayButton(grid!, startDate));
        HeadlessControlHost.PointerMove(FindDayButton(grid!, endDate), leftButtonPressed: true);
        HeadlessControlHost.PointerRelease(FindDayButton(grid!, endDate));
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        var startButton = FindDayButton(grid!, startDate);
        HeadlessControlHost.PointerPress(startButton, KeyModifiers.Control);
        HeadlessControlHost.PointerRelease(startButton, KeyModifiers.Control);
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        calendar.SelectedDates.Should().NotContain(startDate);
        calendar.SelectedDates.Should().Contain(secondDate);
        calendar.SelectedDates.Should().Contain(endDate);
        calendar.SelectedDates.Count.Should().Be(4);
        startButton.IsStartDate.Should().BeFalse();
        FindDayButton(grid!, secondDate).IsStartDate.Should().BeTrue();
    }

    [AvaloniaFact]
    public void MultipleRange_WithoutTap_DragPreviewStartsOnlyAfterMove()
    {
        var calendar = CreateCalendar(new(2026, 5, 15));
        calendar.SelectionMode = CalendarSelectionMode.MultipleRange;
        calendar.AllowTapRangeSelection = false;
        HeadlessControlHost.Show(calendar, new(420, 360));

        var grid = HeadlessControlHost.FindByName<Grid>(calendar, Calendar.PartMonthGrid);
        grid.Should().NotBeNull();

        var startDate = new DateTime(2026, 5, 10);
        var endDate = new DateTime(2026, 5, 14);
        var startButton = FindDayButton(grid!, startDate);
        var endButton = FindDayButton(grid!, endDate);

        HeadlessControlHost.PointerPress(startButton);
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        startButton.IsPreviewStartDate.Should().BeFalse("preview must not start on press alone");
        startButton.IsPreviewEndDate.Should().BeFalse();
        startButton.IsPreviewInRange.Should().BeFalse();

        HeadlessControlHost.PointerEnter(endButton);
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        startButton.IsPreviewStartDate.Should().BeFalse("pointer enter without button down must not start drag preview");

        HeadlessControlHost.PointerMove(endButton, leftButtonPressed: true);
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        startButton.IsPreviewStartDate.Should().BeTrue("preview should start after moving to another cell with button down");
        endButton.IsPreviewEndDate.Should().BeTrue();
    }

    [AvaloniaFact]
    public void SingleRange_DragPreview_MovingAcrossCells_KeepsMiddleCellsHighlighted()
    {
        var calendar = CreateCalendar(new(2026, 5, 15));
        calendar.SelectionMode = CalendarSelectionMode.SingleRange;
        calendar.AllowTapRangeSelection = false;
        HeadlessControlHost.Show(calendar, new(420, 360));

        var grid = HeadlessControlHost.FindByName<Grid>(calendar, Calendar.PartMonthGrid);
        grid.Should().NotBeNull();

        var startDate = new DateTime(2026, 5, 10);
        var middleDate = new DateTime(2026, 5, 12);
        var endDate = new DateTime(2026, 5, 14);
        var startButton = FindDayButton(grid!, startDate);
        var middleButton = FindDayButton(grid!, middleDate);
        var endButton = FindDayButton(grid!, endDate);

        HeadlessControlHost.PointerPress(startButton);
        HeadlessControlHost.PointerMove(middleButton, leftButtonPressed: true);

        middleButton.IsPreviewInRange.Should().BeTrue("middle cell must stay highlighted while dragging across cells");
        startButton.IsPreviewStartDate.Should().BeTrue();

        HeadlessControlHost.PointerMove(endButton, leftButtonPressed: true);

        middleButton.IsPreviewInRange.Should().BeTrue("middle cell must not lose preview when drag end extends");
        startButton.IsPreviewStartDate.Should().BeTrue();
        endButton.IsPreviewEndDate.Should().BeTrue();
    }

    [AvaloniaFact]
    public void SingleRange_DragPreview_GridGap_KeepsPreviewStable()
    {
        var calendar = CreateCalendar(new(2026, 5, 15));
        calendar.SelectionMode = CalendarSelectionMode.SingleRange;
        calendar.AllowTapRangeSelection = true;
        HeadlessControlHost.Show(calendar, new(420, 360));
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Loaded);

        var grid = HeadlessControlHost.FindByName<Grid>(calendar, Calendar.PartMonthGrid);
        grid.Should().NotBeNull();

        var startDate = new DateTime(2026, 5, 3);
        var middleDate = new DateTime(2026, 5, 10);
        var endDate = new DateTime(2026, 5, 17);
        var startButton = FindDayButton(grid!, startDate);
        var middleButton = FindDayButton(grid!, middleDate);
        var endButton = FindDayButton(grid!, endDate);

        HeadlessControlHost.PointerRelease(startButton);
        HeadlessControlHost.PointerEnter(endButton);
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        middleButton.IsPreviewInRange.Should().BeTrue();
        startButton.IsPreviewStartDate.Should().BeTrue();
        endButton.IsPreviewEndDate.Should().BeTrue();

        SimulateVerticalGridGap(grid!, middleButton, endButton);
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        middleButton.IsPreviewInRange.Should().BeTrue("preview must survive vertical margin gap between rows");
        startButton.IsPreviewStartDate.Should().BeTrue();
        endButton.IsPreviewEndDate.Should().BeTrue();
    }

    [AvaloniaFact]
    public void SingleRange_KeyboardPreview_ExtendingRange_OnlyEndCapChanges()
    {
        var calendar = CreateCalendar(new(2026, 5, 15));
        calendar.SelectionMode = CalendarSelectionMode.SingleRange;
        calendar.AllowTapRangeSelection = true;
        HeadlessControlHost.Show(calendar, new(420, 360));
        calendar.Focus();

        var grid = HeadlessControlHost.FindByName<Grid>(calendar, Calendar.PartMonthGrid);
        grid.Should().NotBeNull();

        var startDate = new DateTime(2026, 5, 10);
        var middleDate = new DateTime(2026, 5, 12);
        var endDate = new DateTime(2026, 5, 14);
        calendar.MoveToDate(startDate);
        HeadlessControlHost.KeyDown(calendar, Key.Space);

        HeadlessControlHost.KeyDown(calendar, Key.Right);
        HeadlessControlHost.KeyDown(calendar, Key.Right);
        HeadlessControlHost.KeyDown(calendar, Key.Right);
        HeadlessControlHost.KeyDown(calendar, Key.Right);

        var middleButton = FindDayButton(grid!, middleDate);
        var endButton = FindDayButton(grid!, endDate);
        var startButton = FindDayButton(grid!, startDate);

        middleButton.IsPreviewInRange.Should().BeTrue();
        endButton.IsPreviewEndDate.Should().BeTrue();
        startButton.IsPreviewStartDate.Should().BeTrue();

        HeadlessControlHost.KeyDown(calendar, Key.Right);

        middleButton.IsPreviewInRange.Should().BeTrue("stable middle cell must keep preview-in-range when end cap moves");
        endButton.IsPreviewEndDate.Should().BeFalse("previous end cap becomes in-range");
        FindDayButton(grid!, new DateTime(2026, 5, 15)).IsPreviewEndDate.Should().BeTrue();
    }

    [AvaloniaFact]
    public void MultipleRange_WithoutTap_CtrlClickNoDragPreview()
    {
        var calendar = CreateCalendar(new(2026, 5, 15));
        calendar.SelectionMode = CalendarSelectionMode.MultipleRange;
        calendar.AllowTapRangeSelection = false;
        HeadlessControlHost.Show(calendar, new(420, 360));

        var grid = HeadlessControlHost.FindByName<Grid>(calendar, Calendar.PartMonthGrid);
        grid.Should().NotBeNull();

        var toggleDate = new DateTime(2026, 5, 10);
        var toggleButton = FindDayButton(grid!, toggleDate);

        HeadlessControlHost.PointerPress(toggleButton, KeyModifiers.Control);
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        toggleButton.IsPreviewStartDate.Should().BeFalse("ctrl+click must not start drag preview on press");
        toggleButton.IsPreviewEndDate.Should().BeFalse();
        toggleButton.IsPreviewInRange.Should().BeFalse();
    }

    [AvaloniaFact]
    public void MultipleRange_WithoutTap_ShiftKeyboardPreview()
    {
        var calendar = CreateCalendar(new(2026, 5, 15));
        calendar.SelectionMode = CalendarSelectionMode.MultipleRange;
        calendar.AllowTapRangeSelection = false;
        HeadlessControlHost.Show(calendar, new(420, 360));
        calendar.Focus();

        var grid = HeadlessControlHost.FindByName<Grid>(calendar, Calendar.PartMonthGrid);
        grid.Should().NotBeNull();

        var startDate = new DateTime(2026, 5, 10);
        var endDate = new DateTime(2026, 5, 14);
        var startButton = FindDayButton(grid!, startDate);
        var endButton = FindDayButton(grid!, endDate);
        var middleButton = FindDayButton(grid!, new DateTime(2026, 5, 12));

        EstablishDragModeAnchor(calendar, startDate);

        for (var i = 0; i < 4; i++)
            HeadlessControlHost.KeyDown(calendar, Key.Right, KeyModifiers.Shift);

        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        startButton.IsPreviewStartDate.Should().BeTrue();
        middleButton.IsPreviewInRange.Should().BeTrue();
        endButton.IsPreviewEndDate.Should().BeTrue();
    }

    [AvaloniaFact]
    public void MultipleRange_WithoutTap_CtrlShiftKeyboardAddRange()
    {
        var calendar = CreateCalendar(new(2026, 5, 15));
        calendar.SelectionMode = CalendarSelectionMode.MultipleRange;
        calendar.AllowTapRangeSelection = false;
        HeadlessControlHost.Show(calendar, new(420, 360));
        calendar.Focus();

        var grid = HeadlessControlHost.FindByName<Grid>(calendar, Calendar.PartMonthGrid);
        grid.Should().NotBeNull();

        var firstStart = new DateTime(2026, 5, 5);
        var firstEnd = new DateTime(2026, 5, 9);
        var secondStart = new DateTime(2026, 5, 12);
        var secondEnd = new DateTime(2026, 5, 18);

        HeadlessControlHost.PointerPress(FindDayButton(grid!, firstStart));
        HeadlessControlHost.PointerMove(FindDayButton(grid!, firstEnd), leftButtonPressed: true);
        HeadlessControlHost.PointerRelease(FindDayButton(grid!, firstEnd));
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        EstablishDragModeAnchor(calendar, secondStart);

        for (var i = 0; i < 6; i++)
            HeadlessControlHost.KeyDown(calendar, Key.Right, KeyModifiers.Shift | KeyModifiers.Control);

        HeadlessControlHost.KeyDown(calendar, Key.Space, KeyModifiers.Shift | KeyModifiers.Control);
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        calendar.SelectedDates.Should().Contain(firstStart);
        calendar.SelectedDates.Should().Contain(firstEnd);
        calendar.SelectedDates.Should().Contain(secondStart);
        calendar.SelectedDates.Should().Contain(secondEnd);
        calendar.SelectedDates.Should().Contain(new DateTime(2026, 5, 7));
        calendar.SelectedDates.Should().Contain(new DateTime(2026, 5, 15));
    }

    [AvaloniaFact]
    public void MultipleRange_WithoutTap_CtrlShiftDragAddsRange()
    {
        var calendar = CreateCalendar(new(2026, 5, 15));
        calendar.SelectionMode = CalendarSelectionMode.MultipleRange;
        calendar.AllowTapRangeSelection = false;
        HeadlessControlHost.Show(calendar, new(420, 360));

        var grid = HeadlessControlHost.FindByName<Grid>(calendar, Calendar.PartMonthGrid);
        grid.Should().NotBeNull();

        var firstStart = new DateTime(2026, 5, 1);
        var firstEnd = new DateTime(2026, 5, 7);
        var secondStart = new DateTime(2026, 5, 12);
        var secondEnd = new DateTime(2026, 5, 18);

        HeadlessControlHost.PointerPress(FindDayButton(grid!, firstStart));
        HeadlessControlHost.PointerMove(FindDayButton(grid!, firstEnd), leftButtonPressed: true);
        HeadlessControlHost.PointerRelease(FindDayButton(grid!, firstEnd));
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        var secondStartButton = FindDayButton(grid!, secondStart);
        HeadlessControlHost.PointerPress(secondStartButton, KeyModifiers.Control);
        HeadlessControlHost.PointerRelease(secondStartButton, KeyModifiers.Control);
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        HeadlessControlHost.PointerPress(secondStartButton, KeyModifiers.Shift | KeyModifiers.Control);
        HeadlessControlHost.PointerMove(FindDayButton(grid!, secondEnd), KeyModifiers.Shift | KeyModifiers.Control, leftButtonPressed: true);
        HeadlessControlHost.PointerRelease(FindDayButton(grid!, secondEnd), KeyModifiers.Shift | KeyModifiers.Control);
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        calendar.SelectedDates.Should().Contain(firstStart);
        calendar.SelectedDates.Should().Contain(firstEnd);
        calendar.SelectedDates.Should().Contain(secondStart);
        calendar.SelectedDates.Should().Contain(secondEnd);
    }

    [AvaloniaFact]
    public void MultipleRange_WithoutTap_ShiftKeyDownNoPreviewUntilPointerMove()
    {
        var calendar = CreateCalendar(new(2026, 5, 15));
        calendar.SelectionMode = CalendarSelectionMode.MultipleRange;
        calendar.AllowTapRangeSelection = false;
        HeadlessControlHost.Show(calendar, new(420, 360));
        calendar.Focus();

        var grid = HeadlessControlHost.FindByName<Grid>(calendar, Calendar.PartMonthGrid);
        grid.Should().NotBeNull();

        var startDate = new DateTime(2026, 5, 10);
        var hoverDate = new DateTime(2026, 5, 18);

        EstablishDragModeAnchor(calendar, startDate);
        HeadlessControlHost.PointerEnter(FindDayButton(grid!, hoverDate));
        HeadlessControlHost.KeyDown(calendar, Key.LeftShift);
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        FindDayButton(grid!, hoverDate).IsPreviewEndDate.Should().BeFalse("shift alone must not start pointer preview");
        FindDayButton(grid!, hoverDate).IsPreviewInRange.Should().BeFalse();

        HeadlessControlHost.PointerMove(FindDayButton(grid!, hoverDate), KeyModifiers.Shift);
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        FindDayButton(grid!, startDate).IsPreviewStartDate.Should().BeTrue();
        FindDayButton(grid!, hoverDate).IsPreviewEndDate.Should().BeTrue();
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
        EstablishDragModeAnchor(calendar, startDate);

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
    public void SingleRange_WithoutTap_PointerEnterNoPreview()
    {
        var calendar = CreateCalendar(new(2026, 5, 15));
        calendar.SelectionMode = CalendarSelectionMode.SingleRange;
        calendar.AllowTapRangeSelection = false;
        HeadlessControlHost.Show(calendar, new(420, 360));
        calendar.Focus();

        var grid = HeadlessControlHost.FindByName<Grid>(calendar, Calendar.PartMonthGrid);
        grid.Should().NotBeNull();

        var startDate = new DateTime(2026, 5, 10);
        var hoverDate = new DateTime(2026, 5, 18);
        EstablishDragModeAnchor(calendar, startDate);

        HeadlessControlHost.PointerEnter(FindDayButton(grid!, hoverDate));
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        FindDayButton(grid!, hoverDate).IsPreviewInRange.Should().BeFalse("drag mode must not preview on pointer enter alone");
        FindDayButton(grid!, hoverDate).IsPreviewEndDate.Should().BeFalse();
    }

    [AvaloniaFact]
    public void SingleRange_WithoutTap_ShiftKeyDownNoPreviewUntilPointerMove()
    {
        var calendar = CreateCalendar(new(2026, 5, 15));
        calendar.SelectionMode = CalendarSelectionMode.SingleRange;
        calendar.AllowTapRangeSelection = false;
        HeadlessControlHost.Show(calendar, new(420, 360));
        calendar.Focus();

        var grid = HeadlessControlHost.FindByName<Grid>(calendar, Calendar.PartMonthGrid);
        grid.Should().NotBeNull();

        var startDate = new DateTime(2026, 5, 10);
        var hoverDate = new DateTime(2026, 5, 18);

        EstablishDragModeAnchor(calendar, startDate);
        HeadlessControlHost.PointerEnter(FindDayButton(grid!, hoverDate));
        HeadlessControlHost.KeyDown(calendar, Key.LeftShift);
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        FindDayButton(grid!, hoverDate).IsPreviewEndDate.Should().BeFalse("shift alone must not start pointer preview");
        FindDayButton(grid!, hoverDate).IsPreviewInRange.Should().BeFalse();

        HeadlessControlHost.PointerMove(FindDayButton(grid!, hoverDate), KeyModifiers.Shift);
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        FindDayButton(grid!, startDate).IsPreviewStartDate.Should().BeTrue();
        FindDayButton(grid!, hoverDate).IsPreviewEndDate.Should().BeTrue();
    }

    [AvaloniaFact]
    public void SingleRange_WithoutTap_ShiftPointerMovePreview()
    {
        var calendar = CreateCalendar(new(2026, 5, 15));
        calendar.SelectionMode = CalendarSelectionMode.SingleRange;
        calendar.AllowTapRangeSelection = false;
        HeadlessControlHost.Show(calendar, new(420, 360));
        calendar.Focus();

        var grid = HeadlessControlHost.FindByName<Grid>(calendar, Calendar.PartMonthGrid);
        grid.Should().NotBeNull();

        var startDate = new DateTime(2026, 5, 10);
        var endDate = new DateTime(2026, 5, 18);
        EstablishDragModeAnchor(calendar, startDate);
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        HeadlessControlHost.PointerMove(FindDayButton(grid!, endDate), KeyModifiers.Shift);
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        FindDayButton(grid!, startDate).IsPreviewStartDate.Should().BeTrue();
        FindDayButton(grid!, endDate).IsPreviewEndDate.Should().BeTrue();
        FindDayButton(grid!, new DateTime(2026, 5, 14)).IsPreviewInRange.Should().BeTrue();
    }

    [AvaloniaFact]
    public void SingleRange_WithoutTap_KeyboardShiftPreview_SurvivesShiftRelease()
    {
        var calendar = CreateCalendar(new(2026, 5, 15));
        calendar.SelectionMode = CalendarSelectionMode.SingleRange;
        calendar.AllowTapRangeSelection = false;
        HeadlessControlHost.Show(calendar, new(420, 360));
        calendar.Focus();

        var grid = HeadlessControlHost.FindByName<Grid>(calendar, Calendar.PartMonthGrid);
        grid.Should().NotBeNull();

        var pointerDate = new DateTime(2026, 5, 8);
        var startDate = new DateTime(2026, 5, 10);
        var endDate = new DateTime(2026, 5, 14);

        HeadlessControlHost.PointerEnter(FindDayButton(grid!, pointerDate));
        EstablishDragModeAnchor(calendar, startDate);

        for (var i = 0; i < 4; i++)
            HeadlessControlHost.KeyDown(calendar, Key.Right, KeyModifiers.Shift);

        HeadlessControlHost.KeyUp(calendar, Key.LeftShift);
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        FindDayButton(grid!, pointerDate).IsPreviewEndDate.Should().BeFalse("static pointer must not override keyboard interval end");
        FindDayButton(grid!, endDate).IsPreviewEndDate.Should().BeTrue("keyboard preview must survive shift release");
        FindDayButton(grid!, new DateTime(2026, 5, 12)).IsPreviewInRange.Should().BeTrue();
    }

    [AvaloniaFact]
    public void SingleRange_WithoutTap_KeyboardPreview_StableAcrossFrames()
    {
        var calendar = CreateCalendar(new(2026, 5, 15));
        calendar.SelectionMode = CalendarSelectionMode.SingleRange;
        calendar.AllowTapRangeSelection = false;
        HeadlessControlHost.Show(calendar, new(420, 360));
        calendar.Focus();

        var grid = HeadlessControlHost.FindByName<Grid>(calendar, Calendar.PartMonthGrid);
        grid.Should().NotBeNull();

        var startDate = new DateTime(2026, 5, 10);
        var endDate = new DateTime(2026, 5, 14);
        var startButton = FindDayButton(grid!, startDate);
        var endButton = FindDayButton(grid!, endDate);
        var middleButton = FindDayButton(grid!, new DateTime(2026, 5, 12));

        EstablishDragModeAnchor(calendar, startDate);

        for (var i = 0; i < 4; i++)
            HeadlessControlHost.KeyDown(calendar, Key.Right, KeyModifiers.Shift);

        for (var i = 0; i < 3; i++)
        {
            Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);
            startButton.IsPreviewStartDate.Should().BeTrue("preview start should stay visible after frame " + i);
            middleButton.IsPreviewInRange.Should().BeTrue("keyboard preview should stay visible after frame " + i);
            endButton.IsPreviewEndDate.Should().BeTrue("preview end should stay visible after frame " + i);
        }
    }

    [AvaloniaFact]
    public void SingleRange_WithoutTap_KeyboardShiftPreview_UnaffectedByPointerMoveAfterShiftRelease()
    {
        var calendar = CreateCalendar(new(2026, 5, 15));
        calendar.SelectionMode = CalendarSelectionMode.SingleRange;
        calendar.AllowTapRangeSelection = false;
        HeadlessControlHost.Show(calendar, new(420, 360));
        calendar.Focus();

        var grid = HeadlessControlHost.FindByName<Grid>(calendar, Calendar.PartMonthGrid);
        grid.Should().NotBeNull();

        var pointerDate = new DateTime(2026, 5, 18);
        var startDate = new DateTime(2026, 5, 10);
        var endDate = new DateTime(2026, 5, 14);

        EstablishDragModeAnchor(calendar, startDate);

        for (var i = 0; i < 4; i++)
            HeadlessControlHost.KeyDown(calendar, Key.Right, KeyModifiers.Shift);

        HeadlessControlHost.KeyUp(calendar, Key.LeftShift);
        HeadlessControlHost.PointerMove(FindDayButton(grid!, pointerDate));
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        FindDayButton(grid!, pointerDate).IsPreviewEndDate.Should().BeFalse("pointer move without shift must not affect keyboard preview");
        FindDayButton(grid!, endDate).IsPreviewEndDate.Should().BeTrue();
        FindDayButton(grid!, new DateTime(2026, 5, 12)).IsPreviewInRange.Should().BeTrue();
    }

    [AvaloniaFact]
    public void SingleRange_WithoutTap_KeyboardShiftPreview_HandoffOnMove()
    {
        var calendar = CreateCalendar(new(2026, 5, 15));
        calendar.SelectionMode = CalendarSelectionMode.SingleRange;
        calendar.AllowTapRangeSelection = false;
        HeadlessControlHost.Show(calendar, new(420, 360));
        calendar.Focus();

        var grid = HeadlessControlHost.FindByName<Grid>(calendar, Calendar.PartMonthGrid);
        grid.Should().NotBeNull();

        var pointerDate = new DateTime(2026, 5, 18);
        var startDate = new DateTime(2026, 5, 10);
        var endDate = new DateTime(2026, 5, 14);

        EstablishDragModeAnchor(calendar, startDate);

        for (var i = 0; i < 4; i++)
            HeadlessControlHost.KeyDown(calendar, Key.Right, KeyModifiers.Shift);

        HeadlessControlHost.KeyUp(calendar, Key.LeftShift);
        HeadlessControlHost.PointerMove(FindDayButton(grid!, pointerDate), KeyModifiers.Shift);
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        FindDayButton(grid!, pointerDate).IsPreviewEndDate.Should().BeTrue("shift+move should take over preview end");
        FindDayButton(grid!, endDate).IsPreviewEndDate.Should().BeFalse();
    }

    [AvaloniaFact]
    public void SingleRange_WithoutTap_ShiftKeyboardCommit_ClearsPreview_ShowsCommitted()
    {
        var calendar = CreateCalendar(new(2026, 5, 15));
        calendar.SelectionMode = CalendarSelectionMode.SingleRange;
        calendar.AllowTapRangeSelection = false;
        HeadlessControlHost.Show(calendar, new(420, 360));
        calendar.Focus();

        var grid = HeadlessControlHost.FindByName<Grid>(calendar, Calendar.PartMonthGrid);
        grid.Should().NotBeNull();

        var startDate = new DateTime(2026, 5, 10);
        var endDate = new DateTime(2026, 5, 14);
        var startButton = FindDayButton(grid!, startDate);
        var endButton = FindDayButton(grid!, endDate);
        var middleButton = FindDayButton(grid!, new DateTime(2026, 5, 12));

        EstablishDragModeAnchor(calendar, startDate);

        for (var i = 0; i < 4; i++)
            HeadlessControlHost.KeyDown(calendar, Key.Right, KeyModifiers.Shift);

        startButton.IsPreviewStartDate.Should().BeTrue();
        middleButton.IsPreviewInRange.Should().BeTrue();
        endButton.IsPreviewEndDate.Should().BeTrue();

        HeadlessControlHost.KeyDown(calendar, Key.Space, KeyModifiers.Shift);
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        startButton.IsPreviewStartDate.Should().BeFalse("commit must clear preview pseudos");
        middleButton.IsPreviewInRange.Should().BeFalse();
        endButton.IsPreviewEndDate.Should().BeFalse();
        startButton.IsStartDate.Should().BeTrue("committed range must be visible after commit");
        middleButton.IsInRange.Should().BeTrue();
        endButton.IsEndDate.Should().BeTrue();
        calendar.SelectedDates.Should().Contain(startDate);
        calendar.SelectedDates.Should().Contain(endDate);
    }

    [AvaloniaFact]
    public void SingleRange_WithoutTap_ShiftDragCommit_ClearsPreview()
    {
        var calendar = CreateCalendar(new(2026, 5, 15));
        calendar.SelectionMode = CalendarSelectionMode.SingleRange;
        calendar.AllowTapRangeSelection = false;
        HeadlessControlHost.Show(calendar, new(420, 360));

        var grid = HeadlessControlHost.FindByName<Grid>(calendar, Calendar.PartMonthGrid);
        grid.Should().NotBeNull();

        var startDate = new DateTime(2026, 5, 10);
        var endDate = new DateTime(2026, 5, 14);
        var startButton = FindDayButton(grid!, startDate);
        var endButton = FindDayButton(grid!, endDate);
        var middleButton = FindDayButton(grid!, new DateTime(2026, 5, 12));

        HeadlessControlHost.PointerPress(startButton, KeyModifiers.Shift);
        HeadlessControlHost.PointerMove(endButton, KeyModifiers.Shift, leftButtonPressed: true);
        HeadlessControlHost.PointerRelease(endButton, KeyModifiers.Shift);
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        startButton.IsPreviewStartDate.Should().BeFalse("commit must clear preview pseudos");
        middleButton.IsPreviewInRange.Should().BeFalse();
        endButton.IsPreviewEndDate.Should().BeFalse();
        startButton.IsStartDate.Should().BeTrue();
        middleButton.IsInRange.Should().BeTrue();
        endButton.IsEndDate.Should().BeTrue();
    }

    [AvaloniaFact]
    public void MultipleRange_WithoutTap_PreviewVisibleOverCommittedRange()
    {
        var calendar = CreateCalendar(new(2026, 5, 15));
        calendar.SelectionMode = CalendarSelectionMode.MultipleRange;
        calendar.AllowTapRangeSelection = false;
        HeadlessControlHost.Show(calendar, new(420, 360));
        calendar.Focus();

        var grid = HeadlessControlHost.FindByName<Grid>(calendar, Calendar.PartMonthGrid);
        grid.Should().NotBeNull();

        var rangeStart = new DateTime(2026, 5, 10);
        var rangeEnd = new DateTime(2026, 5, 14);
        var previewEnd = new DateTime(2026, 5, 18);

        HeadlessControlHost.PointerPress(FindDayButton(grid!, rangeStart));
        HeadlessControlHost.PointerMove(FindDayButton(grid!, rangeEnd), leftButtonPressed: true);
        HeadlessControlHost.PointerRelease(FindDayButton(grid!, rangeEnd));
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        var committedMiddle = FindDayButton(grid!, new DateTime(2026, 5, 12));
        committedMiddle.IsInRange.Should().BeTrue("existing range must be committed before preview extension");

        for (var i = 0; i < 4; i++)
            HeadlessControlHost.KeyDown(calendar, Key.Right, KeyModifiers.Shift);

        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        committedMiddle.IsPreviewInRange.Should().BeTrue("preview must overlay already-committed dates while shift is held");
        FindDayButton(grid!, rangeStart).IsPreviewStartDate.Should().BeTrue();
        FindDayButton(grid!, previewEnd).IsPreviewEndDate.Should().BeTrue();
        FindDayButton(grid!, new DateTime(2026, 5, 16)).IsPreviewInRange.Should().BeTrue();
    }

    [AvaloniaFact]
    public void SingleRange_WithoutTap_ArrowWithoutShift_ClearsBand()
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
        EstablishDragModeAnchor(calendar, startDate);

        HeadlessControlHost.KeyDown(calendar, Key.Right, KeyModifiers.Shift);
        HeadlessControlHost.KeyDown(calendar, Key.Right, KeyModifiers.Shift);
        HeadlessControlHost.KeyDown(calendar, Key.Right, KeyModifiers.Shift);
        HeadlessControlHost.KeyDown(calendar, Key.Right, KeyModifiers.Shift);
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        FindDayButton(grid!, new DateTime(2026, 5, 14)).IsPreviewEndDate.Should().BeTrue();

        HeadlessControlHost.KeyDown(calendar, Key.Right);
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        FindDayButton(grid!, middleDate).IsPreviewInRange.Should().BeFalse("non-shift arrow must clear interval preview band");
        FindDayButton(grid!, new DateTime(2026, 5, 15)).IsFocused.Should().BeTrue();
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
    public void SingleRange_KeyboardShiftPreview_UnaffectedByPointerUntilMove()
    {
        var calendar = CreateCalendar(new(2026, 5, 15));
        calendar.SelectionMode = CalendarSelectionMode.SingleRange;
        calendar.AllowTapRangeSelection = true;
        HeadlessControlHost.Show(calendar, new(420, 360));
        calendar.Focus();

        var grid = HeadlessControlHost.FindByName<Grid>(calendar, Calendar.PartMonthGrid);
        grid.Should().NotBeNull();

        var pointerDate = new DateTime(2026, 5, 8);
        var startDate = new DateTime(2026, 5, 10);
        var endDate = new DateTime(2026, 5, 14);

        HeadlessControlHost.PointerEnter(FindDayButton(grid!, pointerDate));

        calendar.MoveToDate(startDate);
        HeadlessControlHost.KeyDown(calendar, Key.Space);

        for (var i = 0; i < 4; i++)
            HeadlessControlHost.KeyDown(calendar, Key.Right, KeyModifiers.Shift);

        HeadlessControlHost.KeyUp(calendar, Key.LeftShift);
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        FindDayButton(grid!, pointerDate).IsPreviewEndDate.Should().BeFalse("pointer hover must not override keyboard interval end");
        FindDayButton(grid!, endDate).IsPreviewEndDate.Should().BeTrue("keyboard interval end must survive shift release");
        FindDayButton(grid!, new DateTime(2026, 5, 12)).IsPreviewInRange.Should().BeTrue();
    }

    [AvaloniaFact]
    public void SingleRange_KeyboardShiftPreview_UpdatesOnPointerMove()
    {
        var calendar = CreateCalendar(new(2026, 5, 15));
        calendar.SelectionMode = CalendarSelectionMode.SingleRange;
        calendar.AllowTapRangeSelection = true;
        HeadlessControlHost.Show(calendar, new(420, 360));
        calendar.Focus();

        var grid = HeadlessControlHost.FindByName<Grid>(calendar, Calendar.PartMonthGrid);
        grid.Should().NotBeNull();

        var pointerDate = new DateTime(2026, 5, 16);
        var startDate = new DateTime(2026, 5, 10);
        var endDate = new DateTime(2026, 5, 14);

        HeadlessControlHost.PointerEnter(FindDayButton(grid!, pointerDate));

        calendar.MoveToDate(startDate);
        HeadlessControlHost.KeyDown(calendar, Key.Space);

        for (var i = 0; i < 4; i++)
            HeadlessControlHost.KeyDown(calendar, Key.Right, KeyModifiers.Shift);

        HeadlessControlHost.KeyUp(calendar, Key.LeftShift);
        HeadlessControlHost.PointerMove(FindDayButton(grid!, pointerDate));
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        FindDayButton(grid!, pointerDate).IsPreviewEndDate.Should().BeTrue("pointer move should take over preview end");
        FindDayButton(grid!, endDate).IsPreviewEndDate.Should().BeFalse();
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
        EstablishDragModeAnchor(calendar, startDate);

        for (var i = 0; i < 4; i++)
            HeadlessControlHost.KeyDown(calendar, Key.Right, KeyModifiers.Shift);

        HeadlessControlHost.KeyUp(calendar, Key.LeftShift);
        HeadlessControlHost.KeyDown(calendar, Key.Enter);

        dayButtonClicks.Should().Be(1);
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
    public void SingleRange_TapPreview_VisibleImmediatelyWithoutDeferredFrame()
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

        middleButton.IsPreviewInRange.Should().BeTrue("preview must be visible on the same frame as pointer enter");
        startButton.IsPreviewStartDate.Should().BeTrue();
        endButton.IsPreviewEndDate.Should().BeTrue();
    }

    [AvaloniaFact]
    public void SingleRange_TapCommit_RangeRolesAppliedImmediatelyWithoutSelectedFlash()
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

        startButton.IsStartDate.Should().BeTrue("committed range styling must be immediate");
        endButton.IsEndDate.Should().BeTrue();
        middleButton.IsInRange.Should().BeTrue();
        startButton.IsSelected.Should().BeFalse("range cap cells must not flash :selected before range roles");
        middleButton.IsSelected.Should().BeFalse();
        endButton.IsSelected.Should().BeFalse();
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

    private static void SimulateVerticalGridGap(Grid grid, CalendarDayButton upperButton, CalendarDayButton lowerButton, bool leftButtonPressed = false)
    {
        var gapPosition = GetVerticalGapPosition(grid, upperButton, lowerButton);

        HeadlessControlHost.PointerExitedAt(upperButton, new(4, 4), leftButtonPressed: leftButtonPressed);
        HeadlessControlHost.PointerExitedAt(grid, gapPosition, leftButtonPressed: leftButtonPressed);
        HeadlessControlHost.PointerMoveAt(grid, gapPosition, leftButtonPressed: leftButtonPressed);
    }

    private static Point GetVerticalGapPosition(Grid grid, CalendarDayButton upperButton, CalendarDayButton lowerButton)
    {
        var upperOrigin = upperButton.TranslatePoint(new Point(0, 0), grid) ?? default;
        var lowerOrigin = lowerButton.TranslatePoint(new Point(0, 0), grid) ?? default;
        var gapY = (upperOrigin.Y + upperButton.Bounds.Height + lowerOrigin.Y) / 2;
        var gapX = upperOrigin.X + (upperButton.Bounds.Width / 2);
        return new Point(gapX, gapY);
    }

    private static void EstablishDragModeAnchor(Calendar calendar, DateTime anchorDate)
    {
        calendar.MoveToDate(anchorDate.AddDays(-1));
        HeadlessControlHost.KeyDown(calendar, Key.Right);
    }

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
