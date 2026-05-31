// -----------------------------------------------------------------------
// <copyright file="NullFallbackConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia.Data;
using Avalonia.Data.Converters;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Converters;
#pragma warning restore IDE0130

/// <summary>
/// Returns the first non-null value from a single binding or multi-binding, or <see cref="BindingOperations.DoNothing"/> if all are null.
/// </summary>
/// <remarks>
/// In single-value mode, the converter parameter is treated as a fallback value.
/// Useful for coalescing primary and secondary sources without writing a custom multi-binding.
/// </remarks>
/// <example>
/// <code>
/// &lt;TextBlock Text="{Binding Primary, Converter={x:Static my:NullFallbackConverter.Default}, ConverterParameter={Binding Secondary}}" /&gt;
/// </code>
/// </example>
public class NullFallbackConverter : IValueConverter, IMultiValueConverter
{
    /// <summary>
    /// Gets the default singleton instance.
    /// </summary>
    public static NullFallbackConverter Default => new();

    /// <inheritdoc/>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => GetValueOrFallback([value, parameter]);

    /// <inheritdoc/>
    public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture) => GetValueOrFallback(values);

    /// <inheritdoc/>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => GetValueOrFallback([value, parameter]);

    private static object GetValueOrFallback(IEnumerable<object?> values) =>
        values.FirstOrDefault(x => x is not null) ?? BindingOperations.DoNothing;
}
