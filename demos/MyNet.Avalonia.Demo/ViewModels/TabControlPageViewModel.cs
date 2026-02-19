// -----------------------------------------------------------------------
// <copyright file="TabControlPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class TabControlPageViewModel : ControlCatalogViewModel
{
    public TabControlPageViewModel()
        : base(nameof(TabControl), [
            new ControlThemeBuilder()
            .AddShapes("shape-circle-items")
            .AddVariants("variant-solid", "variant-light", "variant-outlined", "shadow-header", "shadow-items", "variant-solid-items", "variant-light-items", "variant-outlined-items", "variant-text-items", "layout-uniform")
            .AddThemeRoles()
            .AddItemsThemeRoles()
            .AddDefaultSizes(),

            new ControlThemeBuilder("Indicator")
            .AddItemsThemeRoles()
            .AddDefaultSizes()
        ])
    { }
}
