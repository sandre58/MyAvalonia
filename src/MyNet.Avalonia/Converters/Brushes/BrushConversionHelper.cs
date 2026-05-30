// -----------------------------------------------------------------------
// <copyright file="BrushConversionHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Avalonia.Media;

namespace MyNet.Avalonia.Converters.Brushes;

/// <summary>
/// Shared helpers for brush and color extraction used by gradient and pattern converters.
/// </summary>
internal static class BrushConversionHelper
{
    /// <summary>
    /// Extracts the first available <see cref="Color"/> from the provided values.
    /// </summary>
    internal static Color? TryExtractColor(IEnumerable<object?> values)
    {
        foreach (var value in values)
        {
            switch (value)
            {
                case Color color:
                    return color;
                case SolidColorBrush brush:
                    // GetBaseValue bypasses the active ColorTransition animation.
                    // GetValue(ColorProperty) returns oldColor at t=0 of the animation;
                    // GetBaseValue returns the target color that was locally set.
                    var baseValue = brush.GetBaseValue(SolidColorBrush.ColorProperty);
                    return baseValue.HasValue ? baseValue.Value : brush.Color;
                case ISolidColorBrush solidBrush:
                    return solidBrush.Color;
            }
        }

        return null;
    }

    /// <summary>
    /// Extracts the first available <see cref="IBrush"/> from the provided values.
    /// </summary>
    internal static IBrush? TryExtractBrush(IEnumerable<object?> values)
    {
        foreach (var value in values)
        {
            switch (value)
            {
                case SolidColorBrush scb:
                    var scbBase = scb.GetBaseValue(SolidColorBrush.ColorProperty);
                    return new SolidColorBrush(scbBase.HasValue ? scbBase.Value : scb.Color);
                case ISolidColorBrush solidColorBrush:
                    return new SolidColorBrush(solidColorBrush.Color);
                case IBrush brush:
                    return brush;
                case Color color:
                    return new SolidColorBrush(color);
            }
        }

        return null;
    }

    /// <summary>
    /// Creates the start and end colors for a two-stop gradient from a base color.
    /// </summary>
    internal static (Color Start, Color End) CreateGradientEndColors(Color baseColor, double? startLighten, double? endDarken) =>
        (AdjustBrightness(baseColor, startLighten ?? 0.0), AdjustBrightness(baseColor, -(endDarken ?? 0.0)));

    /// <summary>
    /// Inserts a middle gradient stop when middle color parameters are provided.
    /// </summary>
    internal static void InsertMiddleStopIfNeeded(
        GradientStops stops,
        Color baseColor,
        Color? middleColor,
        double? middleLighten,
        double? middleDarken,
        double middleOffset)
    {
        if (!middleColor.HasValue && !middleLighten.HasValue && !middleDarken.HasValue)
            return;

        var computedMiddle = middleColor ?? AdjustBrightness(
            baseColor,
            middleLighten ?? (-middleDarken ?? 0.0));

        stops.Insert(1, new GradientStop { Color = computedMiddle, Offset = middleOffset });
    }

    /// <summary>
    /// Adjusts the brightness of the specified color by the given factor.
    /// </summary>
    /// <remarks>The alpha component of the color remains unchanged. The adjustment is applied to the red,
    /// green, and blue components. The method ensures that component values remain within the valid byte range
    /// (0–255).</remarks>
    /// <param name="color">The color whose brightness is to be adjusted.</param>
    /// <param name="factor">A value indicating the degree of brightness adjustment. Positive values increase brightness, negative values
    /// decrease brightness. Values should typically be between -1.0 and 1.0. Values close to zero result in minimal
    /// change.</param>
    /// <returns>A new Color instance with the adjusted brightness. If the factor is near zero, the original color is returned.</returns>
    internal static Color AdjustBrightness(Color color, double factor)
    {
        if (Math.Abs(factor) < 0.001)
            return color;

        if (factor > 0)
        {
            var r = color.R + (byte)((255 - color.R) * factor);
            var g = color.G + (byte)((255 - color.G) * factor);
            var b = color.B + (byte)((255 - color.B) * factor);
            return Color.FromArgb(color.A, (byte)r, (byte)g, (byte)b);
        }

        var absFactor = Math.Abs(factor);
        return Color.FromArgb(
            color.A,
            (byte)(color.R * (1 - absFactor)),
            (byte)(color.G * (1 - absFactor)),
            (byte)(color.B * (1 - absFactor)));
    }
}
