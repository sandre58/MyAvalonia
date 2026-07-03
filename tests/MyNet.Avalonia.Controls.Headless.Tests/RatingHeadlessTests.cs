// -----------------------------------------------------------------------
// <copyright file="RatingHeadlessTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using FluentAssertions;
using MyNet.Avalonia.Controls.Enums;
using MyNet.Avalonia.Controls.Internals;

namespace MyNet.Avalonia.Controls.Headless.Tests;

public class RatingHeadlessTests
{
    [AvaloniaFact]
    public void ApplyTemplate_CreatesItemsPanel()
    {
        var rating = new Rating { MaxRating = 5 };

        HeadlessControlHost.Show(rating, new(240, 48));

        HeadlessControlHost.FindByName<Panel>(rating, Rating.PartItemsPanel).Should().NotBeNull();
        rating.Items.Should().HaveCount(5);
    }

    [AvaloniaFact]
    public void Value_UpdatesFillRatio()
    {
        var rating = new Rating { MaxRating = 5, Value = 3.7 };

        HeadlessControlHost.Show(rating, new(240, 48));

        rating.Items[3].FillRatio.Should().BeApproximately(0.7, 0.001);
        rating.Items[0].FillRatio.Should().Be(1);
        rating.Items[4].FillRatio.Should().Be(0);
    }

    [AvaloniaFact]
    public void IsReadOnly_SetsPseudoClass()
    {
        var rating = new Rating { IsReadOnly = true, Value = 2 };

        HeadlessControlHost.Show(rating, new(240, 48));

        rating.Classes.Should().Contain(":readonly");
        rating.Focusable.Should().BeFalse();
    }

    [AvaloniaFact]
    public void Click_UpdatesValue()
    {
        var rating = new Rating { MaxRating = 5, Value = 0, ItemSize = 40 };

        HeadlessControlHost.Show(rating, new(300, 64));

        var item = rating.Items[2];
        HeadlessControlHost.PointerMoveAt(item, new(35, 20));
        HeadlessControlHost.PointerPress(item);
        HeadlessControlHost.PointerRelease(item);

        rating.Value.Should().Be(3);
    }

    [AvaloniaFact]
    public void Click_Integer_SelectsWholeStar()
    {
        var rating = new Rating { MaxRating = 5, Value = 0, ItemSize = 40, Precision = RatingPrecision.Integer };

        HeadlessControlHost.Show(rating, new(300, 64));

        var item = rating.Items[1];
        HeadlessControlHost.PointerMoveAt(item, new(30, 20));
        HeadlessControlHost.PointerPress(item);
        HeadlessControlHost.PointerRelease(item);

        rating.Value.Should().Be(2);
    }

    [AvaloniaFact]
    public void Integer_AnyPosition_SelectsWholeStar()
    {
        var rating = new Rating { MaxRating = 5, Value = 0, ItemSize = 40, Precision = RatingPrecision.Integer };

        HeadlessControlHost.Show(rating, new(300, 64));

        var item = rating.Items[1];
        HeadlessControlHost.PointerMoveAt(item, new(10, 20));
        HeadlessControlHost.PointerPress(item);
        HeadlessControlHost.PointerRelease(item);

        rating.Value.Should().Be(2);
    }

    [AvaloniaFact]
    public void Integer_PointerEnter_SelectsWholeStar()
    {
        var rating = new Rating { MaxRating = 5, Value = 0, ItemSize = 40, Precision = RatingPrecision.Integer };

        HeadlessControlHost.Show(rating, new(300, 64));

        var item = rating.Items[1];
        HeadlessControlHost.PointerEnter(item);

        rating.Items[1].PreviewFillRatio.Should().Be(1);
        rating.Items[1].IsPreview.Should().BeTrue();
    }

    [AvaloniaFact]
    public void PointerFraction_LeftAligned_WideItem()
    {
        var fraction = RatingValueHelper.GetPointerFractionInContent(
            isHorizontal: true,
            contentSize: 40,
            itemWidth: 60,
            itemHeight: 48,
            paddingLeft: 10,
            paddingTop: 4,
            paddingRight: 10,
            paddingBottom: 4,
            x: 30,
            y: 20);

        fraction.Should().BeApproximately(0.5, 0.001);
    }

