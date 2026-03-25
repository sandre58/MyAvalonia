// -----------------------------------------------------------------------
// <copyright file="ListBoxPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog;
using MyNet.Avalonia.Theme.Classes.Enums;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class ListBoxPageViewModel : ControlCatalogViewModel
{
    public ListBoxPageViewModel()
        : base(nameof(ListBox), [
            new ControlThemeBuilder()
            .AddShapes("shape-items-circle")
            .AddVariants("variant-solid", "variant-light", "variant-outlined", "shadow-surface")
            .AddVariants("variant-items-solid", "variant-items-light", "variant-items-outlined", "variant-items-text", "shadow-items")
            .AddVariants("flex-vertical", "flex-horizontal", "flex-uniform", "flex-wrap")
            .AddThemeRoles()
            .AddItemsThemeRoles()
            .AddDefaultSizes(),

            new ControlThemeBuilder(null, "kind-cards")
            .AddShapes("shape-items-circle")
            .AddVariants("variant-solid", "variant-light", "variant-outlined", "shadow-surface")
            .AddVariants("variant-items-solid", "variant-items-light", "variant-items-outlined", "variant-items-text", "shadow-items")
            .AddVariants("flex-vertical", "flex-horizontal", "flex-uniform", "flex-wrap")
            .AddThemeRoles()
            .AddItemsThemeRoles()
            .AddDefaultSizes(),

            new ControlThemeBuilder(null, "kind-toggle")
            .AddVariants("variant-solid", "variant-light", "variant-outlined", "shadow-surface")
            .AddVariants("variant-items-solid", "variant-items-light", "variant-items-outlined", "variant-items-text", "shadow-items")
            .AddVariants("flex-vertical", "flex-horizontal", "flex-uniform", "flex-wrap")
            .AddDefaultRoles()
            .AddItemsThemeRoles()
            .AddDefaultSizes(),

            new ControlThemeBuilder("Tabs")
            .AddVariants("variant-solid", "variant-light", "variant-outlined", "shadow-surface")
            .AddVariants("variant-items-solid", "variant-items-light", "variant-items-outlined", "variant-items-text", "shadow-items")
            .AddVariants("flex-vertical", "flex-horizontal", "flex-uniform", "flex-wrap")
            .AddThemeRoles()
            .AddItemsThemeRoles()
            .AddDefaultSizes(),

            new ControlThemeBuilder("Icon")
            .AddVariants("flex-vertical", "flex-horizontal", "flex-uniform", "flex-wrap")
            .AddItemsThemeRoles()
            .AddDefaultSizes(),

            new ControlThemeBuilder("Indicator")
            .AddVariants("flex-vertical", "flex-horizontal", "flex-uniform", "flex-wrap")
            .AddItemsThemeRoles()
            .AddDefaultSizes()
        ])
    { }

    /// <inheritdoc/>
    public override IconData Icon => IconData.ListBox;
}
