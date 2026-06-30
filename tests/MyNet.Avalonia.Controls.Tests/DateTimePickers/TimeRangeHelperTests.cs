// -----------------------------------------------------------------------
// <copyright file="TimeRangeHelperTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using FluentAssertions;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Controls.Internals;
using Xunit;

namespace MyNet.Avalonia.Controls.Tests.DateTimePickers;

public class TimeRangeHelperTests
{
    private static readonly DateTime ReferenceDate = new(2026, 6, 15);

    [Fact]
    public void BuildPeriod_SwapsWhenEndBeforeStart()
    {
        var result = TimeRangeHelper.BuildPeriod(
            new(17, 0, 0),
            new(9, 0, 0),
            ReferenceDate);

        result.IsValid.Should().BeTrue();
        TimeRangeHelper.GetPeriodStartTime(result.Period!).Should().Be(new(9, 0, 0));
        TimeRangeHelper.GetPeriodEndTime(result.Period!).Should().Be(new(17, 0, 0));
    }

    [Fact]
    public void BuildPeriod_AllowsOvernight()
    {
        var result = TimeRangeHelper.BuildPeriod(
            new(22, 0, 0),
            new(2, 0, 0),
            ReferenceDate,
            allowOvernight: true,
            TimeRangeInvalidBehavior.Swap);

        result.IsValid.Should().BeTrue();
        result.Period!.End!.Value.Value.Date.Should().Be(ReferenceDate.AddDays(1).Date);
    }

    [Fact]
    public void BuildPeriod_ReportErrorWhenEndBeforeStart()
    {
        var result = TimeRangeHelper.BuildPeriod(
            new(17, 0, 0),
            new(9, 0, 0),
            ReferenceDate,
            allowOvernight: false,
            TimeRangeInvalidBehavior.ReportError);

        result.IsValid.Should().BeFalse();
        result.ShouldReportError.Should().BeTrue();
    }

    [Fact]
    public void SpansOvernight_ReturnsTrueWhenEndIsNextDay()
    {
        var result = TimeRangeHelper.BuildPeriod(
            new(22, 0, 0),
            new(2, 0, 0),
            ReferenceDate,
            allowOvernight: true,
            TimeRangeInvalidBehavior.Swap);

        TimeRangeHelper.SpansOvernight(result.Period!).Should().BeTrue();
    }
}