    [AvaloniaFact]
    public void Half_PointerRightHalf_SelectsFullStar()
    {
        var rating = new Rating { MaxRating = 5, Value = 0, ItemSize = 40, Precision = RatingPrecision.Half };

        HeadlessControlHost.Show(rating, new(300, 64));

        var item = rating.Items[1];
        HeadlessControlHost.PointerMoveAt(item, new(30, 20));

        item.PreviewFillRatio.Should().BeApproximately(1, 0.001);
        item.IsPreview.Should().BeTrue();
    }

    [AvaloniaFact]
    public void Half_PointerCenter_SetsHalfPreviewFill()
    {
        var rating = new Rating { MaxRating = 5, Value = 0, ItemSize = 40, Precision = RatingPrecision.Half };

        HeadlessControlHost.Show(rating, new(300, 64));

        var item = rating.Items[1];
        HeadlessControlHost.PointerMoveAt(item, new(20, 20));

        item.PreviewFillRatio.Should().BeApproximately(0.5, 0.001);
        item.IsPreview.Should().BeTrue();
    }

    [AvaloniaFact]
    public void Continuous_PointerPartial_SetsPreviewFill()
    {
        var rating = new Rating { MaxRating = 5, Value = 0, ItemSize = 40, Precision = RatingPrecision.Continuous };

        HeadlessControlHost.Show(rating, new(300, 64));

        var item = rating.Items[1];
        HeadlessControlHost.PointerMoveAt(item, new(16, 20));

        item.PreviewFillRatio.Should().BeApproximately(0.4, 0.001);
        item.IsPreview.Should().BeTrue();
    }

    [AvaloniaFact]
    public void ArrowRight_UpdatesPreviewOnly()
    {
        var rating = new Rating { MaxRating = 5, Value = 2, Precision = RatingPrecision.Integer };

        HeadlessControlHost.Show(rating, new(240, 48));
        rating.Focus();

        HeadlessControlHost.KeyDown(rating, Key.Right);

        rating.Value.Should().Be(2);
        rating.Items[2].IsPreviewExtend.Should().BeTrue();
        rating.Items[2].PreviewFillRatio.Should().Be(1);
    }

    [AvaloniaFact]
    public void ArrowThenEnter_CommitsPreview()
    {
        var rating = new Rating { MaxRating = 5, Value = 2, Precision = RatingPrecision.Integer };

        HeadlessControlHost.Show(rating, new(240, 48));
        rating.Focus();

        HeadlessControlHost.KeyDown(rating, Key.Right);
        HeadlessControlHost.KeyDown(rating, Key.Enter);

        rating.Value.Should().Be(3);
        rating.Items.Should().OnlyContain(item => !item.IsPreview);
    }

    [AvaloniaFact]
    public void ArrowThenSpace_CommitsPreview()
    {
        var rating = new Rating { MaxRating = 5, Value = 2, Precision = RatingPrecision.Integer };

        HeadlessControlHost.Show(rating, new(240, 48));
        rating.Focus();

        HeadlessControlHost.KeyDown(rating, Key.Right);
        HeadlessControlHost.KeyDown(rating, Key.Space);

        rating.Value.Should().Be(3);
        rating.Items.Should().OnlyContain(item => !item.IsPreview);
    }

    [AvaloniaFact]
    public void ArrowThenEscape_CancelsPreview()
    {
        var rating = new Rating { MaxRating = 5, Value = 2, Precision = RatingPrecision.Integer };

        HeadlessControlHost.Show(rating, new(240, 48));
        rating.Focus();

        HeadlessControlHost.KeyDown(rating, Key.Right);
        HeadlessControlHost.KeyDown(rating, Key.Escape);

        rating.Value.Should().Be(2);
        rating.Items.Should().OnlyContain(item => !item.IsPreview);
    }

    [AvaloniaFact]
    public void EnterWithoutPreview_DoesNotChangeValue()
    {
        var rating = new Rating { MaxRating = 5, Value = 2, Precision = RatingPrecision.Integer };

        HeadlessControlHost.Show(rating, new(240, 48));
        rating.Focus();

        HeadlessControlHost.KeyDown(rating, Key.Enter);

        rating.Value.Should().Be(2);
        rating.Items.Should().OnlyContain(item => !item.IsPreview);
    }

