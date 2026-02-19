// -----------------------------------------------------------------------
// <copyright file="ListBoxPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class ListBoxPageViewModel : ControlCatalogViewModel
{
    public ListBoxPageViewModel()
        : base(nameof(ListBox), [
            new ControlThemeBuilder()
            .AddShapes("shape-circle-items")
            .AddVariants("variant-solid", "variant-light", "variant-outlined", "shadow-surface")
            .AddVariants("variant-solid-items", "variant-light-items", "variant-outlined-items", "variant-text-items", "shadow-items")
            .AddVariants("layout-spacing", "layout-vertical", "layout-horizontal", "layout-uniform", "layout-wrap")
            .AddThemeRoles()
            .AddItemsThemeRoles()
            .AddDefaultSizes(),

            new ControlThemeBuilder(null, "kind-cards")
            .AddShapes("shape-circle-items")
            .AddVariants("variant-solid", "variant-light", "variant-outlined", "shadow-surface")
            .AddVariants("variant-solid-items", "variant-light-items", "variant-outlined-items", "variant-text-items", "shadow-items")
            .AddVariants("layout-spacing", "layout-vertical", "layout-horizontal", "layout-uniform", "layout-wrap")
            .AddThemeRoles()
            .AddItemsThemeRoles()
            .AddDefaultSizes(),

            new ControlThemeBuilder(null, "kind-toggle")
            .AddVariants("variant-solid", "variant-light", "variant-outlined", "shadow-surface")
            .AddVariants("variant-solid-items", "variant-light-items", "variant-outlined-items", "variant-text-items", "shadow-items")
            .AddVariants("layout-spacing", "layout-vertical", "layout-horizontal", "layout-uniform", "layout-wrap")
            .AddDefaultRoles()
            .AddItemsThemeRoles()
            .AddDefaultSizes(),

            new ControlThemeBuilder("Tabs")
            .AddVariants("variant-solid", "variant-light", "variant-outlined", "shadow-surface")
            .AddVariants("variant-solid-items", "variant-light-items", "variant-outlined-items", "variant-text-items", "shadow-items")
            .AddVariants("layout-spacing", "layout-vertical", "layout-horizontal", "layout-uniform", "layout-wrap")
            .AddThemeRoles()
            .AddItemsThemeRoles()
            .AddDefaultSizes(),

            new ControlThemeBuilder("Icon")
            .AddVariants("layout-spacing", "layout-vertical", "layout-horizontal", "layout-uniform", "layout-wrap")
            .AddItemsThemeRoles()
            .AddDefaultSizes()
        ])
    { }
}
