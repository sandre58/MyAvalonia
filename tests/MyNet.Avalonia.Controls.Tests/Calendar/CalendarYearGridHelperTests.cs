// -----------------------------------------------------------------------
// <copyright file="CalendarYearGridHelperTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using FluentAssertions;
using MyNet.Avalonia.Controls.Internals.Calendar;
using MyNet.Avalonia.Controls.Primitives;
using Xunit;

namespace MyNet.Avalonia.Controls.Tests.Calendar;

public class CalendarYearGridHelperTests
{
    [Fact]
    public void BuildCells_ForYearContext_ReturnsTwelveMonths()
    {
        var cells = CalendarYearGridHelper.BuildCells(new YearContext(2026), new(5, 2026));

        cells.Should().HaveCount(12);
        cells[0].DateContext.Should().Be(new MonthContext(1, 2026));
        cells[4].IsSelected.Should().BeTrue();
        cells[0].IsSelected.Should().BeFalse();
    }

    [Fact]
    public void BuildCells_ForDecadeContext_MarksInactiveYearsOutsideDecade()
    {
        var cells = CalendarYearGridHelper.BuildCells(new DecadeContext(2020), new(5, 2026));

        cells.Should().HaveCount(12);
        cells[0].DateContext.Should().BeOfType<YearContext>().Which.Year.Should().Be(2019);
        cells[0].IsInactive.Should().BeTrue();
        cells[1].IsInactive.Should().BeFalse();
    }

    [Fact]
    public void BuildCells_ForMonthContext_ReturnsEmpty() => CalendarYearGridHelper.BuildCells(new MonthContext(5, 2026), new(5, 2026))
        .Should().BeEmpty();
}
