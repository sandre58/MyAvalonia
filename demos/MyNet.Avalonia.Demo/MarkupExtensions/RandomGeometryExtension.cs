// -----------------------------------------------------------------------
// <copyright file="RandomGeometryExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Markup.Xaml;
using MyNet.Avalonia.Theme.Enums;
using MyNet.Avalonia.Theme.Extensions;
using MyNet.Utilities.Generator;

namespace MyNet.Avalonia.Demo.MarkupExtensions;

internal sealed class RandomGeometryExtension : MarkupExtension
{
    public override object ProvideValue(IServiceProvider serviceProvider) => RandomGenerator.Enum<IconData>().ToGeometry();
}
