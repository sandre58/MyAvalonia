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
using MyNet.Avalonia.Colors;
using MyNet.Avalonia.Theme.Diagnostics;
using MyNet.Primitives;

namespace MyNet.Avalonia.Theme.Theming.Brushes;

/// <summary>
/// Represents a set of brushes for a single color, supporting animated color transitions and multiple opacity levels.
/// Provides access to the main brush, a contrast brush for accessibility, and brushes with custom opacity, all synchronized to the current color.
/// </summary>
public class BrushSet
{
    private readonly ColorTransition? _colorTransition;
    private readonly Dictionary<ColorInterpolation, SolidColorBrush> _brushes = [];
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
            _colorTransition = new()
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
    /// Gets a transformed brush based on the specified color interpolation value.
    /// </summary>
    /// <param name="value">The color interpolation value.</param>
    /// <returns>A <see cref="SolidColorBrush"/> with the specified transformation.</returns>
    public SolidColorBrush GetTransformedBrush(ColorInterpolation value)
    {
        if (_brushes.TryGetValue(value, out var existing)) return existing;

        using (PerformanceMonitor.Measure($"[BrushSet] Created new brush ({value})", maxBeforeWarning: 1.Milliseconds(), category: PerformanceCategory.Brushes))
        {
            // Create new brush with color transition animation
            var newBrush = CreateBrush(value.Contrast ? Contrast.Color : Brush.Color, value.Opacity ?? 1.0, value.Darken, value.Lighten);
            _brushes[value] = newBrush;

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
        foreach (var brush in _brushes)
        {
            brush.Value.Color = CreateColor(brush.Key.Contrast ? contrastedColor : newColor, brush.Key.Darken, brush.Key.Lighten);
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

        PerformanceMonitor.Debug($"[BrushSet] Enabled transitions for {_brushes.Count + 2} brushes", PerformanceCategory.Brushes);
    }

    /// <summary>
    /// Creates a new <see cref="SolidColorBrush"/> with the current color, specified opacity, and color transition animation.
    /// Transitions are NOT added at creation time for performance - they're added lazily when theme changes.
    /// </summary>
    /// <param name="color">Color of the brush.</param>
    /// <param name="opacity">The opacity value for the brush (optional; defaults to 1.0).</param>
    /// <param name="darken">The amount to darken the color (optional).</param>
    /// <param name="lighten">The amount to lighten the color (optional).</param>
    /// <returns>A new <see cref="SolidColorBrush"/> instance.</returns>
    private SolidColorBrush CreateBrush(Color color, double opacity = 1.0, double? darken = null, double? lighten = null)
    {
        color = CreateColor(color, darken, lighten);

        var brush = new SolidColorBrush(color) { Opacity = opacity };

        // Only add transitions if already enabled (during theme change)
        if (_transitionsEnabled && _colorTransition is not null)
        {
            brush.Transitions = [_colorTransition];
        }

        return brush;
    }

    /// <summary>
    /// Creates a new color by applying optional darkening or lightening adjustments to the specified color.
    /// </summary>
    /// <remarks>If both <paramref name="darken"/> and <paramref name="lighten"/> are null, the method returns
    /// the input color unchanged.</remarks>
    /// <param name="color">The base color to modify.</param>
    /// <param name="darken">An optional value specifying the amount to darken the color. If provided, the color is darkened by this amount.</param>
    /// <param name="lighten">An optional value specifying the amount to lighten the color. If provided, the color is lightened by this
    /// amount.</param>
    /// <returns>A new color instance with the specified darkening or lightening applied. If neither adjustment is specified, the
    /// original color is returned.</returns>
    private static Color CreateColor(Color color, double? darken, double? lighten)
    {
        if (darken.HasValue || lighten.HasValue)
            color = color.Apply(new(null, false, darken, lighten));
        return color;
    }
}
