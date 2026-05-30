// -----------------------------------------------------------------------
// <copyright file="CountriesExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using Avalonia.Markup.Xaml;
using MyNet.Humanizer;
using MyNet.Utilities;
using MyNet.Utilities.Geography;

namespace MyNet.Avalonia.MarkupExtensions;

public sealed class CountriesExtension : MarkupExtension
{
    public override object ProvideValue(IServiceProvider serviceProvider) => EnumClass.GetAll<Country>().OrderBy(x => x.Humanize());
}
