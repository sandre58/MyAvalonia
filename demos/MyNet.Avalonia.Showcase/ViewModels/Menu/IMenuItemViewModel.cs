// -----------------------------------------------------------------------
// <copyright file="IMenuItemViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Material.Icons;
using MyNet.UI.Navigation.Models;

namespace MyNet.Avalonia.Showcase.ViewModels.Menu;

/// <summary>
/// View model for a showcase navigation menu entry (leaf page, group, or separator).
/// </summary>
public interface IMenuItemViewModel
{
    /// <summary>Gets the localized menu label.</summary>
    string? Title { get; }

    /// <summary>Gets the Material icon shown in the rail.</summary>
    MaterialIconKind Icon { get; }

    /// <summary>Gets a value indicating whether this entry is a collapsible group.</summary>
    bool IsGroup { get; }

    /// <summary>Gets a value indicating whether this entry is a non-interactive section header (<see cref="NavigationMenuItem.IsSectionHeader"/>).</summary>
    bool IsSectionHeader { get; }

    /// <summary>Gets the navigation target for leaf items, or <see langword="null"/> for groups and separators.</summary>
    INavigationPage? NavigationTarget { get; }

    /// <summary>Gets child entries when <see cref="IsGroup"/> is true.</summary>
    IReadOnlyList<IMenuItemViewModel> Items { get; }
}
