// -----------------------------------------------------------------------
// <copyright file="IMenuItemViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Material.Icons;
using MyNet.UI.Navigation.Models;

namespace MyNet.Avalonia.Showcase.ViewModels.Base;

/// <summary>
/// Defines an interface for menu item view models, providing properties for the title and icon of a menu item. The title is a string that represents the display text for the menu item, while the icon is specified using the <see cref="MaterialIconKind"/> enumeration, allowing for visual representation of the menu item in user interfaces. Implementing this interface allows for consistent handling of menu items across different parts of the application, ensuring that each menu item has a defined title and icon for display purposes.
/// </summary>
public interface IMenuItemViewModel
{
    /// <summary>
    /// Gets the title of the menu item, which is a string representing the display text for the menu item. The title should be concise and descriptive, providing users with a clear understanding of the menu item's purpose or action when displayed in user interfaces. Ensure that the title is properly localized if necessary to support multiple languages in the application.
    /// </summary>
    string? Title { get; }

    /// <summary>
    /// Gets the icon data associated with the menu item, specified as a <see cref="MaterialIconKind"/> enumeration value. The icon can be used to visually represent the menu item in user interfaces, providing users with a quick visual cue about the menu item's function or category. Ensure that the icon is properly chosen to match the context of the menu item and enhance user experience without causing confusion.
    /// </summary>
    MaterialIconKind Icon { get; }

    /// <summary>
    /// Gets a value indicating whether the menu item represents a group of items rather than a single actionable item. This property can be used to differentiate between menu items that serve as containers for other items (groups) and those that represent individual actions or pages. When this property is true, it indicates that the menu item is a group, which may contain child items that can be displayed in a nested manner in user interfaces.
    /// </summary>
    bool IsGroup { get; }

    /// <summary>
    /// Gets the navigation page passed to the shell navigate command, or <see langword="null"/> for groups.
    /// </summary>
    INavigationPage? NavigationTarget { get; }

    /// <summary>
    /// Gets child menu items for grouped entries, or an empty sequence for leaf items.
    /// </summary>
    IReadOnlyList<IMenuItemViewModel> Items { get; }
}
