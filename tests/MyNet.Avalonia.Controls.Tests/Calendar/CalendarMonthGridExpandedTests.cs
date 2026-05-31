// -----------------------------------------------------------------------
// <copyright file="CalendarMonthGridExpandedTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using FluentAssertions;
using MyNet.Avalonia.Controls.Internals;
using MyNet.Avalonia.Controls.Primitives;
using Xunit;

namespace MyNet.Avalonia.Controls.Tests.Calendar;

public class CalendarMonthGridExpandedTests
{
    [Theory]
    [InlineData(DayOfWeek.Sunday, 0)]
    [InlineData(DayOfWeek.Monday, 1)]
    [InlineData(DayOfWeek.Saturday, 6)]
    public void GetDayTitleColumnIndex_MapsSundayFirst(DayOfWeek firstDay, int expectedColumnForSunday)
    {
        CalendarMonthGridHelper.GetDayTitleColumnIndex(0, firstDay).Should().Be(expectedColumnForSunday);
    }

    [Fact]
    public void GetLeadingDayCount_ForFebruary2026SundayFirst_ReturnsZero()
    {
        CalendarMonthGridHelper.GetLeadingDayCount(new MonthContext(2, 2026), DayOfWeek.Sunday).Should().Be(7);
    }

    [Fact]
    public void EnumerateDayCells_LastCellIsInNextMonth()
    {
        var cells = CalendarMonthGridHelper.EnumerateDayCells(new MonthContext(2, 2026), DayOfWeek.Sunday, 42).ToList();

        cells[^1].Date.Month.Should().Be(3);
    }
}
