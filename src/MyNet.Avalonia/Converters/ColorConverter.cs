// -----------------------------------------------------------------------
// <copyright file="ColorConverter.cs" company="Stéphane ANDRE">
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

public sealed class ColorConverter : IValueConverter
{
    private enum Mode
    {
        None,

        Contrast,

        Darken,

        Lighten
    }

    public static readonly ColorConverter Default = new(Mode.None);
    public static readonly ColorConverter Contrast = new(Mode.Contrast);
    public static readonly ColorConverter Darken = new(Mode.Darken);
    public static readonly ColorConverter Lighten = new(Mode.Lighten);

    private readonly Mode _mode;

    private ColorConverter(Mode mode) => _mode = mode;

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not SolidColorBrush and not Color and not string) return AvaloniaProperty.UnsetValue;

        var color = value switch
        {
            SolidColorBrush b => b.Color,
            Color c => c,
            string s => s.ToColor().GetValueOrDefault(),
            _ => Colors.White
        };
        var opacity = value is SolidColorBrush brush ? brush.Opacity : 1.0D;

        return _mode switch
        {
            Mode.Contrast => IdealTextColor(Color.FromArgb(System.Convert.ToByte(255 * opacity), color.R, color.G, color.B)),
            Mode.Darken => color.Darken(parameter is not null ? System.Convert.ToInt32(parameter, CultureInfo.InvariantCulture) : 1),
            Mode.Lighten => color.Lighten(parameter is not null ? System.Convert.ToInt32(parameter, CultureInfo.InvariantCulture) : 1),
            Mode.None => color,
            _ => throw new InvalidOperationException()
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => AvaloniaProperty.UnsetValue;

    private static Color IdealTextColor(Color bg)
    {
        const int nThreshold = 86;
        var bgDelta = System.Convert.ToInt32((bg.R * 0.299) + (bg.G * 0.587) + (bg.B * 0.114));
        var foreColor = 255 - bgDelta < nThreshold ? Colors.Black : Colors.White;
        return foreColor;
    }
}
