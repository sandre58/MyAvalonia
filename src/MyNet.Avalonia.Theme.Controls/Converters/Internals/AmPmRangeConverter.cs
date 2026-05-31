// -----------------------------------------------------------------------
// <copyright file="AmPmRangeConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using Avalonia.Data.Converters;
using MyNet.Primitives.Temporal;

namespace MyNet.Avalonia.Theme.Controls.Converters.Internals;

internal sealed class AmPmRangeConverter : IValueConverter
{
    private readonly bool _isMinimum;

    private AmPmRangeConverter(bool isMinimum) => _isMinimum = isMinimum;

    public static AmPmRangeConverter Minimum { get; } = new(true);

    public static AmPmRangeConverter Maximum { get; } = new(false);

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => value is TimeFormat format
        ? (object)(format switch
        {
            TimeFormat.TwelveHour when _isMinimum => 1,
            TimeFormat.TwelveHour when !_isMinimum => 12,
            TimeFormat.TwentyFourHour when _isMinimum => 0,
            TimeFormat.TwentyFourHour when !_isMinimum => 23,
            _ => throw new InvalidOperationException()
        })
        : throw new NotSupportedException();

    /// <inheritdoc />
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotSupportedException();
}
