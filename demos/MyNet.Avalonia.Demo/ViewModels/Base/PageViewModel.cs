// -----------------------------------------------------------------------
// <copyright file="PageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Theme.Classes.Enums;
using MyNet.UI.ViewModels.Workspace;
using MyNet.Utilities;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Demo.ViewModels;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// Provides a base view model for pages in the application, deriving from <see cref="NavigableWorkspaceViewModel"/> and implementing common functionality such as title generation based on the class name with localization support.
/// </summary>
internal abstract class PageViewModel : NavigableWorkspaceViewModel
{
    /// <summary>
    /// Creates a user-friendly title for the page by deriving it from the runtime view model class name and applying localization.
    /// </summary>
    /// <returns>The localized title for the page.</returns>
    protected override string CreateTitle()
    {
        // Derive title from the runtime view model class name.
        // Examples: "AvatarPageViewModel" => "Avatar", "ThemePageViewModel" => "Theme".
        var name = GetType().Name ?? string.Empty;
        foreach (var suffix in new[] { "PageViewModel", "ViewModel", "Page" })
        {
            if (name.EndsWith(suffix, System.StringComparison.OrdinalIgnoreCase))
            {
                name = name.Substring(0, name.Length - suffix.Length);
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
    public virtual IconData Icon { get; } = IconData.None;
}
