// -----------------------------------------------------------------------
// <copyright file="BrushSet.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Media;
using MyNet.Avalonia.Extensions;
using MyNet.Utilities;

namespace MyNet.Avalonia.Theme.Palettes;

/// <summary>
/// Represents a set of brushes for a single color, supporting animated color transitions and multiple opacity levels.
/// Provides access to the main brush, a contrast brush for accessibility, and brushes with custom opacity, all synchronized to the current color.
/// </summary>
public class BrushSet
{
    private readonly ColorTransition _colorTransition;
    private readonly Dictionary<double, SolidColorBrush> _brushes = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="BrushSet"/> class with the specified color, transition duration, and easing.
    /// </summary>
    /// <param name="color">The base color for the brush set.</param>
    /// <param name="contrastedColor">The color to use for the contrast brush (optional; defaults to the contrasting color of the current color).</param>
    /// <param name="colorTransitionDuration">The duration of the color transition animation.</param>
    /// <param name="colorTransitionEasing">The easing function for the color transition animation.</param>
    public BrushSet(Color color, Color? contrastedColor, TimeSpan colorTransitionDuration, Easing colorTransitionEasing)
    {
        _colorTransition = new ColorTransition
        {
            Duration = colorTransitionDuration,
            Easing = colorTransitionEasing,
            Property = SolidColorBrush.ColorProperty
        };

        Color = color;
        Brush = CreateBrush();
        Contrast = CreateContrastBrush(contrastedColor);
    }

    /// <summary>
    /// Gets the current color of the brush set.
    /// </summary>
    public Color Color { get; private set; }

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
        if (_brushes.TryGetValue(opacity, out var existing)) return existing;

        // Create new brush with color transition animation
        var newBrush = CreateBrush(opacity);

        _brushes.AddOrUpdate(opacity, newBrush);

        return newBrush;
    }

    /// <summary>
    /// Updates the brush set to use a new color. All brushes managed by this set will be updated to the new color.
    /// </summary>
    /// <param name="newColor">The new color to apply to the brush set.</param>
    /// <param name="contrastedColor">The color to use for the contrast brush (optional; defaults to the contrasting color of the current color).</param>
    public void UpdateColor(Color newColor, Color? contrastedColor = null)
    {
        if (Color != newColor)
        {
            Color = newColor;
            Brush.Color = newColor;
            Contrast.Color = contrastedColor ?? newColor.ContrastingForegroundColor();

            // Update all opacity brushes
            foreach (var brush in _brushes.Values)
            {
                brush.Color = newColor;
            }
        }
    }

    /// <summary>
    /// Creates a new <see cref="SolidColorBrush"/> with the current color, specified opacity, and color transition animation.
    /// </summary>
    /// <param name="opacity">The opacity value for the brush (optional; defaults to 1.0).</param>
    /// <returns>A new <see cref="SolidColorBrush"/> instance.</returns>
    private SolidColorBrush CreateBrush(double? opacity = null) => new(Color) { Opacity = opacity ?? 1.0, Transitions = [_colorTransition] };

    /// <summary>
    /// Creates a new <see cref="SolidColorBrush"/> with a color that contrasts with the current color, for accessibility purposes.
    /// </summary>
    /// <param name="contrastedColor">The color to use for the contrast brush (optional; defaults to the contrasting color of the current color).</param>
    /// <param name="opacity">The opacity value for the contrast brush (optional; defaults to 1.0).</param>
    /// <returns>A new <see cref="SolidColorBrush"/> instance with a contrasting color.</returns>
    private SolidColorBrush CreateContrastBrush(Color? contrastedColor = null, double? opacity = null) => new(contrastedColor ?? Color.FromArgb(Convert.ToByte(255 * opacity, CultureInfo.InvariantCulture), Color.R, Color.G, Color.B).ContrastingForegroundColor()) { Opacity = opacity ?? 1.0, Transitions = [_colorTransition] };
}
