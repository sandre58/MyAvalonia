// -----------------------------------------------------------------------
// <copyright file="MathConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia;
using Avalonia.Data.Converters;
using MyNet.Utilities;

namespace MyNet.Avalonia.Converters;

public sealed class MathConverter : IValueConverter, IMultiValueConverter
{
    private enum MathOperation
    {
        Add,
        Subtract,
        Multiply,
        Divide,
        Percent,
        PercentToValue,
        Pow,
        Modulo
    }

    private readonly MathOperation _operation;

    public static MathConverter Add => new(MathOperation.Add);

    public static MathConverter Subtract => new(MathOperation.Subtract);

    public static MathConverter Multiply => new(MathOperation.Multiply);

    public static MathConverter Divide => new(MathOperation.Divide);

    public static MathConverter Percent => new(MathOperation.Percent);

    public static MathConverter PercentToValue => new(MathOperation.PercentToValue);

    public static MathConverter Pow => new(MathOperation.Pow);

    public static MathConverter Modulo => new(MathOperation.Modulo);

    private MathConverter(MathOperation operation) => _operation = operation;

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => DoConvert([value, parameter], _operation);

    public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture) => values.Count < 2 ? AvaloniaProperty.UnsetValue : DoConvert(values, _operation);

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => DoConvert([value, parameter], Inverse(_operation));

    private static object DoConvert(IEnumerable<object?> values, MathOperation operation)
    {
        try
        {
            var validValues = values.NotNull().Select(x => System.Convert.ToDouble(x, CultureInfo.InvariantCulture));

            return operation switch
            {
                MathOperation.Add => validValues.Aggregate((x, y) => x + y),
                MathOperation.Divide => validValues.Aggregate((x, y) => y.NearlyEqual(0) ? 0 : x / y),
                MathOperation.Multiply => validValues.Aggregate((x, y) => x * y),
                MathOperation.Subtract => validValues.Aggregate((x, y) => x - y),
                MathOperation.Percent => validValues.Aggregate((x, y) => y.NearlyEqual(0) ? 0 : x / y * 100.00),
                MathOperation.PercentToValue => validValues.Aggregate((x, y) => x * y / 100.00),
                MathOperation.Pow => validValues.Aggregate(Math.Pow),
                MathOperation.Modulo => validValues.Aggregate((x, y) => x % y),
                _ => AvaloniaProperty.UnsetValue
            };
        }
        catch (Exception)
        {
            return AvaloniaProperty.UnsetValue;
        }
    }

    private static MathOperation Inverse(MathOperation mathOperation) => mathOperation switch
    {
        MathOperation.Add => MathOperation.Subtract,
        MathOperation.Subtract => MathOperation.Add,
        MathOperation.Multiply => MathOperation.Divide,
        MathOperation.Divide => MathOperation.Multiply,
        MathOperation.Percent => MathOperation.PercentToValue,
        MathOperation.PercentToValue => MathOperation.Percent,
        MathOperation.Pow => mathOperation,
        MathOperation.Modulo => mathOperation,
        _ => throw new InvalidOperationException()
    };
}
