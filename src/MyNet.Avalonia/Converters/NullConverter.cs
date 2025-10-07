// -----------------------------------------------------------------------
// <copyright file="NullConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Avalonia.Data.Converters;

namespace MyNet.Avalonia.Converters;

/// <summary>
/// Converts a null value to Visibility.Visible and any other value to Visibility.Collapsed.
/// </summary>
public class NullConverter(bool nullValue = true) : IValueConverter
{
    public static readonly NullConverter IsNull = new();
    public static readonly NullConverter IsNotNull = new(false);

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
        var flag = value switch
        {
            string str => string.IsNullOrEmpty(str),
            double dbl => double.IsNaN(dbl),
            Array arr => arr.Length == 0,
            DateTime date => date == DateTime.MinValue,
            _ => value == null
        };

        return flag ? nullValue : !nullValue;
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
