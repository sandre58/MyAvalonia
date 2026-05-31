// -----------------------------------------------------------------------
// <copyright file="PaginationExpandedTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using FluentAssertions;
using MyNet.Avalonia.Controls.Internals;
using Xunit;

namespace MyNet.Avalonia.Controls.Tests.Pagination;

public class PaginationExpandedTests
{
    [Fact]
    public void CoerceCurrentPage_WithNull_ReturnsNull()
    {
        PaginationHelper.CoerceCurrentPage(null, 10).Should().BeNull();
    }

    [Fact]
    public void CoerceCurrentPage_WithZeroPageCount_ClampsToOne()
    {
        PaginationHelper.CoerceCurrentPage(5, 0).Should().Be(1);
    }

    [Fact]
    public void ClampQuickJump_ClampsDecimalValue()
    {
        PaginationHelper.ClampQuickJump(0, 10).Should().Be(1);
        PaginationHelper.ClampQuickJump(99, 10).Should().Be(10);
        PaginationHelper.ClampQuickJump(4.9m, 10).Should().Be(4);
    }

    [Fact]
    public void BuildButtonStates_WithZeroPageCount_HidesAllButtons()
    {
        PaginationLayoutHelper.BuildButtonStates(1, 0).Should().OnlyContain(x => !x.IsVisible);
    }

    [Fact]
    public void BuildButtonStates_WithSevenPages_ShowsAllWithoutEllipsis()
    {
        var states = PaginationLayoutHelper.BuildButtonStates(4, 7);

        states.Should().OnlyContain(x => x.IsVisible);
        states.Should().NotContain(x => x.IsLeftEllipsis || x.IsRightEllipsis);
        states[3].IsSelected.Should().BeTrue();
    }

    [Fact]
    public void BuildButtonStates_AtStart_ShowsRightEllipsisOnly()
    {
        var states = PaginationLayoutHelper.BuildButtonStates(1, 20);

        states[1].IsLeftEllipsis.Should().BeFalse();
        states[5].IsRightEllipsis.Should().BeTrue();
    }
}
