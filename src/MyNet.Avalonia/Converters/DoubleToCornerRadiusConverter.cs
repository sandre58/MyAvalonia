// -----------------------------------------------------------------------
// <copyright file="DoubleToCornerRadiusConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace MyNet.Avalonia.Converters;

public sealed class DoubleToCornerRadiusConverter : IValueConverter
{
    private enum Mode
    {
        All,

        Top,

        Left,

        Right,

        Bottom
    }

    public static readonly DoubleToCornerRadiusConverter All = new(Mode.All);
    public static readonly DoubleToCornerRadiusConverter Top = new(Mode.Top);
    public static readonly DoubleToCornerRadiusConverter Left = new(Mode.Left);
    public static readonly DoubleToCornerRadiusConverter Right = new(Mode.Right);
    public static readonly DoubleToCornerRadiusConverter Bottom = new(Mode.Bottom);

    private readonly Mode _mode;

    private DoubleToCornerRadiusConverter(Mode mode) => _mode = mode;

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var val = (double?)value ?? 0;
        return _mode switch
        {
            Mode.Left => new CornerRadius(Math.Max(0, val), 0, 0, Math.Max(0, val)),
            Mode.Right => new CornerRadius(0, Math.Max(0, val), Math.Max(0, val), 0),
            Mode.Top => new CornerRadius(Math.Max(0, val), Math.Max(0, val), 0, 0),
            Mode.Bottom => new CornerRadius(0, 0, Math.Max(0, val), Math.Max(0, val)),
            Mode.All => new CornerRadius(Math.Max(0, val)),
            _ => throw new InvalidOperationException()
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new InvalidOperationException();
}
