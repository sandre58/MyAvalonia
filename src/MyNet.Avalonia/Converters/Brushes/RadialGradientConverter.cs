// -----------------------------------------------------------------------
// <copyright file="RadialGradientConverter.cs" company="Stéphane ANDRE">
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
/// Provides value conversion logic for creating radial gradient brushes from a base color in Avalonia UI.
/// Supports conversion from a base color to a <see cref="RadialGradientBrush"/> with customizable center, origin, radius and color variations.
/// </summary>
/// <remarks>
/// Pass a <see cref="RadialGradientParameters"/> instance as the converter parameter to control center, radii,
/// and optional middle color stops.
/// </remarks>
public sealed class RadialGradientConverter : IValueConverter, IMultiValueConverter
{
    /// <summary>
    /// Gets the default singleton instance of <see cref="RadialGradientConverter"/>.
    /// </summary>
    public static readonly RadialGradientConverter Default = new();

    /// <inheritdoc/>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => Convert([value], targetType, parameter, culture);

    /// <inheritdoc/>
    public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values.Count == 0)
            return AvaloniaProperty.UnsetValue;

        var parameters = parameter as RadialGradientParameters ?? new RadialGradientParameters();
        var baseColor = BrushConversionHelper.TryExtractColor(values);

        return !baseColor.HasValue ? AvaloniaProperty.UnsetValue : CreateRadialGradient(baseColor.Value, parameters);
    }

    /// <inheritdoc/>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => AvaloniaProperty.UnsetValue;

    /// <summary>
    /// Creates a <see cref="RadialGradientBrush"/> based on the provided base color and gradient parameters.
    /// </summary>
    /// <param name="baseColor">The base color for the gradient.</param>
    /// <param name="parameters">The gradient parameters to apply.</param>
    /// <returns>A <see cref="RadialGradientBrush"/> configured with the specified parameters.</returns>
    private static RadialGradientBrush CreateRadialGradient(Color baseColor, RadialGradientParameters parameters)
    {
        var (startColor, endColor) = BrushConversionHelper.CreateGradientEndColors(baseColor, parameters.StartLighten, parameters.EndDarken);

        var gradient = new RadialGradientBrush
        {
            Center = parameters.Center ?? new RelativePoint(0.5, 0.5, RelativeUnit.Relative),
            GradientOrigin = parameters.GradientOrigin ?? new RelativePoint(0.5, 0.5, RelativeUnit.Relative),
            RadiusX = parameters.RadiusX ?? new RelativeScalar(0.5, RelativeUnit.Relative),
            RadiusY = parameters.RadiusY ?? new RelativeScalar(0.5, RelativeUnit.Relative),
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
/// Describes parameters for creating a radial gradient brush from a base color.
/// </summary>
/// <param name="Center">The center point for the radial gradient.</param>
/// <param name="GradientOrigin">The origin point for the radial gradient.</param>
/// <param name="RadiusX">The horizontal radius for the radial gradient.</param>
/// <param name="RadiusY">The vertical radius for the radial gradient.</param>
/// <param name="StartLighten">Optional lighten factor for the start color (0.0 to 1.0).</param>
/// <param name="EndDarken">Optional darken factor for the end color (0.0 to 1.0).</param>
/// <param name="MiddleColor">Optional middle color for three-stop gradients.</param>
/// <param name="MiddleLighten">Optional lighten factor for the middle color (0.0 to 1.0).</param>
/// <param name="MiddleDarken">Optional darken factor for the middle color (0.0 to 1.0).</param>
/// <param name="MiddleOffset">The offset for the middle gradient stop (0.0 to 1.0, default 0.5).</param>
public record RadialGradientParameters(
    RelativePoint? Center = null,
    RelativePoint? GradientOrigin = null,
    RelativeScalar? RadiusX = null,
    RelativeScalar? RadiusY = null,
    double? StartLighten = null,
    double? EndDarken = null,
    Color? MiddleColor = null,
    double? MiddleLighten = null,
    double? MiddleDarken = null,
    double MiddleOffset = 0.5);
