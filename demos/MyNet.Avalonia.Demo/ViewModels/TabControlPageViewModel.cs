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
            .AddShapes("shape-items-circle")
            .AddVariants("variant-solid", "variant-light", "variant-outlined", "shadow-header", "shadow-items", "variant-items-solid", "variant-items-light", "variant-items-outlined", "variant-items-text", "layout-uniform")
            .AddThemeRoles()
            .AddItemsThemeRoles()
            .AddDefaultSizes(),

            new ControlThemeBuilder("Indicator")
            .AddItemsThemeRoles()
            .AddDefaultSizes()
        ])
    { }
}
