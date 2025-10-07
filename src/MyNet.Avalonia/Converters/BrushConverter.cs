// -----------------------------------------------------------------------
// <copyright file="BrushConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;
using MyNet.Avalonia.Extensions;

namespace MyNet.Avalonia.Converters;

public sealed class BrushConverter : IValueConverter
{
    private enum Mode
    {
        None,

        Contrast,

        Opacity,

        ContrastAndOpacity,

        Darken,

        Lighten
    }

    public static readonly BrushConverter Default = new(Mode.None);
    public static readonly BrushConverter ContrastAndOpacity = new(Mode.ContrastAndOpacity);
    public static readonly BrushConverter Contrast = new(Mode.Contrast);
    public static readonly BrushConverter Opacity = new(Mode.Opacity);
    public static readonly BrushConverter Darken = new(Mode.Darken);
    public static readonly BrushConverter Lighten = new(Mode.Lighten);

    private readonly Mode _mode;

    private BrushConverter(Mode mode) => _mode = mode;

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not SolidColorBrush and not Color and not HsvColor and not HslColor and not string) return AvaloniaProperty.UnsetValue;

        var color = value switch
        {
            SolidColorBrush b => b.Color,
            Color c => c,
            HsvColor hsv => hsv.ToRgb(),
            HslColor hsl => hsl.ToRgb(),
            _ => value.ToString().ToColor() ?? Colors.White
        };
        var opacity = value is SolidColorBrush brush ? brush.Opacity : 1.0D;

        switch (_mode)
        {
            case Mode.ContrastAndOpacity:
                color = IdealTextColor(Color.FromArgb(System.Convert.ToByte(255 * opacity), color.R, color.G, color.B));
                opacity = parameter is not null ? System.Convert.ToDouble(parameter, CultureInfo.InvariantCulture) : 1.0D;
                break;
            case Mode.Contrast:
                color = IdealTextColor(Color.FromArgb(System.Convert.ToByte(255 * opacity), color.R, color.G, color.B));
                break;
            case Mode.Opacity:
                opacity = parameter is not null ? System.Convert.ToDouble(parameter, CultureInfo.InvariantCulture) : 1.0D;
                break;
            case Mode.Darken:
                color = color.Darken(parameter is not null ? System.Convert.ToInt32(parameter, CultureInfo.InvariantCulture) : 1);
                break;
            case Mode.Lighten:
                color = color.Lighten(parameter is not null ? System.Convert.ToInt32(parameter, CultureInfo.InvariantCulture) : 1);
                break;
            case Mode.None:
                break;
            default:
                throw new InvalidOperationException();
        }

        return new SolidColorBrush(color) { Opacity = opacity };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => AvaloniaProperty.UnsetValue;

    private static Color IdealTextColor(Color bg)
    {
        const int nThreshold = 86;
        var bgDelta = System.Convert.ToInt32((bg.R * 0.299) + (bg.G * 0.587) + (bg.B * 0.114));
        var foreColor = 255 - bgDelta < nThreshold ? Colors.Black : Colors.White;
        return foreColor;
    }
}
