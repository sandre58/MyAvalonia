// -----------------------------------------------------------------------
// <copyright file="MathConverterTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;
using Avalonia;
using FluentAssertions;
using MyNet.Avalonia.Converters;
using Xunit;

namespace MyNet.Avalonia.Tests.Converters;

public class MathConverterTests
{
    [Theory]
    [InlineData(10, 5, 15)]
    [InlineData(10, -3, 7)]
    public void Add_ReturnsSum(object left, object right, double expected)
    {
        var result = MathConverter.Add.Convert(left, typeof(double), right, CultureInfo.InvariantCulture);

        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(10, 4, 2.5)]
    [InlineData(10, 0, 0)]
    public void Divide_HandlesOperands(object left, object right, double expected)
    {
        var result = MathConverter.Divide.Convert(left, typeof(double), right, CultureInfo.InvariantCulture);

        result.Should().Be(expected);
    }

    [Fact]
    public void Multiply_MultiBinding_AggregatesValues()
    {
        var result = MathConverter.Multiply.Convert([2, 3, 4], typeof(double), null, CultureInfo.InvariantCulture);

        result.Should().Be(24);
    }

    [Fact]
    public void Subtract_ConvertBack_AppliesInverse()
    {
        var result = MathConverter.Subtract.ConvertBack(7, typeof(double), 3, CultureInfo.InvariantCulture);

        result.Should().Be(10);
    }

    [Fact]
    public void InvalidInput_ReturnsUnsetValue()
    {
        var result = MathConverter.Add.Convert("not-a-number", typeof(double), 1, CultureInfo.InvariantCulture);

        result.Should().Be(AvaloniaProperty.UnsetValue);
    }
}
