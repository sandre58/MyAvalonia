// -----------------------------------------------------------------------
// <copyright file="BrushConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using Avalonia;
using Avalonia.Media;

namespace MyNet.Avalonia.Converters;

public class BrushConverter : ColorConverter
{
    public static readonly BrushConverter Default = new();

    public override object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not SolidColorBrush and not Color and not HsvColor and not HslColor and not string) return AvaloniaProperty.UnsetValue;

        var color = value switch
        {
            SolidColorBrush b => b.Color,
            Color c => c,
            HsvColor hsv => hsv.ToRgb(),
            HslColor hsl => hsl.ToRgb(),
            _ => value.ToString().ToColor() ?? global::Avalonia.Media.Colors.White
        };
        var opacity = Opacity ?? (value is SolidColorBrush { Opacity: < 1.0 } brush ? brush.Opacity : 1.0);

        color = color.Apply(new(null, Contrast, Darken, Lighten));

        return new SolidColorBrush(color) { Opacity = opacity };
    }
}
