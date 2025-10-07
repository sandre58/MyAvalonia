// -----------------------------------------------------------------------
// <copyright file="ThicknessConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace MyNet.Avalonia.Converters;

public sealed class ThicknessConverter : IValueConverter
{
    private enum Side
    {
        Left,
        Top,
        Right,
        Bottom,
        RemoveLeft,
        RemoveRight,
        RemoveBottom,
        RemoveTop
    }

    private readonly Side _side;

    public static readonly ThicknessConverter Left = new(Side.Left);
    public static readonly ThicknessConverter Top = new(Side.Top);
    public static readonly ThicknessConverter Right = new(Side.Right);
    public static readonly ThicknessConverter Bottom = new(Side.Bottom);
    public static readonly ThicknessConverter RemoveLeft = new(Side.RemoveLeft);
    public static readonly ThicknessConverter RemoveTop = new(Side.RemoveTop);
    public static readonly ThicknessConverter RemoveRight = new(Side.RemoveRight);
    public static readonly ThicknessConverter RemoveBottom = new(Side.RemoveBottom);

    private ThicknessConverter(Side side) => _side = side;

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => value is Thickness thickness
            ? _side switch
            {
                Side.Left => new Thickness(thickness.Left),
                Side.Top => new Thickness(thickness.Top),
                Side.Right => new Thickness(thickness.Right),
                Side.Bottom => new Thickness(thickness.Bottom),
                Side.RemoveLeft => new Thickness(0, thickness.Top, thickness.Right, thickness.Bottom),
                Side.RemoveTop => new Thickness(thickness.Left, 0, thickness.Right, thickness.Bottom),
                Side.RemoveRight => new Thickness(thickness.Left, thickness.Top, 0, thickness.Bottom),
                Side.RemoveBottom => new Thickness(thickness.Left, thickness.Top, thickness.Right, 0),
                _ => default
            }
            : 0;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => AvaloniaProperty.UnsetValue;
}
