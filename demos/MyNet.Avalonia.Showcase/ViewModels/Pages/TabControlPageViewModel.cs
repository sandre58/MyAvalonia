// -----------------------------------------------------------------------
// <copyright file="TabControlPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using Material.Icons;
using MyNet.Avalonia.Showcase.Resources;
using MyNet.Avalonia.Showcase.ThemeBuilder;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders.Editors;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.Avalonia.Theme.Classes;
using MyNet.UI.Commands;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class TabControlPageViewModel(ICommandFactory commands) : ShowcaseViewModel(nameof(TabControl), commands, [
    new ControlThemeBuilder()
        .AddShapes(CssClass.ShapeItemsCircle)
        .AddHeaderStandardVariants()
        .AddVariant(CssClass.ShadowHeader)
        .AddItemsStandardVariants()
        .AddVariant(CssClass.ShadowItems)
        .AddVariant(CssClass.Uniform)
        .AddThemeRoles()
        .AddItemsThemeRoles()
        .AddDefaultSizes()
        .AddEnumProperty<Dock, ListBoxEditor>(TabControl.TabStripPlacementProperty, Dock.Top, x => x.DisplayName(nameof(SettingsResources.Layout)), configureChoice: (x, y) => y.WithIcon(Enum.Parse<MaterialIconKind>($"Dock{x}"))),

    new ControlThemeBuilder("Indicator")
        .AddVariant(new("hidden-content"))
        .AddVariant(CssClass.Uniform)
        .AddItemsThemeRoles()
        .AddDefaultSizes()
        .AddEnumProperty<Dock, ListBoxEditor>(TabControl.TabStripPlacementProperty, Dock.Top, x => x.DisplayName(nameof(SettingsResources.Layout)), configureChoice: (x, y) => y.WithIcon(Enum.Parse<MaterialIconKind>($"Dock{x}")))
])
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.Tab;
}
