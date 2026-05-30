// -----------------------------------------------------------------------
// <copyright file="RandomBoolExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Markup.Xaml;
using MyNet.Utilities.Generator;

namespace MyNet.Avalonia.Showcase.MarkupExtensions;

internal sealed class RandomBoolExtension : MarkupExtension
{
    public override object ProvideValue(IServiceProvider serviceProvider)
        => RandomGenerator.Bool();
}
