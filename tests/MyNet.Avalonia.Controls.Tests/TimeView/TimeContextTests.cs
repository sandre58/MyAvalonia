// -----------------------------------------------------------------------
// <copyright file="TimeContextTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using FluentAssertions;
using MyNet.Avalonia.Controls.Primitives;
using MyNet.Primitives.Temporal;
using Xunit;

namespace MyNet.Avalonia.Controls.Tests.TimeView;

public class TimeContextTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(5)]
    [InlineData(12)]
    [InlineData(23)]
    public void ToTimeSpan_TwentyFourHour_PreservesHour(int hour)
    {
        var context = new TimeContext(hour, 30, 0, false);

        context.ToTimeSpan(TimeFormat.TwentyFourHour).Should().Be(new TimeSpan(hour, 30, 0));
    }

    [Theory]
    [InlineData(12, true, 0)]
    [InlineData(12, false, 12)]
    [InlineData(3, true, 3)]
    [InlineData(3, false, 15)]
    [InlineData(1, true, 1)]
    [InlineData(11, false, 23)]
    public void ToTimeSpan_TwelveHour_ConvertsUsingAmPm(int hour, bool isAm, int expectedHour)
    {
        var context = new TimeContext(hour, 45, 0, isAm);

        context.ToTimeSpan(TimeFormat.TwelveHour).Should().Be(new TimeSpan(expectedHour, 45, 0));
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(12, 0, 0)]
    [InlineData(12, 30, 0)]
    [InlineData(15, 0, 0)]
    [InlineData(23, 59, 59)]
    public void RoundTrip_TwentyFourHour_PreservesTime(int hour, int minute, int second)
    {
        var original = new TimeSpan(hour, minute, second);
        var context = TimeContext.FromTimeSpan(original, TimeFormat.TwentyFourHour);

        context.ToTimeSpan(TimeFormat.TwentyFourHour).Should().Be(original);
    }

    [Theory]
    [InlineData(0, 0, 0, 12, true)]
    [InlineData(12, 0, 0, 12, false)]
    [InlineData(15, 30, 0, 3, false)]
    [InlineData(9, 15, 45, 9, true)]
    public void RoundTrip_TwelveHour_PreservesTime(int hour, int minute, int second, int expectedDisplayHour, bool expectedIsAm)
    {
        var original = new TimeSpan(hour, minute, second);
        var context = TimeContext.FromTimeSpan(original, TimeFormat.TwelveHour);

        context.Hours.Should().Be(expectedDisplayHour);
        context.IsAm.Should().Be(expectedIsAm);
        context.ToTimeSpan(TimeFormat.TwelveHour).Should().Be(original);
    }

    [Fact]
    public void ToTimeSpan_ReturnsNull_WhenHoursMissing()
    {
        var context = new TimeContext(null, 30, 0, true);

        context.ToTimeSpan(TimeFormat.TwentyFourHour).Should().BeNull();
        context.ToTimeSpan(TimeFormat.TwelveHour).Should().BeNull();
    }
}
