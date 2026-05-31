// -----------------------------------------------------------------------
// <copyright file="PaginationLayoutHelperTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using FluentAssertions;
using MyNet.Avalonia.Controls.Internals;
using Xunit;

namespace MyNet.Avalonia.Controls.Tests.Pagination;

public class PaginationLayoutHelperTests
{
    [Fact]
    public void BuildButtonStates_ForSmallPageCount_ShowsSequentialPages()
    {
        var states = PaginationLayoutHelper.BuildButtonStates(2, 5);

        states.Should().HaveCount(7);
        states[0].Should().Be(new PaginationButtonState(1, true, false, false, false));
        states[1].Should().Be(new PaginationButtonState(2, true, true, false, false));
        states[2].Should().Be(new PaginationButtonState(3, true, false, false, false));
        states[5].IsVisible.Should().BeFalse();
    }

    [Fact]
    public void BuildButtonStates_ForLargePageCount_UsesEllipsisNearEdges()
    {
        var states = PaginationLayoutHelper.BuildButtonStates(5, 20);

        states[0].Page.Should().Be(1);
        states[6].Page.Should().Be(20);
        states[3].Page.Should().Be(5);
        states[1].IsLeftEllipsis.Should().BeTrue();
        states[5].IsRightEllipsis.Should().BeTrue();
    }

    [Fact]
    public void BuildButtonStates_WhenNearEnd_ReplacesRightEllipsisWithPageNumber()
    {
        var states = PaginationLayoutHelper.BuildButtonStates(18, 20);

        states[5].IsRightEllipsis.Should().BeFalse();
        states[5].Page.Should().Be(19);
        states[6].Page.Should().Be(20);
    }
}
