// -----------------------------------------------------------------------
// <copyright file="GenderPalette.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Avalonia.Media;
using MyNet.Primitives;

namespace MyNet.Avalonia.Theme.Theming.Palettes;

/// <summary>
/// Represents a palette of colors for gender-specific themes.
/// </summary>
public class GenderPalette
{
    /// <summary>
    /// Gets the color associated with the male gender.
    /// </summary>
    public required Color Male { get; init; }

    /// <summary>
    /// Gets the color associated with the female gender.
    /// </summary>
    public required Color Female { get; init; }

    /// <summary>
    /// Converts the gender palette to a read-only dictionary suitable for use as resource dictionary keys and values.
    /// </summary>
    /// <returns>A dictionary containing all gender colors with their corresponding resource keys.</returns>
    public IReadOnlyDictionary<string, object> ToResourceDictionary(string prefix = nameof(ThemeVariantPalette.Gender)) => new Dictionary<string, object>
        {
            { nameof(Male).WithPrefix(prefix, "."), Male },
            { nameof(Female).WithPrefix(prefix, "."), Female }
        };

    /// <summary>
    /// Creates a GenderPalette instance from a resource dictionary.
    /// </summary>
    /// <param name="dictionary">The resource dictionary containing color definitions.</param>
    /// <param name="prefix">The prefix used for resource keys (default: "Gender").</param>
    /// <returns>A new GenderPalette instance.</returns>
    public static GenderPalette FromResourceDictionary(IReadOnlyDictionary<string, object> dictionary, string prefix = nameof(ThemeVariantPalette.Gender)) => new()
    {
        Male = (Color)dictionary.GetValueOrDefault(nameof(Male).WithPrefix(prefix, "."), global::Avalonia.Media.Colors.Blue),
        Female = (Color)dictionary.GetValueOrDefault(nameof(Female).WithPrefix(prefix, "."), global::Avalonia.Media.Colors.Pink)
    };
}
