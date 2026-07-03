// -----------------------------------------------------------------------
// <copyright file="IconsExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Avalonia.Markup.Xaml;
using MyNet.Avalonia.Controls.Icons;

namespace MyNet.Avalonia.Showcase.MarkupExtensions;

internal sealed class IconsExtension : MarkupExtension
{
    private static readonly List<MaterialIconKindGroup> Icons = [.. MaterialIconCatalog.Groups];

    public override object ProvideValue(IServiceProvider serviceProvider) => Icons;
}
