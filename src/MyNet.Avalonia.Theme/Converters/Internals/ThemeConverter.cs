// -----------------------------------------------------------------------
// <copyright file="ThemeConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;
using MyNet.Avalonia.Theme.Theming.Core;
using MyNet.Utilities;

namespace MyNet.Avalonia.Theme.Converters.Internals;

/// <summary>
/// Provides value conversion logic for theme brushes and role-based palette colors in Avalonia UI.
/// Supports conversion from theme parameters to the correct <see cref="IBrush"/> instance, including opacity and contrast transformations.
/// Used by markup extensions and bindings to resolve theme resources dynamically.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ThemeConverter"/> class with specified services.
/// </remarks>
/// <param name="brushService">The theme brush service.</param>
/// <param name="resolver">The theme resolver.</param>
internal sealed class ThemeConverter(IThemeBrushService brushService, IThemeResolver resolver) : IValueConverter, IMultiValueConverter
{
    private readonly IThemeBrushService _brushService = brushService;
    private readonly IThemeResolver _resolver = resolver;

    /// <summary>
    /// Gets the default singleton instance of <see cref="ThemeConverter"/>.
    /// </summary>
    public static readonly ThemeConverter Default = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="ThemeConverter"/> class.
    /// </summary>
    public ThemeConverter()
        : this(MyTheme.Current, new ThemeResolver())
    {
    }

    /// <summary>
    /// Converts a value to a theme brush or role-based palette brush, applying opacity and contrast as specified.
    /// </summary>
    /// <param name="value">The input value, typically a <see cref="SolidColorBrush"/> or <see cref="AvaloniaObject"/>.</param>
    /// <param name="targetType">The target type for the conversion (usually <see cref="IBrush"/>).</param>
    /// <param name="parameter">A parameter describing the theme brush or role to resolve.</param>
    /// <param name="culture">The culture for conversion (not used).</param>
    /// <returns>The resolved <see cref="IBrush"/> or <see cref="AvaloniaProperty.UnsetValue"/> if conversion fails.</returns>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => Convert([value], targetType, parameter, culture);

    /// <summary>
    /// Converts multiple values to a theme brush or role-based palette brush, applying opacity and contrast as specified.
    /// </summary>
    /// <param name="values">The list of values to convert, typically including a brush or theme context and optional foreground.</param>
    /// <param name="targetType">The target type for the conversion (usually <see cref="IBrush"/>).</param>
    /// <param name="parameter">A parameter describing the theme brush or role to resolve.</param>
    /// <param name="culture">The culture for conversion (not used).</param>
    /// <returns>The resolved <see cref="IBrush"/> or <see cref="AvaloniaProperty.UnsetValue"/> if conversion fails.</returns>
    public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values.Count == 0) return AvaloniaProperty.UnsetValue;

        var brushParameters = parameter as ThemeBrushParameters;

        ThemeRole? role = null;
        ThemeContext? context = null;
        string? resourceKey = null;
        IBrush? directBrush = null;
        IBrush? foreground = null;

        for (var i = 0; i < values.Count; i++)
        {
            switch (values[i])
            {
                case ThemeRole r:
                    role = r;
                    break;
                case ThemeContext c:
                    context = c;
                    break;
                case string s:
                    resourceKey = s;
                    break;
                case IBrush brush when directBrush is null:
                    directBrush = brush;
                    foreground = brush;
                    break;
                case IBrush brush:
                    foreground = brush;
                    break;
            }
        }

        var result = _resolver.Resolve(role, context, resourceKey);

        var contrast = result.UseContrast && brushParameters?.Contrast == true;

        switch (result.Kind)
        {
            case ThemeBrushResolutionKind.UseDirectBrush:
                return _brushService.GetBrush(directBrush, brushParameters?.Opacity, contrast, brushParameters?.Darken, brushParameters?.Lighten);

            case ThemeBrushResolutionKind.UseForeground:
                if (!string.IsNullOrEmpty(result.OpacityKey))
                {
                    var contrastOpacity = _brushService.GetOpacity(result.OpacityKey);
                    foreground = _brushService.GetBrush(foreground, contrastOpacity?.ToString(CultureInfo.InvariantCulture).OrEmpty());
                }

                return _brushService.GetBrush(foreground, brushParameters?.Opacity, contrast, brushParameters?.Darken, brushParameters?.Lighten);

            case ThemeBrushResolutionKind.UseKey:
                return _brushService.GetBrush(result.BrushKey, brushParameters?.Opacity, contrast, brushParameters?.Darken, brushParameters?.Lighten);
        }

        return AvaloniaProperty.UnsetValue;
    }

    /// <summary>
    /// Not supported. Always returns <see cref="AvaloniaProperty.UnsetValue"/>.
    /// </summary>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => AvaloniaProperty.UnsetValue;
}

/// <summary>
/// Describes parameters for resolving and transforming a theme brush, including opacity, contrast, darken, and lighten.
/// </summary>
public record ThemeBrushParameters(string? Opacity, bool Contrast, double? Darken = null, double? Lighten = null);
