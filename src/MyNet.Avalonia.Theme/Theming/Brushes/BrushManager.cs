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
using MyNet.Avalonia.Colors;
using MyNet.Avalonia.Theme.Diagnostics;
using MyNet.Collections;
using MyNet.Primitives;

namespace MyNet.Avalonia.Theme.Theming.Brushes;

/// <summary>
/// Manages registration, retrieval, and updating of brushes with animated color transitions and opacity support.
/// Maintains a cache of <see cref="BrushSet"/> instances identified by string keys for efficient reuse and hot theme switching.
/// </summary>
public class BrushManager(TimeSpan? colorTransitionDuration, Easing? colorTransitionEasing)
{
    public static readonly IBrush FallbackBrush = global::Avalonia.Media.Brushes.Fuchsia;

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

        var newBrushSet = CreateBrushSet(color, contrastedColor ?? color.ContrastingForegroundColor());
        _sets[key] = newBrushSet;
        RegisterBrushSet(newBrushSet);

        opacities?.ForEach(opacity =>
        {
            TrackBrush(newBrushSet.GetTransformedBrush(new(opacity)), newBrushSet, false);
            TrackBrush(newBrushSet.GetTransformedBrush(new(opacity, true)), newBrushSet, true);
        });

        return newBrushSet.Brush;
    }

    /// <summary>
    /// Retrieves the main brush set associated with the specified key.
    /// </summary>
    /// <param name="key">The key identifying the brush set.</param>
    /// <param name="colorInterpolation">The color interpolation settings.</param>
    /// <returns>The <see cref="SolidColorBrush"/> associated with the key.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the key does not exist in the manager.</exception>
    public IBrush Get(string key, ColorInterpolation colorInterpolation)
    {
        using (PerformanceMonitor.Measure($"[BrushManager] GetBrush(key: '{key}', {colorInterpolation})", maxBeforeWarning: 1.Milliseconds(), category: PerformanceCategory.Brushes))
        {
            if (!_sets.TryGetValue(key, out var set))
            {
                PerformanceMonitor.Warning($"[BrushManager] Brush not found (key: '{key}', {colorInterpolation})", category: PerformanceCategory.Brushes);
                return FallbackBrush;
            }

            return ResolveBrushFromSet(set, colorInterpolation);
        }
    }

    /// <summary>
    /// Retrieves a brush set from the manager matching the specified brush instance, applying color interpolation options such as opacity and contrast.
    /// </summary>
    /// <param name="brush">The brush instance to search for.</param>
    /// <param name="colorInterpolation">The color interpolation settings.</param>
    /// <returns>The matching <see cref="SolidColorBrush"/> with the specified transformations applied.</returns>
    public IBrush Get(IBrush brush, ColorInterpolation colorInterpolation)
    {
        switch (brush)
        {
            case SolidColorBrush solidColorBrush:
                using (PerformanceMonitor.Measure($"[BrushManager] Get Brush({solidColorBrush.Color}, {colorInterpolation})", 1.Milliseconds(), category: PerformanceCategory.Brushes))
                {
                    if (_reverse.TryGetValue(solidColorBrush, out var registration))
                    {
                        var computedColorInterpolation = new ColorInterpolation(
                            colorInterpolation.Opacity.HasValue ? colorInterpolation.Opacity.Value * solidColorBrush.Opacity : solidColorBrush.Opacity,
                            colorInterpolation.Contrast ^ registration.IsContrast,
                            colorInterpolation.Darken,
                            colorInterpolation.Lighten);
                        return colorInterpolation.Contrast && registration.IsContrast
                            ? ComputeUnknownBrush(solidColorBrush, new(computedColorInterpolation.Opacity, colorInterpolation.Contrast, computedColorInterpolation.Darken, computedColorInterpolation.Lighten))
                            : ResolveBrushFromSet(registration.Set, computedColorInterpolation);
                    }

                    PerformanceMonitor.Warning($"[BrushManager] Brush not registered ({solidColorBrush.Color}, {colorInterpolation})", PerformanceCategory.Theme);
                    return ComputeUnknownBrush(solidColorBrush, colorInterpolation);
                }

            case IImmutableSolidColorBrush immutableSolidColorBrush:
                PerformanceMonitor.Warning($"[BrushManager] Try to get ImmutableSolidColorBrush({immutableSolidColorBrush.Color}, {colorInterpolation})", category: PerformanceCategory.Theme);
                return ComputeUnknownBrush(immutableSolidColorBrush, colorInterpolation);

            default:
                PerformanceMonitor.Warning($"[BrushManager] Try to get Brush({brush})", category: PerformanceCategory.Theme);
                return brush;
        }
    }

    /// <summary>
    /// Computes a new brush based on an unknown brush instance by applying color interpolation transformations such as opacity, contrast, darkening, and lightening.
    /// </summary>
    /// <param name="brush">The unknown brush instance.</param>
    /// <param name="colorInterpolation">The color interpolation settings.</param>
    /// <returns>The computed brush with the specified transformations applied.</returns>
    private static ISolidColorBrush ComputeUnknownBrush(ISolidColorBrush brush, ColorInterpolation colorInterpolation)
    {
        if (colorInterpolation is { IsEmpty: true, Contrast: false })
            return brush;

        var color = brush.Color.Apply(colorInterpolation);
        return new SolidColorBrush(color);
    }

    /// <summary>
    /// Resolves the correct brush from a <see cref="BrushSet"/> based on opacity and contrast settings.
    /// </summary>
    /// <param name="set">The brush set to resolve from.</param>
    /// <param name="colorInterpolation">The color interpolation settings.</param>
    /// <returns>The resolved <see cref="SolidColorBrush"/>.</returns>
    private ISolidColorBrush ResolveBrushFromSet(BrushSet set, ColorInterpolation colorInterpolation)
        => colorInterpolation.IsEmpty
            ? colorInterpolation.Contrast ? set.Contrast : set.Brush
            : TrackBrush(set.GetTransformedBrush(colorInterpolation), set, colorInterpolation.Contrast);

    private BrushSet CreateBrushSet(Color color, Color contrastedColor)
        => new(
            color,
            contrastedColor,
            colorTransitionDuration,
            colorTransitionEasing,
            onTransformedBrushEvicted: UntrackBrush);

    private void UntrackBrush(ISolidColorBrush brush) => _reverse.Remove(brush);

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
        _reverse.AddOrUpdate(brush, new(owner, isContrast));
        return brush;
    }

    /// <summary>
    /// Represents a registration entry for a brush in the reverse lookup table, associating it with its owning <see cref="BrushSet"/> and contrast status.
    /// </summary>
    private sealed record BrushRegistration(BrushSet Set, bool IsContrast);
}
