// -----------------------------------------------------------------------
// <copyright file="MathComparisonConverterTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;
using Avalonia;
using FluentAssertions;
using MyNet.Avalonia.Converters;
using Xunit;

namespace MyNet.Avalonia.Tests.Converters;

public class MathComparisonConverterTests
{
    [Theory]
    [InlineData(5, 5, true)]
    [InlineData(5, 6, false)]
    public void IsEqualsTo_ComparesNumericValues(object left, object right, bool expected)
    {
        var result = MathComparisonConverter.IsEqualsTo.Convert(left, typeof(bool), right, CultureInfo.InvariantCulture);

        result.Should().Be(expected);
    }

    [Fact]
    public void IsGreaterThan_ReturnsTrueWhenFirstIsLarger()
    {
        MathComparisonConverter.IsGreaterThan.Convert(10, typeof(bool), 3, CultureInfo.InvariantCulture).Should().Be(true);
        MathComparisonConverter.IsGreaterThan.Convert(3, typeof(bool), 10, CultureInfo.InvariantCulture).Should().Be(false);
    }

    [Fact]
    public void NullOperand_ReturnsUnsetValue()
    {
        var converter = MathComparisonConverter.IsLessThan;

        var result = converter.Convert(null!, typeof(bool), 1, CultureInfo.InvariantCulture);

        result.Should().BeSameAs(AvaloniaProperty.UnsetValue);
    }
}
