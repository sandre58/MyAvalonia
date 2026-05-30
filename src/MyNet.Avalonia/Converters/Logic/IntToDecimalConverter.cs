// -----------------------------------------------------------------------
// <copyright file="IntToDecimalConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using Avalonia.Data.Converters;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Converters;
#pragma warning restore IDE0130

/// <summary>
/// Converts between <see cref="int"/> and <see cref="decimal"/> for controls that use different numeric types.
/// </summary>
/// <remarks>
/// Returns <c>null</c> when the input type does not match. Supports two-way binding (for example, pagination or time pickers).
/// </remarks>
public class IntToDecimalConverter : IValueConverter
{
    /// <summary>
    /// Gets the default singleton instance.
    /// </summary>
    public static readonly IntToDecimalConverter Default = new();

    /// <inheritdoc/>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => value is int d ? System.Convert.ToDecimal(d) : null;

    /// <inheritdoc/>
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => value is decimal d ? System.Convert.ToInt32(d) : null;
}
