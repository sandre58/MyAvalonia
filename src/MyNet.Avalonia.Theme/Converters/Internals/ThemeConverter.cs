// -----------------------------------------------------------------------
// <copyright file="ThemeConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;
using MyNet.Avalonia.Theme.Assists;
using MyNet.Avalonia.Theme.Palettes;

namespace MyNet.Avalonia.Theme.Converters.Internals;

/// <summary>
/// Provides value conversion logic for theme brushes and role-based palette colors in Avalonia UI.
/// Supports conversion from theme parameters to the correct <see cref="IBrush"/> instance, including opacity and contrast transformations.
/// Used by markup extensions and bindings to resolve theme resources dynamically.
/// </summary>
internal class ThemeConverter : IValueConverter
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
    public virtual object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        switch (parameter)
        {
            case ThemeRoleParameters roleParameters:
                if (value is not AvaloniaObject control) return AvaloniaProperty.UnsetValue;

                var role = ThemeAssist.GetRole(control);

                return ProvideRoleBrush(control, role, roleParameters);

            case ThemeBrushParameters brushParameters:
                if (value is not SolidColorBrush brush) return AvaloniaProperty.UnsetValue;
                return ProvideThemeBrush(brush, brushParameters);

            default:
                return value is IBrush brush1 ? brush1 : AvaloniaProperty.UnsetValue;
        }
    }

    /// <summary>
    /// Not supported. Always returns <see cref="AvaloniaProperty.UnsetValue"/>.
    /// </summary>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => AvaloniaProperty.UnsetValue;

    /// <summary>
    /// Resolves a theme brush from a <see cref="SolidColorBrush"/> and brush parameters.
    /// </summary>
    /// <param name="brush">The base brush to transform.</param>
    /// <param name="parameters">Parameters specifying opacity and contrast.</param>
    /// <returns>The resolved <see cref="SolidColorBrush"/>.</returns>
    private static SolidColorBrush ProvideThemeBrush(SolidColorBrush brush, ThemeBrushParameters parameters) => MyTheme.Current.GetBrush(brush, parameters.Opacity, parameters.Contrast);

    /// <summary>
    /// Resolves a role-based theme brush for a control, using the specified role and parameters.
    /// </summary>
    /// <param name="control">The control to resolve the role for.</param>
    /// <param name="role">The theme role to use.</param>
    /// <param name="parameters">Parameters specifying palette color, opacity, and contrast.</param>
    /// <returns>The resolved <see cref="IBrush"/>.</returns>
    private static IBrush ProvideRoleBrush(AvaloniaObject control, ThemeRole role, ThemeRoleParameters parameters) =>
        role switch
        {
            ThemeRole.Default or ThemeRole.Custom => ResolveCustomBrush(control, parameters),
            ThemeRole.Inverse => ResolveInverseBrush(control, parameters),
            _ => MyTheme.Current.GetBrush(role.ToString(), parameters.Opacity, parameters.Contrast)
        };

    /// <summary>
    /// Resolves a custom palette brush for a control, using the specified palette color type.
    /// </summary>
    /// <param name="control">The control to resolve the palette for.</param>
    /// <param name="parameters">Parameters specifying palette color, opacity, and contrast.</param>
    /// <returns>The resolved <see cref="IBrush"/>.</returns>
    private static IBrush ResolveCustomBrush(AvaloniaObject control, ThemeRoleParameters parameters)
    {
        var brush = parameters.Type switch
        {
            PaletteColor.Primary => PaletteAssist.GetPrimary(control),
            PaletteColor.Secondary => PaletteAssist.GetSecondary(control),
            PaletteColor.Tertiary => PaletteAssist.GetTertiary(control),
            _ => null
        };

        return brush is null
            ? Brushes.Transparent
            : brush is SolidColorBrush solidBrush
            ? MyTheme.Current.GetBrush(solidBrush, parameters.Opacity, parameters.Contrast)
            : brush;
    }

    /// <summary>
    /// Resolves the inverse foreground brush for a control, using inherited foreground color.
    /// </summary>
    /// <param name="control">The control to resolve the foreground for.</param>
    /// <param name="parameters">Parameters specifying opacity and contrast.</param>
    /// <returns>The resolved <see cref="IBrush"/>.</returns>
    private static IBrush ResolveInverseBrush(AvaloniaObject control, ThemeRoleParameters parameters)
        => ThemeAssist.GetInheritedForeground(control) is not SolidColorBrush brush ? Brushes.Transparent : MyTheme.Current.GetBrush(brush, parameters.Opacity, parameters.Contrast);
}

/// <summary>
/// Describes parameters for resolving a theme brush, including opacity and contrast.
/// </summary>
public record ThemeBrushParameters(string? Opacity, bool Contrast);

/// <summary>
/// Describes parameters for resolving a role-based theme brush, including palette color type, opacity, and contrast.
/// </summary>
internal record ThemeRoleParameters(PaletteColor Type, string? Opacity, bool Contrast) : ThemeBrushParameters(Opacity, Contrast);