    [AvaloniaFact]
    public void LostFocus_ClearsPreview()
    {
        var rating = new Rating { MaxRating = 5, Value = 2, Precision = RatingPrecision.Integer };
        var button = new Button();
        var panel = new StackPanel { Children = { rating, button } };

        HeadlessControlHost.Show(panel, new(240, 96));
        rating.Focus();

        HeadlessControlHost.KeyDown(rating, Key.Right);
        rating.Items[2].IsPreviewExtend.Should().BeTrue();

        button.Focus();

        rating.Value.Should().Be(2);
        rating.Items.Should().OnlyContain(item => !item.IsPreview);
    }

    [AvaloniaFact]
    public void DigitKey_SetsValue()
    {
        var rating = new Rating { MaxRating = 5, Value = 0 };

        HeadlessControlHost.Show(rating, new(240, 48));
        rating.Focus();

        HeadlessControlHost.KeyDown(rating, Key.D4);

        rating.Value.Should().Be(4);
    }

    [AvaloniaFact]
    public void Preview_OnlyMarksStarsAboveCommittedValue()
    {
        var rating = new Rating { MaxRating = 5, Value = 2, ItemSize = 40 };

        HeadlessControlHost.Show(rating, new(300, 64));

        HeadlessControlHost.PointerMoveAt(rating.Items[3], new(30, 20));

        rating.Items[0].IsPreview.Should().BeFalse();
        rating.Items[1].IsPreview.Should().BeFalse();
        rating.Items[2].IsPreview.Should().BeFalse();
        rating.Items[3].IsPreviewExtend.Should().BeTrue();
        rating.Items[3].PreviewFillRatio.Should().Be(1);
    }

    [AvaloniaFact]
    public void Preview_ShowsPreviewLayerOnRetract()
    {
        var rating = new Rating { MaxRating = 5, Value = 4, ItemSize = 40 };

        HeadlessControlHost.Show(rating, new(300, 64));

        HeadlessControlHost.PointerMoveAt(rating.Items[1], new(30, 20));

        rating.Items[0].IsPreviewHold.Should().BeTrue();
        rating.Items[1].IsPreviewHold.Should().BeTrue();
        rating.Items[2].IsPreviewRetract.Should().BeTrue();
        rating.Items[3].IsPreviewRetract.Should().BeTrue();
        rating.Items[2].PreviewFillRatio.Should().Be(0);
        rating.Items[3].PreviewFillRatio.Should().Be(0);
        rating.Items[2].Classes.Should().Contain(":preview-retract");
        rating.Items[3].Classes.Should().Contain(":preview-retract");
        rating.Items[0].Classes.Should().Contain(":preview-hold");
        rating.Items[1].Classes.Should().Contain(":preview-hold");
        rating.Items[0].Classes.Should().Contain(":full");
        rating.Items[1].FillRatio.Should().Be(1);
        rating.Items[2].FillRatio.Should().Be(1);
        rating.Items[3].FillRatio.Should().Be(1);
    }

    [AvaloniaFact]
    public void Preview_ExtensionDelta()
    {
        var rating = new Rating { MaxRating = 5, Value = 2, ItemSize = 40 };

        HeadlessControlHost.Show(rating, new(300, 64));

        HeadlessControlHost.PointerMoveAt(rating.Items[3], new(30, 20));

        rating.Items[0].IsPreview.Should().BeFalse();
        rating.Items[1].IsPreview.Should().BeFalse();
        rating.Items[2].IsPreview.Should().BeFalse();
        rating.Items[3].IsPreviewExtend.Should().BeTrue();
        rating.Items[3].PreviewFillRatio.Should().Be(1);
    }

    [AvaloniaFact]
    public void Preview_Retract_KeepsAttenuatedCommitted()
    {
        var rating = new Rating { MaxRating = 5, Value = 4, ItemSize = 40 };

        HeadlessControlHost.Show(rating, new(300, 64));

        HeadlessControlHost.PointerMoveAt(rating.Items[1], new(30, 20));

        rating.Items[2].FillRatio.Should().Be(1);
        rating.Items[3].FillRatio.Should().Be(1);
        rating.Items[2].IsPreviewRetract.Should().BeTrue();
        rating.Items[3].IsPreviewRetract.Should().BeTrue();
    }

