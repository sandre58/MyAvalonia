// -----------------------------------------------------------------------
// <copyright file="BrushManager.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Avalonia.Animation.Easings;
using Avalonia.Media;
using MyNet.Avalonia.Extensions;
using MyNet.Utilities;

namespace MyNet.Avalonia.Theme.Palettes;

/// <summary>
/// Manages registration, retrieval, and updating of brushes with animated color transitions and opacity support.
/// Maintains a cache of <see cref="BrushSet"/> instances identified by string keys for efficient reuse and hot theme switching.
/// </summary>
public class BrushManager(TimeSpan colorTransitionDuration, Easing colorTransitionEasing)
{
    private readonly Dictionary<string, BrushSet> _cache = [];
    private readonly Dictionary<SolidColorBrush, string> _brushToKey = [];

    /// <summary>
    /// Registers a brush with the specified key and color. If a brush with the key already exists, its color is updated and animated.
    /// Optionally sets a custom contrast color for accessibility.
    /// </summary>
    /// <param name="key">The unique key identifying the brush.</param>
    /// <param name="color">The color to associate with the brush.</param>
    /// <param name="contrastedColor">The color to use for the contrast brush (optional; defaults to the contrasting color of the current color).</param>
    /// <returns>The registered <see cref="SolidColorBrush"/> instance.</returns>
    public SolidColorBrush RegisterBrush(string key, Color color, Color? contrastedColor = null)
    {
        // Inject or update Brush resources with transition
        if (_cache.TryGetValue(key, out var brushCache))
        {
            // Update brush color (will animate via ColorTransition)
            brushCache.UpdateColor(color, contrastedColor);
            return brushCache.Brush;
        }
        else
        {
            // Create new brush set with color transition animation
            var newBrushSet = new BrushSet(color, contrastedColor, colorTransitionDuration, colorTransitionEasing);
            _cache.AddOrUpdate(key, newBrushSet);
            _brushToKey.AddOrUpdate(newBrushSet.Brush, key);
            return newBrushSet.Brush;
        }
    }

    /// <summary>
    /// Retrieves the main brush associated with the specified key.
    /// </summary>
    /// <param name="key">The key identifying the brush.</param>
    /// <param name="colorInterpolation">Options for opacity, contrast, darken, and lighten transformations.</param>
    /// <returns>The <see cref="SolidColorBrush"/> associated with the key.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the key does not exist in the manager.</exception>
    public SolidColorBrush GetBrush(string key, ColorInterpolation? colorInterpolation = null)
    {
        var brush = _cache.TryGetValue(key, out var brushSet)
            ? brushSet.Brush
            : throw new KeyNotFoundException($"Brush '{key}' not found.");

        return GetBrush(brush, colorInterpolation ?? new ColorInterpolation());
    }

    /// <summary>
    /// Retrieves a brush from the manager matching the specified brush instance, applying color interpolation options such as opacity, contrast, darken, and lighten.
    /// </summary>
    /// <param name="brush">The brush instance to search for.</param>
    /// <param name="colorInterpolation">Options for opacity, contrast, darken, and lighten transformations.</param>
    /// <returns>The matching <see cref="SolidColorBrush"/> with the specified transformations applied.</returns>
    public SolidColorBrush GetBrush(SolidColorBrush brush, ColorInterpolation colorInterpolation)
    {
        SolidColorBrush cachedBrush;
        if (_brushToKey.TryGetValue(brush, out var key) && _cache.TryGetValue(key, out var brushSet))
        {
            cachedBrush = colorInterpolation.Contrast ? brushSet.Contrast
                            : colorInterpolation.Opacity.HasValue ? brushSet.GetOpacityBrush(colorInterpolation.Opacity.Value)
                            : brushSet.Brush;
        }
        else
        {
            // Fallback: create a new brush with the requested opacity if not found in the manager
            cachedBrush = colorInterpolation.Opacity.HasValue
            ? new SolidColorBrush(brush.Color, colorInterpolation.Opacity.Value) { Transitions = brush.Transitions }
            : brush;
        }

        if (colorInterpolation.Darken.HasValue)
            cachedBrush = new SolidColorBrush(cachedBrush.Color.Darken(), cachedBrush.Opacity);
        if (colorInterpolation.Lighten.HasValue)
            cachedBrush = new SolidColorBrush(cachedBrush.Color.Lighten(), cachedBrush.Opacity);

        return cachedBrush;
    }
}
