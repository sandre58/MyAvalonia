// -----------------------------------------------------------------------
// <copyright file="CalendarHeadlessTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using FluentAssertions;
using MyNet.Avalonia.Controls.Primitives;

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
