// -----------------------------------------------------------------------
// <copyright file="PositionToDockConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using MyNet.Avalonia.Controls.Enums;

namespace MyNet.Avalonia.Controls.Converters;

public sealed class PositionToDockConverter : IValueConverter
{
    public static PositionToDockConverter Default { get; } = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is Position position ? ToDock(position) : Dock.Left;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is Dock dock ? ToPosition(dock) : Position.Left;

    private static Dock ToDock(Position position)
        => position switch
        {
            Position.Left => Dock.Left,
            Position.Right => Dock.Right,
            Position.Top => Dock.Top,
            Position.Bottom => Dock.Bottom,
            _ => Dock.Left
        };

    private static Position ToPosition(Dock dock)
        => dock switch
        {
            Dock.Left => Position.Left,
            Dock.Right => Position.Right,
            Dock.Top => Position.Top,
            Dock.Bottom => Position.Bottom,
            _ => Position.Left
        };
}
