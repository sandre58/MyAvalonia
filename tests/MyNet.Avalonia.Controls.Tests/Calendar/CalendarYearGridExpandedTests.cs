// -----------------------------------------------------------------------
// <copyright file="CalendarYearGridExpandedTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using FluentAssertions;
using MyNet.Avalonia.Controls.Internals.Calendar;
using MyNet.Avalonia.Controls.Primitives;
using Xunit;

namespace MyNet.Avalonia.Controls.Tests.Calendar;

public class CalendarYearGridExpandedTests
{
    [Fact]
    public void BuildCells_ForCenturyContext_ReturnsTwelveDecades()
    {
        var cells = CalendarYearGridHelper.BuildCells(new CenturyContext(2000), new(5, 2026));

        cells.Should().HaveCount(12);
        cells[1].DateContext.Should().BeOfType<DecadeContext>().Which.StartYear.Should().Be(2000);
        cells[0].IsInactive.Should().BeTrue();
    }

    [Fact]
    public void BuildCells_ForYearContext_MarksSelectedMonth()
    {
        var cells = CalendarYearGridHelper.BuildCells(new YearContext(2026), new(1, 2026));

        cells[0].IsSelected.Should().BeTrue();
        cells[1].IsSelected.Should().BeFalse();
    }

    [Fact]
    public void CellCount_IsTwelve() => CalendarYearGridHelper.CellCount.Should().Be(12);
}
