// -----------------------------------------------------------------------
// <copyright file="RandomPagesExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using Avalonia.Markup.Xaml;
using MyNet.Avalonia.Showcase.Helpers;

namespace MyNet.Avalonia.Showcase.MarkupExtensions;

internal sealed class RandomPagesExtension : MarkupExtension
{
    public int Min { get; set; } = 3;

    public int Max { get; set; } = 5;

    public override object ProvideValue(IServiceProvider serviceProvider)
        => Enumerable.Range(1, RandomGenerator.Current.Int(Min, Max)).Select(x => PageHelper.MakeNavigationPage($"Page {x}", string.Empty));
}
