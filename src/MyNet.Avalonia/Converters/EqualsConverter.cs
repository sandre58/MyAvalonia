// -----------------------------------------------------------------------
// <copyright file="EqualsConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Avalonia.Data.Converters;

namespace MyNet.Avalonia.Converters;

public class EqualsConverter(bool isEquals = true) : IMultiValueConverter, IValueConverter
{
    public static readonly EqualsConverter IsEquals = new();
    public static readonly EqualsConverter IsNotEquals = new(false);

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
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => Convert([value, parameter], targetType, parameter, culture);

    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values.Count <= 1)
        {
            // Empty or single value: consider all values equal
            return isEquals;
        }

        var first = values[0];

        for (var i = 1; i < values.Count; i++)
        {
            if (!Equals(first, values[i]))
            {
                return !isEquals;
            }
        }

        return isEquals;
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
