// -----------------------------------------------------------------------
// <copyright file="RandomCountriesExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Markup.Xaml;
using MyNet.Humanizer;
using MyNet.Observable.Translatables;
using MyNet.Utilities;
using MyNet.Utilities.Generator;
using MyNet.Utilities.Geography;
using MyNet.Utilities.Geography.Extensions;

namespace MyNet.Avalonia.Showcase.MarkupExtensions;

internal sealed class RandomCountriesExtension : MarkupExtension
{
    public int Min { get; set; } = 3;

    public int Max { get; set; } = 5;

    public bool All { get; set; }

    public bool ByAlpha { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var countries = All ? EnumClass.GetAll<Country>().OrderBy(x => x.Name) : RandomGenerator.ListItems(EnumClass.GetAll<Country>(), RandomGenerator.Int(Min, Max)).OrderBy(x => x.Name);

        return ByAlpha
            ? countries.GroupBy(x => x.Humanize()![..1]).Select(x => new CountriesWrapper(x.OrderBy(y => y.GetDisplayName()), x.Key)).OrderBy(x => x.DisplayName.Value).ToList()
            : countries.ToList();
    }
}

public class CountriesWrapper(IEnumerable<Country> item, string key) : DisplayWrapper<IEnumerable<Country>>(item, key);
