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
using MyNet.Avalonia.Converters.Brushes;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Converters;
#pragma warning restore IDE0130

/// <summary>
/// Provides value conversion logic for creating a tiled dotted grid <see cref="DrawingBrush"/> from a base brush or color in Avalonia UI.
/// Supports customizable tile size, dot radius and opacity.
/// </summary>
/// <remarks>
/// Pass a <see cref="DottedGridParameters"/> instance as the converter parameter to control tile size, dot radius, and opacity.
/// </remarks>
public sealed class DottedGridBrushConverter : IValueConverter, IMultiValueConverter
{
    /// <summary>
    /// Gets the default singleton instance of <see cref="DottedGridBrushConverter"/>.
    /// </summary>
    public static readonly DottedGridBrushConverter Default = new();

    /// <inheritdoc/>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => Convert([value], targetType, parameter, culture);

    /// <inheritdoc/>
    public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values.Count == 0)
            return AvaloniaProperty.UnsetValue;

        var parameters = parameter as DottedGridParameters ?? new DottedGridParameters();
        var brush = BrushConversionHelper.TryExtractBrush(values);

        return brush is null ? AvaloniaProperty.UnsetValue : CreateDottedGrid(brush, parameters);
    }

    /// <inheritdoc/>
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

        return new()
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
                    Center = new(center, center),
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
public record DottedGridParameters(double TileSize = 16.0, double DotRadius = 0.5, double Opacity = 1.0);
