// -----------------------------------------------------------------------
// <copyright file="CalendarDateRangeHelperTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using FluentAssertions;
using MyNet.Avalonia.Controls.Internals;
using Xunit;

namespace MyNet.Avalonia.Controls.Tests.Calendar;

public class CalendarDateRangeHelperTests
{
    [Fact]
    public void GetSelectedMin_ReturnsEarliestDate()
    {
        var dates = new[] { new DateTime(2026, 3, 10), new DateTime(2026, 1, 5), new DateTime(2026, 6, 1) };

        CalendarDateRangeHelper.GetSelectedMin(dates).Should().Be(new(2026, 1, 5));
    }

    [Fact]
    public void ClampToRange_ClampsOutsideBounds()
    {
        var start = new DateTime(2026, 2, 1);
        var end = new DateTime(2026, 2, 28);

        CalendarDateRangeHelper.ClampToRange(new(2026, 1, 1), start, end).Should().Be(start);
        CalendarDateRangeHelper.ClampToRange(new(2026, 3, 1), start, end).Should().Be(end);
        CalendarDateRangeHelper.ClampToRange(new(2026, 2, 15), start, end).Should().Be(new(2026, 2, 15));
    }

    [Fact]
    public void GetSelectedMax_ReturnsLatestDate()
    {
        var dates = new[] { new DateTime(2026, 3, 10), new DateTime(2026, 1, 5), new DateTime(2026, 6, 1) };

        CalendarDateRangeHelper.GetSelectedMax(dates).Should().Be(new(2026, 6, 1));
    }

    [Fact]
    public void GetRangeStartAndEnd_UseMinMaxWhenUnset()
    {
        CalendarDateRangeHelper.GetRangeStart(null).Should().Be(DateTime.MinValue);
        CalendarDateRangeHelper.GetRangeEnd(null).Should().Be(DateTime.MaxValue);
    }

    [Fact]
    public void ResolveDisplayDateEndChange_ExpandsEndWhenSelectionExceedsRange()
    {
        var adjustment = CalendarDateRangeHelper.ResolveDisplayDateEndChange(
            new(2026, 3, 31),
            new(2026, 1, 1),
            new(2026, 5, 1),
            new DateTime(2026, 6, 15));

        adjustment.Should().NotBeNull();
        adjustment.Value.DisplayDateEnd.Should().Be(new(2026, 6, 15));
    }

    [Fact]
    public void ResolveDisplayDateStartChange_ExpandsEndWhenStartAfterEnd()
    {
        var adjustment = CalendarDateRangeHelper.ResolveDisplayDateStartChange(
            new(2026, 6, 1),
            new(2026, 3, 31),
            new(2026, 2, 1),
            null,
            new DateTime(2026, 1, 1));

        adjustment.Should().NotBeNull();
        adjustment.Value.DisplayDateEnd.Should().Be(new(2026, 1, 1));
    }
}
