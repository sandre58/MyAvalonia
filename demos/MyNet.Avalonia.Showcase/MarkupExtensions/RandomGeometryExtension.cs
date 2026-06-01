// -----------------------------------------------------------------------
// <copyright file="RandomGeometryExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Material.Icons;
using MyNet.Avalonia.Theme.Controls.MarkupExtensions;

namespace MyNet.Avalonia.Showcase.MarkupExtensions;

internal sealed class RandomGeometryExtension : MaterialGeometryExtension
{
    public RandomGeometryExtension() => Kind = RandomGenerator.Current.Enum<MaterialIconKind>();
}
