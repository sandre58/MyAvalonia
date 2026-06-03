// -----------------------------------------------------------------------
// <copyright file="PlaygroundStyleComposer.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using MyNet.Avalonia.Theme.Theming;
using MyNet.Text;

namespace MyNet.Avalonia.Showcase.ViewModels.Playground;

/// <summary>
/// Maps control theme resource keys to playground CSS class names.
/// </summary>
internal static class PlaygroundStyleComposer
{
    /// <summary>
    /// Resolves the theme CSS class for the playground preview from a theme resource key.
    /// </summary>
    /// <param name="controlName">The showcased control type name.</param>
    /// <param name="themeKey">The optional theme resource key.</param>
    /// <returns>A CSS class name such as <c>theme-default</c> or <c>theme-rounded</c>.</returns>
    public static string ResolveThemeClassName(string controlName, string? themeKey) => string.IsNullOrEmpty(themeKey)
        ? "theme-default"
        : themeKey
            .ToLowerCase()
            .Replace(ThemeResourceKeyFactory.Theme(controlName), "theme", StringComparison.OrdinalIgnoreCase)
            .Replace(".", "-", StringComparison.OrdinalIgnoreCase);
}
