// -----------------------------------------------------------------------
// <copyright file="NullConverterTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using FluentAssertions;
using MyNet.Avalonia.Converters;
using Xunit;

namespace MyNet.Avalonia.Tests.Converters;

public class NullConverterTests
{
    [Theory]
    [InlineData(null, true)]
    [InlineData("", true)]
    [InlineData("text", false)]
    [InlineData(double.NaN, true)]
    [InlineData(1.0, false)]
    public void IsEmpty_DetectsEmptyValues(object? value, bool expected)
    {
        var result = NullConverter.IsEmpty.Convert(value, typeof(bool), null, CultureInfo.InvariantCulture);

        result.Should().Be(expected);
    }

    [Fact]
    public void IsPresent_InvertsIsEmpty()
    {
        NullConverter.IsPresent.Convert("value", typeof(bool), null, CultureInfo.InvariantCulture).Should().Be(true);
        NullConverter.IsPresent.Convert(null, typeof(bool), null, CultureInfo.InvariantCulture).Should().Be(false);
    }

    [Fact]
    public void EmptyArray_IsEmpty()
    {
        var result = NullConverter.IsEmpty.Convert(Array.Empty<int>(), typeof(bool), null, CultureInfo.InvariantCulture);

        result.Should().Be(true);
    }
}
