// -----------------------------------------------------------------------
// <copyright file="DottedGridBrushConverter.cs" company="Stéphane ANDRE">
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
/// Provides value conversion logic for creating a tiled dotted grid <see cref="DrawingBrush"/> from a base brush or color in Avalonia UI.
/// Supports customizable tile size, dot radius and opacity.
/// </summary>
public sealed class DottedGridBrushConverter : IValueConverter, IMultiValueConverter
{
    /// <summary>
    /// Gets the default singleton instance of <see cref="DottedGridBrushConverter"/>.
    /// </summary>
    public static readonly DottedGridBrushConverter Default = new();

    /// <summary>
    /// Converts a value to a dotted grid drawing brush based on the specified parameters.
    /// </summary>
    /// <param name="value">The input value, typically an <see cref="IBrush"/> or <see cref="Color"/>.</param>
    /// <param name="targetType">The target type for the conversion (usually <see cref="IBrush"/>).</param>
    /// <param name="parameter">A <see cref="DottedGridParameters"/> instance describing the pattern options to apply.</param>
    /// <param name="culture">The culture for conversion (not used).</param>
    /// <returns>The resolved <see cref="DrawingBrush"/> or <see cref="AvaloniaProperty.UnsetValue"/> if conversion fails.</returns>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => Convert([value], targetType, parameter, culture);

    /// <summary>
    /// Converts multiple values to a dotted grid drawing brush based on the specified parameters.
    /// </summary>
    /// <param name="values">The list of values to convert, typically including a brush or color.</param>
    /// <param name="targetType">The target type for the conversion (usually <see cref="IBrush"/>).</param>
    /// <param name="parameter">A <see cref="DottedGridParameters"/> instance describing the pattern options to apply.</param>
    /// <param name="culture">The culture for conversion (not used).</param>
    /// <returns>The resolved <see cref="DrawingBrush"/> or <see cref="AvaloniaProperty.UnsetValue"/> if conversion fails.</returns>
    public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values.Count == 0) return AvaloniaProperty.UnsetValue;

        var parameters = parameter as DottedGridParameters ?? new DottedGridParameters();
        IBrush? brush = null;

        foreach (var value in values)
        {
            switch (value)
            {
                case SolidColorBrush scb:
                    // GetBaseValue bypasses the active ColorTransition animation.
                    // GetValue(ColorProperty) returns oldColor at t=0 of the animation;
                    // GetBaseValue returns the target color that was locally set.
                    var scbBase = scb.GetBaseValue(SolidColorBrush.ColorProperty);
                    brush = new SolidColorBrush(scbBase.HasValue ? scbBase.Value : scb.Color);
                    break;
                case ISolidColorBrush solidColorBrush:
                    brush = new SolidColorBrush(solidColorBrush.Color);
                    break;
                case IBrush b:
                    brush = b;
                    break;
                case Color c:
                    brush = new SolidColorBrush(c);
                    break;
            }

            if (brush is not null)
                break;
        }

        return brush is null ? AvaloniaProperty.UnsetValue : CreateDottedGrid(brush, parameters);
    }

    /// <summary>
    /// Not supported. Always returns <see cref="AvaloniaProperty.UnsetValue"/>.
    /// </summary>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => AvaloniaProperty.UnsetValue;

    /// <summary>
    /// Creates a tiled <see cref="DrawingBrush"/> with a single dot per tile based on the provided brush and parameters.
    /// </summary>
    /// <param name="brush">The brush used to paint the dots.</param>
    /// <param name="parameters">The pattern parameters to apply.</param>
    /// <returns>A <see cref="DrawingBrush"/> configured with the specified parameters.</returns>
    private static DrawingBrush CreateDottedGrid(IBrush brush, DottedGridParameters parameters)
    {
        var tileSize = parameters.TileSize;
        var center = tileSize / 4.0;
        var rect = new RelativeRect(0, 0, tileSize, tileSize, RelativeUnit.Absolute);

        return new DrawingBrush
        {
            TileMode = TileMode.Tile,
            SourceRect = rect,
            DestinationRect = rect,
            Opacity = parameters.Opacity,
            Drawing = new GeometryDrawing
            {
                Brush = brush,
                Geometry = new EllipseGeometry
                {
                    Center = new Point(center, center),
                    RadiusX = parameters.DotRadius,
                    RadiusY = parameters.DotRadius
                }
            }
        };
    }
}

/// <summary>
/// Describes parameters for creating a tiled dotted grid drawing brush from a base brush or color.
/// </summary>
/// <param name="TileSize">The width and height of each tile in pixels (default: 16).</param>
/// <param name="DotRadius">The radius of the dot ellipse in pixels (default: 0.5).</param>
/// <param name="Opacity">The overall opacity of the drawing brush (default: 1.0).</param>
public record DottedGridParameters(
    double TileSize = 16.0,
    double DotRadius = 0.5,
    double Opacity = 1.0);
