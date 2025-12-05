// -----------------------------------------------------------------------
// <copyright file="GenderPalette.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Avalonia.Media;
using MyNet.Avalonia.Theme.Extensions;

namespace MyNet.Avalonia.Theme.Palettes;

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
    public IReadOnlyDictionary<string, object> ToResourceDictionary(string prefix = nameof(ThemeVariantColors.Gender)) => new Dictionary<string, object>
        {
            { nameof(Male).WithPrefix(prefix), Male },
            { nameof(Female).WithPrefix(prefix), Female }
        };
}
