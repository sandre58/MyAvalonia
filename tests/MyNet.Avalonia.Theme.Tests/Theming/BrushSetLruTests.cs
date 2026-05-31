// -----------------------------------------------------------------------
// <copyright file="BrushSetLruTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Avalonia.Media;
using FluentAssertions;
using MyNet.Avalonia.Theme.Theming.Brushes;
using MediaColors = Avalonia.Media.Colors;
using Xunit;

namespace MyNet.Avalonia.Theme.Tests.Theming;

public class BrushSetLruTests
{
    [Fact]
    public void GetTransformedBrush_ExceedsCapacity_EvictsLeastRecentlyUsed()
    {
        var evicted = new List<ISolidColorBrush>();
        var set = new BrushSet(MediaColors.Navy, MediaColors.White, transformedBrushCapacity: 2, onTransformedBrushEvicted: evicted.Add);

        var first = set.GetTransformedBrush(new(0.25));
        var second = set.GetTransformedBrush(new(0.5));
        set.TransformedBrushCacheCount.Should().Be(2);

        var third = set.GetTransformedBrush(new(0.75));

        set.TransformedBrushCacheCount.Should().Be(2);
        evicted.Should().ContainSingle().Which.Should().BeSameAs(first);

        set.GetTransformedBrush(new(0.5)).Should().BeSameAs(second);
        set.GetTransformedBrush(new(0.75)).Should().BeSameAs(third);
        set.GetTransformedBrush(new(0.25)).Should().NotBeSameAs(first);
    }

    [Fact]
    public void GetTransformedBrush_RecentEntry_IsPromotedAndNotEvicted()
    {
        var evicted = new List<ISolidColorBrush>();
        var set = new BrushSet(MediaColors.Black, MediaColors.White, transformedBrushCapacity: 2, onTransformedBrushEvicted: evicted.Add);

        var keep = set.GetTransformedBrush(new(0.25));
        set.GetTransformedBrush(new(0.5));

        set.GetTransformedBrush(new(0.25));
        set.GetTransformedBrush(new(0.75));

        evicted.Should().ContainSingle();
        set.GetTransformedBrush(new(0.25)).Should().BeSameAs(keep);
    }

    [Fact]
    public void UpdateColor_UpdatesOnlyCachedTransformedBrushes()
    {
        var set = new BrushSet(MediaColors.Blue, MediaColors.White, transformedBrushCapacity: 4);
        var half = set.GetTransformedBrush(new(0.5));

        set.UpdateColor(MediaColors.Red, MediaColors.Black);

        half.Color.Should().Be(MediaColors.Red);
        half.Opacity.Should().BeApproximately(0.5, 0.001);
        set.Brush.Color.Should().Be(MediaColors.Red);
    }
}
