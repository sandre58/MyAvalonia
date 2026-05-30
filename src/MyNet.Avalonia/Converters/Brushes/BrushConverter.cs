// -----------------------------------------------------------------------
// <copyright file="BrushConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using Avalonia;
using Avalonia.Media;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Converters;
#pragma warning restore IDE0130

/// <summary>
/// Converts color sources to a <see cref="SolidColorBrush"/> with optional visual adjustments.
/// </summary>
/// <remarks>
/// Extends <see cref="ColorConverter"/> and additionally accepts <see cref="HsvColor"/> and <see cref="HslColor"/>.
/// Commonly used in Theme bindings: <c>Converter={x:Static my:BrushConverter.Default}</c>.
/// </remarks>
/// <example>
/// <code>
/// &lt;Border Background="{Binding AccentColor, Converter={x:Static my:BrushConverter.Default}}" /&gt;
/// </code>
/// </example>
public class BrushConverter : ColorConverter
{
    /// <summary>
    /// Gets the default brush converter instance.
    /// </summary>
    public static readonly BrushConverter Default = new();

    /// <inheritdoc/>
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
