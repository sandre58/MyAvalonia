// -----------------------------------------------------------------------
// <copyright file="EqualsConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Avalonia.Data.Converters;

namespace MyNet.Avalonia.Converters;

public class EqualsConverter(bool isEquals = true) : IValueConverter
{
    public static readonly NullConverter IsEquals = new();
    public static readonly NullConverter IsNotEquals = new(false);

    /// <summary>
    /// Converts a value.
    /// </summary>
    /// <param name="value">The value produced by the binding source.</param>
    /// <param name="targetType">The type of the binding target property.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>
    /// A converted value. If the method returns null, the valid null value is used.
    /// </returns>
    [SuppressMessage("Maintainability", "CA1508", Justification = "False positive")]
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var result = parameter != null && value?.ToString() is { } str && str.Equals(parameter.ToString(), StringComparison.OrdinalIgnoreCase);

        return result ? isEquals : !isEquals;
    }

    /// <summary>
    /// Converts a value.
    /// </summary>
    /// <param name="value">The value that is produced by the binding target.</param>
    /// <param name="targetType">The type to convert to.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>
    /// A converted value. If the method returns null, the valid null value is used.
    /// </returns>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotSupportedException();
}
