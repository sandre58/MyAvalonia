// -----------------------------------------------------------------------
// <copyright file="YearDecadeCenturyContextTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using FluentAssertions;
using MyNet.Avalonia.Controls.Primitives;
using Xunit;

namespace MyNet.Avalonia.Controls.Tests.Calendar;

public class YearDecadeCenturyContextTests
{
    [Fact]
    public void YearContext_NavigationSteps()
    {
        var year = new YearContext(2026);
        year.Next().Should().Be(new YearContext(2027));
        year.Previous().Should().Be(new YearContext(2025));
        year.FastNext().Should().Be(new YearContext(2036));
        year.FastPrevious().Should().Be(new YearContext(2016));
        year.ToDate().Should().Be(new DateTime(2026, 1, 1));
    }

    [Fact]
    public void DecadeContext_ReportsEndYearAndSimilarity()
    {
        var decade = new DecadeContext(2020);
        decade.EndYear.Should().Be(2030);
        decade.IsSimilar(new DateTime(2029, 12, 31)).Should().BeTrue();
        decade.IsSimilar(new DateTime(2031, 1, 1)).Should().BeFalse();
        decade.FastNext().Should().Be(new DecadeContext(2120));
    }

    [Fact]
    public void CenturyContext_ReportsEndYearAndFastNavigation()
    {
        var century = new CenturyContext(2000);
        century.EndYear.Should().Be(2100);
        century.IsSimilar(new DateTime(2099, 12, 31)).Should().BeTrue();
        century.FastNext().Should().Be(new CenturyContext(3000));
        century.FastPrevious().Should().Be(new CenturyContext(1000));
    }

    [Fact]
    public void FromDate_AlignsToContextStart()
    {
        new DecadeContext(2020).FromDate(new DateTime(2026, 5, 15)).Should().Be(new DecadeContext(2026));
        new CenturyContext(2000).FromDate(new DateTime(2026, 5, 15)).Should().Be(new CenturyContext(2026));
    }
}
