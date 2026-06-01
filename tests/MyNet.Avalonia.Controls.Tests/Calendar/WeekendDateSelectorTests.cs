// -----------------------------------------------------------------------
// <copyright file="WeekendDateSelectorTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using FluentAssertions;
using MyNet.Avalonia.Controls.Primitives;
using Xunit;

namespace MyNet.Avalonia.Controls.Tests.Calendar;

public class WeekendDateSelectorTests
{
    [Theory]
    [InlineData(DayOfWeek.Saturday, true)]
    [InlineData(DayOfWeek.Sunday, true)]
    [InlineData(DayOfWeek.Monday, false)]
    [InlineData(DayOfWeek.Friday, false)]
    public void Match_IdentifiesWeekendDays(DayOfWeek dayOfWeek, bool expected)
    {
        // 2026-05-31 is a Sunday
        var date = new DateTime(2026, 5, 31).AddDays(dayOfWeek - DayOfWeek.Sunday);

        WeekendDateSelector.Instance.Match(date).Should().Be(expected);
    }

    [Fact]
    public void Match_ReturnsFalseForNullDate() => WeekendDateSelector.Instance.Match(null).Should().BeFalse();
}
