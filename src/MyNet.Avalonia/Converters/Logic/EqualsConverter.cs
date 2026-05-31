// -----------------------------------------------------------------------
// <copyright file="EqualsConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Converters;
#pragma warning restore IDE0130

/// <summary>
/// Compares two or more bound values for equality and returns a boolean result.
/// </summary>
/// <example>
/// <code>
/// &lt;Border IsVisible="{Binding Status, Converter={x:Static my:EqualsConverter.IsEquals}, ConverterParameter=Active}" /&gt;
/// </code>
/// </example>
public class EqualsConverter(bool isEquals = true) : IMultiValueConverter, IValueConverter
{
    /// <summary>
    /// Gets a converter that returns <c>true</c> when all compared values are equal.
    /// </summary>
    public static readonly EqualsConverter IsEquals = new();

    /// <summary>
    /// Gets a converter that returns <c>true</c> when compared values differ.
    /// </summary>
    public static readonly EqualsConverter IsNotEquals = new(false);

    /// <inheritdoc/>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => Convert([value, parameter], targetType, parameter, culture);

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotSupportedException();
}
