// -----------------------------------------------------------------------
// <copyright file="ThicknessConverterTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;
using Avalonia;
using FluentAssertions;
using MyNet.Avalonia.Converters;
using Xunit;

namespace MyNet.Avalonia.Tests.Converters;

public class ThicknessConverterTests
{
    [Fact]
    public void FromDoubleAll_AppliesUniformThickness()
    {
        var result = ThicknessFromDoubleConverter.All.Convert(8.0, typeof(Thickness), null, CultureInfo.InvariantCulture);

        result.Should().BeOfType<Thickness>().Which.Should().Be(new Thickness(8));
    }

    [Fact]
    public void FromDoubleRight_SetsRightOnly()
    {
        var result = ThicknessFromDoubleConverter.Right.Convert(12.0, typeof(Thickness), null, CultureInfo.InvariantCulture);

        ((Thickness)result!).Should().Be(new Thickness(0, 0, 12, 0));
    }

    [Fact]
    public void ToDoubleLeft_ExtractsLeftComponent()
    {
        var thickness = new Thickness(3, 4, 5, 6);

        var result = ThicknessToDoubleConverter.Left.Convert(thickness, typeof(double), null, CultureInfo.InvariantCulture);

        result.Should().Be(3);
    }
}
