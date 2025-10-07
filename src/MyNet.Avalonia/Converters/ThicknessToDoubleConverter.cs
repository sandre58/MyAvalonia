// -----------------------------------------------------------------------
// <copyright file="ThicknessToDoubleConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace MyNet.Avalonia.Converters;

public sealed class ThicknessToDoubleConverter : IValueConverter
{
    private enum Side
    {
        Left,
        Top,
        Right,
        Bottom
    }

    private readonly Side _side;

    public static readonly ThicknessToDoubleConverter Left = new(Side.Left);
    public static readonly ThicknessToDoubleConverter Top = new(Side.Top);
    public static readonly ThicknessToDoubleConverter Right = new(Side.Right);
    public static readonly ThicknessToDoubleConverter Bottom = new(Side.Bottom);

    private ThicknessToDoubleConverter(Side side) => _side = side;

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => value is Thickness thickness
            ? _side switch
            {
                Side.Left => thickness.Left,
                Side.Top => thickness.Top,
                Side.Right => thickness.Right,
                Side.Bottom => thickness.Bottom,
                _ => 0
            }
            : (object)0;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => AvaloniaProperty.UnsetValue;
}
