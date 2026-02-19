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
using Avalonia.Controls.Documents;
using Avalonia.Data.Converters;
using Avalonia.Media;
using MyNet.Avalonia.Theme.Palettes;
using MyNet.Utilities;

namespace MyNet.Avalonia.Theme.Converters.Internals;

/// <summary>
/// Provides value conversion logic for theme brushes and role-based palette colors in Avalonia UI.
/// Supports conversion from theme parameters to the correct <see cref="IBrush"/> instance, including opacity and contrast transformations.
/// Used by markup extensions and bindings to resolve theme resources dynamically.
/// </summary>
internal sealed class ThemeConverter : IValueConverter, IMultiValueConverter
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
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => Convert([value], targetType, parameter, culture)!;

    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values.Count == 0) return AvaloniaProperty.UnsetValue;
        var value = values[0];

        switch (parameter)
        {
            case ThemeContextParameters themeContextParameters:
                if (value is not ThemeContext context) return AvaloniaProperty.UnsetValue;

                switch (context)
                {
                    case ThemeContext.Contrast:
                        var inheritedForeground = provideForeground(values.GetByIndex(2) as IBrush, values.GetByIndex(1) as Control);
                        var contrastOpacity = MyTheme.Current.GetOpacity(themeContextParameters.BrushKey);
                        var contrastForeground = ResolveBrush(inheritedForeground, new ThemeBrushParameters(contrastOpacity?.ToString(CultureInfo.InvariantCulture).OrEmpty(), false, themeContextParameters.Darken, themeContextParameters.Lighten));
                        return ResolveBrush(contrastForeground, themeContextParameters);

                    default:
                        return ResolveBrush(themeContextParameters.BrushKey, themeContextParameters);
                }

            case ThemeRoleParameters roleParameters:
                if (value is not ThemeRole role) return AvaloniaProperty.UnsetValue;

                switch (role)
                {
                    case ThemeRole.Default:
                        if (values.GetByIndex(1) is not IBrush brush) return AvaloniaProperty.UnsetValue;
                        return ResolveBrush(brush, new ThemeBrushParameters(roleParameters.Opacity, false, roleParameters.Darken, roleParameters.Lighten));

                    case ThemeRole.Custom:
                        if (values.GetByIndex(1) is not IBrush brush1) return AvaloniaProperty.UnsetValue;
                        return ResolveBrush(brush1, roleParameters);

                    case ThemeRole.Contrast:
                        var inheritedForeground = provideForeground(values.GetByIndex(3) as IBrush, values.GetByIndex(2) as Control);
                        return ResolveBrush(inheritedForeground, roleParameters);

                    case ThemeRole.Inverse:
                        return MyTheme.Current.GetBrush(ThemeResourceKeyFactory.InverseSurfaceKey, roleParameters.Opacity, roleParameters.Contrast, roleParameters.Darken, roleParameters.Lighten);

                    default:
                        return MyTheme.Current.GetBrush(role.ToString(), roleParameters.Opacity, roleParameters.Contrast, roleParameters.Darken, roleParameters.Lighten);
                }

            case ThemeBrushParameters brushParameters:
                if (value is not IBrush brush2) return AvaloniaProperty.UnsetValue;
                return ResolveBrush(brush2, brushParameters);

            default:
                return value is IBrush brush3 ? brush3 : AvaloniaProperty.UnsetValue;
        }

        static IBrush? provideForeground(IBrush? foreground, Control? control) => foreground ?? (control?.Parent is Control parent ? TextElement.GetForeground(parent) : null);
    }

    /// <summary>
    /// Not supported. Always returns <see cref="AvaloniaProperty.UnsetValue"/>.
    /// </summary>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => AvaloniaProperty.UnsetValue;

    /// <summary>
    /// Resolves a theme brush from a <see cref="IBrush"/> and brush parameters.
    /// </summary>
    /// <param name="brush">The base brush to transform.</param>
    /// <param name="parameters">Parameters specifying opacity, contrast, darken, and lighten.</param>
    /// <returns>The resolved <see cref="IBrush"/>.</returns>
    private static IBrush ResolveBrush(IBrush? brush, ThemeBrushParameters parameters) => brush is null
            ? BrushManager.FallbackBrush
            : MyTheme.Current.GetBrush(brush, parameters.Opacity, parameters.Contrast, parameters.Darken, parameters.Lighten);

    /// <summary>
    /// Resolves a theme brush from a <see cref="IBrush"/> and brush parameters.
    /// </summary>
    /// <param name="brushKey">The base brush to transform.</param>
    /// <param name="parameters">Parameters specifying opacity, contrast, darken, and lighten.</param>
    /// <returns>The resolved <see cref="IBrush"/>.</returns>
    private static IBrush ResolveBrush(string? brushKey, ThemeBrushParameters parameters) => brushKey is null
            ? BrushManager.FallbackBrush
            : MyTheme.Current.GetBrush(brushKey, parameters.Opacity, parameters.Contrast, parameters.Darken, parameters.Lighten);
}

/// <summary>
/// Describes parameters for resolving a theme brush, including opacity, contrast, darken, and lighten.
/// </summary>
public record ThemeBrushParameters(string? Opacity, bool Contrast, double? Darken = null, double? Lighten = null);

/// <summary>
/// Describes parameters for resolving a role-based theme brush, including palette color type, opacity, contrast, darken, and lighten.
/// </summary>
internal sealed record ThemeRoleParameters(PaletteColor Type, string? Opacity, bool Contrast, double? Darken = null, double? Lighten = null) : ThemeBrushParameters(Opacity, Contrast, Darken, Lighten);

/// <summary>
/// Describes parameters for resolving a theme context brush, including resource key, opacity, contrast, darken, and lighten.
/// </summary>
internal sealed record ThemeContextParameters(string? BrushKey, string? Opacity, bool Contrast, double? Darken = null, double? Lighten = null) : ThemeBrushParameters(Opacity, Contrast, Darken, Lighten);
