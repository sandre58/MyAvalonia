// -----------------------------------------------------------------------
// <copyright file="ColorManipulationsTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Media;
using FluentAssertions;
using Xunit;

namespace MyNet.Avalonia.Tests.Colors;

public class ColorManipulationsTests
{
    [Fact]
    public void Darken_WhenAmountIsZero_ReturnsOriginalColor()
    {
        var color = Color.FromArgb(123, 180, 120, 60);

        var result = color.Darken(0);

        result.Should().Be(color);
    }

    [Fact]
    public void Lighten_WhenAmountIsZero_ReturnsOriginalColor()
    {
        var color = Color.FromArgb(123, 60, 120, 180);

        var result = color.Lighten(0);

        result.Should().Be(color);
    }

    [Fact]
    public void Apply_WhenDarkenIsZero_ReturnsOriginalColor()
    {
        var color = Color.FromArgb(200, 160, 160, 160);

        var result = color.Apply(new(Darken: 0));

        result.Should().Be(color);
    }

    [Fact]
    public void Apply_WhenLightenIsZero_ReturnsOriginalColor()
    {
        var color = Color.FromArgb(200, 96, 96, 96);

        var result = color.Apply(new(Lighten: 0));

        result.Should().Be(color);
    }

    [Fact]
    public void Darken_WhenAmountIncreases_ProducesProgressivelyDarkerColors()
    {
        var color = Color.FromRgb(180, 180, 180);

        var dark05 = color.Darken(0.5);
        var dark1 = color.Darken(1.25);
        var dark15 = color.Darken(1.5);

        Brightness(dark05).Should().BeLessThan(Brightness(color));
        Brightness(dark1).Should().BeLessThan(Brightness(dark05));
        Brightness(dark15).Should().BeLessThan(Brightness(dark1));
        dark05.Should().NotBe(dark1);
        dark1.Should().NotBe(dark15);
    }

    [Fact]
    public void Lighten_WhenAmountIncreases_ProducesProgressivelyLighterColors()
    {
        var color = Color.FromRgb(80, 80, 80);

        var light05 = color.Lighten(0.5);
        var light1 = color.Lighten(1.25);
        var light15 = color.Lighten(1.5);

        Brightness(light05).Should().BeGreaterThan(Brightness(color));
        Brightness(light1).Should().BeGreaterThan(Brightness(light05));
        Brightness(light15).Should().BeGreaterThan(Brightness(light1));
        light05.Should().NotBe(light1);
        light1.Should().NotBe(light15);
    }

    [Fact]
    public void Apply_WhenDarkenAmountChanges_ProducesDistinctResults()
    {
        var color = Color.FromRgb(170, 170, 170);

        var dark05 = color.Apply(new(Darken: 0.5));
        var dark1 = color.Apply(new(Darken: 1));
        var dark15 = color.Apply(new(Darken: 1.5));

        Brightness(dark05).Should().BeLessThan(Brightness(color));
        Brightness(dark1).Should().BeLessThan(Brightness(dark05));
        Brightness(dark15).Should().BeLessThan(Brightness(dark1));
    }

    [Fact]
    public void Apply_WhenLightenAmountChanges_ProducesDistinctResults()
    {
        var color = Color.FromRgb(90, 90, 90);

        var light05 = color.Apply(new(Lighten: 0.5));
        var light1 = color.Apply(new(Lighten: 1));
        var light15 = color.Apply(new(Lighten: 1.5));

        Brightness(light05).Should().BeGreaterThan(Brightness(color));
        Brightness(light1).Should().BeGreaterThan(Brightness(light05));
        Brightness(light15).Should().BeGreaterThan(Brightness(light1));
    }

    [Fact]
    public void DarkenAndLighten_PreserveAlphaChannel()
    {
        var color = Color.FromArgb(111, 140, 140, 140);

        var darkened = color.Darken(1.25);
        var lightened = color.Lighten(1.25);

        darkened.A.Should().Be(color.A);
        lightened.A.Should().Be(color.A);
    }

    [Fact]
    public void Darken_OnBlackWithOpacity_IncreasesOpacity()
    {
        var blackWithAlpha = Color.FromArgb(200, 0, 0, 0);

        var darkened = blackWithAlpha.Darken(0.5);

        darkened.A.Should().BeGreaterThan(blackWithAlpha.A);
        darkened.R.Should().Be(0);
        darkened.G.Should().Be(0);
        darkened.B.Should().Be(0);
    }

    [Fact]
    public void Darken_OnBlackWithOpacity_MoreAmountMoreOpacity()
    {
        var black = Color.FromArgb(200, 0, 0, 0);

        var dark05 = black.Darken(0.5);
        var dark1 = black.Darken();

        dark1.A.Should().BeGreaterThan(dark05.A);
    }

    [Fact]
    public void Lighten_OnBlackWithOpacity_ReducesOpacity()
    {
        var blackWithAlpha = Color.FromArgb(200, 0, 0, 0);

        var lightened = blackWithAlpha.Lighten(0.5);

        lightened.A.Should().BeLessThan(blackWithAlpha.A);
        lightened.R.Should().Be(0);
        lightened.G.Should().Be(0);
        lightened.B.Should().Be(0);
    }

    [Fact]
    public void Darken_OnColorWithLightness_DoesNotReduceOpacity()
    {
        var lightGray = Color.FromArgb(150, 180, 180, 180);
        var originalAlpha = lightGray.A;

        var darkened = lightGray.Darken(0.5);

        // For non-black colors, alpha should be preserved even if darkened
        darkened.A.Should().Be(originalAlpha);
    }

    private static int Brightness(Color color) => color.R + color.G + color.B;
}
