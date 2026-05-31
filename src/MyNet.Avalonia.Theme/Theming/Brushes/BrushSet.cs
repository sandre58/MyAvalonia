// -----------------------------------------------------------------------
// <copyright file="BrushSet.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
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
    private readonly BrushInterpolationLruCache _transformedBrushes;
    private bool _transitionsEnabled;

    /// <summary>
    /// Initializes a new instance of the <see cref="BrushSet"/> class with the specified color, transition duration, and easing.
    /// </summary>
    /// <param name="color">The base color for the brush set.</param>
    /// <param name="contrastedColor">The color to use for the contrast brush (optional; defaults to the contrasting color of the current color).</param>
    /// <param name="colorTransitionDuration">The duration of the color transition animation.</param>
    /// <param name="colorTransitionEasing">The easing function for the color transition animation.</param>
    /// <param name="transformedBrushCapacity">Maximum transformed brushes retained in the LRU cache.</param>
    /// <param name="onTransformedBrushEvicted">Optional callback when a transformed brush is evicted from the cache.</param>
    public BrushSet(
        Color color,
        Color contrastedColor,
        TimeSpan? colorTransitionDuration = null,
        Easing? colorTransitionEasing = null,
        int? transformedBrushCapacity = null,
        Action<ISolidColorBrush>? onTransformedBrushEvicted = null)
    {
        _transformedBrushes = new(
            transformedBrushCapacity ?? BrushSetOptions.TransformedBrushCapacity,
            onTransformedBrushEvicted);

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
    /// Gets the number of transformed brushes currently held in the LRU cache (excludes <see cref="Brush"/> and <see cref="Contrast"/>).
    /// </summary>
    internal int TransformedBrushCacheCount => _transformedBrushes.Count;

    /// <summary>
    /// Gets a transformed brush based on the specified color interpolation value.
    /// </summary>
    /// <param name="value">The color interpolation value.</param>
    /// <returns>A <see cref="SolidColorBrush"/> with the specified transformation.</returns>
    public SolidColorBrush GetTransformedBrush(ColorInterpolation value)
    {
        if (_transformedBrushes.TryGet(value, out var existing))
            return existing;

        using (PerformanceMonitor.Measure($"[BrushSet] Created new brush ({value})", maxBeforeWarning: 1.Milliseconds(), category: PerformanceCategory.Brushes))
        {
            var newBrush = CreateBrush(value.Contrast ? Contrast.Color : Brush.Color, value.Opacity ?? 1.0, value.Darken, value.Lighten);
            _transformedBrushes.Set(value, newBrush);
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
        if (!_transitionsEnabled)
            EnableTransitions();

        Brush.Color = newColor;
        Contrast.Color = contrastedColor;

        foreach (var (interpolation, brush) in _transformedBrushes.EnumerateEntries())
        {
            brush.Color = CreateColor(
                interpolation.Contrast ? contrastedColor : newColor,
                interpolation.Darken,
                interpolation.Lighten);
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

        Brush.Transitions = [_colorTransition];
        Contrast.Transitions = [_colorTransition];

        foreach (var (_, brush) in _transformedBrushes.EnumerateEntries())
            brush.Transitions = [_colorTransition];

        PerformanceMonitor.Debug($"[BrushSet] Enabled transitions for {_transformedBrushes.Count + 2} brushes", PerformanceCategory.Brushes);
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

        if (_transitionsEnabled && _colorTransition is not null)
            brush.Transitions = [_colorTransition];

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
