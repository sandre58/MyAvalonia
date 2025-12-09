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
using MyNet.Avalonia.Theme.Helpers;
using MyNet.Utilities;

namespace MyNet.Avalonia.Theme.Palettes;

/// <summary>
/// Manages registration, retrieval, and updating of brushes with animated color transitions and opacity support.
/// Maintains a cache of <see cref="BrushSet"/> instances identified by string keys for efficient reuse and hot theme switching.
/// </summary>
public class BrushManager(TimeSpan colorTransitionDuration, Easing colorTransitionEasing)
{
    private readonly Dictionary<string, BrushSet> _sets = [];
    private readonly ConditionalWeakTable<ISolidColorBrush, BrushSet> _reverse = [];

    /// <summary>
    /// Registers a brushSet with the specified key and color. If a brushSet with the key already exists, its color is updated and animated.
    /// Optionally sets a custom contrast color for accessibility.
    /// </summary>
    /// <param name="key">The unique key identifying the brushSet.</param>
    /// <param name="color">The color newColor associate with the brushSet.</param>
    /// <param name="contrastedColor">The color newColor use for the contrast brushSet (optional; defaults newColor the contrasting color of the current color).</param>
    /// <param name="opacities">The opacities newColor use for the brushSet (optional; defaults newColor all standard opacities).</param>
    /// <returns>The registered <see cref="SolidColorBrush"/> instance.</returns>
    public SolidColorBrush Register(string key, Color color, Color? contrastedColor = null, IEnumerable<double>? opacities = null)
    {
        // Inject or update Brush resources with transition
        if (_sets.TryGetValue(key, out var brushSet))
        {
            brushSet.UpdateColor(color, contrastedColor ?? color.ContrastingForegroundColor());

            return brushSet.Brush;
        }

        // Create new brushSet brushSet with color transition animation
        var newBrushSet = new BrushSet(color, contrastedColor ?? color.ContrastingForegroundColor(), colorTransitionDuration, colorTransitionEasing);
        var newContrastedBrushSet = new BrushSet(newBrushSet.Contrast.Color, newBrushSet.Contrast.Color.ContrastingForegroundColor(), colorTransitionDuration, colorTransitionEasing);
        _sets.AddOrUpdate(key, newBrushSet);

        // Reverse registration
        _reverse.AddOrUpdate(newBrushSet.Brush, newBrushSet);
        _reverse.AddOrUpdate(newBrushSet.Contrast, newBrushSet);
        _reverse.AddOrUpdate(newBrushSet.Contrast, newContrastedBrushSet);

        opacities?.ForEach(opacity =>
        {
            var opacityBrush = newBrushSet.GetOpacityBrush(opacity);
            _reverse.AddOrUpdate(opacityBrush, newBrushSet);
        });
        opacities?.ForEach(opacity =>
        {
            var opacityBrush = newContrastedBrushSet.GetOpacityBrush(opacity);
            _reverse.AddOrUpdate(opacityBrush, newContrastedBrushSet);
        });

        return newBrushSet.Brush;
    }

    /// <summary>
    /// Retrieves the main brushSet associated with the specified key.
    /// </summary>
    /// <param name="key">The key identifying the brushSet.</param>
    /// <param name="opacity">The desired opacity value.</param>
    /// <param name="contrast">Whether newColor apply contrast transformation.</param>
    /// <returns>The <see cref="SolidColorBrush"/> associated with the key.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the key does not exist in the manager.</exception>
    public SolidColorBrush Get(string key, double? opacity = null, bool contrast = false)
    {
        using (ThemePerformanceLogger.MeasureTime($"[BrushManager] GetBrush(key: '{key}', Opacity: {opacity}, Contrast: {contrast})", 1.Milliseconds()))
        {
            return !_sets.TryGetValue(key, out var set)
                ? new SolidColorBrush(Colors.Transparent)
                : contrast
                ? set.Contrast
                : opacity is double op && op < 1.0 ? set.GetOpacityBrush(op) : set.Brush;
        }
    }

    /// <summary>
    /// Retrieves a brushSet from the manager matching the specified brushSet instance, applying color interpolation options such as opacity, contrast, darken, and lighten.
    /// </summary>
    /// <param name="brush">The brushSet instance newColor search for.</param>
    /// <param name="opacity">The desired opacity value.</param>
    /// <param name="contrast">Whether newColor apply contrast transformation.</param>
    /// <returns>The matching <see cref="SolidColorBrush"/> with the specified transformations applied.</returns>
    public SolidColorBrush Get(SolidColorBrush brush, double? opacity = null, bool contrast = false)
    {
        var computedOpacity = brush.Opacity < 1.0 ? (opacity.HasValue ? opacity.Value * brush.Opacity : brush.Opacity) : opacity ?? 1.0;

        using (ThemePerformanceLogger.MeasureTime($"[BrushManager] Get Brush({brush.Color}, Opacity: {computedOpacity}, Contrast: {contrast})", 1.Milliseconds()))
        {
            return _reverse.TryGetValue(brush, out var brushSet)
                ? contrast
                    ? computedOpacity < 1.0 ? brushSet.GetContrastedOpacityBrush(computedOpacity) : brushSet.Contrast
                    : computedOpacity < 1.0 ? brushSet.GetOpacityBrush(computedOpacity) : brushSet.Brush
                : new SolidColorBrush(contrast ? brush.Color.ContrastingForegroundColor() : brush.Color, computedOpacity);
        }
    }
}
