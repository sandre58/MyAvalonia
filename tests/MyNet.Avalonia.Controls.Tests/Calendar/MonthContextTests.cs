// -----------------------------------------------------------------------
// <copyright file="MonthContextTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using FluentAssertions;
using MyNet.Avalonia.Controls.Primitives;
using Xunit;

namespace MyNet.Avalonia.Controls.Tests.Calendar;

public class MonthContextTests
{
    [Fact]
    public void Add_SupportsMultiYearOffset()
    {
        new MonthContext(11, 2025).Add(3).Should().Be(new MonthContext(2, 2026));
    }

    [Fact]
    public void AddYears_AddDecades_AddCenturies_Work()
    {
        var context = new MonthContext(5, 2020);
        context.AddYears(2).Should().Be(new MonthContext(5, 2022));
        context.AddDecades(1).Should().Be(new MonthContext(5, 2030));
        context.AddCenturies(1).Should().Be(new MonthContext(5, 2120));
    }

    [Fact]
    public void BeginningAndEnd_BoundariesAreCorrect()
    {
        var context = new MonthContext(5, 2026);
        context.BeginningOfYear().Should().Be(new MonthContext(1, 2026));
        context.EndOfYear().Should().Be(new MonthContext(12, 2026));
        context.BeginningOfDecade().Year.Should().Be(2020);
        context.EndOfDecade().Should().Be(new MonthContext(12, 2029));
        context.BeginningOfCentury().Year.Should().Be(2000);
        context.EndOfCentury().Should().Be(new MonthContext(12, 2099));
    }

    [Fact]
    public void FastNext_And_FastPrevious_MoveByYear()
    {
        new MonthContext(5, 2026).FastNext().Should().Be(new MonthContext(5, 2027));
        new MonthContext(5, 2026).FastPrevious().Should().Be(new MonthContext(5, 2025));
    }

    [Fact]
    public void IsSimilar_MatchesMonthAndYearOnly()
    {
        var context = new MonthContext(5, 2026);
        context.IsSimilar(new DateTime(2026, 5, 31)).Should().BeTrue();
        context.IsSimilar(new DateTime(2026, 6, 1)).Should().BeFalse();
    }
}
