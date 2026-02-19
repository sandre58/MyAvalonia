// -----------------------------------------------------------------------
// <copyright file="RandomDaysExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Markup.Xaml;
using MyNet.Utilities.Generator;

namespace MyNet.Avalonia.Demo.MarkupExtensions;

internal sealed class RandomDaysExtension : MarkupExtension
{
    public int Min { get; set; } = 3;

    public int Max { get; set; } = 5;

    public override object ProvideValue(IServiceProvider serviceProvider)
        => RandomGenerator.ListItems(Enum.GetValues<DayOfWeek>(), RandomGenerator.Int(Min, Max));
}
