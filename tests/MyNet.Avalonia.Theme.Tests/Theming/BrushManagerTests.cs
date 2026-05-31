// -----------------------------------------------------------------------
// <copyright file="BrushManagerTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Media;
using FluentAssertions;
using MyNet.Avalonia.Theme.Theming.Brushes;
using MediaColors = Avalonia.Media.Colors;
using Xunit;

namespace MyNet.Avalonia.Theme.Tests.Theming;

public class BrushManagerTests
{
    private readonly BrushManager _manager = new(null, null);

    [Fact]
    public void Register_CreatesMutableBrushForKey()
    {
        var brush = _manager.Register("MyNet.Brush.Test", MediaColors.DodgerBlue);

        brush.Should().BeOfType<SolidColorBrush>();
        brush.Color.Should().Be(MediaColors.DodgerBlue);
        _manager.Get("MyNet.Brush.Test", new()).Should().BeSameAs(brush);
    }

    [Fact]
    public void Register_ExistingKey_UpdatesColorInPlace()
    {
        var brush = _manager.Register("MyNet.Brush.Test", MediaColors.DodgerBlue);

        _manager.Register("MyNet.Brush.Test", MediaColors.Crimson);

        brush.Color.Should().Be(MediaColors.Crimson);
    }

    [Fact]
    public void Get_UnknownKey_ReturnsFallbackBrush()
    {
        var result = _manager.Get("MyNet.Brush.Missing", new());

        result.Should().BeSameAs(BrushManager.FallbackBrush);
    }

    [Fact]
    public void Get_WithOpacity_ReturnsTransformedBrush()
    {
        _manager.Register("MyNet.Brush.Test", MediaColors.White);

        var half = _manager.Get("MyNet.Brush.Test", new(0.5));

        half.Should().BeOfType<SolidColorBrush>();
        ((SolidColorBrush)half).Opacity.Should().BeApproximately(0.5, 0.001);
    }

    [Fact]
    public void Get_ByBrushInstance_ResolvesRegisteredBrush()
    {
        var brush = _manager.Register("MyNet.Brush.Test", MediaColors.Navy);

        var resolved = _manager.Get(brush, new());

        resolved.Should().BeSameAs(brush);
    }
}
