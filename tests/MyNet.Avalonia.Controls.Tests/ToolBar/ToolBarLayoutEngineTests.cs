// -----------------------------------------------------------------------
// <copyright file="ToolBarLayoutEngineTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Layout;
using FluentAssertions;
using MyNet.Avalonia.Controls.Primitives;
using Xunit;

namespace MyNet.Avalonia.Controls.Tests.ToolBar;

/// <summary>
/// Pure C# unit tests for the toolbar layout engines.
/// No Avalonia headless platform required — engines only operate on Size/Rect/primitives.
/// Control.Element is passed as null! since engines never call methods on it.
/// </summary>
public class ToolBarLayoutEngineTests
{
    private static ToolBarChildState Item(double width, ToolBarOverflowPriority priority = ToolBarOverflowPriority.Normal, double height = 32, bool isSeparator = false)
        => new(null!, new(width, height), priority, isSeparator, null);

    // ── StandardToolBarLayoutEngine.Measure ──────────────────────────────────

    [Fact]
    public void Measure_EmptyList_ReturnsSizeEmpty()
    {
        var engine = new StandardToolBarLayoutEngine();
        engine.Measure([], Orientation.Horizontal, 4).Should().Be(new Size(0, 0));
    }

    [Fact]
    public void Measure_SingleItem_Horizontal_ReturnsSingleItemSize()
    {
        var engine = new StandardToolBarLayoutEngine();
        var result = engine.Measure([Item(50, height: 30)], Orientation.Horizontal, 4);
        result.Width.Should().Be(50);
        result.Height.Should().Be(30);
    }

    [Fact]
    public void Measure_ThreeItems_Horizontal_IncludesSpacing()
    {
        var engine = new StandardToolBarLayoutEngine();
        // 50 + 4 + 30 + 4 + 40 = 128
        var result = engine.Measure([Item(50), Item(30), Item(40)], Orientation.Horizontal, 4);
        result.Width.Should().Be(128);
    }

    [Fact]
    public void Measure_TwoItems_Vertical_IncludesSpacing()
    {
        var engine = new StandardToolBarLayoutEngine();
        // height = 40 + 4 + 30 = 74; width = max(60, 60) = 60
        var result = engine.Measure([Item(60, height: 40), Item(60, height: 30)], Orientation.Vertical, 4);
        result.Width.Should().Be(60);
        result.Height.Should().Be(74);
    }

    // ── StandardToolBarLayoutEngine.Arrange ──────────────────────────────────

    [Fact]
    public void Arrange_Horizontal_PositionsItemsLeftToRight()
    {
        var engine = new StandardToolBarLayoutEngine();
        var items = new[] { Item(50), Item(30), Item(40) };
        var input = new ToolBarArrangeInput(items, new(200, 32), Orientation.Horizontal, 4);
        var result = engine.Arrange(input);

        result.ArrangedItems.Should().HaveCount(3);
        result.ArrangedItems[0].Rect.X.Should().Be(0);
        result.ArrangedItems[1].Rect.X.Should().Be(54);   // 50 + 4
        result.ArrangedItems[2].Rect.X.Should().Be(88);   // 50 + 4 + 30 + 4
    }

    [Fact]
    public void Arrange_Horizontal_SetsFullHeightForAllItems()
    {
        var engine = new StandardToolBarLayoutEngine();
        var items = new[] { Item(50, height: 20), Item(30, height: 24) };
        var input = new ToolBarArrangeInput(items, new(200, 40), Orientation.Horizontal, 4);
        var result = engine.Arrange(input);

        result.ArrangedItems[0].Rect.Height.Should().Be(40);
        result.ArrangedItems[1].Rect.Height.Should().Be(40);
    }

    [Fact]
    public void Arrange_Vertical_PositionsItemsTopToBottom()
    {
        var engine = new StandardToolBarLayoutEngine();
        var items = new[] { Item(32, height: 40), Item(32, height: 30) };
        var input = new ToolBarArrangeInput(items, new(32, 200), Orientation.Vertical, 4);
        var result = engine.Arrange(input);

        result.ArrangedItems[0].Rect.Y.Should().Be(0);
        result.ArrangedItems[1].Rect.Y.Should().Be(44);   // 40 + 4
    }

    [Fact]
    public void Arrange_EmptyList_ReturnsEmptyResult()
    {
        var engine = new StandardToolBarLayoutEngine();
        var input = new ToolBarArrangeInput([], new(200, 32), Orientation.Horizontal, 4);
        var result = engine.Arrange(input);
        result.ArrangedItems.Should().BeEmpty();
    }

