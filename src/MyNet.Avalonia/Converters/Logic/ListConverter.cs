// -----------------------------------------------------------------------
// <copyright file="ListConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia;
using Avalonia.Data.Converters;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Converters;
#pragma warning restore IDE0130

/// <summary>
/// Compares collection counts or numeric values and returns a boolean result.
/// </summary>
/// <remarks>
/// When the bound value is an <see cref="IEnumerable"/>, the item count is compared.
/// Optional predicates filter items before counting (for example, only <c>true</c> values).
/// </remarks>
/// <remarks>
/// Predefined instances: <see cref="Any"/> (count &gt; 0), <see cref="One"/> (count = 1), <see cref="Many"/> (count &gt; 1),
/// and predicate variants such as <see cref="AnyTrue"/> / <see cref="OneFalse"/>.
/// <see cref="ToList"/> converts an integer count into a placeholder enumerable for item generation.
/// </remarks>
/// <example>
/// <code>
/// &lt;Expander IsVisible="{Binding Items.Count, Converter={x:Static my:ListConverter.Any}}" /&gt;
/// </code>
/// </example>
public sealed class ListConverter : IValueConverter, IMultiValueConverter
{
    private static readonly Func<object?, bool>? TruePredicate = x => x is true;
    private static readonly Func<object?, bool>? FalsePredicate = x => x is false;

    private readonly MathComparisonConverter _converter;
    private readonly int? _parameter;
    private readonly Func<object?, bool>? _predicate;

    /// <summary>Converts an integer count into a sequence of placeholder items.</summary>
    public static readonly IValueConverter ToList = new FuncValueConverter<int?, IEnumerable>(count => Enumerable.Repeat(new object(), count ?? 0));

    /// <summary>Returns <c>true</c> when count is greater than 0.</summary>
    public static readonly ListConverter Any = new(MathComparisonConverter.IsGreaterThan, 0);

    /// <summary>Returns <c>true</c> when count is 0.</summary>
    public static readonly ListConverter NotAny = new(MathComparisonConverter.IsLessThan, 1);

    /// <summary>Returns <c>true</c> when count is greater than 1.</summary>
    public static readonly ListConverter Many = new(MathComparisonConverter.IsGreaterThan, 1);

    /// <summary>Returns <c>true</c> when count is less than 2.</summary>
    public static readonly ListConverter NotMany = new(MathComparisonConverter.IsLessThan, 2);

    /// <summary>Returns <c>true</c> when count equals 1.</summary>
    public static readonly ListConverter One = new(MathComparisonConverter.IsEqualsTo, 1);

    /// <summary>Returns <c>true</c> when count is greater than a specified parameter (default 1).</summary>
    public static readonly ListConverter HasGreaterThan = new(MathComparisonConverter.IsGreaterThan);

    /// <summary>
    /// Returns <c>true</c> when count is less than a specified parameter (default 1).
    /// </summary>
    public static readonly ListConverter HasLessThan = new(MathComparisonConverter.IsLessThan);

    /// <summary>
    /// Returns <c>true</c> when count equals to a specified parameter (default 1).
    /// </summary>
    public static readonly ListConverter Has = new(MathComparisonConverter.IsEqualsTo);

    /// <summary>
    /// Returns <c>true</c> when the count of items matching the predicate is greater than 0, effectively checking if any items match.
    /// </summary>
    public static readonly ListConverter AnyTrue = new(MathComparisonConverter.IsGreaterThan, 0, TruePredicate);

    /// <summary>
    /// Returns <c>true</c> when the count of items matching the predicate is less than 1, effectively checking if no items match.
    /// </summary>
    public static readonly ListConverter NotAnyTrue = new(MathComparisonConverter.IsLessThan, 1, TruePredicate);

    /// <summary>
    /// Returns <c>true</c> when the count of items matching the predicate is greater than 1, effectively checking if multiple items match.
    /// </summary>
    public static readonly ListConverter ManyTrue = new(MathComparisonConverter.IsGreaterThan, 1, TruePredicate);

    /// <summary>
    /// Returns <c>true</c> when the count of items matching the predicate is less than 2, effectively checking if at most one item matches.
    /// </summary>
    public static readonly ListConverter NotManyTrue = new(MathComparisonConverter.IsLessThan, 2, TruePredicate);

