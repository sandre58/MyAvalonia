// -----------------------------------------------------------------------
// <copyright file="IColorRegistry.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;
using Avalonia.Media;

namespace MyNet.Avalonia.Colors;

/// <summary>
/// Maps Avalonia <see cref="Color"/> values to localized display names via the translation catalog.
/// </summary>
public interface IColorRegistry
{
    /// <summary>
    /// Gets the localized display name for a color, or <see langword="null"/> when the color is unknown.
    /// </summary>
    string? GetDisplayName(Color color, CultureInfo? culture = null);

    /// <summary>
    /// Resolves a color from a localized display name, an invariant resource key, or <see langword="null"/>.
    /// </summary>
    Color? TryResolve(string name, CultureInfo? culture = null);
}
