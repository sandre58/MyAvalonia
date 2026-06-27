// -----------------------------------------------------------------------
// <copyright file="CalendarHeadlessTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
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
