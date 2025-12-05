// -----------------------------------------------------------------------
// <copyright file="AmPmRangeConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using Avalonia.Data.Converters;
using MyNet.Utilities.DateTimes;

namespace MyNet.Avalonia.Theme.Converters.Internals;

internal sealed class AmPmRangeConverter : IValueConverter
{
    public static AmPmRangeConverter Minimum { get; } = new() { IsMinimum = true };

    public static AmPmRangeConverter Maximum { get; } = new() { IsMinimum = false };

    public bool IsMinimum { get; set; }

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => value is TimeFormat format
            ? (object)(format switch
            {
                TimeFormat.TwelveHour when IsMinimum => 1,
                TimeFormat.TwelveHour when !IsMinimum => 12,
                TimeFormat.TwentyFourHour when IsMinimum => 0,
                TimeFormat.TwentyFourHour when !IsMinimum => 23,
                _ => throw new InvalidOperationException()
            })
            : throw new NotSupportedException();

    /// <inheritdoc />
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotSupportedException();
}
