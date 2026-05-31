// -----------------------------------------------------------------------
// <copyright file="BrushManagerPrewarmTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Media;
using FluentAssertions;
using MyNet.Avalonia.Theme.Theming.Brushes;
using MediaColors = Avalonia.Media.Colors;
using Xunit;

namespace MyNet.Avalonia.Theme.Tests.Theming;

public class BrushManagerPrewarmTests
{
    [Fact]
    public void Register_WithPrewarmEnabled_CreatesStandardOpacityBrushes()
    {
        BrushManagerOptions.PrewarmThemeOpacityLevels = true;
        var manager = new BrushManager(null, null);

        manager.Register("MyNet.Brush.Prewarm.Test", MediaColors.Teal);

        var hover = manager.Get("MyNet.Brush.Prewarm.Test", new(0.12));
        var overlay = manager.Get("MyNet.Brush.Prewarm.Test", new(0.08));

        hover.Should().BeOfType<SolidColorBrush>();
        overlay.Should().BeOfType<SolidColorBrush>();
        ((SolidColorBrush)hover).Opacity.Should().BeApproximately(0.12, 0.001);
    }

    [Fact]
    public void Register_WithPrewarmDisabled_OnlyCreatesBrushOnDemand()
    {
        BrushManagerOptions.PrewarmThemeOpacityLevels = false;
        var manager = new BrushManager(null, null);

        manager.Register("MyNet.Brush.Prewarm.Cold", MediaColors.Purple);

        var half = manager.Get("MyNet.Brush.Prewarm.Cold", new(0.5));
        half.Should().BeOfType<SolidColorBrush>();
    }
}
