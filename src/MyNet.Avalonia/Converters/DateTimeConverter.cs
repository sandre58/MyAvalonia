// -----------------------------------------------------------------------
// <copyright file="DateTimeConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using MyNet.Humanizer;
using MyNet.Utilities;
using MyNet.Utilities.Helpers;
using MyNet.Utilities.Localization;

namespace MyNet.Avalonia.Converters;

public sealed class DateTimeConverter(DateTimeConverter.DateTimeConverterKind target, LetterCasing letterCasing = LetterCasing.Normal) : IValueConverter, IMultiValueConverter
{
    public enum DateTimeConverterKind
    {
        Default,

        Current,

        Local,

        Utc
    }

    public static readonly DateTimeConverter Default = new(DateTimeConverterKind.Default);
    public static readonly DateTimeConverter ToLocal = new(DateTimeConverterKind.Local);
    public static readonly DateTimeConverter ToUtc = new(DateTimeConverterKind.Utc);
    public static readonly DateTimeConverter ToCurrent = new(DateTimeConverterKind.Current);

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        DateTime? dateToConvert = value switch
        {
            DateTimeOffset dateTimeOffset => dateTimeOffset.DateTime,
            DateTime date => date,
            DateOnly date1 => date1.BeginningOfDay(),
            TimeSpan time => DateTime.Today.At(time),
            TimeOnly time1 => DateTime.Today.At(time1),
            _ => null
        };

        if (!dateToConvert.HasValue) return AvaloniaProperty.UnsetValue;

        var finalDate = target switch
        {
            DateTimeConverterKind.Utc => dateToConvert.Value.ToUniversalTime(),
            DateTimeConverterKind.Local => dateToConvert.Value.ToLocalTime(),
            DateTimeConverterKind.Current => GlobalizationService.Current.Convert(dateToConvert.Value),
            DateTimeConverterKind.Default => dateToConvert.Value,
            _ => throw new InvalidOperationException()
        };

        var format = parameter is not null ? DateTimeHelper.TranslateDatePattern(parameter.ToString().OrEmpty(), culture) : null;
        return finalDate.ToString(format, culture).ApplyCase(letterCasing);
    }

    public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture) => Convert(values.Count > 0 ? values[0] : null, targetType, values.Count > 1 ? values[1] : null, culture);

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotSupportedException();
}
