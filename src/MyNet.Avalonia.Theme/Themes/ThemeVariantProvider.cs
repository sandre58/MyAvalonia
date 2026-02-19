// -----------------------------------------------------------------------
// <copyright file="ThemeVariantProvider.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Styling;

namespace MyNet.Avalonia.Theme.Themes;

/// <summary>
/// Provides theme variant definitions for the application, such as HighContrast.
/// Used to register and identify custom or extended theme variants in the theming system.
/// </summary>
public static class ThemeVariantProvider
{
    /// <summary>
    /// Gets the high contrast theme variant, based on the Dark theme variant.
    /// </summary>
    public static readonly ThemeVariant HighContrast = new(nameof(HighContrast), ThemeVariant.Dark);

    /// <summary>
    /// Gets the dark blue theme variant, based on the Dark theme variant.
    /// </summary>
    public static readonly ThemeVariant DarkBlue = new(nameof(DarkBlue), ThemeVariant.Dark);
}
