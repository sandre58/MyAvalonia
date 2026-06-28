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
        HeadlessControlHost.Show(calendar, new(420, 360));

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
