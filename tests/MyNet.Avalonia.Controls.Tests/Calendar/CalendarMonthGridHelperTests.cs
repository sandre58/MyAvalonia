// -----------------------------------------------------------------------
// <copyright file="CalendarMonthGridHelperTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using FluentAssertions;
using MyNet.Avalonia.Controls.Primitives;
using MyNet.Avalonia.Controls.Primitives.Internals;
using Xunit;

namespace MyNet.Avalonia.Controls.Tests.Calendar;

public class CalendarMonthGridHelperTests
{
    [Fact]
    public void GetLeadingDayCount_ForMarch2026MondayFirst_ReturnsSix()
    {
        var context = new MonthContext(3, 2026);

        CalendarMonthGridHelper.GetLeadingDayCount(context, DayOfWeek.Monday).Should().Be(6);
    }

    [Fact]
    public void GetDayTitleColumnIndex_RotatesFromFirstDayOfWeek()
    {
        CalendarMonthGridHelper.GetDayTitleColumnIndex(0, DayOfWeek.Monday).Should().Be(1);
        CalendarMonthGridHelper.GetDayTitleColumnIndex(6, DayOfWeek.Monday).Should().Be(0);
    }

    [Fact]
    public void EnumerateDayCells_IncludesLeadingAndTrailingDays()
    {
        var cells = CalendarMonthGridHelper.EnumerateDayCells(new MonthContext(3, 2026), DayOfWeek.Monday, 42).ToList();

        cells.Should().HaveCount(42);
        cells[0].Date.Should().Be(new DateTime(2026, 2, 23));
        cells.Should().Contain(x => x.DateContext == new DayContext(1, 3, 2026));
        cells.Should().Contain(x => x.IsInactive);
    }
}
