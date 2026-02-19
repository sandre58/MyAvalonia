// -----------------------------------------------------------------------
// <copyright file="ThemeBase.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Styling;
using MyNet.UI.Theming;
using MyNet.Utilities;

namespace MyNet.Avalonia.Extended.Theming;

/// <summary>
/// Represents the base theme configuration for a specific theme variant (e.g., Dark, Light, HighContrast) in the theming system.
/// </summary>
/// <param name="themeVariant">The theme variant associated with this theme base.</param>
/// <param name="isDark">A value indicating whether this theme base represents a dark theme variant.</param>
/// <param name="isHighContrast">A value indicating whether this theme base represents a high contrast theme variant.</param>
public class ThemeBase(ThemeVariant themeVariant, bool isDark, bool isHighContrast) : IThemeBase
{
    /// <summary>
    /// Gets the name of the theme base, derived from the theme variant key. This is used to identify the theme base in the theming system.
    /// </summary>
    public string Name => themeVariant.Key.ToString().OrEmpty();

    /// <summary>
    /// Gets a value indicating whether the theme variant associated with this theme base, which indicates the overall theme mode (e.g., Dark, Light, HighContrast) that this theme base represents.
    /// </summary>
    public bool IsDark => isDark;

    /// <summary>
    /// Gets a value indicating whether this theme base represents a high contrast theme variant, which is used to provide enhanced visibility and accessibility for users with visual impairments.
    /// </summary>
    public bool IsHighContrast => isHighContrast;

    /// <summary>
    /// Gets a string representation of the theme base, which is the name of the theme variant. This is used for debugging and logging purposes to easily identify the theme base by its associated theme variant.
    /// </summary>
    /// <returns>The name of the theme variant.</returns>
    public override string ToString() => Name;

    /// <summary>
    /// Gets a value indicating whether the specified object is equal to the current theme base. Two theme bases are considered equal if they have the same name, which is derived from their associated theme variant. This allows for easy comparison of theme bases based on their theme variant identity.
    /// </summary>
    /// <param name="obj">The object to compare with the current theme base.</param>
    /// <returns>True if the specified object is equal to the current theme base; otherwise, false.</returns>
    public override bool Equals(object? obj) => obj is ThemeBase other && Name == other.Name;

    /// <summary>
    /// Gets the hash code for the theme base, which is based on the name of the theme variant. This allows for efficient hashing and retrieval of theme bases in collections that use hashing, such as dictionaries or hash sets.
    /// </summary>
    /// <returns>The hash code for the theme base.</returns>
    public override int GetHashCode() => Name.GetHashCode(System.StringComparison.OrdinalIgnoreCase);
}
