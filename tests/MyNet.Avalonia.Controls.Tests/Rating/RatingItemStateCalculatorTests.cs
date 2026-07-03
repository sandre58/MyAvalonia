// -----------------------------------------------------------------------
// <copyright file="RatingItemStateCalculatorTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using FluentAssertions;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Controls.Internals.Rating;
using Xunit;

namespace MyNet.Avalonia.Controls.Tests.Rating;

public class RatingItemStateCalculatorTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Case1_ValueZero_AllItemsEmpty(int index)
    {
        var state = RatingItemStateCalculator.Calculate(index, value: 0, previewValue: null);

        state.FillRatio.Should().Be(0);
        state.PreviewFillRatio.Should().Be(0);
        state.IsPreview.Should().BeFalse();
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 1)]
    [InlineData(3, 1)]
    [InlineData(4, 1)]
    [InlineData(5, 0)]
    public void Case2_ValueFourNoPreview_FillRatios(int index, double expectedFillRatio)
    {
        var state = RatingItemStateCalculator.Calculate(index, value: 4, previewValue: null);

        state.FillRatio.Should().Be(expectedFillRatio);
        state.IsPreview.Should().BeFalse();
    }

    [Fact]
    public void Case3_ValueThreePreviewFour_ExtensionOnFourthItem()
    {
        RatingItemStateCalculator.Calculate(1, 3, 4).IsPreview.Should().BeFalse();
        RatingItemStateCalculator.Calculate(2, 3, 4).IsPreview.Should().BeFalse();
        RatingItemStateCalculator.Calculate(3, 3, 4).IsPreview.Should().BeFalse();

        var item4 = RatingItemStateCalculator.Calculate(4, 3, 4);
        item4.FillRatio.Should().Be(0);
        item4.PreviewFillRatio.Should().Be(1);
        item4.IsPreviewExtend.Should().BeTrue();
        item4.IsPreviewHold.Should().BeFalse();
        item4.IsPreviewRetract.Should().BeFalse();

        var item5 = RatingItemStateCalculator.Calculate(5, 3, 4);
        item5.FillRatio.Should().Be(0);
        item5.IsPreview.Should().BeFalse();
    }

    [Fact]
    public void Case4_ValueFourPreviewTwo_RetractKeepsCommittedFill()
    {
        var item1 = RatingItemStateCalculator.Calculate(1, 4, 2);
        item1.FillRatio.Should().Be(1);
        item1.PreviewFillRatio.Should().Be(1);
        item1.IsPreviewHold.Should().BeTrue();
        item1.IsPreviewRetract.Should().BeFalse();

        var item2 = RatingItemStateCalculator.Calculate(2, 4, 2);
        item2.IsPreviewHold.Should().BeTrue();

        var item3 = RatingItemStateCalculator.Calculate(3, 4, 2);
        item3.FillRatio.Should().Be(1);
        item3.PreviewFillRatio.Should().Be(0);
        item3.IsPreviewRetract.Should().BeTrue();
        item3.IsPreviewHold.Should().BeFalse();

        var item4 = RatingItemStateCalculator.Calculate(4, 4, 2);
        item4.FillRatio.Should().Be(1);
        item4.IsPreviewRetract.Should().BeTrue();

        var item5 = RatingItemStateCalculator.Calculate(5, 4, 2);
        item5.FillRatio.Should().Be(0);
        item5.IsPreview.Should().BeFalse();
    }

    [Fact]
    public void ValueFour_PreviewTwoPointFive_SplitBoundary()
    {
        var item1 = RatingItemStateCalculator.Calculate(1, 4, 2.5);
        item1.IsPreviewHold.Should().BeTrue();
        item1.PreviewFillRatio.Should().Be(1);
        item1.IsPreviewSplit.Should().BeFalse();

        var item2 = RatingItemStateCalculator.Calculate(2, 4, 2.5);
        item2.IsPreviewHold.Should().BeTrue();
        item2.PreviewFillRatio.Should().Be(1);

        var item3 = RatingItemStateCalculator.Calculate(3, 4, 2.5);
        item3.FillRatio.Should().Be(1);
        item3.PreviewFillRatio.Should().BeApproximately(0.5, 0.001);
        item3.RetractFillRatio.Should().BeApproximately(0.5, 0.001);
        item3.IsPreviewSplit.Should().BeTrue();
        item3.IsPreviewHold.Should().BeFalse();
        item3.IsPreviewRetract.Should().BeFalse();

        var item4 = RatingItemStateCalculator.Calculate(4, 4, 2.5);
        item4.IsPreviewRetract.Should().BeTrue();
        item4.PreviewFillRatio.Should().Be(0);
        item4.IsPreviewSplit.Should().BeFalse();

        var item5 = RatingItemStateCalculator.Calculate(5, 4, 2.5);
        item5.IsPreview.Should().BeFalse();
    }

    [Fact]
    public void PartialFillRatio_ValueThreePointFive()
    {
        var state = RatingItemStateCalculator.Calculate(4, 3.5, previewValue: null);

        state.FillRatio.Should().BeApproximately(0.5, 0.001);
        state.IsPreview.Should().BeFalse();
    }

    [Fact]
    public void PreviewExtend_DoesNotMarkCommittedItems()
    {
        var state = RatingItemStateCalculator.Calculate(2, 3, 4);

        state.FillRatio.Should().Be(1);
        state.IsPreview.Should().BeFalse();
    }
}
