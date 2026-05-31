// -----------------------------------------------------------------------
// <copyright file="CalendarMonthGridHelperTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
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
}
