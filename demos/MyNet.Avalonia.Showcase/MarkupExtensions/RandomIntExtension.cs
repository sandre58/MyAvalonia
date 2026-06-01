// -----------------------------------------------------------------------
// <copyright file="RandomIntExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Markup.Xaml;

namespace MyNet.Avalonia.Showcase.MarkupExtensions;

internal sealed class RandomIntExtension : MarkupExtension
{
    public int Min { get; set; }

    public int Max { get; set; } = 10;

    public override object ProvideValue(IServiceProvider serviceProvider)
        => RandomGenerator.Current.Int(Min, Max);
}
