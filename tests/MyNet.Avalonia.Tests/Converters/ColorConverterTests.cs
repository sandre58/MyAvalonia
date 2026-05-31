// -----------------------------------------------------------------------
// <copyright file="ColorConverterTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;
using Avalonia.Media;
using FluentAssertions;
using MyNet.Avalonia;
using MyNet.Avalonia.Converters;
using Xunit;

namespace MyNet.Avalonia.Tests.Converters;

public class ColorConverterTests
{
    [Fact]
    public void Convert_HexString_ReturnsColor()
    {
        var converter = new ColorConverter();

        var result = converter.Convert("#FF0000", typeof(Color), null, CultureInfo.InvariantCulture);

        result.Should().BeOfType<Color>().Which.Should().Be(global::Avalonia.Media.Colors.Red);
    }

    [Fact]
    public void Convert_SolidColorBrush_ReturnsColor()
    {
        var converter = new ColorConverter();

        var result = converter.Convert(new SolidColorBrush(global::Avalonia.Media.Colors.Blue), typeof(Color), null, CultureInfo.InvariantCulture);

        result.Should().Be(global::Avalonia.Media.Colors.Blue);
    }
}
