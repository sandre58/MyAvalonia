// -----------------------------------------------------------------------
// <copyright file="NullToDefaultConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using Avalonia.Data.Converters;
using MyNet.Reflection;
using MyNet.Utilities;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Converters;
#pragma warning restore IDE0130

/// <summary>
/// Returns the bound value when present, otherwise the default value for the binding target type.
/// </summary>
/// <remarks>
/// Null values and the string <c>"NaN"</c> are replaced with <see cref="TypeExtensions.GetDefault"/> for the target type.
/// </remarks>
/// <example>
/// <code>
/// &lt;my:Ripple IsActive="{Binding Tag, Converter={x:Static my:NullToDefaultConverter.Default}}" /&gt;
/// </code>
/// </example>
public class NullToDefaultConverter : IValueConverter
{
    /// <summary>
    /// Gets the default singleton instance.
    /// </summary>
    public static IValueConverter Default { get; } = new NullToDefaultConverter();

    /// <inheritdoc />
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => IsMissing(value) ? targetType.GetDefault() : value;

    /// <inheritdoc />
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => string.IsNullOrEmpty(value?.ToString()) ? targetType.GetDefault() : value;

    private static bool IsMissing(object? value) => value is null || value.ToString() == "NaN";
}
