// -----------------------------------------------------------------------
// <copyright file="CornerRadiusConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace MyNet.Avalonia.Converters;

public sealed class CornerRadiusConverter : IValueConverter
{
    private enum Side
    {
        Left,
        Top,
        Right,
        Bottom
    }

    private readonly Side _side;

    public static readonly CornerRadiusConverter Left = new(Side.Left);
    public static readonly CornerRadiusConverter Top = new(Side.Top);
    public static readonly CornerRadiusConverter Right = new(Side.Right);
    public static readonly CornerRadiusConverter Bottom = new(Side.Bottom);

    private CornerRadiusConverter(Side side) => _side = side;

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => value is CornerRadius cornerRadius
            ? _side switch
            {
                Side.Left => new CornerRadius(cornerRadius.TopLeft, 0, 0, cornerRadius.BottomLeft),
                Side.Top => new CornerRadius(cornerRadius.TopLeft, cornerRadius.TopRight, 0, 0),
                Side.Right => new CornerRadius(0, cornerRadius.TopRight, cornerRadius.BottomRight, 0),
                Side.Bottom => new CornerRadius(0, 0, cornerRadius.BottomRight, cornerRadius.BottomLeft),
                _ => default
            }
            : 0;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => AvaloniaProperty.UnsetValue;
}
