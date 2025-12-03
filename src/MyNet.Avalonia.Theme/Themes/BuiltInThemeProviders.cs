// -----------------------------------------------------------------------
// <copyright file="BuiltInThemeProviders.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Theme.Palettes;

namespace MyNet.Avalonia.Theme.Themes;

/// <summary>
/// Built-in theme variant providers.
/// These are the default themes provided by the library.
/// </summary>
public static class BuiltInThemeProviders
{
    /// <summary>
    /// Gets dark theme.
    /// </summary>
    public static ThemePalette Dark { get; } = DarkThemePalette.Create();

    /// <summary>
    /// Gets light theme.
    /// </summary>
    public static ThemePalette Light { get; } = LightThemePalette.Create();

    /// <summary>
    /// Gets high contrast theme (accessibility).
    /// </summary>
    public static ThemePalette HighContrast { get; } = LightThemePalette.Create();
}
