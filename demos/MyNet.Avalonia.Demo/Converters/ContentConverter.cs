// -----------------------------------------------------------------------
// <copyright file="ContentConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;
using MyNet.Avalonia.Demo.Controls;
using MyNet.Avalonia.Theme.Converters.Internals;
using MyNet.Avalonia.Theme.Enums;
using MyNet.Avalonia.Theme.Extensions;
using MyNet.Utilities;
using MyNet.Utilities.Generator;

namespace MyNet.Avalonia.Demo.Converters;

internal sealed class ContentConverter : IValueConverter, IMultiValueConverter
{
    /// <summary>
    /// Gets the default singleton instance of <see cref="ThemeConverter"/>.
    /// </summary>
    public static readonly ContentConverter Default = new();

    /// <summary>
    /// Converts a value to a theme brush or role-based palette brush, applying opacity and contrast as specified.
    /// </summary>
    /// <param name="value">The input value, typically a <see cref="SolidColorBrush"/> or <see cref="AvaloniaObject"/>.</param>
    /// <param name="targetType">The target type for the conversion (usually <see cref="IBrush"/>).</param>
    /// <param name="parameter">A parameter describing the theme brush or role to resolve.</param>
    /// <param name="culture">The culture for conversion (not used).</param>
    /// <returns>The resolved <see cref="IBrush"/> or <see cref="AvaloniaProperty.UnsetValue"/> if conversion fails.</returns>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => Convert([value], targetType, parameter, culture)!;

    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture) => values.GetByIndex(0) is not ContentType contentType
            ? AvaloniaProperty.UnsetValue
            : contentType switch
            {
                ContentType.None => string.Empty,
                ContentType.Icon => RandomGenerator.Enum<IconData>().ToIcon(),
                ContentType.Geometry => RandomGenerator.Enum<IconData>().ToGeometry(),
                _ => values.GetByIndex(1),
            };

    /// <summary>
    /// Not supported. Always returns <see cref="AvaloniaProperty.UnsetValue"/>.
    /// </summary>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => AvaloniaProperty.UnsetValue;
}
