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
using MyNet.Avalonia.Converters.Brushes;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Converters;
#pragma warning restore IDE0130

/// <summary>
/// Provides value conversion logic for creating linear gradient brushes from a base color in Avalonia UI.
/// Supports conversion from a base color to a <see cref="LinearGradientBrush"/> with customizable orientation and color variations.
/// </summary>
/// <remarks>
/// Pass a <see cref="LinearGradientParameters"/> instance as the converter parameter to control orientation,
/// lighten/darken factors, and optional middle stops.
/// </remarks>
public sealed class LinearGradientConverter : IValueConverter, IMultiValueConverter
{
    /// <summary>
    /// Gets the default singleton instance of <see cref="LinearGradientConverter"/>.
    /// </summary>
    public static readonly LinearGradientConverter Default = new();

    /// <inheritdoc/>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => Convert([value], targetType, parameter, culture);

    /// <inheritdoc/>
    public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values.Count == 0)
            return AvaloniaProperty.UnsetValue;

        var parameters = parameter as LinearGradientParameters ?? new LinearGradientParameters();
        var baseColor = BrushConversionHelper.TryExtractColor(values);

        return !baseColor.HasValue ? AvaloniaProperty.UnsetValue : CreateLinearGradient(baseColor.Value, parameters);
    }

    /// <inheritdoc/>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => AvaloniaProperty.UnsetValue;

    /// <summary>
    /// Creates a <see cref="LinearGradientBrush"/> based on the provided base color and gradient parameters.
    /// </summary>
    /// <param name="baseColor">The base color for the gradient.</param>
    /// <param name="parameters">The gradient parameters to apply.</param>
    /// <returns>A <see cref="LinearGradientBrush"/> configured with the specified parameters.</returns>
    private static LinearGradientBrush CreateLinearGradient(Color baseColor, LinearGradientParameters parameters)
    {
        var (startColor, endColor) = BrushConversionHelper.CreateGradientEndColors(baseColor, parameters.StartLighten, parameters.EndDarken);

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

        BrushConversionHelper.InsertMiddleStopIfNeeded(
            gradient.GradientStops,
            baseColor,
            parameters.MiddleColor,
            parameters.MiddleLighten,
            parameters.MiddleDarken,
            parameters.MiddleOffset);

        return gradient;
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
