// -----------------------------------------------------------------------
// <copyright file="NavigationMenuDemoItem.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.ObjectModel;
using Material.Icons;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class NavigationMenuDemoItem
{
    public string Header { get; init; } = string.Empty;

    public MaterialIconKind Icon { get; init; } = MaterialIconKind.CircleOutline;

    public bool IsSectionHeader { get; init; }

    public ObservableCollection<NavigationMenuDemoItem>? Children { get; init; }
}
