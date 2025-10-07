// -----------------------------------------------------------------------
// <copyright file="CornerRadiusToDoubleConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace MyNet.Avalonia.Converters;

public sealed class CornerRadiusToDoubleConverter : IValueConverter
{
    private enum Side
    {
        BottomLeft,
        BottomRight,
        TopRight,
        TopLeft
    }

    private readonly Side _side;

    public static readonly CornerRadiusToDoubleConverter BottomLeft = new(Side.BottomLeft);
    public static readonly CornerRadiusToDoubleConverter BottomRight = new(Side.BottomRight);
    public static readonly CornerRadiusToDoubleConverter TopRight = new(Side.TopRight);
    public static readonly CornerRadiusToDoubleConverter TopLeft = new(Side.TopLeft);

    private CornerRadiusToDoubleConverter(Side side) => _side = side;

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => value is CornerRadius cornerRadius
            ? _side switch
            {
                Side.BottomLeft => cornerRadius.BottomLeft,
                Side.BottomRight => cornerRadius.BottomRight,
                Side.TopRight => cornerRadius.TopRight,
                Side.TopLeft => cornerRadius.TopLeft,
                _ => 0
            }
            : (object)0;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => AvaloniaProperty.UnsetValue;
}
