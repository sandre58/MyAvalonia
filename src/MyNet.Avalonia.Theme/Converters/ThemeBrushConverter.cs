// -----------------------------------------------------------------------
// <copyright file="ThemeBrushConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace MyNet.Avalonia.Theme.Converters;

/// <summary>
/// Converts a <see cref="SolidColorBrush"/> to a themed brush with optional opacity, contrast, darken, and lighten effects.
/// Used for dynamic theming and color transformations in the UI.
/// </summary>
public class ThemeBrushConverter : IValueConverter
{
    /// <summary>
    /// Gets or sets the opacity value or key to apply to the brush.
    /// </summary>
    public string? Opacity { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to use the contrast color for accessibility.
    /// </summary>
    public bool Contrast { get; set; }

    /// <summary>
    /// Gets or sets the amount to darken the brush color (0.0 to 1.0).
    /// </summary>
    public double? Darken { get; set; }

    /// <summary>
    /// Gets or sets the amount to lighten the brush color (0.0 to 1.0).
    /// </summary>
    public double? Lighten { get; set; }

    /// <summary>
    /// Converts a <see cref="SolidColorBrush"/> to a themed brush with the specified options.
    /// </summary>
    /// <param name="value">The source brush to convert.</param>
    /// <param name="targetType">The target type of the binding.</param>
    /// <param name="parameter">Optional converter parameter.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>A themed <see cref="SolidColorBrush"/> with the specified opacity, contrast, and color transformations, or <see cref="AvaloniaProperty.UnsetValue"/> if conversion fails.</returns>
    public virtual object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => value is not SolidColorBrush brush ? AvaloniaProperty.UnsetValue : ProvideThemeBrush(brush);

    /// <summary>
    /// Provides the themed brush based on the current theme, opacity, and contrast settings.
    /// </summary>
    /// <param name="brush">The source brush.</param>
    /// <returns>The themed <see cref="SolidColorBrush"/>.</returns>
    protected virtual SolidColorBrush ProvideThemeBrush(SolidColorBrush brush) => MyTheme.Current.GetBrush(brush, Opacity, Contrast, Darken, Lighten);

    /// <summary>
    /// Not supported. Always returns <see cref="AvaloniaProperty.UnsetValue"/>.
    /// </summary>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => AvaloniaProperty.UnsetValue;
}