    [AvaloniaFact]
    public void Preview_Extend_UnchangedStarsNoPreview()
    {
        var rating = new Rating { MaxRating = 5, Value = 3, ItemSize = 40 };

        HeadlessControlHost.Show(rating, new(300, 64));

        HeadlessControlHost.PointerMoveAt(rating.Items[3], new(30, 20));

        rating.Items[0].IsPreview.Should().BeFalse();
        rating.Items[1].IsPreview.Should().BeFalse();
        rating.Items[2].IsPreview.Should().BeFalse();
        rating.Items[3].IsPreviewExtend.Should().BeTrue();
    }

    [AvaloniaFact]
    public void Preview_Clear_ResetsPseudoClasses()
    {
        var rating = new Rating { MaxRating = 5, Value = 0, ItemSize = 40 };

        HeadlessControlHost.Show(rating, new(300, 64));

        var item = rating.Items[2];
        HeadlessControlHost.PointerMoveAt(item, new(20, 20));
        item.IsPreviewExtend.Should().BeTrue();
        item.Classes.Should().Contain(":preview-extend");

        HeadlessControlHost.PointerExitedAt(rating, new(300, 64));
        item.IsPreview.Should().BeFalse();
        item.PreviewFillRatio.Should().Be(0);
        item.Classes.Should().Contain(":empty");
        item.Classes.Should().NotContain(":preview");
        item.Classes.Should().NotContain(":preview-extend");
    }

    [AvaloniaFact]
    public void ValueZero_AllItemsEmpty()
    {
        var rating = new Rating { MaxRating = 5, Value = 0 };

        HeadlessControlHost.Show(rating, new(240, 48));

        foreach (var item in rating.Items)
        {
            item.FillRatio.Should().Be(0);
            item.Classes.Should().Contain(":empty");
            item.Classes.Should().NotContain(":full");
            item.Classes.Should().NotContain(":partial");
        }
    }

    [AvaloniaFact]
    public void ValueFourNoPreview_FullAndEmptyPseudoClasses()
    {
        var rating = new Rating { MaxRating = 5, Value = 4 };

        HeadlessControlHost.Show(rating, new(240, 48));

        rating.Items[0].Classes.Should().Contain(":full");
        rating.Items[3].Classes.Should().Contain(":full");
        rating.Items[4].Classes.Should().Contain(":empty");
        rating.Items[4].Classes.Should().NotContain(":full");
    }

    [AvaloniaFact]
    public void Precision_Half_PreviewTwoPointFive_SetsSplitOnThirdItem()
    {
        var rating = new Rating { MaxRating = 5, Value = 4, ItemSize = 40, Precision = RatingPrecision.Half };

        HeadlessControlHost.Show(rating, new(300, 64));

        HeadlessControlHost.PointerMoveAt(rating.Items[2], new(10, 20));

        rating.Items[2].IsPreviewSplit.Should().BeTrue();
        rating.Items[2].PreviewFillRatio.Should().BeApproximately(0.5, 0.001);
        rating.Items[2].RetractFillRatio.Should().BeApproximately(0.5, 0.001);
        rating.Items[2].FilledClipRatio.Should().BeApproximately(0.5, 0.001);
        rating.Items[2].FilledClipOffsetRatio.Should().BeApproximately(0.5, 0.001);
        rating.Items[2].FilledSymbolOffsetRatio.Should().BeApproximately(-0.5, 0.001);
        rating.Items[2].Classes.Should().Contain(":preview-split");
        rating.Items[0].IsPreviewHold.Should().BeTrue();
        rating.Items[1].IsPreviewHold.Should().BeTrue();
        rating.Items[3].IsPreviewRetract.Should().BeTrue();
    }

    [AvaloniaFact]
    public void Value_HalfPrecision_SetsPartialFillRatio()
    {
        var rating = new Rating { MaxRating = 5, Value = 3.5, Precision = RatingPrecision.Half };

        HeadlessControlHost.Show(rating, new(240, 48));

        rating.Items[3].FillRatio.Should().BeApproximately(0.5, 0.001);
        rating.Items[3].Classes.Should().Contain(":partial");
    }
}
