// -----------------------------------------------------------------------
// <copyright file="LinearGradientConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace MyNet.Avalonia.Converters;

/// <summary>
/// Provides value conversion logic for creating linear gradient brushes from a base color in Avalonia UI.
/// Supports conversion from a base color to a <see cref="LinearGradientBrush"/> with customizable orientation and color variations.
/// </summary>
public sealed class LinearGradientConverter : IValueConverter, IMultiValueConverter
{
    /// <summary>
    /// Gets the default singleton instance of <see cref="LinearGradientConverter"/>.
    /// </summary>
    public static readonly LinearGradientConverter Default = new();

    /// <summary>
    /// Converts a value to a linear gradient brush based on the specified parameters.
    /// </summary>
    /// <param name="value">The input value, typically a <see cref="Color"/> or <see cref="IBrush"/>.</param>
    /// <param name="targetType">The target type for the conversion (usually <see cref="IBrush"/>).</param>
    /// <param name="parameter">A parameter describing the gradient options to apply.</param>
    /// <param name="culture">The culture for conversion (not used).</param>
    /// <returns>The resolved <see cref="LinearGradientBrush"/> or <see cref="AvaloniaProperty.UnsetValue"/> if conversion fails.</returns>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => Convert([value], targetType, parameter, culture);

    /// <summary>
    /// Converts multiple values to a linear gradient brush based on the specified parameters.
    /// </summary>
    /// <param name="values">The list of values to convert, typically including a color or brush.</param>
    /// <param name="targetType">The target type for the conversion (usually <see cref="IBrush"/>).</param>
    /// <param name="parameter">A parameter describing the gradient options to apply.</param>
    /// <param name="culture">The culture for conversion (not used).</param>
    /// <returns>The resolved <see cref="LinearGradientBrush"/> or <see cref="AvaloniaProperty.UnsetValue"/> if conversion fails.</returns>
    public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values.Count == 0) return AvaloniaProperty.UnsetValue;

        var parameters = parameter as LinearGradientParameters ?? new LinearGradientParameters();
        Color? baseColor = null;

        foreach (var value in values)
        {
            switch (value)
            {
                case Color color:
                    baseColor = color;
                    break;
                case SolidColorBrush brush:
                    // GetBaseValue bypasses the active ColorTransition animation.
                    // GetValue(ColorProperty) returns oldColor at t=0 of the animation;
                    // GetBaseValue returns the target color that was locally set.
                    var bv = brush.GetBaseValue(SolidColorBrush.ColorProperty);
                    baseColor = bv.HasValue ? bv.Value : brush.Color;
                    break;
                case ISolidColorBrush solidBrush:
                    baseColor = solidBrush.Color;
                    break;
            }

            if (baseColor.HasValue)
                break;
        }

        return !baseColor.HasValue ? AvaloniaProperty.UnsetValue : CreateLinearGradient(baseColor.Value, parameters);
    }

    /// <summary>
    /// Not supported. Always returns <see cref="AvaloniaProperty.UnsetValue"/>.
    /// </summary>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => AvaloniaProperty.UnsetValue;

    /// <summary>
    /// Creates a <see cref="LinearGradientBrush"/> based on the provided base color and gradient parameters.
    /// </summary>
    /// <param name="baseColor">The base color for the gradient.</param>
    /// <param name="parameters">The gradient parameters to apply.</param>
    /// <returns>A <see cref="LinearGradientBrush"/> configured with the specified parameters.</returns>
    private static LinearGradientBrush CreateLinearGradient(Color baseColor, LinearGradientParameters parameters)
    {
        var startColor = AdjustBrightness(baseColor, parameters.StartLighten ?? 0.0);
        var endColor = AdjustBrightness(baseColor, -parameters.EndDarken ?? 0.0);

        var gradient = new LinearGradientBrush
        {
            StartPoint = parameters.Orientation switch
            {
                GradientOrientation.Horizontal => new(0, 0.5, RelativeUnit.Relative),
                GradientOrientation.Vertical => new(0.5, 0, RelativeUnit.Relative),
                GradientOrientation.DiagonalUp => new(0, 1, RelativeUnit.Relative),
                GradientOrientation.DiagonalDown => new(0, 0, RelativeUnit.Relative),
                _ => new(0, 0.5, RelativeUnit.Relative)
            },
            EndPoint = parameters.Orientation switch
            {
                GradientOrientation.Horizontal => new(1, 0.5, RelativeUnit.Relative),
                GradientOrientation.Vertical => new(0.5, 1, RelativeUnit.Relative),
                GradientOrientation.DiagonalUp => new(1, 0, RelativeUnit.Relative),
                GradientOrientation.DiagonalDown => new(1, 1, RelativeUnit.Relative),
                _ => new(1, 0.5, RelativeUnit.Relative)
            },
            GradientStops =
            [
                new() { Color = startColor, Offset = 0.0 },
                new() { Color = endColor, Offset = 1.0 }
            ]
        };

        if (parameters.MiddleColor.HasValue || parameters.MiddleLighten.HasValue || parameters.MiddleDarken.HasValue)
        {
            var middleColor = parameters.MiddleColor ?? AdjustBrightness(
                baseColor,
                parameters.MiddleLighten ?? (-parameters.MiddleDarken ?? 0.0));

            gradient.GradientStops.Insert(1, new() { Color = middleColor, Offset = parameters.MiddleOffset });
        }

        return gradient;
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
    private static Color AdjustBrightness(Color color, double factor)
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
        else
        {
            var absFactor = Math.Abs(factor);
            var r = (byte)(color.R * (1 - absFactor));
            var g = (byte)(color.G * (1 - absFactor));
            var b = (byte)(color.B * (1 - absFactor));
            return Color.FromArgb(color.A, r, g, b);
        }
    }
}

/// <summary>
/// Describes parameters for creating a linear gradient brush from a base color.
/// </summary>
/// <param name="Orientation">The orientation of the gradient (horizontal, vertical, or diagonal).</param>
/// <param name="StartLighten">Optional lighten factor for the start color (0.0 to 1.0).</param>
/// <param name="EndDarken">Optional darken factor for the end color (0.0 to 1.0).</param>
/// <param name="MiddleColor">Optional middle color for three-stop gradients.</param>
/// <param name="MiddleLighten">Optional lighten factor for the middle color (0.0 to 1.0).</param>
/// <param name="MiddleDarken">Optional darken factor for the middle color (0.0 to 1.0).</param>
/// <param name="MiddleOffset">The offset for the middle gradient stop (0.0 to 1.0, default 0.5).</param>
public record LinearGradientParameters(
    GradientOrientation Orientation = GradientOrientation.Horizontal,
    double? StartLighten = null,
    double? EndDarken = null,
    Color? MiddleColor = null,
    double? MiddleLighten = null,
    double? MiddleDarken = null,
    double MiddleOffset = 0.5);

/// <summary>
/// Defines the orientation of a linear gradient brush.
/// </summary>
public enum GradientOrientation
{
    /// <summary>
    /// Horizontal gradient from left to right.
    /// </summary>
    Horizontal,

    /// <summary>
    /// Vertical gradient from top to bottom.
    /// </summary>
    Vertical,

    /// <summary>
    /// Diagonal gradient from bottom-left to top-right.
    /// </summary>
    DiagonalUp,

    /// <summary>
    /// Diagonal gradient from top-left to bottom-right.
    /// </summary>
    DiagonalDown
}
