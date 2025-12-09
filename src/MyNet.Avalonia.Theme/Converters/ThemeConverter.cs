// -----------------------------------------------------------------------
// <copyright file="ThemeConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Data.Converters;
using Avalonia.LogicalTree;
using Avalonia.Media;
using MyNet.Avalonia.Theme.Assists;
using MyNet.Avalonia.Theme.Palettes;

namespace MyNet.Avalonia.Theme.Converters;

internal class ThemeConverter : IValueConverter
{
    public static readonly ThemeConverter Default = new();

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

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => AvaloniaProperty.UnsetValue;

    private static SolidColorBrush ProvideThemeBrush(SolidColorBrush brush, ThemeBrushParameters parameters) =>
        MyTheme.Current.GetBrush(brush, parameters.Opacity, parameters.Contrast);

    private static IBrush ProvideRoleBrush(AvaloniaObject control, ThemeRole role, ThemeRoleParameters parameters) =>
        role switch
        {
            ThemeRole.Default or ThemeRole.Custom => ResolveCustomBrush(control, parameters),
            ThemeRole.Inverse => ResolveInverseBrush(control, parameters),
            ThemeRole.Dark => MyTheme.Current.GetBrush("Surface.Background.Dark", parameters.Opacity, parameters.Contrast),
            _ => MyTheme.Current.GetBrush(role.ToString(), parameters.Opacity, parameters.Contrast)
        };

    private static IBrush ResolveCustomBrush(AvaloniaObject control, ThemeRoleParameters parameters)
    {
        var brush = parameters.Type switch
        {
            ColorType.Primary => PaletteAssist.GetPrimary(control),
            ColorType.Secondary => PaletteAssist.GetSecondary(control),
            ColorType.Tertiary => PaletteAssist.GetTertiary(control),
            _ => null
        };

        return brush is null
            ? Brushes.Transparent
            : brush is SolidColorBrush solidBrush
            ? MyTheme.Current.GetBrush(solidBrush, parameters.Opacity, parameters.Contrast)
            : brush;
    }

    private static IBrush ResolveInverseBrush(AvaloniaObject control, ThemeRoleParameters parameters)
    {
        var parent = (control as ILogical)?.FindLogicalAncestorOfType<Control>();
        return parent is null || TextElement.GetForeground(parent) is not { } parentForeground
            ? Brushes.Transparent
            : parentForeground is SolidColorBrush solidBrush
            ? MyTheme.Current.GetBrush(solidBrush, parameters.Opacity, parameters.Contrast)
            : parentForeground;
    }
}

public record ThemeBrushParameters(string? Opacity, bool Contrast);

internal record ThemeRoleParameters(ColorType Type, string? Opacity, bool Contrast) : ThemeBrushParameters(Opacity, Contrast);

/// <summary>
/// Enumerates palette color types for use with the <see cref="ThemeConverter"/>.
/// </summary>
public enum ColorType
{
    /// <summary>
    /// Primary palette color.
    /// </summary>
    Primary,

    /// <summary>
    /// Secondary palette color.
    /// </summary>
    Secondary,

    /// <summary>
    /// Tertiary palette color.
    /// </summary>
    Tertiary
}
