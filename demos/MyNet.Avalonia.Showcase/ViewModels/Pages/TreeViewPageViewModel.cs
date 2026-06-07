// -----------------------------------------------------------------------
// <copyright file="TreeViewPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Material.Icons;
using MyNet.Avalonia.Showcase.ThemeBuilder;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.Avalonia.Theme.Classes;
using MyNet.UI.Commands;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class TreeViewPageViewModel(ICommandFactory commands) : ShowcaseViewModel(nameof(TreeView), commands, [
    new ControlThemeBuilder()
            .AddItemsStandardVariants()
            .AddVariant(CssClass.ShadowItems)
            .AddItemsThemeRoles()
                .AddDefaultSizes()
    ])
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.FileTree;
}
