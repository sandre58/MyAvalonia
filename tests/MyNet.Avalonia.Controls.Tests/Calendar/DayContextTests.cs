// -----------------------------------------------------------------------
// <copyright file="DayContextTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using FluentAssertions;
using MyNet.Avalonia.Controls.Primitives;
using Xunit;

namespace MyNet.Avalonia.Controls.Tests.Calendar;

public class DayContextTests
{
    [Fact]
    public void FromDate_RoundTripsThroughToDate()
    {
        var date = new DateTime(2026, 5, 15);
        var context = new DayContext(15, 5, 2026).FromDate(date);

        context.Should().Be(new DayContext(15, 5, 2026));
        context.ToDate().Should().Be(date);
    }

    [Fact]
    public void Next_AdvancesOneDay() => new DayContext(31, 5, 2026).Next().Should().Be(new DayContext(1, 6, 2026));

    [Fact]
    public void FastNext_AdvancesOneMonth() => new DayContext(15, 5, 2026).FastNext().Should().Be(new DayContext(15, 6, 2026));

    [Fact]
    public void Previous_And_FastPrevious_MoveBackward()
    {
        new DayContext(1, 6, 2026).Previous().Should().Be(new DayContext(31, 5, 2026));
        new DayContext(15, 6, 2026).FastPrevious().Should().Be(new DayContext(15, 5, 2026));
    }
}