    // ── DefaultToolBarOverflowEngine.Resolve ─────────────────────────────────

    [Fact]
    public void Overflow_ModeNone_AllItemsVisibleRegardlessOfWidth()
    {
        var engine = new DefaultToolBarOverflowEngine();
        var items = new[] { Item(100), Item(100), Item(100) };
        var input = new ToolBarOverflowInput(items, new(10, 32), 4, ToolBarOverflowMode.None);
        var result = engine.Resolve(input);

        result.VisibleItems.Should().HaveCount(3);
        result.HasOverflow.Should().BeFalse();
    }

    [Fact]
    public void Overflow_Adaptive_AllFit_NoOverflow()
    {
        var engine = new DefaultToolBarOverflowEngine();
        var items = new[] { Item(50), Item(30), Item(40) };
        // 50+4+30+4+40 = 128 fits in 200
        var input = new ToolBarOverflowInput(items, new(200, 32), 4, ToolBarOverflowMode.Adaptive);
        var result = engine.Resolve(input);

        result.VisibleItems.Should().HaveCount(3);
        result.HasOverflow.Should().BeFalse();
    }

    [Fact]
    public void Overflow_Adaptive_TooNarrow_RemovesLowPriorityBeforeNormal()
    {
        var engine = new DefaultToolBarOverflowEngine();
        var normalItem = Item(50);
        var lowItem    = Item(50, ToolBarOverflowPriority.Low);
        var items = new[] { normalItem, lowItem };
        // 50+4+50 = 104 does not fit in 60 → lowItem goes first
        var input = new ToolBarOverflowInput(items, new(60, 32), 4, ToolBarOverflowMode.Adaptive);
        var result = engine.Resolve(input);

        result.VisibleItems.Should().ContainSingle().Which.Should().Be(normalItem);
        result.HasOverflow.Should().BeTrue();
    }

    [Fact]
    public void Overflow_AlwaysOverflowPriority_AlwaysExcluded_EvenWhenSpaceAvailable()
    {
        var engine = new DefaultToolBarOverflowEngine();
        var normalItem  = Item(30);
        var alwaysItem  = Item(30, ToolBarOverflowPriority.AlwaysOverflow);
        var items = new[] { normalItem, alwaysItem };
        var input = new ToolBarOverflowInput(items, new(500, 32), 4, ToolBarOverflowMode.Adaptive);
        var result = engine.Resolve(input);

        result.VisibleItems.Should().ContainSingle().Which.Should().Be(normalItem);
        result.HasOverflow.Should().BeTrue();
    }

    [Fact]
    public void Overflow_NeverOverflowPriority_AlwaysVisible_EvenWhenNoSpace()
    {
        var engine = new DefaultToolBarOverflowEngine();
        var neverItem  = Item(50, ToolBarOverflowPriority.NeverOverflow);
        var normalItem = Item(50);
        var items = new[] { neverItem, normalItem };
        // Only 60px available: normalItem overflows, neverItem stays
        var input = new ToolBarOverflowInput(items, new(60, 32), 4, ToolBarOverflowMode.Adaptive);
        var result = engine.Resolve(input);

        result.VisibleItems.Should().Contain(neverItem);
        result.VisibleItems.Should().NotContain(normalItem);
    }

    [Fact]
    public void Overflow_PreservesOriginalItemOrder_InVisibleSet()
    {
        var engine = new DefaultToolBarOverflowEngine();
        var a = Item(30);
        var b = Item(30, ToolBarOverflowPriority.Low);
        var c = Item(30);
        var items = new[] { a, b, c };
        // 30+4+30+4+30 = 98. Available = 70 → b (Low) goes
        var input = new ToolBarOverflowInput(items, new(70, 32), 4, ToolBarOverflowMode.Adaptive);
        var result = engine.Resolve(input);

        // a and c should be visible (in original order), b in overflow
        result.VisibleItems.Should().NotContain(b);
        result.VisibleItems.Should().HaveCount(2);
        result.VisibleItems[0].Should().Be(a);
        result.VisibleItems[1].Should().Be(c);
    }

    [Fact]
    public void Overflow_Adaptive_IgnoresInfiniteDesiredWidth()
    {
        var engine = new DefaultToolBarOverflowEngine();
        var infiniteItem = Item(double.PositiveInfinity);
        var normalItem = Item(50);
        var items = new[] { infiniteItem, normalItem };
        var input = new ToolBarOverflowInput(items, new(100, 32), 4, ToolBarOverflowMode.Adaptive);
        var result = engine.Resolve(input);

        result.VisibleItems.Should().HaveCount(2);
        result.HasOverflow.Should().BeFalse();
    }
}
