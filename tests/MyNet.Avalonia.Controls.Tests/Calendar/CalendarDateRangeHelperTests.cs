// -----------------------------------------------------------------------
// <copyright file="CalendarDateRangeHelperTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using FluentAssertions;
using MyNet.Avalonia.Controls.Internals.Calendar;
using MyNet.Primitives;
using MyNet.Primitives.Temporal;
using Xunit;

namespace MyNet.Avalonia.Controls.Tests.Calendar;

public class CalendarDateRangeHelperTests
{
    [Fact]
    public void NormalizeDateRange_OrdersAscending()
    {
        var (start, end) = new DateTime(2026, 5, 20, 14, 30, 0).Normalize(new(2026, 5, 10, 8, 0, 0));

        start.Should().Be(new(2026, 5, 10));
        end.Should().Be(new(2026, 5, 20));
    }

    [Fact]
    public void NormalizeDateRange_DiscardsTimeComponent()
    {
        var (start, end) = new DateTime(2026, 1, 1, 23, 59, 59).Normalize(new(2026, 1, 2, 0, 0, 1));

        start.Should().Be(new(2026, 1, 1));
        end.Should().Be(new(2026, 1, 2));
    }

    [Fact]
    public void EnumerateDays_Forward_IncludesBothEnds()
    {
        var dates = new DateTime(2026, 5, 10).Range(new(2026, 5, 12), 1, TimeUnit.Day).ToList();

        dates.Should().Equal(new DateTime(2026, 5, 10), new DateTime(2026, 5, 11), new DateTime(2026, 5, 12));
    }

    [Fact]
    public void EnumerateDays_Reverse_IncludesBothEnds()
    {
        var dates = new DateTime(2026, 5, 12).Range(new(2026, 5, 10), -1, TimeUnit.Day).ToList();

        dates.Should().Equal(new DateTime(2026, 5, 12), new DateTime(2026, 5, 11), new DateTime(2026, 5, 10));
    }

    [Fact]
    public void EnumerateDays_SingleDay_ReturnsOneDate() => new DateTime(2026, 5, 15).Range(new(2026, 5, 15), 1, TimeUnit.Day)
        .Should().ContainSingle().Which.Should().Be(new(2026, 5, 15));

    [Fact]
    public void ToDateRangePeriod_SingleDay_ReturnsSameCalendarBounds()
    {
        var date = new DateTime(2026, 6, 15);
        var period = CalendarDateRangeHelper.ToDateRangePeriod(date, date);

        period.Start!.Value.Value.DiscardTime().Should().Be(date);
        period.End!.Value.Value.DiscardTime().Should().Be(date);
    }

    [Fact]
    public void ToDateRangePeriod_MultiDay_PreservesInclusiveBounds()
    {
        var start = new DateTime(2026, 6, 1);
        var end = new DateTime(2026, 6, 7);
        var period = CalendarDateRangeHelper.ToDateRangePeriod(start, end);

        period.Start!.Value.Value.Should().Be(start);
        period.End!.Value.Value.Should().Be(end);
    }

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
