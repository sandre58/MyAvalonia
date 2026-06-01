// -----------------------------------------------------------------------
// <copyright file="StandardColorPaletteTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using FluentAssertions;
using MyNet.Avalonia.Controls.ColorPalettes;
using Xunit;

namespace MyNet.Avalonia.Controls.Tests.ColorPalettes;

public class StandardColorPaletteTests
{
    private readonly StandardColorPalette _palette = new();

    [Fact]
    public void ColorCount_ReturnsExpectedPaletteSize()
    {
        _palette.ColorCount.Should().Be(11);
        _palette.ShadeCount.Should().Be(1);
    }

    [Fact]
    public void GetColor_ReturnsKnownColors()
    {
        _palette.GetColor(0, 0).Should().Be(global::Avalonia.Media.Colors.White);
        _palette.GetColor(1, 0).Should().Be(global::Avalonia.Media.Colors.Black);
        _palette.GetColor(4, 0).Should().Be(global::Avalonia.Media.Colors.Red);
        _palette.GetColor(5, 0).Should().Be(global::Avalonia.Media.Colors.Orange);
    }

    [Fact]
    public void GetColor_ClampsOutOfRangeIndexes()
    {
        _palette.GetColor(-5, 0).Should().Be(global::Avalonia.Media.Colors.White);
        _palette.GetColor(999, 0).Should().Be(global::Avalonia.Media.Colors.Pink);
    }
}
