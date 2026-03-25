// -----------------------------------------------------------------------
// <copyright file="TreeViewPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog;
using MyNet.Avalonia.Theme.Classes.Enums;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class TreeViewPageViewModel() : ControlCatalogViewModel(nameof(TreeView),
    [
        new ControlThemeBuilder()
                .AddVariants("variant-items-solid", "variant-items-light", "variant-items-outlined", "variant-items-text", "shadow-items")
                .AddItemsThemeRoles()
                .AddDefaultSizes()
    ])
{
    /// <inheritdoc/>
    public override IconData Icon => IconData.FileTree;
}
