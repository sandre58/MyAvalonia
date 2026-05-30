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

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Converters;
#pragma warning restore IDE0130

/// <summary>
/// Converts color sources to a <see cref="Color"/> with optional opacity, contrast, lighten, or darken adjustments.
/// </summary>
/// <remarks>
/// Accepts <see cref="SolidColorBrush"/>, <see cref="Color"/>, or color strings (hex, name).
/// Set <see cref="Opacity"/>, <see cref="Contrast"/>, <see cref="Darken"/>, and <see cref="Lighten"/> on the instance
/// or use <see cref="BrushConverter"/> when the target property expects an <see cref="IBrush"/>.
/// </remarks>
public class ColorConverter : IValueConverter
{
    /// <summary>
    /// Gets or sets the opacity key or value to apply.
    /// </summary>
    public double? Opacity { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to use the contrast brush.
    /// </summary>
    public bool Contrast { get; set; }

    /// <summary>
    /// Gets or sets the darken amount (0.0 to 1.0).
    /// </summary>
    public double? Darken { get; set; }

    /// <summary>
    /// Gets or sets the lighten amount (0.0 to 1.0).
    /// </summary>
    public double? Lighten { get; set; }

    /// <inheritdoc/>
    public virtual object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not SolidColorBrush and not Color and not string) return AvaloniaProperty.UnsetValue;

        var color = value switch
        {
            SolidColorBrush b => b.Color,
            Color c => c,
            string s => s.ToColor().GetValueOrDefault(),
            _ => global::Avalonia.Media.Colors.White
        };
        var opacity = Opacity ?? (value is SolidColorBrush { Opacity: < 1.0 } brush ? brush.Opacity : null);

        return color.Apply(new(opacity, Contrast, Darken, Lighten));
    }

    /// <inheritdoc/>
    public virtual object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => AvaloniaProperty.UnsetValue;
}
