// -----------------------------------------------------------------------
// <copyright file="CountryConverterTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;
using Avalonia;
using FluentAssertions;
using MyNet.Avalonia.Geography.Converters;
using MyNet.Geography;
using Xunit;

namespace MyNet.Avalonia.Geography.Tests;

public class CountryConverterTests
{
    [Fact]
    public void ToDisplayName_ReturnsCountryName()
    {
        var result = CountryConverter.ToDisplayName.Convert(Country.France, typeof(string), null, CultureInfo.InvariantCulture);

        result.Should().Be(Country.France.Name);
    }

    [Fact]
    public void ToAlpha2_ReturnsUpperCaseCode()
    {
        var result = CountryConverter.ToAlpha2.Convert(Country.France, typeof(string), null, CultureInfo.InvariantCulture);

        result.Should().Be("FR");
    }

    [Fact]
    public void ToDisplayName_FromCultureInfo_ReturnsCountryName()
    {
        var culture = CultureInfo.GetCultureInfo("fr-FR");

        var result = CountryConverter.ToDisplayName.Convert(culture, typeof(string), null, CultureInfo.InvariantCulture);

        result.Should().Be(Country.France.Name);
    }

    [Fact]
    public void Convert_UnknownValue_ReturnsUnsetValue()
    {
        var result = CountryConverter.ToDisplayName.Convert("invalid", typeof(string), null, CultureInfo.InvariantCulture);

        result.Should().Be(AvaloniaProperty.UnsetValue);
    }
}
