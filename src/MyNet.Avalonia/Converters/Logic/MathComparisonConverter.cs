// -----------------------------------------------------------------------
// <copyright file="MathComparisonConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using MyNet.Primitives;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Converters;
#pragma warning restore IDE0130

/// <summary>
/// Compares numeric binding values and returns a boolean result.
/// </summary>
/// <remarks>
/// Supports single-value binding with a converter parameter as the second operand,
/// or multi-binding where the first two values are compared.
/// </remarks>
public sealed class MathComparisonConverter : IValueConverter, IMultiValueConverter
{
    private enum MathComparisonForConverter
    {
        IsEqualsTo,
        IsGreaterThan,
        IsLessThan
    }

    private MathComparisonForConverter Comparison { get; }

    private MathComparisonConverter(MathComparisonForConverter operation) => Comparison = operation;

    /// <summary>Gets a converter that returns <c>true</c> when operands are numerically equal.</summary>
    public static readonly MathComparisonConverter IsEqualsTo = new(MathComparisonForConverter.IsEqualsTo);

    /// <summary>Gets a converter that returns <c>true</c> when the first operand is greater than the second.</summary>
    public static readonly MathComparisonConverter IsGreaterThan = new(MathComparisonForConverter.IsGreaterThan);

    /// <summary>Gets a converter that returns <c>true</c> when the first operand is less than the second.</summary>
    public static readonly MathComparisonConverter IsLessThan = new(MathComparisonForConverter.IsLessThan);

    /// <inheritdoc/>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => DoConvert(value, parameter, Comparison);

    /// <inheritdoc/>
    public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture) => values.Count < 2 ? AvaloniaProperty.UnsetValue : DoConvert(values[0], values[1], Comparison);

    /// <inheritdoc/>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => AvaloniaProperty.UnsetValue;

    private static object DoConvert(object? firstValue, object? secondValue, MathComparisonForConverter operation)
    {
        if (firstValue == null
            || secondValue == null
            || firstValue == AvaloniaProperty.UnsetValue
            || secondValue == AvaloniaProperty.UnsetValue
            || firstValue == DBNull.Value
            || secondValue == DBNull.Value)
        {
            return AvaloniaProperty.UnsetValue;
        }

        try
        {
            var firstCulture = firstValue is string ? CultureInfo.CurrentCulture : CultureInfo.InvariantCulture;
            var secondCulture = secondValue is string ? CultureInfo.CurrentCulture : CultureInfo.InvariantCulture;
            var value1 = (firstValue as double?) ?? System.Convert.ToDouble(firstValue, firstCulture);
            var value2 = (secondValue as double?) ?? System.Convert.ToDouble(secondValue, secondCulture);

            return operation switch
            {
                MathComparisonForConverter.IsEqualsTo => value1.IsCloseTo(value2),
                MathComparisonForConverter.IsGreaterThan => value1 > value2,
                MathComparisonForConverter.IsLessThan => value1 < value2,
                _ => AvaloniaProperty.UnsetValue
            };
        }
        catch (Exception)
        {
            return AvaloniaProperty.UnsetValue;
        }
    }
}
