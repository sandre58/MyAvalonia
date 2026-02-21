// -----------------------------------------------------------------------
// <copyright file="CountConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace MyNet.Avalonia.Converters;

/// <summary>
/// Converts a null value to Visibility.Visible and any other value to Visibility.Collapsed.
/// </summary>
public sealed class CountConverter
    : IValueConverter
{
    private readonly MathComparisonConverter _converter;
    private readonly int _parameter;

    public static readonly CountConverter Any = new(MathComparisonConverter.IsGreaterThan, 0);
    public static readonly CountConverter NotAny = new(MathComparisonConverter.IsLessThan, 1);
    public static readonly CountConverter Many = new(MathComparisonConverter.IsGreaterThan, 1);
    public static readonly CountConverter NotMany = new(MathComparisonConverter.IsLessThan, 2);
    public static readonly CountConverter One = new(MathComparisonConverter.IsEqualsTo, 1);

    private CountConverter(MathComparisonConverter converter, int parameter)
    {
        _converter = converter;
        _parameter = parameter;
    }

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => _converter.Convert(value, targetType, _parameter, culture);

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => _converter.ConvertBack(value, targetType, _parameter, culture);
}
