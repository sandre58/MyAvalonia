// -----------------------------------------------------------------------
// <copyright file="RandomCountriesExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Markup.Xaml;
using MyNet.Avalonia.Geography;
using MyNet.Geography;
using MyNet.Humanizer.Facade;
using MyNet.Utilities.Generator;

namespace MyNet.Avalonia.Showcase.MarkupExtensions;

internal sealed class RandomCountriesExtension : MarkupExtension
{
    public int Min { get; set; } = 3;

    public int Max { get; set; } = 5;

    public bool All { get; set; }

    public bool ByAlpha { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var allCountries = CountrySource.GetAllOrderedByDisplay().ToList();
        var countries = All
            ? allCountries.OrderBy(x => x.Name)
            : RandomGenerator.ListItems(allCountries, RandomGenerator.Int(Min, Max)).OrderBy(x => x.Name);

        return ByAlpha
            ? countries.GroupBy(x => x.Humanize()![..1]).Select(x => new CountriesWrapper(x.OrderBy(y => y.Humanize()), x.Key)).OrderBy(x => x.DisplayText).ToList()
            : countries.ToList();
    }
}

/// <summary>
/// Groups countries under an alphabetical header for tree views.
/// </summary>
public sealed class CountriesWrapper(IEnumerable<Country> item, string displayText)
{
    public IEnumerable<Country> Item { get; } = item;

    public string DisplayText { get; } = displayText;
}
