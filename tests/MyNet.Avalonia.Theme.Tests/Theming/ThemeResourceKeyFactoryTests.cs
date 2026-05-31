// -----------------------------------------------------------------------
// <copyright file="ThemeResourceKeyFactoryTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using FluentAssertions;
using MyNet.Avalonia.Theme.Theming;
using Xunit;

namespace MyNet.Avalonia.Theme.Tests.Theming;

public class ThemeResourceKeyFactoryTests
{
    [Fact]
    public void Brush_FormatsResourceKey()
    {
        ThemeResourceKeyFactory.Brush("Primary").Should().Be("MyNet.Brush.Primary");
    }

    [Fact]
    public void ContrastedColor_SurfaceLevel_UsesPrimaryForeground()
    {
        ThemeResourceKeyFactory.ContrastedColor("Surface.Level1")
            .Should().Be("MyNet.Color.Foreground.Primary");
    }

    [Fact]
    public void ContrastedColor_InverseSurface_UsesInverseForeground()
    {
        ThemeResourceKeyFactory.ContrastedColor(ThemeResourceKeyFactory.InverseSurfaceKey)
            .Should().Be("MyNet.Color.Foreground.Inverse");
    }

    [Fact]
    public void ContrastedColor_InverseForeground_UsesPrimaryForeground()
    {
        ThemeResourceKeyFactory.ContrastedColor(ThemeResourceKeyFactory.InverseForegroundKey)
            .Should().Be("MyNet.Color.Foreground.Primary");
    }

    [Fact]
    public void ContrastedColor_UnknownKey_ReturnsNull()
    {
        ThemeResourceKeyFactory.ContrastedColor("Overlay").Should().BeNull();
    }
}
