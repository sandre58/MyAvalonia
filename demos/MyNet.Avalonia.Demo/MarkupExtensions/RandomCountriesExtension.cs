// -----------------------------------------------------------------------
// <copyright file="RandomCountriesExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using Avalonia.Markup.Xaml;
using MyNet.Utilities;
using MyNet.Utilities.Generator;
using MyNet.Utilities.Geography;

namespace MyNet.Avalonia.Demo.MarkupExtensions;

internal sealed class RandomCountriesExtension : MarkupExtension
{
    public int Min { get; set; } = 3;

    public int Max { get; set; } = 5;

    public bool All { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider)
        => All ? EnumClass.GetAll<Country>().OrderBy(x => x.Name) : RandomGenerator.ListItems(EnumClass.GetAll<Country>(), RandomGenerator.Int(Min, Max)).OrderBy(x => x.Name);
}
