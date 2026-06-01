// -----------------------------------------------------------------------
// <copyright file="DateContextTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using FluentAssertions;
using MyNet.Avalonia.Controls.Primitives;
using Xunit;

namespace MyNet.Avalonia.Controls.Tests.Calendar;

public class DateContextTests
{
    [Fact]
    public void MonthContext_Next_WrapsYearAtDecember()
    {
        var context = new MonthContext(12, 2025);

        context.Next().Should().Be(new MonthContext(1, 2026));
    }

    [Fact]
    public void MonthContext_Previous_WrapsYearAtJanuary()
    {
        var context = new MonthContext(1, 2026);

        context.Previous().Should().Be(new MonthContext(12, 2025));
    }

    [Fact]
    public void MonthContext_Add_SupportsNegativeOffset() => new MonthContext(3, 2026).Add(-2).Should().Be(new MonthContext(1, 2026));

    [Fact]
    public void DayContext_IsSimilar_MatchesExactDay()
    {
        var context = new DayContext(10, 5, 2026);

        context.IsSimilar(new(2026, 5, 10)).Should().BeTrue();
        context.IsSimilar(new(2026, 5, 11)).Should().BeFalse();
    }

    [Fact]
    public void DecadeContext_IsSimilar_MatchesYearsInRange()
    {
        var context = new DecadeContext(2020);

        context.IsSimilar(new(2020, 1, 1)).Should().BeTrue();
        context.IsSimilar(new(2029, 12, 31)).Should().BeTrue();
        context.IsSimilar(new(2019, 12, 31)).Should().BeFalse();
    }

    [Fact]
    public void YearContext_FastNext_AdvancesByTenYears() => new YearContext(2020).FastNext().Should().Be(new YearContext(2030));
}
