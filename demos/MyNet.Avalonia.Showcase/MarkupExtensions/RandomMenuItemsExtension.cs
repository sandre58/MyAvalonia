// -----------------------------------------------------------------------
// <copyright file="RandomMenuItemsExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Markup.Xaml;
using MyNet.Avalonia.Showcase.Helpers;

namespace MyNet.Avalonia.Showcase.MarkupExtensions;

internal sealed class RandomMenuItemsExtension : MarkupExtension
{
    public int CurrentDepth { get; set; } = 1;

    public int Min { get; set; } = 3;

    public int Max { get; set; } = 5;

    public int MaxDepth { get; set; } = 3;

    public override object ProvideValue(IServiceProvider serviceProvider)
        => MenuHelper.RandomizeMenuItems(CurrentDepth, Min, Max, MaxDepth);
}
