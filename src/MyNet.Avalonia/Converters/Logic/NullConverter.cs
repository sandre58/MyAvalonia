// -----------------------------------------------------------------------
// <copyright file="NullConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Avalonia.Data.Converters;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Converters;
#pragma warning restore IDE0130

/// <summary>
/// Converts empty or null-like values to a boolean result for visibility and enable bindings.
/// </summary>
/// <remarks>
/// Treats null, empty strings, NaN doubles, empty arrays, and <see cref="DateTime.MinValue"/> as empty.
/// Returns <c>true</c> or <c>false</c> depending on the configured polarity — not <see cref="Avalonia.Controls.Visibility"/>.
/// </remarks>
/// <example>
/// <code>
/// &lt;TextBlock IsVisible="{Binding Header, Converter={x:Static my:NullConverter.IsPresent}}" /&gt;
/// </code>
/// </example>
public class NullConverter(bool isEmptyValue = true) : IValueConverter
{
    /// <summary>
    /// Gets a converter that returns <c>true</c> when the value is empty or null-like.
    /// </summary>
    public static readonly NullConverter IsEmpty = new();

    /// <summary>
    /// Gets a converter that returns <c>true</c> when the value is present.
    /// </summary>
    public static readonly NullConverter IsPresent = new(false);

    /// <inheritdoc/>
    [SuppressMessage("Maintainability", "CA1508", Justification = "False positive")]
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var isEmpty = value switch
        {
            string str => string.IsNullOrEmpty(str),
            double dbl => double.IsNaN(dbl),
            Array arr => arr.Length == 0,
            DateTime date => date == DateTime.MinValue,
            _ => value == null
        };

        return isEmpty ? isEmptyValue : !isEmptyValue;
    }

    /// <inheritdoc/>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotSupportedException();
}
