// -----------------------------------------------------------------------
// <copyright file="PaginationHelperTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using FluentAssertions;
using MyNet.Avalonia.Controls.Internals.Pagination;
using Xunit;

namespace MyNet.Avalonia.Controls.Tests.Pagination;

public class PaginationHelperTests
{
    [Theory]
    [InlineData(100, 10, 10)]
    [InlineData(101, 10, 11)]
    [InlineData(0, 10, 0)]
    [InlineData(5, 0, 0)]
    public void CalculatePageCount_ComputesExpectedPages(int totalCount, int pageSize, int expected) => PaginationHelper.CalculatePageCount(totalCount, pageSize).Should().Be(expected);

    [Theory]
    [InlineData(0, 10, 1)]
    [InlineData(15, 10, 10)]
    [InlineData(5, 10, 5)]
    public void CoerceCurrentPage_ClampsToValidRange(int page, int pageCount, int expected) => PaginationHelper.CoerceCurrentPage(page, pageCount).Should().Be(expected);

    [Fact]
    public void AddPageOffset_ClampsWithinRange()
    {
        PaginationHelper.AddPageOffset(3, 10, 10).Should().Be(10);
        PaginationHelper.AddPageOffset(3, -5, 10).Should().Be(1);
    }

    [Fact]
    public void GetNavigationState_ReflectsCurrentPage()
    {
        PaginationHelper.GetNavigationState(1, 10).Should().Be((false, true));
        PaginationHelper.GetNavigationState(10, 10).Should().Be((true, false));
        PaginationHelper.GetNavigationState(null, 10).Should().Be((true, true));
    }

    [Fact]
    public void BuildButtonStates_ForSmallPageCount_ShowsSequentialPages()
    {
        var states = PaginationHelper.BuildButtonStates(2, 5);

        states.Should().HaveCount(7);
        states[0].Should().Be(new PaginationButtonState(1, true, false, false, false));
        states[1].Should().Be(new PaginationButtonState(2, true, true, false, false));
        states[2].Should().Be(new PaginationButtonState(3, true, false, false, false));
        states[5].IsVisible.Should().BeFalse();
    }

    [Fact]
    public void BuildButtonStates_ForLargePageCount_UsesEllipsisNearEdges()
    {
        var states = PaginationHelper.BuildButtonStates(5, 20);

        states[0].Page.Should().Be(1);
        states[6].Page.Should().Be(20);
        states[3].Page.Should().Be(5);
        states[1].IsLeftEllipsis.Should().BeTrue();
        states[5].IsRightEllipsis.Should().BeTrue();
    }

    [Fact]
    public void BuildButtonStates_WhenNearEnd_ReplacesRightEllipsisWithPageNumber()
    {
        var states = PaginationHelper.BuildButtonStates(18, 20);

        states[5].IsRightEllipsis.Should().BeFalse();
        states[5].Page.Should().Be(19);
        states[6].Page.Should().Be(20);
    }
}
