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
        RemoveTop,
        AddLeftToRight,
        AddLeftToTop,
        AddLeftToBottom,
        AddRightToLeft,
        AddRightToTop,
        AddRightToBottom,
        AddBottomToTop,
        AddBottomToLeft,
        AddBottomToRight,
        AddTopToBottom,
        AddTopToLeft,
        AddTopToRight
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
    public static readonly ThicknessConverter AddLeftToRight = new(Side.AddLeftToRight);
    public static readonly ThicknessConverter AddLeftToTop = new(Side.AddLeftToTop);
    public static readonly ThicknessConverter AddLeftToBottom = new(Side.AddLeftToBottom);
    public static readonly ThicknessConverter AddRightToLeft = new(Side.AddRightToLeft);
    public static readonly ThicknessConverter AddRightToTop = new(Side.AddRightToTop);
    public static readonly ThicknessConverter AddRightToBottom = new(Side.AddRightToBottom);
    public static readonly ThicknessConverter AddBottomToTop = new(Side.AddBottomToTop);
    public static readonly ThicknessConverter AddBottomToLeft = new(Side.AddBottomToLeft);
    public static readonly ThicknessConverter AddBottomToRight = new(Side.AddBottomToRight);
    public static readonly ThicknessConverter AddTopToBottom = new(Side.AddTopToBottom);
    public static readonly ThicknessConverter AddTopToLeft = new(Side.AddTopToLeft);
    public static readonly ThicknessConverter AddTopToRight = new(Side.AddTopToRight);

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
                Side.AddLeftToRight => new Thickness(thickness.Left, thickness.Top, thickness.Right + thickness.Left, thickness.Bottom),
                Side.AddLeftToTop => new Thickness(thickness.Left, thickness.Top + thickness.Left, thickness.Right, thickness.Bottom),
                Side.AddLeftToBottom => new Thickness(thickness.Left, thickness.Top, thickness.Right, thickness.Bottom + thickness.Left),
                Side.AddRightToLeft => new Thickness(thickness.Left + thickness.Right, thickness.Top, thickness.Right, thickness.Bottom),
                Side.AddRightToTop => new Thickness(thickness.Left, thickness.Top + thickness.Right, thickness.Right, thickness.Bottom),
                Side.AddRightToBottom => new Thickness(thickness.Left, thickness.Top, thickness.Right, thickness.Bottom + thickness.Right),
                Side.AddBottomToTop => new Thickness(thickness.Left, thickness.Top + thickness.Bottom, thickness.Right, thickness.Bottom),
                Side.AddBottomToLeft => new Thickness(thickness.Left + thickness.Bottom, thickness.Top, thickness.Right, thickness.Bottom),
                Side.AddBottomToRight => new Thickness(thickness.Left, thickness.Top, thickness.Right + thickness.Bottom, thickness.Bottom),
                Side.AddTopToBottom => new Thickness(thickness.Left, thickness.Top, thickness.Right, thickness.Bottom + thickness.Top),
                Side.AddTopToLeft => new Thickness(thickness.Left + thickness.Top, thickness.Top, thickness.Right, thickness.Bottom),
                Side.AddTopToRight => new Thickness(thickness.Left, thickness.Top, thickness.Right + thickness.Top, thickness.Bottom),
                _ => default
            }
            : 0;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => AvaloniaProperty.UnsetValue;
}
