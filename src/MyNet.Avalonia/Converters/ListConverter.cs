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

namespace MyNet.Avalonia.Converters;

/// <summary>
/// Converts a null value to Visibility.Visible and any other value to Visibility.Collapsed.
/// </summary>
public sealed class ListConverter : IValueConverter, IMultiValueConverter
{
    private static readonly Func<object?, bool>? TruePredicate = x => x is true;
    private static readonly Func<object?, bool>? FalsePredicate = x => x is false;

    private readonly MathComparisonConverter _converter;
    private readonly int? _parameter;
    private readonly Func<object?, bool>? _predicate;

    public static readonly IValueConverter ToList = new FuncValueConverter<int?, IEnumerable>(count => Enumerable.Repeat(new object(), count ?? 0));

    public static readonly ListConverter Any = new(MathComparisonConverter.IsGreaterThan, 0);
    public static readonly ListConverter NotAny = new(MathComparisonConverter.IsLessThan, 1);
    public static readonly ListConverter Many = new(MathComparisonConverter.IsGreaterThan, 1);
    public static readonly ListConverter NotMany = new(MathComparisonConverter.IsLessThan, 2);
    public static readonly ListConverter One = new(MathComparisonConverter.IsEqualsTo, 1);
    public static readonly ListConverter HasGreaterThan = new(MathComparisonConverter.IsGreaterThan);
    public static readonly ListConverter HasLessThan = new(MathComparisonConverter.IsLessThan);
    public static readonly ListConverter Has = new(MathComparisonConverter.IsEqualsTo);

    public static readonly ListConverter AnyTrue = new(MathComparisonConverter.IsGreaterThan, 0, TruePredicate);
    public static readonly ListConverter NotAnyTrue = new(MathComparisonConverter.IsLessThan, 1, TruePredicate);
    public static readonly ListConverter ManyTrue = new(MathComparisonConverter.IsGreaterThan, 1, TruePredicate);
    public static readonly ListConverter NotManyTrue = new(MathComparisonConverter.IsLessThan, 2, TruePredicate);
    public static readonly ListConverter OneTrue = new(MathComparisonConverter.IsEqualsTo, 1, TruePredicate);
    public static readonly ListConverter HasTrueGreaterThan = new(MathComparisonConverter.IsGreaterThan, predicate: TruePredicate);
    public static readonly ListConverter HasTrueLessThan = new(MathComparisonConverter.IsLessThan, predicate: TruePredicate);
    public static readonly ListConverter HasTrue = new(MathComparisonConverter.IsEqualsTo, predicate: TruePredicate);

    public static readonly ListConverter AnyFalse = new(MathComparisonConverter.IsGreaterThan, 0, FalsePredicate);
    public static readonly ListConverter NotAnyFalse = new(MathComparisonConverter.IsLessThan, 1, FalsePredicate);
    public static readonly ListConverter ManyFalse = new(MathComparisonConverter.IsGreaterThan, 1, FalsePredicate);
    public static readonly ListConverter NotManyFalse = new(MathComparisonConverter.IsLessThan, 2, FalsePredicate);
    public static readonly ListConverter OneFalse = new(MathComparisonConverter.IsEqualsTo, 1, FalsePredicate);
    public static readonly ListConverter HasFalseGreaterThan = new(MathComparisonConverter.IsGreaterThan, predicate: FalsePredicate);
    public static readonly ListConverter HasFalseLessThan = new(MathComparisonConverter.IsLessThan, predicate: FalsePredicate);
    public static readonly ListConverter HasFalse = new(MathComparisonConverter.IsEqualsTo, predicate: FalsePredicate);

    private ListConverter(MathComparisonConverter converter, int? parameter = null, Func<object?, bool>? predicate = null)
    {
        _converter = converter;
        _parameter = parameter;
        _predicate = predicate;
    }

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => _converter.Convert(value is IEnumerable items ? items.OfType<object?>().Count(x => _predicate?.Invoke(x) ?? true) : value, targetType, _parameter ?? parameter ?? 1, culture);

    public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        => _converter.Convert(values.Count(x => _predicate?.Invoke(x) ?? true), targetType, _parameter ?? parameter ?? 1, culture);

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => AvaloniaProperty.UnsetValue;
}
