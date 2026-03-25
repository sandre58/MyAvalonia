// -----------------------------------------------------------------------
// <copyright file="ColorViewPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Avalonia.Controls;
using MyNet.Avalonia.Controls.ColorPalettes;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog;
using MyNet.Avalonia.Theme.Classes.Enums;
using MyNet.Observable.Translatables;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class ColorViewPageViewModel : ControlCatalogViewModel
{
    public static readonly ImmutableList<DisplayWrapper<IColorPalette>> Palettes =
    [.. new List<Type>
        {
            typeof(FlatColorPalette),
            typeof(FlatHalfColorPalette),
            typeof(FluentColorPalette),
            typeof(MaterialColorPalette),
            typeof(MaterialHalfColorPalette),
            typeof(SixteenColorPalette),
            typeof(LightColorPalette),
            typeof(DarkColorPalette),
            typeof(StandardColorPalette)
        }.Select(x => new DisplayWrapper<IColorPalette>((IColorPalette)Activator.CreateInstance(x)!, x.Name))];

    public ColorViewPageViewModel()
        : base(nameof(ColorView),
            [
                new ControlThemeBuilder()
                    .AddThemeRoles(),

                new ControlThemeBuilder("Simple")
                    .AddThemeRoles()
            ])
    {
    }

    /// <inheritdoc/>
    public override IconData Icon => IconData.Palette;
}
