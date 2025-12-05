// -----------------------------------------------------------------------
// <copyright file="ThemeRoleConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.LogicalTree;
using Avalonia.Media;
using MyNet.Avalonia.Theme.Assists;
using MyNet.Avalonia.Theme.MarkupExtensions;
using MyNet.Avalonia.Theme.Palettes;

namespace MyNet.Avalonia.Theme.Converters;

/// <summary>
/// Converts a control to a themed brush based on its <see cref="ThemeAssist.Role"/> and <see cref="PaletteAssist"/> configuration.
/// Supports opacity, contrast, darken, and lighten transformations.
/// </summary>
public class ThemeRoleConverter : ThemeBrushConverter
{
    /// <summary>
    /// Gets or sets the palette color type to use (Primary, Secondary, Tertiary).
    /// </summary>
    public ColorType ColorType { get; set; } = ColorType.Primary;

    /// <summary>
    /// Converts a control to a themed brush based on its role and palette configuration.
    /// </summary>
    /// <param name="value">The control (AvaloniaObject) to extract the role from.</param>
    /// <param name="targetType">The target type of the binding.</param>
    /// <param name="parameter">Optional converter parameter.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>A themed <see cref="IBrush"/> based on the role and color type, or <see cref="AvaloniaProperty.UnsetValue"/> if conversion fails.</returns>
    public override object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not AvaloniaObject control) return AvaloniaProperty.UnsetValue;

        // Extract the role from the control's attached property
        var role = ThemeAssist.GetRole(control);

        return role switch
        {
            ThemeRole.Default or ThemeRole.Custom => ResolveCustomBrush(control, ColorType),
            ThemeRole.Inverse => ResolveInverseBrush(control),
            ThemeRole.Dark => MyTheme.Current.GetBrush("Surface.Background.Dark", Opacity, Contrast, Darken, Lighten),
            _ => MyTheme.Current.GetBrush(role.ToString(), Opacity, Contrast, Darken, Lighten)
        };
    }

    /// <summary>
    /// Resolves a custom brush from the control's palette attached properties.
    /// </summary>
    private IBrush ResolveCustomBrush(AvaloniaObject control, ColorType colorType)
    {
        var brush = colorType switch
        {
            ColorType.Primary => PaletteAssist.GetPrimary(control),
            ColorType.Secondary => PaletteAssist.GetSecondary(control),
            ColorType.Tertiary => PaletteAssist.GetTertiary(control),
            _ => null
        };

        return brush is null
            ? Brushes.Transparent
            : brush is SolidColorBrush solidBrush
            ? MyTheme.Current.GetBrush(solidBrush, Opacity, Contrast, Darken, Lighten)
            : brush;
    }

    /// <summary>
    /// Resolves an inverse brush by getting the foreground of the parent control.
    /// </summary>
    private IBrush ResolveInverseBrush(AvaloniaObject control)
    {
        var parent = (control as ILogical)?.FindLogicalAncestorOfType<Control>();
        return parent is null || TextElement.GetForeground(parent) is not { } parentForeground
            ? Brushes.Transparent
            : parentForeground is SolidColorBrush solidBrush
            ? MyTheme.Current.GetBrush(solidBrush, Opacity, Contrast, Darken, Lighten)
            : parentForeground;
    }
}
