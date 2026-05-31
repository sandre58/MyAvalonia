// -----------------------------------------------------------------------
// <copyright file="GenderTypeConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Material.Icons;
using MyNet.Primitives;

namespace MyNet.Avalonia.Theme.Controls.Converters;

/// <summary>
/// Converts a <see cref="GenderType"/> value to a theme brush or icon, depending on the converter mode.
/// Supports both brush and icon conversion for gender-based theming in the UI.
/// </summary>
public sealed class GenderTypeConverter : IValueConverter
{
    private enum Mode
    {
        /// <summary>
        /// Converts to a theme brush.
        /// </summary>
        Brush,

        /// <summary>
        /// Converts to an icon.
        /// </summary>
        Icon
    }

    /// <summary>
    /// Gets a converter instance for brush conversion.
    /// </summary>
    public static readonly GenderTypeConverter Brush = new(Mode.Brush);

    /// <summary>
    /// Gets a converter instance for icon conversion.
    /// </summary>
    public static readonly GenderTypeConverter Icon = new(Mode.Icon);

    private readonly Mode _mode;

    private GenderTypeConverter(Mode mode) => _mode = mode;

    /// <summary>
    /// Converts a <see cref="GenderType"/> value to a theme brush or icon, depending on the converter mode.
    /// </summary>
    /// <param name="value">The gender type value to convert.</param>
    /// <param name="targetType">The target type of the binding.</param>
    /// <param name="parameter">Optional converter parameter.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>A theme brush or icon corresponding to the gender type, or <see cref="AvaloniaProperty.UnsetValue"/> if conversion fails.</returns>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => value is not GenderType genderType
            ? AvaloniaProperty.UnsetValue
            : _mode switch
            {
                Mode.Brush => MyTheme.Current.GetBrush($"Gender.{genderType}"),
                Mode.Icon => Enum.TryParse<MaterialIconKind>($"Gender{genderType}", out var kind) ? kind : MaterialIconKind.GenderMaleFemale,
                _ => AvaloniaProperty.UnsetValue
            };

    /// <summary>
    /// Not supported. Always returns <see cref="AvaloniaProperty.UnsetValue"/>.
    /// </summary>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => AvaloniaProperty.UnsetValue;
}
