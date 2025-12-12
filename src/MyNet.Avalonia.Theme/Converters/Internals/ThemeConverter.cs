// -----------------------------------------------------------------------
// <copyright file="ThemeConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using MyNet.Avalonia.Theme.Assists;
using MyNet.Avalonia.Theme.Palettes;
using MyNet.Utilities;

namespace MyNet.Avalonia.Theme.Converters.Internals;

/// <summary>
/// Provides value conversion logic for theme brushes and role-based palette colors in Avalonia UI.
/// Supports conversion from theme parameters to the correct <see cref="IBrush"/> instance, including opacity and contrast transformations.
/// Used by markup extensions and bindings to resolve theme resources dynamically.
/// </summary>
internal class ThemeConverter : IValueConverter, IMultiValueConverter
{
    /// <summary>
    /// Gets the default singleton instance of <see cref="ThemeConverter"/>.
    /// </summary>
    public static readonly ThemeConverter Default = new();

    /// <summary>
    /// Converts a value to a theme brush or role-based palette brush, applying opacity and contrast as specified.
    /// </summary>
    /// <param name="value">The input value, typically a <see cref="SolidColorBrush"/> or <see cref="AvaloniaObject"/>.</param>
    /// <param name="targetType">The target type for the conversion (usually <see cref="IBrush"/>).</param>
    /// <param name="parameter">A parameter describing the theme brush or role to resolve.</param>
    /// <param name="culture">The culture for conversion (not used).</param>
    /// <returns>The resolved <see cref="IBrush"/> or <see cref="AvaloniaProperty.UnsetValue"/> if conversion fails.</returns>
    public virtual object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => Convert([value], targetType, parameter, culture)!;

    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values.Count == 0) return AvaloniaProperty.UnsetValue;
        var value = values[0];

        switch (parameter)
        {
            case ThemeRoleParameters roleParameters:
                if (value is not ThemeRole role) return AvaloniaProperty.UnsetValue;
                var brush = values.GetByIndex(1) as IBrush;
                var inheritedBrush = values.GetByIndex(2) is StyledElement control ? ProvideInheritedForeground(control) : null;

                return role switch
                {
                    ThemeRole.Default or ThemeRole.Custom => ResolveBrush(brush, roleParameters),
                    ThemeRole.Inverse => ResolveBrush(inheritedBrush, roleParameters),
                    _ => MyTheme.Current.GetBrush(role.ToString(), roleParameters.Opacity, roleParameters.Contrast)
                };

            case ThemeBrushParameters brushParameters:
                if (value is not SolidColorBrush brush1) return AvaloniaProperty.UnsetValue;
                return ResolveBrush(brush1, brushParameters);

            default:
                return value is IBrush brush2 ? brush2 : AvaloniaProperty.UnsetValue;
        }
    }

    /// <summary>
    /// Not supported. Always returns <see cref="AvaloniaProperty.UnsetValue"/>.
    /// </summary>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => AvaloniaProperty.UnsetValue;

    /// <summary>
    /// Resolves a theme brush from a <see cref="IBrush"/> and brush parameters.
    /// </summary>
    /// <param name="brush">The base brush to transform.</param>
    /// <param name="parameters">Parameters specifying opacity and contrast.</param>
    /// <returns>The resolved <see cref="IBrush"/>.</returns>
    private static IBrush ResolveBrush(IBrush? brush, ThemeBrushParameters parameters) => brush is null
            ? Brushes.Transparent
            : brush is SolidColorBrush solidBrush && (!string.IsNullOrEmpty(parameters.Opacity) || parameters.Contrast)
            ? MyTheme.Current.GetBrush(solidBrush, parameters.Opacity, parameters.Contrast)
            : brush;

    private static IBrush? ProvideInheritedForeground(StyledElement control)
    {
        var inheritedBrush = ThemeAssist.GetInheritedForeground(control);

        return (inheritedBrush != Brushes.Transparent && inheritedBrush != Brushes.Black && inheritedBrush != Brushes.White) || control.Parent is null
            ? inheritedBrush
            : ProvideInheritedForeground(control.Parent);
    }
}

/// <summary>
/// Describes parameters for resolving a theme brush, including opacity and contrast.
/// </summary>
public record ThemeBrushParameters(string? Opacity, bool Contrast);

/// <summary>
/// Describes parameters for resolving a role-based theme brush, including palette color type, opacity, and contrast.
/// </summary>
internal record ThemeRoleParameters(PaletteColor Type, string? Opacity, bool Contrast) : ThemeBrushParameters(Opacity, Contrast);
