// -----------------------------------------------------------------------
// <copyright file="EnumConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using Avalonia;
using Avalonia.Data.Converters;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Converters;
#pragma warning restore IDE0130

/// <summary>
/// Compares an enum (or numeric) binding value to a <see cref="ConverterParameter"/> and returns a boolean.
/// </summary>
/// <remarks>
/// <see cref="Any"/> returns <c>true</c> when the value matches the parameter (or any value in an enumerable parameter).
/// <see cref="NotAny"/> inverts the result. Supports <see cref="ConvertBack"/> for two-way enum pickers.
/// </remarks>
/// <example>
/// <code>
/// &lt;Border IsVisible="{Binding Role, Converter={x:Static my:EnumConverter.NotAny}, ConverterParameter={x:Static palettes:ThemeRole.Default}}" /&gt;
/// </code>
/// </example>
public sealed class EnumConverter : IValueConverter
{
    /// <summary>
    /// Gets a converter that returns <c>true</c> when the value matches the parameter.
    /// </summary>
    public static readonly EnumConverter Any = new(true);

    /// <summary>
    /// Gets a converter that returns <c>true</c> when the value does not match the parameter.
    /// </summary>
    public static readonly EnumConverter NotAny = new(false);

    private EnumConverter(bool any) => _any = any;

    private readonly bool _any;

    /// <inheritdoc/>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (parameter == null || value == null)
        {
            return false;
        }

        var val = parameter is IEnumerable parameters
            ? parameters.Cast<object>().Any(parameter2 =>
                System.Convert.ToInt32(parameter2, culture) == System.Convert.ToInt32(value, culture))
            : System.Convert.ToInt32(parameter, culture) == System.Convert.ToInt32(value, culture);

        return _any ? val : !val;
    }

    /// <inheritdoc/>
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var val = value != null && (parameter == null || !(bool)value)
            ? AvaloniaProperty.UnsetValue
            : parameter is IEnumerable parameters ? parameters.Cast<object>().FirstOrDefault() : parameter;

        return val;
    }
}