    /// <summary>
    /// Returns <c>true</c> when the count of items matching the predicate equals 1, effectively checking if exactly one item matches.
    /// </summary>
    public static readonly ListConverter OneTrue = new(MathComparisonConverter.IsEqualsTo, 1, TruePredicate);

    /// <summary>
    /// Returns <c>true</c> when the count of items matching the predicate is greater than a specified parameter (default 1), effectively checking if any, all, or a specific number of items match.
    /// </summary>
    public static readonly ListConverter HasTrueGreaterThan = new(MathComparisonConverter.IsGreaterThan, predicate: TruePredicate);

    /// <summary>
    /// Returns <c>true</c> when the count of items matching the predicate is less than a specified parameter (default 1), effectively checking if no, not all, or a specific number of items match.
    /// </summary>
    public static readonly ListConverter HasTrueLessThan = new(MathComparisonConverter.IsLessThan, predicate: TruePredicate);

    /// <summary>
    /// Returns <c>true</c> when the count of items matching the predicate equals a specified parameter (default 1), effectively checking if any, all, or a specific number of items match.
    /// </summary>
    public static readonly ListConverter HasTrue = new(MathComparisonConverter.IsEqualsTo, predicate: TruePredicate);

    /// <summary>
    /// Returns <c>true</c> when the count of items matching the predicate is greater than 0, effectively checking if any items match.
    /// </summary>
    public static readonly ListConverter AnyFalse = new(MathComparisonConverter.IsGreaterThan, 0, FalsePredicate);

    /// <summary>
    /// Returns <c>true</c> when the count of items matching the predicate is less than 1, effectively checking if no items match.
    /// </summary>
    public static readonly ListConverter NotAnyFalse = new(MathComparisonConverter.IsLessThan, 1, FalsePredicate);

    /// <summary>
    /// Returns <c>true</c> when the count of items matching the predicate is greater than 1, effectively checking if multiple items match.
    /// </summary>
    public static readonly ListConverter ManyFalse = new(MathComparisonConverter.IsGreaterThan, 1, FalsePredicate);

    /// <summary>
    /// Returns <c>true</c> when the count of items matching the predicate is less than 2, effectively checking if at most one item matches.
    /// </summary>
    public static readonly ListConverter NotManyFalse = new(MathComparisonConverter.IsLessThan, 2, FalsePredicate);

    /// <summary>
    /// Returns <c>true</c> when the count of items matching the predicate equals 1, effectively checking if exactly one item matches.
    /// </summary>
    public static readonly ListConverter OneFalse = new(MathComparisonConverter.IsEqualsTo, 1, FalsePredicate);

    /// <summary>
    /// Returns <c>true</c> when the count of items matching the predicate is greater than a specified parameter (default 1), effectively checking if any, all, or a specific number of items match.
    /// </summary>
    public static readonly ListConverter HasFalseGreaterThan = new(MathComparisonConverter.IsGreaterThan, predicate: FalsePredicate);

    /// <summary>
    /// Returns <c>true</c> when the count of items matching the predicate is less than a specified parameter (default 1), effectively checking if no, not all, or a specific number of items match.
    /// </summary>
    public static readonly ListConverter HasFalseLessThan = new(MathComparisonConverter.IsLessThan, predicate: FalsePredicate);

    /// <summary>
    /// Returns <c>true</c> when the count of items matching the predicate equals a specified parameter (default 1), effectively checking if any, all, or a specific number of items match.
    /// </summary>
    public static readonly ListConverter HasFalse = new(MathComparisonConverter.IsEqualsTo, predicate: FalsePredicate);

    private ListConverter(MathComparisonConverter converter, int? parameter = null, Func<object?, bool>? predicate = null)
    {
        _converter = converter;
        _parameter = parameter;
        _predicate = predicate;
    }

    /// <inheritdoc/>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => _converter.Convert(value is IEnumerable items ? items.OfType<object?>().Count(x => _predicate?.Invoke(x) ?? true) : value, targetType, _parameter ?? parameter ?? 1, culture);

    /// <inheritdoc/>
    public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture) => _converter.Convert(values.Count(x => _predicate?.Invoke(x) ?? true), targetType, _parameter ?? parameter ?? 1, culture);

    /// <inheritdoc/>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => AvaloniaProperty.UnsetValue;
}
