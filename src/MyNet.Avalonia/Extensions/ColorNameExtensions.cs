// -----------------------------------------------------------------------
// <copyright file="ColorNameExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Avalonia.Media;

namespace MyNet.Avalonia.Extensions;

/// <summary>
/// Provides extension methods for color manipulation, conversion, and analysis.
/// Supports conversions between RGB, Hex, color names, and color space transformations (XYZ, LAB).
/// </summary>
[SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "Extensions methods must be in a static class, and extension methods cannot be in a nested class.")]
public static class ColorNameExtensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="string"/> class to convert color names or hex codes to <see cref="Color"/> objects.
    /// </summary>
    extension(string? colorName)
    {
        /// <summary>
        /// Converts a string to a <see cref="Color"/> object, attempting multiple parsing strategies.
        /// Tries the following in order: hex code (with or without '#'), named color lookup, and fallback with '#' prefix.
        /// </summary>
        /// <returns>The parsed <see cref="Color"/> if successful; otherwise, null.</returns>
        public Color? ToColor()
        {
            if (string.IsNullOrWhiteSpace(colorName))
                return null;

            // Try direct parsing
            var result = colorName.TryToColor();
            if (result.HasValue)
                return result;

            // Fallback: add '#' prefix if not present and try again
            if (!colorName.StartsWith('#'))
            {
                result = ("#" + colorName).TryToColor();
            }

            return result;
        }

        /// <summary>
        /// Attempts to convert a string to a <see cref="Color"/> object without throwing exceptions.
        /// Checks for named colors in the resource dictionary, then tries parsing as hex code.
        /// </summary>
        /// <returns>The parsed <see cref="Color"/> if successful; otherwise, null.</returns>
        public Color? TryToColor()
        {
            if (string.IsNullOrWhiteSpace(colorName))
                return null;

            // Try named color lookup first (if not starting with '#')
            if (!colorName.StartsWith('#'))
            {
                var namedColor = ResourceLocator.ColorResourcesDictionary
                    .FirstOrDefault(x => string.Equals(x.Value, colorName, StringComparison.OrdinalIgnoreCase));

                if (!namedColor.Equals(default(KeyValuePair<Color, string>)))
                    return namedColor.Key;
            }

            // Try parsing as hex code
            if (Color.TryParse(colorName, out var color))
                return color;

            // Last attempt: try with '#' prefix if not already present
            return !colorName.StartsWith('#') && Color.TryParse($"#{colorName}", out color) ? color : null;
        }
    }
}
