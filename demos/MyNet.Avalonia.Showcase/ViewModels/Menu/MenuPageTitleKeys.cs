// -----------------------------------------------------------------------
// <copyright file="MenuPageTitleKeys.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace MyNet.Avalonia.Showcase.ViewModels.Menu;

/// <summary>
/// Maps showcase page view model types to <see cref="Resources.MenuResources"/> title keys.
/// </summary>
internal static class MenuPageTitleKeys
{
    public static string For(Type viewModelType)
    {
        ArgumentNullException.ThrowIfNull(viewModelType);

        var name = viewModelType.Name;
        foreach (var suffix in new[] { "PageViewModel", "ViewModel", "Page" })
        {
            if (!name.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
                continue;

            name = name[..^suffix.Length];
            break;
        }

        return name;
    }
}
