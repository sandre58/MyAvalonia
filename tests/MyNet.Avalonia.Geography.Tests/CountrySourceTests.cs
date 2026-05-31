// -----------------------------------------------------------------------
// <copyright file="CountrySourceTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using MyNet.Avalonia.Geography.MarkupExtensions;
using MyNet.Geography;
using Xunit;

namespace MyNet.Avalonia.Geography.Tests;

public class CountrySourceTests
{
    [Fact]
    public void GetAllOrderedByDisplay_ContainsKnownCountry()
    {
        var countries = CountrySource.GetAllOrderedByDisplay();
        var france = Country.All.First(c => c.Name == "France");

        countries.Should().NotBeEmpty();
        countries.Should().Contain(france);
    }

    [Fact]
    public void CountriesExtension_ProvideValue_ReturnsOrderedCountries()
    {
        var extension = new CountriesExtension();

        var result = (IReadOnlyList<Country>)extension.ProvideValue(null!);

        result.Should().BeEquivalentTo(CountrySource.GetAllOrderedByDisplay(), options => options.WithStrictOrdering());
    }
}
