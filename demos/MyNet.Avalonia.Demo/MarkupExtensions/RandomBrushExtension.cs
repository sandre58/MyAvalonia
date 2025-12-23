// -----------------------------------------------------------------------
// <copyright file="RandomBrushExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using MyNet.Avalonia.Extensions;
using MyNet.Utilities.Generator;

namespace MyNet.Avalonia.Demo.MarkupExtensions;

internal sealed class RandomBrushExtension : MarkupExtension
{
    public override object ProvideValue(IServiceProvider serviceProvider)
        => new SolidColorBrush(RandomGenerator.Color().ToColor() ?? default);
}
