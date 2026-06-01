// -----------------------------------------------------------------------
// <copyright file="DropDownButtonPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using Material.Icons;
using MyNet.Avalonia.Controls.Behaviors;
using MyNet.Avalonia.Controls.Enums;
using MyNet.Avalonia.Showcase.Extensions;
using MyNet.Avalonia.Showcase.Resources;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders.Editors;
using MyNet.Avalonia.Showcase.ThemeBuilder.Definitions;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.Avalonia.Theme.Classes;
using MyNet.UI.Commands;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class DropDownButtonPageViewModel(ICommandFactory commands) : ShowcaseViewModel(nameof(DropDownButton), commands, [
    new ControlThemeBuilder()
        .WithContent(ContentControl.ContentProperty, ContentProviderType.Text)
        .AddShapes(CssClass.ShapeCircle)
        .AddStandardVariants()
        .AddVariant(CssClass.ShadowControl)
        .AddDefaultSizes()
        .AddDefaultRoles()
        .AddEnumClass<Position, ListBoxEditor>(Position.Right, x => x.DisplayName(nameof(SettingsResources.DropDownPlacement)), configureChoice: (x, y) => y.WithIcon(Enum.Parse<MaterialIconKind>($"Dock{x}")))
        .AddEnumProperty<PlacementMode, ComboBoxEditor>(PopupBehavior.PlacementProperty, PlacementMode.BottomEdgeAlignedRight, x => x.DisplayName(nameof(SettingsResources.PopupPlacement))),

    new ControlThemeBuilder("Rounded")
        .WithContent(ContentControl.ContentProperty, ContentProviderType.Icon)
        .AddStandardVariants()
        .AddVariant(CssClass.ShadowControl)
        .AddDefaultSizes()
        .AddDefaultRoles()
        .AddEnumClass<Position, ListBoxEditor>(Position.Right, x => x.DisplayName(nameof(SettingsResources.DropDownPlacement)), configureChoice: (x, y) => y.WithIcon(Enum.Parse<MaterialIconKind>($"Dock{x}")))
        .AddEnumProperty<PlacementMode, ComboBoxEditor>(PopupBehavior.PlacementProperty, PlacementMode.BottomEdgeAlignedRight, x => x.DisplayName(nameof(SettingsResources.PopupPlacement)))
])
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.FormSelect;
}
