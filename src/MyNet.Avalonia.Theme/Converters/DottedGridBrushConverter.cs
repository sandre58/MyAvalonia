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

namespace MyNet.Avalonia.Theme.Converters;

/// <summary>
/// Creates a tiled dotted grid <see cref="DrawingBrush"/> from a base brush or color.
/// </summary>
public sealed class DottedGridBrushConverter : IValueConverter, IMultiValueConverter
{
    /// <summary>
    /// Gets the default singleton instance.
    /// </summary>
    public static readonly DottedGridBrushConverter Default = new();

    /// <inheritdoc/>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => Convert([value], targetType, parameter, culture);

    /// <inheritdoc/>
    public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values.Count == 0)
            return AvaloniaProperty.UnsetValue;

        var parameters = parameter as DottedGridParameters ?? new DottedGridParameters();
        var brush = TryExtractBrush(values);

        return brush is null ? AvaloniaProperty.UnsetValue : CreateDottedGrid(brush, parameters);
    }

    /// <inheritdoc/>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => AvaloniaProperty.UnsetValue;

    private static IBrush? TryExtractBrush(IEnumerable<object?> values)
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
/// Parameters for <see cref="DottedGridBrushConverter"/>.
/// </summary>
/// <param name="TileSize">Tile width and height in pixels.</param>
/// <param name="DotRadius">Dot ellipse radius in pixels.</param>
/// <param name="Opacity">Overall brush opacity.</param>
public record DottedGridParameters(double TileSize = 16.0, double DotRadius = 0.5, double Opacity = 1.0);
