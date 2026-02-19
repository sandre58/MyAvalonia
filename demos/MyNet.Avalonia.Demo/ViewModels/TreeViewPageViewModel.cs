// -----------------------------------------------------------------------
// <copyright file="TreeViewPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class TreeViewPageViewModel : ControlCatalogViewModel
{
    public TreeViewPageViewModel()
        : base(nameof(TreeView), [
            new ControlThemeBuilder()
            .AddVariants("variant-solid-items", "variant-light-items", "variant-outlined-items", "variant-text-items", "shadow-items")
            .AddItemsThemeRoles().AddDefaultSizes()
        ])
    { }
}
