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
using MyNet.Collections;
using MyNet.Primitives;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Converters;
#pragma warning restore IDE0130

/// <summary>
/// Performs arithmetic operations on numeric binding values.
/// </summary>
/// <remarks>
/// Single-value mode uses <paramref name="parameter"/> as the second operand.
/// Multi-binding mode aggregates all values with the selected operation.
/// <see cref="ConvertBack"/> applies the inverse operation where defined (not for <see cref="Pow"/> or <see cref="Modulo"/>).
/// Returns <see cref="AvaloniaProperty.UnsetValue"/> on invalid input.
/// </remarks>
/// <example>
/// <code>
/// &lt;Slider Maximum="{Binding Total, Converter={x:Static my:MathConverter.Subtract}, ConverterParameter={Binding Used}}" /&gt;
/// </code>
/// </example>
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

    /// <summary>Gets a converter that adds all operands.</summary>
    public static MathConverter Add => new(MathOperation.Add);

    /// <summary>Gets a converter that subtracts operands sequentially.</summary>
    public static MathConverter Subtract => new(MathOperation.Subtract);

    /// <summary>Gets a converter that multiplies all operands.</summary>
    public static MathConverter Multiply => new(MathOperation.Multiply);

    /// <summary>Gets a converter that divides operands sequentially (returns 0 when dividing by zero).</summary>
    public static MathConverter Divide => new(MathOperation.Divide);

    /// <summary>Gets a converter that computes first / second × 100.</summary>
    public static MathConverter Percent => new(MathOperation.Percent);

    /// <summary>Gets a converter that converts a percentage to a value: first × second / 100.</summary>
    public static MathConverter PercentToValue => new(MathOperation.PercentToValue);

    /// <summary>Gets a converter that raises the first operand to the power of the second.</summary>
    public static MathConverter Pow => new(MathOperation.Pow);

    /// <summary>Gets a converter that computes the modulo of operands sequentially.</summary>
    public static MathConverter Modulo => new(MathOperation.Modulo);

    private MathConverter(MathOperation operation) => _operation = operation;

    /// <inheritdoc/>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => DoConvert([value, parameter], _operation);

    /// <inheritdoc/>
    public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture) => values.Count < 2 ? AvaloniaProperty.UnsetValue : DoConvert(values, _operation);

    /// <inheritdoc/>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => DoConvert([value, parameter], Inverse(_operation));

    private static object DoConvert(IEnumerable<object?> values, MathOperation operation)
    {
        try
        {
            var validValues = values.NotNull().Select(x => System.Convert.ToDouble(x, CultureInfo.InvariantCulture));

            return operation switch
            {
                MathOperation.Add => validValues.Aggregate((x, y) => x + y),
                MathOperation.Divide => validValues.Aggregate((x, y) => y.IsCloseTo(0) ? 0 : x / y),
                MathOperation.Multiply => validValues.Aggregate((x, y) => x * y),
                MathOperation.Subtract => validValues.Aggregate((x, y) => x - y),
                MathOperation.Percent => validValues.Aggregate((x, y) => y.IsCloseTo(0) ? 0 : x / y * 100.00),
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
