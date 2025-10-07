// -----------------------------------------------------------------------
// <copyright file="DoubleToThicknessConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace MyNet.Avalonia.Converters;

public sealed class DoubleToThicknessConverter : IValueConverter
{
    private enum Mode
    {
        All,

        Top,

        Left,

        Right,

        Bottom,

        Horizontal,

        Vertical
    }

    public static readonly DoubleToThicknessConverter All = new(Mode.All);
    public static readonly DoubleToThicknessConverter Top = new(Mode.Top);
    public static readonly DoubleToThicknessConverter Left = new(Mode.Left);
    public static readonly DoubleToThicknessConverter Right = new(Mode.Right);
    public static readonly DoubleToThicknessConverter Bottom = new(Mode.Bottom);

    private readonly Mode _mode;

    private DoubleToThicknessConverter(Mode mode) => _mode = mode;

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var val = (double?)value ?? 0;
        return _mode switch
        {
            Mode.Left => new Thickness(Math.Max(0, val), 0, 0, 0),
            Mode.Right => new Thickness(0, 0, Math.Max(0, val), 0),
            Mode.Top => new Thickness(0, Math.Max(0, val), 0, 0),
            Mode.Bottom => new Thickness(0, 0, 0, Math.Max(0, val)),
            Mode.Horizontal => new Thickness(Math.Max(0, val), 0, Math.Max(0, val), 0),
            Mode.Vertical => new Thickness(0, Math.Max(0, val), 0, Math.Max(0, val)),
            Mode.All => new Thickness(Math.Max(0, val)),
            _ => throw new InvalidOperationException()
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new InvalidOperationException();
}
