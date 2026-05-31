// -----------------------------------------------------------------------
// <copyright file="CalendarDisplayContextHelperTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using FluentAssertions;
using MyNet.Avalonia.Controls.Primitives;
using MyNet.Avalonia.Controls.Primitives.Internals;
using Xunit;

namespace MyNet.Avalonia.Controls.Tests.Calendar;

public class CalendarDisplayContextHelperTests
{
    [Fact]
    public void CoerceDisplayDateContext_NormalizesDecadeStartYear()
    {
        var coerced = CalendarDisplayContextHelper.CoerceDisplayDateContext(new DecadeContext(2023));

        coerced.Should().BeOfType<DecadeContext>().Which.StartYear.Should().Be(2020);
    }

    [Fact]
    public void CoerceDisplayDateContext_NormalizesCenturyStartYear()
    {
        var coerced = CalendarDisplayContextHelper.CoerceDisplayDateContext(new CenturyContext(2023));

        coerced.Should().BeOfType<CenturyContext>().Which.StartYear.Should().Be(2000);
    }

    [Fact]
    public void CoerceDisplayDateContext_LeavesAlignedContextUnchanged()
    {
        var decade = new DecadeContext(2020);

        CalendarDisplayContextHelper.CoerceDisplayDateContext(decade).Should().Be(decade);
    }

    [Fact]
    public void GetFocusedDate_ReturnsLastSelectedWhenInSameContext()
    {
        var lastSelected = new DateTime(2026, 5, 15);
        var context = new MonthContext(5, 2026);

        CalendarDisplayContextHelper.GetFocusedDate(lastSelected, context, new DateTime(2026, 5, 1))
            .Should().Be(lastSelected);
    }

    [Fact]
    public void GetFocusedDate_ReturnsTodayWhenContextMatchesToday()
    {
        var today = new DateTime(2026, 5, 20);
        var context = new MonthContext(today.Month, today.Year);

        CalendarDisplayContextHelper.GetFocusedDate(null, context, today).Should().Be(today);
    }

    [Fact]
    public void GetFocusedDate_ReturnsContextDateWhenNoSelectionAndNotToday()
    {
        var context = new MonthContext(3, 2026);

        CalendarDisplayContextHelper.GetFocusedDate(null, context, new DateTime(2026, 5, 20))
            .Should().Be(new DateTime(2026, 3, 1));
    }
}
