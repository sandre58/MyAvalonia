// -----------------------------------------------------------------------
// <copyright file="TimeRangeHelperTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using FluentAssertions;
using MyNet.Avalonia.Controls.DateTimePickers.Internal;
using Xunit;

namespace MyNet.Avalonia.Controls.Tests.DateTimePickers;

public class TimeRangeHelperTests
{
    private static readonly DateTime ReferenceDate = new(2026, 6, 15);

    [Fact]
    public void CoerceSameDayRange_WhenEndEdited_AdjustsStartToEnd()
    {
        var (start, end) = TimeRangeHelper.CoerceSameDayRange(
            new(15, 32, 0),
            new(14, 12, 0),
            TimeRangeBoundary.End);

        start.Should().Be(new(14, 12, 0));
        end.Should().Be(new(14, 12, 0));
    }

    [Fact]
    public void CoerceSameDayRange_WhenStartEdited_AdjustsEndToStart()
    {
        var (start, end) = TimeRangeHelper.CoerceSameDayRange(
            new(19, 0, 0),
            new(18, 55, 0),
            TimeRangeBoundary.Start);

        start.Should().Be(new(19, 0, 0));
        end.Should().Be(new(19, 0, 0));
    }

    [Fact]
    public void CoerceSameDayRange_WhenValid_LeavesTimesUnchanged()
    {
        var start = new TimeSpan(9, 0, 0);
        var end = new TimeSpan(17, 0, 0);

        TimeRangeHelper.CoerceSameDayRange(start, end, TimeRangeBoundary.End)
            .Should().Be((start, end));
    }

    [Fact]
    public void BuildPeriod_SameDayInstant_UsesMinimalNonEmptyInterval()
    {
        var result = TimeRangeHelper.BuildPeriod(
            new(14, 12, 0),
            new(14, 12, 0),
            ReferenceDate);

        result.IsValid.Should().BeTrue();
        TimeRangeHelper.GetPeriodStartTime(result.Period!).Should().Be(new(14, 12, 0));
        TimeRangeHelper.GetPeriodEndTime(result.Period!).Should().Be(new(14, 12, 0));
    }

    [Fact]
    public void BuildPeriod_AllowsOvernight()
    {
        var result = TimeRangeHelper.BuildPeriod(
            new(22, 0, 0),
            new(2, 0, 0),
            ReferenceDate,
            allowOvernight: true);

        result.IsValid.Should().BeTrue();
        result.Period!.End!.Value.Value.Date.Should().Be(ReferenceDate.AddDays(1).Date);
    }

    [Fact]
    public void SpansOvernight_ReturnsTrueWhenEndIsNextDay()
    {
        var result = TimeRangeHelper.BuildPeriod(
            new(22, 0, 0),
            new(2, 0, 0),
            ReferenceDate,
            allowOvernight: true);

        TimeRangeHelper.SpansOvernight(result.Period!).Should().BeTrue();
    }
}
