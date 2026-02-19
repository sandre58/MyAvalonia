// -----------------------------------------------------------------------
// <copyright file="BrushSet.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Media;
using MyNet.Avalonia.Helpers;
using MyNet.Utilities;

namespace MyNet.Avalonia.Theme.Palettes;

/// <summary>
/// Represents a set of brushes for a single color, supporting animated color transitions and multiple opacity levels.
/// Provides access to the main brush, a contrast brush for accessibility, and brushes with custom opacity, all synchronized to the current color.
/// </summary>
public class BrushSet
{
    private readonly ColorTransition? _colorTransition;
    private readonly Dictionary<double, SolidColorBrush> _brushes = [];
    private readonly Dictionary<double, SolidColorBrush> _contrastedBrushes = [];
    private bool _transitionsEnabled;

    /// <summary>
    /// Initializes a new instance of the <see cref="BrushSet"/> class with the specified color, transition duration, and easing.
    /// </summary>
    /// <param name="color">The base color for the brush set.</param>
    /// <param name="contrastedColor">The color to use for the contrast brush (optional; defaults to the contrasting color of the current color).</param>
    /// <param name="colorTransitionDuration">The duration of the color transition animation.</param>
    /// <param name="colorTransitionEasing">The easing function for the color transition animation.</param>
    public BrushSet(Color color, Color contrastedColor, TimeSpan? colorTransitionDuration = null, Easing? colorTransitionEasing = null)
    {
        if (colorTransitionDuration > TimeSpan.Zero && colorTransitionEasing != null)
        {
            _colorTransition = new ColorTransition
            {
                Duration = colorTransitionDuration.Value,
                Easing = colorTransitionEasing,
                Property = SolidColorBrush.ColorProperty
            };
        }

        using (PerformanceMonitor.Measure("[BrushSet] Constructor", maxBeforeWarning: 1.Milliseconds(), category: PerformanceCategory.Brushes))
        {
            Brush = CreateBrush(color);
            Contrast = CreateBrush(contrastedColor);
        }
    }

    /// <summary>
    /// Gets the main brush for the brush set, using the current color and full opacity.
    /// </summary>
    public SolidColorBrush Brush { get; }

    /// <summary>
    /// Gets the contrast brush for the brush set, providing a color with high contrast for accessibility.
    /// </summary>
    public SolidColorBrush Contrast { get; }

    /// <summary>
    /// Gets a brush with the specified opacity, using the current color and animated transitions.
    /// If a brush with the requested opacity already exists, it is returned; otherwise, a new one is created and cached.
    /// </summary>
    /// <param name="opacity">The opacity value for the brush (0.0 to 1.0).</param>
    /// <returns>A <see cref="SolidColorBrush"/> with the specified opacity.</returns>
    public SolidColorBrush GetOpacityBrush(double opacity)
    {
        opacity = NormalizeOpacity(opacity);
        if (_brushes.TryGetValue(opacity, out var existing)) return existing;

        using (PerformanceMonitor.Measure($"[BrushSet] Created new opacity brush ({opacity:F2}) (Current opacity variants: {_brushes.Count})", maxBeforeWarning: 1.Milliseconds(), category: PerformanceCategory.Brushes))
        {
            // Create new brush with color transition animation
            var newBrush = CreateBrush(Brush.Color, opacity);

            _brushes.AddOrUpdate(opacity, newBrush);

            return newBrush;
        }
    }

    /// <summary>
    /// Gets a brush with the specified opacity, using the current color and animated transitions.
    /// If a brush with the requested opacity already exists, it is returned; otherwise, a new one is created and cached.
    /// </summary>
    /// <param name="opacity">The opacity value for the brush (0.0 to 1.0).</param>
    /// <returns>A <see cref="SolidColorBrush"/> with the specified opacity.</returns>
    public SolidColorBrush GetContrastedOpacityBrush(double opacity)
    {
        opacity = NormalizeOpacity(opacity);
        if (_contrastedBrushes.TryGetValue(opacity, out var existing)) return existing;

        using (PerformanceMonitor.Measure($"[BrushSet] Created new contrasted opacity brush ({opacity:F2}) (Current opacity variants: {_contrastedBrushes.Count})", maxBeforeWarning: 1.Milliseconds(), category: PerformanceCategory.Brushes))
        {
            // Create new brush with color transition animation
            var newBrush = CreateBrush(Contrast.Color, opacity);

            _contrastedBrushes.AddOrUpdate(opacity, newBrush);

            return newBrush;
        }
    }

    /// <summary>
    /// Updates the brush set to use a new color. All brushes managed by this set will be updated to the new color.
    /// </summary>
    /// <param name="newColor">The new color to apply to the brush set.</param>
    /// <param name="contrastedColor">The color to use for the contrast brush (optional; defaults to the contrasting color of the current color).</param>
    public void UpdateColor(Color newColor, Color contrastedColor)
    {
        // Enable transitions for smooth theme change animation
        if (!_transitionsEnabled)
        {
            EnableTransitions();
        }

        Brush.Color = newColor;
        Contrast.Color = contrastedColor;

        // Update all opacity brushes
        foreach (var brush in _brushes.Values)
        {
            brush.Color = newColor;
        }

        foreach (var brush in _contrastedBrushes.Values)
        {
            brush.Color = contrastedColor;
        }
    }

    /// <summary>
    /// Enables color transitions on all brushes in this set.
    /// Called automatically during the first theme change to add smooth animations.
    /// </summary>
    private void EnableTransitions()
    {
        if (_colorTransition == null) return;

        _transitionsEnabled = true;

        // Add transitions to main brushes
        Brush.Transitions = [_colorTransition];
        Contrast.Transitions = [_colorTransition];

        // Add transitions to all cached opacity brushes
        foreach (var brush in _brushes.Values)
        {
            brush.Transitions = [_colorTransition];
        }

        foreach (var brush in _contrastedBrushes.Values)
        {
            brush.Transitions = [_colorTransition];
        }

        PerformanceMonitor.Debug($"[BrushSet] Enabled transitions for {_brushes.Count + _contrastedBrushes.Count + 2} brushes", PerformanceCategory.Brushes);
    }

    /// <summary>
    /// Normalizes an opacity value to the range [0, 1] and rounds to three decimals for consistent caching.
    /// </summary>
    /// <param name="opacity">The opacity value to normalize.</param>
    /// <returns>The normalized opacity value.</returns>
    private static double NormalizeOpacity(double opacity)
    {
        var clamped = Math.Clamp(opacity, 0d, 1d);
        return Math.Round(clamped, 3, MidpointRounding.AwayFromZero);
    }

    /// <summary>
    /// Creates a new <see cref="SolidColorBrush"/> with the current color, specified opacity, and color transition animation.
    /// Transitions are NOT added at creation time for performance - they're added lazily when theme changes.
    /// </summary>
    /// <param name="color">Color of the brush.</param>
    /// <param name="opacity">The opacity value for the brush (optional; defaults to 1.0).</param>
    /// <returns>A new <see cref="SolidColorBrush"/> instance.</returns>
    private SolidColorBrush CreateBrush(Color color, double opacity = 1.0)
    {
        var brush = new SolidColorBrush(color) { Opacity = opacity };

        // Only add transitions if already enabled (during theme change)
        if (_transitionsEnabled && _colorTransition is not null)
        {
            brush.Transitions = [_colorTransition];
        }

        return brush;
    }
}
