// -----------------------------------------------------------------------
// <copyright file="EqualsConverterTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;
using FluentAssertions;
using MyNet.Avalonia.Converters;
using Xunit;

namespace MyNet.Avalonia.Tests.Converters;

public class EqualsConverterTests
{
    [Theory]
    [InlineData("A", "A", true)]
    [InlineData("A", "B", false)]
    public void IsEquals_SingleValue_ComparesToParameter(object value, object parameter, bool expected)
    {
        var result = EqualsConverter.IsEquals.Convert(value, typeof(bool), parameter, CultureInfo.InvariantCulture);

        result.Should().Be(expected);
    }

    [Fact]
    public void IsEquals_MultiValue_AllEqual_ReturnsTrue()
    {
        var result = EqualsConverter.IsEquals.Convert(["x", "x", "x"], typeof(bool), null, CultureInfo.InvariantCulture);

        result.Should().Be(true);
    }

    [Fact]
    public void IsNotEquals_MultiValue_OneDifferent_ReturnsTrue()
    {
        var result = EqualsConverter.IsNotEquals.Convert(["x", "y"], typeof(bool), null, CultureInfo.InvariantCulture);

        result.Should().Be(true);
    }

    [Fact]
    public void SingleValue_ComparedToParameter_ReturnsExpected()
    {
        EqualsConverter.IsEquals.Convert("only", typeof(bool), "only", CultureInfo.InvariantCulture).Should().Be(true);
        EqualsConverter.IsNotEquals.Convert("only", typeof(bool), "other", CultureInfo.InvariantCulture).Should().Be(true);
    }
}
