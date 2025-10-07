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
using MyNet.Utilities;

namespace MyNet.Avalonia.Converters;

/// <summary>
/// MathConverter provides a value converter which can be used for math operations.
/// It can be used for normal binding or multi binding as well.
/// If it is used for normal binding the given parameter will be used as operands with the selected operation.
/// If it is used for multi binding then the first and second binding will be used as operands with the selected operation.
/// This class cannot be inherited.
/// </summary>
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

    public static readonly MathComparisonConverter IsEqualsTo = new(MathComparisonForConverter.IsEqualsTo);

    public static readonly MathComparisonConverter IsGreaterThan = new(MathComparisonForConverter.IsGreaterThan);

    public static readonly MathComparisonConverter IsLessThan = new(MathComparisonForConverter.IsLessThan);

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => DoConvert(value, parameter, Comparison);

    public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture) => values.Count < 2 ? AvaloniaProperty.UnsetValue : DoConvert(values[0], values[1], Comparison);

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
                MathComparisonForConverter.IsEqualsTo => value1.NearlyEqual(value2),
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
