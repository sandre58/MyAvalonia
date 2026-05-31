// -----------------------------------------------------------------------
// <copyright file="ColorParsingExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Avalonia.Media;
using MyNet.Avalonia.Colors;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// Parses color names and hex strings into <see cref="Color"/> values.
/// </summary>
[SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "Extension methods must be in a static class.")]
public static class ColorParsingExtensions
{
    extension(string? colorName)
    {
        /// <summary>
        /// Converts a string to a <see cref="Color"/>, trying hex codes and named colors.
        /// </summary>
        public Color? ToColor()
        {
            if (string.IsNullOrWhiteSpace(colorName))
                return null;

            var result = colorName.TryToColor();
            if (result.HasValue)
                return result;

            if (!colorName.StartsWith('#'))
                result = ("#" + colorName).TryToColor();

            return result;
        }

        /// <summary>
        /// Attempts to convert a string to a <see cref="Color"/> without throwing.
        /// </summary>
        public Color? TryToColor()
        {
            if (string.IsNullOrWhiteSpace(colorName))
                return null;

            if (!colorName.StartsWith('#'))
            {
                var namedColor = ColorRegistry.Instance.TryResolve(colorName);
                if (namedColor.HasValue)
                    return namedColor;
            }

            return Color.TryParse(colorName, out var color)
                ? color
                : !colorName.StartsWith('#') && Color.TryParse($"#{colorName}", out color) ? color : null;
        }
    }
}
