// -----------------------------------------------------------------------
// <copyright file="OpacityLevels.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using MyNet.Avalonia.Theme.Extensions;

namespace MyNet.Avalonia.Theme.Palettes;

/// <summary>
/// Represents a collection of named opacity levels for various UI states and elevations in a theme.
/// Provides consistent opacity values for overlays, interactions, and elevation effects, enabling unified visual behavior across the application.
/// </summary>
public class OpacityLevels
{
    /// <summary>
    /// Gets the opacity level for overlay elements.
    /// </summary>
    public double Overlay { get; init; }

    /// <summary>
    /// Gets the opacity level for hover state.
    /// </summary>
    public double Hover { get; init; }

    /// <summary>
    /// Gets the opacity level for focus state.
    /// </summary>
    public double Focus { get; init; }

    /// <summary>
    /// Gets the opacity level for pressed state.
    /// </summary>
    public double Pressed { get; init; }

    /// <summary>
    /// Gets the opacity level for drag state.
    /// </summary>
    public double Drag { get; init; }

    /// <summary>
    /// Gets the opacity level for scrim overlays (background dimming).
    /// </summary>
    public double Scrim { get; init; }

    /// <summary>
    /// Gets the opacity level for disabled UI elements.
    /// </summary>
    public double Disabled { get; init; }

    /// <summary>
    /// Gets the opacity level for low elevation surfaces.
    /// </summary>
    public double Low { get; init; }

    /// <summary>
    /// Gets the opacity level for medium elevation surfaces.
    /// </summary>
    public double Medium { get; init; }

    /// <summary>
    /// Gets the opacity level for high elevation surfaces.
    /// </summary>
    public double High { get; init; }

    /// <summary>
    /// Converts the opacity levels to a resource dictionary for use in theming and resource lookups.
    /// </summary>
    /// <param name="prefix">The prefix to use for resource keys (default: "Opacity").</param>
    /// <returns>A dictionary mapping resource keys to opacity values.</returns>
    public Dictionary<string, object> ToResourceDictionary(string prefix = nameof(ThemeVariantColors.Opacity))
        => new()
        {
            [nameof(Overlay).WithPrefix(prefix)] = Overlay,
            [nameof(Hover).WithPrefix(prefix)] = Hover,
            [nameof(Focus).WithPrefix(prefix)] = Focus,
            [nameof(Pressed).WithPrefix(prefix)] = Pressed,
            [nameof(Drag).WithPrefix(prefix)] = Drag,
            [nameof(Scrim).WithPrefix(prefix)] = Scrim,
            [nameof(Disabled).WithPrefix(prefix)] = Disabled,
            [nameof(Low).WithPrefix(prefix)] = Low,
            [nameof(Medium).WithPrefix(prefix)] = Medium,
            [nameof(High).WithPrefix(prefix)] = High,
        };
}
