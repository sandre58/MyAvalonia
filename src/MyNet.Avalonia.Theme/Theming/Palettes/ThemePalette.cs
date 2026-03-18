// -----------------------------------------------------------------------
// <copyright file="ThemePalette.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Media;

namespace MyNet.Avalonia.Theme.Theming.Palettes;

/// <summary>
/// Represents a complete theme configuration including brand colors (Primary, Accent) and theme variants colors.
/// Provides a convenient way to define and apply comprehensive theming to an application.
/// </summary>
public class ThemePalette
{
    /// <summary>
    /// Gets the primary brand color palette with automatic shade generation.
    /// Used for main actions, headers, and primary UI elements.
    /// </summary>
    public required ColorShades Primary { get; init; }

    /// <summary>
    /// Gets the accent brand color palette with automatic shade generation.
    /// Used for highlights, floating action buttons, and secondary actions.
    /// </summary>
    public required ColorShades Accent { get; init; }

    /// <summary>
    /// Gets the theme variant colors for different theme modes (Dark, Light, HighContrast).
    /// </summary>
    public required ThemeVariantPalette ThemeVariant { get; init; }

    /// <summary>
    /// Creates a CompleteTheme with simple configuration using base colors.
    /// </summary>
    /// <param name="themeVariant">The theme variant colors.</param>
    /// <param name="primaryColor">The primary brand color.</param>
    /// <param name="accentColor">The accent brand color.</param>
    /// <returns>A new CompleteTheme instance.</returns>
    public static ThemePalette Create(ThemeVariantPalette themeVariant, Color primaryColor, Color accentColor) => new()
    {
        Primary = new ColorShades(primaryColor),
        Accent = new ColorShades(accentColor),
        ThemeVariant = themeVariant
    };
}
