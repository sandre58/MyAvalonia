// -----------------------------------------------------------------------
// <copyright file="BrushManager.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Avalonia.Animation.Easings;
using Avalonia.Media;
using MyNet.Avalonia.Extensions;
using MyNet.Avalonia.Helpers;
using MyNet.Utilities;

namespace MyNet.Avalonia.Theme.Palettes;

/// <summary>
/// Manages registration, retrieval, and updating of brushes with animated color transitions and opacity support.
/// Maintains a cache of <see cref="BrushSet"/> instances identified by string keys for efficient reuse and hot theme switching.
/// </summary>
public class BrushManager(TimeSpan colorTransitionDuration, Easing colorTransitionEasing)
{
    private readonly Dictionary<string, BrushSet> _sets = [];
    private readonly ConditionalWeakTable<ISolidColorBrush, BrushRegistration> _reverse = [];

    /// <summary>
    /// Registers a <see cref="BrushSet"/> with the specified key and color. If a brush set with the key already exists, its color is updated and animated.
    /// Optionally sets a custom contrast color for accessibility.
    /// </summary>
    /// <param name="key">The unique key identifying the brush set.</param>
    /// <param name="color">The color to associate with the brush set.</param>
    /// <param name="contrastedColor">The color to use for the contrast brush set (optional; defaults to the contrasting color of the current color).</param>
    /// <param name="opacities">The opacities to use for the brush set (optional; defaults to all standard opacities).</param>
    /// <returns>The registered <see cref="SolidColorBrush"/> instance.</returns>
    public SolidColorBrush Register(string key, Color color, Color? contrastedColor = null, IEnumerable<double>? opacities = null)
    {
        if (_sets.TryGetValue(key, out var brushSet))
        {
            brushSet.UpdateColor(color, contrastedColor ?? color.ContrastingForegroundColor());
            return brushSet.Brush;
        }

        var newBrushSet = new BrushSet(color, contrastedColor ?? color.ContrastingForegroundColor(), colorTransitionDuration, colorTransitionEasing);
        _sets.AddOrUpdate(key, newBrushSet);
        RegisterBrushSet(newBrushSet);

        opacities?.ForEach(opacity =>
        {
            TrackBrush(newBrushSet.GetOpacityBrush(opacity), newBrushSet, false);
            TrackBrush(newBrushSet.GetContrastedOpacityBrush(opacity), newBrushSet, true);
        });

        return newBrushSet.Brush;
    }

    /// <summary>
    /// Retrieves the main brush set associated with the specified key.
    /// </summary>
    /// <param name="key">The key identifying the brush set.</param>
    /// <param name="opacity">The desired opacity value.</param>
    /// <param name="contrast">Whether to apply contrast transformation.</param>
    /// <returns>The <see cref="SolidColorBrush"/> associated with the key.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the key does not exist in the manager.</exception>
    public SolidColorBrush Get(string key, double? opacity = null, bool contrast = false)
    {
        using (PerformanceMonitor.Measure($"[BrushManager] GetBrush(key: '{key}', Opacity: {opacity}, Contrast: {contrast})", 1.Milliseconds()))
            return !_sets.TryGetValue(key, out var set) ? new SolidColorBrush(Colors.Transparent) : ResolveBrushFromSet(set, opacity, contrast);
    }

    /// <summary>
    /// Retrieves a brush set from the manager matching the specified brush instance, applying color interpolation options such as opacity and contrast.
    /// </summary>
    /// <param name="brush">The brush instance to search for.</param>
    /// <param name="opacity">The desired opacity value.</param>
    /// <param name="contrast">Whether to apply contrast transformation.</param>
    /// <returns>The matching <see cref="SolidColorBrush"/> with the specified transformations applied.</returns>
    public SolidColorBrush Get(SolidColorBrush brush, double? opacity = null, bool contrast = false)
    {
        var computedOpacity = opacity.HasValue ? opacity.Value * brush.Opacity : brush.Opacity;
        using (PerformanceMonitor.Measure($"[BrushManager] Get Brush({brush.Color}, Opacity: {computedOpacity}, Contrast: {contrast})", 1.Milliseconds()))
        {
            return _reverse.TryGetValue(brush, out var registration)
                ? ResolveBrushFromSet(registration.Set, computedOpacity, contrast || registration.IsContrast)
                : new SolidColorBrush(contrast ? brush.Color.ContrastingForegroundColor() : brush.Color, computedOpacity);
        }
    }

    /// <summary>
    /// Resolves the correct brush from a <see cref="BrushSet"/> based on opacity and contrast settings.
    /// </summary>
    /// <param name="set">The brush set to resolve from.</param>
    /// <param name="opacity">The desired opacity value.</param>
    /// <param name="contrast">Whether to apply contrast transformation.</param>
    /// <returns>The resolved <see cref="SolidColorBrush"/>.</returns>
    private SolidColorBrush ResolveBrushFromSet(BrushSet set, double? opacity, bool contrast) => contrast
            ? opacity.HasValue && opacity < 1.0 ? TrackBrush(set.GetContrastedOpacityBrush(opacity.Value), set, true) : set.Contrast
            : opacity.HasValue && opacity < 1.0 ? TrackBrush(set.GetOpacityBrush(opacity.Value), set, false) : set.Brush;

    /// <summary>
    /// Registers the main and contrast brushes of a <see cref="BrushSet"/> in the reverse lookup table.
    /// </summary>
    /// <param name="set">The brush set to register.</param>
    private void RegisterBrushSet(BrushSet set)
    {
        TrackBrush(set.Brush, set, false);
        TrackBrush(set.Contrast, set, true);
    }

    /// <summary>
    /// Tracks a brush in the reverse lookup table, associating it with its owning <see cref="BrushSet"/> and contrast status.
    /// </summary>
    /// <param name="brush">The brush to track.</param>
    /// <param name="owner">The owning brush set.</param>
    /// <param name="isContrast">Whether the brush is a contrast variant.</param>
    /// <returns>The tracked <see cref="SolidColorBrush"/>.</returns>
    private SolidColorBrush TrackBrush(SolidColorBrush brush, BrushSet owner, bool isContrast)
    {
        _reverse.AddOrUpdate(brush, new BrushRegistration(owner, isContrast));
        return brush;
    }

    /// <summary>
    /// Represents a registration entry for a brush in the reverse lookup table, associating it with its owning <see cref="BrushSet"/> and contrast status.
    /// </summary>
    private record BrushRegistration(BrushSet Set, bool IsContrast);
}
