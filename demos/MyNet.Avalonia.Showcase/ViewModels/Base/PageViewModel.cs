// -----------------------------------------------------------------------
// <copyright file="PageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Material.Icons;
using MyNet.UI.ViewModels.Workspace;
using MyNet.Utilities;

namespace MyNet.Avalonia.Showcase.ViewModels.Base;

/// <summary>
/// Provides a base view model for pages in the application, deriving from <see cref="NavigableWorkspaceViewModel"/> and implementing common functionality such as title generation based on the class name with localization support.
/// </summary>
internal abstract class PageViewModel : NavigableWorkspaceViewModel, IMenuItemViewModel
{
    /// <summary>
    /// Creates a user-friendly title for the page by deriving it from the runtime view model class name and applying localization.
    /// </summary>
    /// <returns>The localized title for the page.</returns>
    protected override string CreateTitle()
    {
        // Derive title from the runtime view model class name.
        // Examples: "AvatarPageViewModel" => "Avatar", "ThemePageViewModel" => "Theme".
        var name = GetType().Name;
        foreach (var suffix in new[] { "PageViewModel", "ViewModel", "Page" })
        {
            if (name.EndsWith(suffix, System.StringComparison.OrdinalIgnoreCase))
            {
                name = name[..^suffix.Length];
                break;
            }
        }

        return name.Translate();
    }

    /// <summary>
    /// Gets the icon data associated with the current instance.
    /// </summary>
    /// <remarks>The icon data can be used to visually represent the instance in user interfaces. Ensure that
    /// the icon is properly initialized before accessing this property.</remarks>
    public virtual MaterialIconKind Icon { get; } = MaterialIconKind.CircleOffOutline;

    /// <summary>
    /// Gets a value indicating whether the menu item represents a group of items rather than a single actionable item. This property can be used to differentiate between menu items that serve as containers for other items (groups) and those that represent individual actions or pages. When this property is true, it indicates that the menu item is a group, which may contain child items that can be displayed in a nested manner in user interfaces.
    /// </summary>
    public bool IsGroup => false;
}
