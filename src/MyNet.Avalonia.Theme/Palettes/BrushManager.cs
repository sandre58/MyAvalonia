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
public class BrushManager(TimeSpan? colorTransitionDuration, Easing? colorTransitionEasing)
{
    public static readonly IBrush FallbackBrush = Brushes.Fuchsia;

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
    /// <param name="darken">Optional darken factor (value between 0.0 and 1.0).</param>
    /// <param name="lighten">Optional lighten factor (value between 0.0 and 1.0).</param>
    /// <returns>The <see cref="SolidColorBrush"/> associated with the key.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the key does not exist in the manager.</exception>
    public IBrush Get(string key, double? opacity = null, bool contrast = false, double? darken = null, double? lighten = null)
    {
        using (PerformanceMonitor.Measure($"[BrushManager] GetBrush(key: '{key}', Opacity: {opacity}, Contrast: {contrast}, Darken: {darken}, Lighten: {lighten})", maxBeforeWarning: 1.Milliseconds(), category: PerformanceCategory.Brushes))
        {
            if (!_sets.TryGetValue(key, out var set))
            {
                PerformanceMonitor.Warning($"[BrushManager] Brush not found (key: '{key}', Opacity: {opacity}, Contrast: {contrast}, Darken: {darken}, Lighten: {lighten})", category: PerformanceCategory.Brushes);
                return FallbackBrush;
            }
            else
            {
                return ResolveBrushFromSet(set, opacity, contrast, darken, lighten);
            }
        }
    }

    /// <summary>
    /// Retrieves a brush set from the manager matching the specified brush instance, applying color interpolation options such as opacity and contrast.
    /// </summary>
    /// <param name="brush">The brush instance to search for.</param>
    /// <param name="opacity">The desired opacity value.</param>
    /// <param name="contrast">Whether to apply contrast transformation.</param>
    /// <param name="darken">Optional darken factor (value between 0.0 and 1.0).</param>
    /// <param name="lighten">Optional lighten factor (value between 0.0 and 1.0).</param>
    /// <returns>The matching <see cref="SolidColorBrush"/> with the specified transformations applied.</returns>
    public IBrush Get(IBrush brush, double? opacity = null, bool contrast = false, double? darken = null, double? lighten = null)
    {
        switch (brush)
        {
            case SolidColorBrush solidColorBrush:
                var computedOpacity = opacity.HasValue ? opacity.Value * solidColorBrush.Opacity : solidColorBrush.Opacity;
                using (PerformanceMonitor.Measure($"[BrushManager] Get Brush({solidColorBrush.Color}, Opacity: {computedOpacity}, Contrast: {contrast}, Darken: {darken}, Lighten: {lighten})", 1.Milliseconds(), category: PerformanceCategory.Brushes))
                {
                    if (_reverse.TryGetValue(solidColorBrush, out var registration))
                    {
                        return contrast && registration.IsContrast
                            ? ComputeUnknownBrush(solidColorBrush, computedOpacity, contrast, darken, lighten)
                            : ResolveBrushFromSet(registration.Set, computedOpacity, contrast ^ registration.IsContrast, darken, lighten);
                    }
                    else
                    {
                        PerformanceMonitor.Warning($"[BrushManager] Brush not registered ({solidColorBrush.Color}, Opacity: {computedOpacity}, Contrast: {contrast}, Darken: {darken}, Lighten: {lighten})", PerformanceCategory.Theme);

                        return ComputeUnknownBrush(solidColorBrush, computedOpacity, contrast, darken, lighten);
                    }
                }

            case IImmutableSolidColorBrush immutableSolidColorBrush:
                var computedOpacity1 = opacity.HasValue ? opacity.Value * immutableSolidColorBrush.Opacity : immutableSolidColorBrush.Opacity;
                PerformanceMonitor.Warning($"[BrushManager] Try to get ImmutableSolidColorBrush({immutableSolidColorBrush.Color}, Opacity: {computedOpacity1}, Contrast: {contrast})", category: PerformanceCategory.Theme);

                return ComputeUnknownBrush(immutableSolidColorBrush, computedOpacity1, contrast, darken, lighten);

            default:
                PerformanceMonitor.Warning($"[BrushManager] Try to get Brush({brush})", category: PerformanceCategory.Theme);
                return brush;
        }
    }

    /// <summary>
    /// Computes a new brush based on an unknown brush instance by applying color interpolation transformations such as opacity, contrast, darkening, and lightening.
    /// </summary>
    /// <param name="brush">The unknown brush instance.</param>
    /// <param name="opacity">The desired opacity value.</param>
    /// <param name="contrast">Whether to apply contrast transformation.</param>
    /// <param name="darken">Optional darken factor (value between 0.0 and 1.0).</param>
    /// <param name="lighten">Optional lighten factor (value between 0.0 and 1.0).</param>
    /// <returns>The computed brush with the specified transformations applied.</returns>
    private static ISolidColorBrush ComputeUnknownBrush(ISolidColorBrush brush, double opacity, bool contrast, double? darken, double? lighten)
    {
        if (opacity == 1.0 && !contrast && !darken.HasValue && !lighten.HasValue)
            return brush;

        var color = brush.Color.Apply(new ColorInterpolation(null, contrast, darken, lighten));
        return opacity < 1.0 || contrast || darken.HasValue || lighten.HasValue
            ? new SolidColorBrush(color, opacity)
            : brush;
    }

    /// <summary>
    /// Resolves the correct brush from a <see cref="BrushSet"/> based on opacity and contrast settings.
    /// </summary>
    /// <param name="set">The brush set to resolve from.</param>
    /// <param name="opacity">The desired opacity value.</param>
    /// <param name="contrast">Whether to apply contrast transformation.</param>
    /// <param name="darken">Optional darken factor (value between 0.0 and 1.0).</param>
    /// <param name="lighten">Optional lighten factor (value between 0.0 and 1.0).</param>
    /// <returns>The resolved <see cref="SolidColorBrush"/>.</returns>
    private ISolidColorBrush ResolveBrushFromSet(BrushSet set, double? opacity, bool contrast, double? darken, double? lighten)
    {
        // If we need to darken or lighten, we create a new brush with the transformed color
        if (darken.HasValue || lighten.HasValue)
        {
            var baseBrush = contrast
                ? opacity.HasValue && opacity < 1.0 ? set.GetContrastedOpacityBrush(opacity.Value) : set.Contrast
                : opacity.HasValue && opacity < 1.0 ? set.GetOpacityBrush(opacity.Value) : set.Brush;

            var transformedColor = baseBrush.Color.Apply(new ColorInterpolation(null, contrast, darken, lighten));
            return new SolidColorBrush(transformedColor, baseBrush.Opacity);
        }

        // Otherwise, use the standard resolution
        return contrast
            ? opacity.HasValue && opacity < 1.0 ? TrackBrush(set.GetContrastedOpacityBrush(opacity.Value), set, true) : set.Contrast
            : opacity.HasValue && opacity < 1.0 ? TrackBrush(set.GetOpacityBrush(opacity.Value), set, false) : set.Brush;
    }

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
    /// <returns>The tracked <see cref="ISolidColorBrush"/>.</returns>
    private ISolidColorBrush TrackBrush(ISolidColorBrush brush, BrushSet owner, bool isContrast)
    {
        _reverse.AddOrUpdate(brush, new BrushRegistration(owner, isContrast));
        return brush;
    }

    /// <summary>
    /// Represents a registration entry for a brush in the reverse lookup table, associating it with its owning <see cref="BrushSet"/> and contrast status.
    /// </summary>
    private sealed record BrushRegistration(BrushSet Set, bool IsContrast);
}
