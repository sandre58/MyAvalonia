// -----------------------------------------------------------------------
// <copyright file="RandomBrushExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using MyNet.Fakers.Static;

namespace MyNet.Avalonia.Showcase.MarkupExtensions;

internal sealed class RandomBrushExtension : MarkupExtension
{
    public override object ProvideValue(IServiceProvider serviceProvider)
        => new SolidColorBrush(Faker.Colors.Hex().ToColor() ?? default);
}
