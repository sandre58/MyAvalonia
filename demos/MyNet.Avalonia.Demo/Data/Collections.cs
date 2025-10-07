// -----------------------------------------------------------------------
// <copyright file="Collections.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Avalonia.Controls;
using MyNet.Avalonia.Controls.ColorPalettes;
using MyNet.Humanizer;
using MyNet.Observable.Translatables;
using MyNet.Utilities;
using MyNet.Utilities.Geography;
using MyNet.Utilities.Geography.Extensions;
using MyNet.Utilities.Helpers;

namespace MyNet.Avalonia.Demo.Data;

public static class Collections
{
    public static readonly ImmutableList<DisplayWrapper<IColorPalette>> Palettes =
        [.. new List<Type>
        {
            typeof(FlatColorPalette),
            typeof(FlatHalfColorPalette),
            typeof(FluentColorPalette),
            typeof(MaterialColorPalette),
            typeof(MaterialHalfColorPalette),
            typeof(SixteenColorPalette),
            typeof(LightColorPalette),
            typeof(DarkColorPalette),
            typeof(StandardColorPalette)
        }.Select(x => new DisplayWrapper<IColorPalette>((IColorPalette)Activator.CreateInstance(x)!, x.Name))];

    public static readonly ImmutableList<int> Integers = [.. EnumerableHelper.Range(1, 100)];

    public static readonly ImmutableList<string> Countries = [.. EnumClass.GetAll<Country>().Select(x => x.GetDisplayName()).NotNull().OrderBy(x => x)];

    public static readonly ImmutableList<CountriesWrapper> CountriesByAplha =
    [
        .. EnumClass.GetAll<Country>().GroupBy(x => x.Humanize()![..1]).Select(x => new CountriesWrapper(x.OrderBy(y => y.GetDisplayName()), x.Key))
                                                                         .OrderBy(x => x.DisplayName.Value)
    ];
}

public class CountriesWrapper(IEnumerable<Country> item, string key) : DisplayWrapper<IEnumerable<Country>>(item, key);
