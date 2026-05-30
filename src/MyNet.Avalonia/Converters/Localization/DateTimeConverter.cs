// -----------------------------------------------------------------------
// <copyright file="DateTimeConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using MyNet.Collections;
using MyNet.Globalization.Facade;
using MyNet.Primitives;
using MyNet.Text.TextCasing;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Converters;
#pragma warning restore IDE0130

/// <summary>
/// Specifies the kind of date/time conversion to apply.
/// </summary>
public enum DateTimeConverterKind
{
    /// <summary>
    /// No conversion; uses the original value.
    /// </summary>
    Default,

    /// <summary>
    /// Converts to the current culture's date/time.
    /// </summary>
    Current,

    /// <summary>
    /// Converts to local time.
    /// </summary>
    Local,

    /// <summary>
    /// Converts to UTC time.
    /// </summary>
    Utc
}

/// <summary>
/// Converts date and time values to localized, formatted strings for UI display.
/// Supports conversion to local, UTC, or current culture time, and custom time zones.
/// </summary>
/// <remarks>
/// This converter is designed for advanced date/time localization scenarios in Avalonia applications.
/// It supports formatting, casing, and time zone conversion, and can be used in both single and multi-value bindings.
/// </remarks>
/// <example>
/// <code>
/// <!-- Usage in XAML -->
/// <TextBlock Text="{Binding MyDate, Converter={x:Static my:DateTimeConverter.Default}}" />
/// <TextBlock Text="{Binding MyDate, Converter={x:Static my:DateTimeConverter.ToLocal}}" />
/// <TextBlock Text="{Binding MyDate, Converter={x:Static my:DateTimeConverter.ToUtc}}" />
/// </code>
/// </example>
/// <remarks>
/// Initializes a new instance of the <see cref="DateTimeConverter"/> class.
/// </remarks>
/// <param name="dateTimeConverterKind">The kind of date/time conversion to apply.</param>
/// <param name="casing">The casing to apply to the result.</param>
/// <param name="culture">A custom culture to use for localization. If null, uses the provided culture parameter.</param>
public sealed class DateTimeConverter(DateTimeConverterKind dateTimeConverterKind = DateTimeConverterKind.Default, LetterCasing casing = LetterCasing.Normal, CultureInfo? culture = null) : LocalizableConverter(casing, culture)
{
    private static readonly ReadOnlyDictionary<DateTimeConverterKind, DateTimeConverter> Converters = Enum.GetValues<DateTimeConverterKind>().ToDictionary(c => c, c => new DateTimeConverter(c)).AsReadOnly();

    /// <summary>
    /// Gets the default date/time converter (no conversion).
    /// </summary>
    public static readonly DateTimeConverter Default = Converters[DateTimeConverterKind.Default];

    /// <summary>
    /// Gets a converter that converts to local time.
    /// </summary>
    public static readonly DateTimeConverter ToLocal = Converters[DateTimeConverterKind.Local];

    /// <summary>
    /// Gets a converter that converts to UTC time.
    /// </summary>
    public static readonly DateTimeConverter ToUtc = Converters[DateTimeConverterKind.Utc];

    /// <summary>
    /// Gets a converter that converts to the current culture's date/time.
    /// </summary>
    public static readonly DateTimeConverter ToCurrent = Converters[DateTimeConverterKind.Current];

    /// <summary>
    /// Gets or sets the custom time zone to use for conversion. If null, uses the default or specified kind.
    /// </summary>
    public TimeZoneInfo? TimeZone { get; set; }

    /// <summary>
    /// Converts multiple values to a localized, formatted date/time string.
    /// </summary>
    /// <param name="values">The values to convert (date/time, format, culture, time zone).</param>
    /// <param name="targetType">The target type.</param>
    /// <param name="parameter">The format string or additional parameter.</param>
    /// <param name="culture">The culture to use for localization.</param>
    /// <returns>The converted string value.</returns>
    public override object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        var value = values.GetByIndex(0);
        var format = values.GetByIndex(1) as string ?? parameter as string ?? Format;
        var effectiveCulture = values.OfType<CultureInfo>().FirstOrDefault() ?? Culture ?? culture;
        var effectiveTimeZone = values.OfType<TimeZoneInfo>().FirstOrDefault() ?? TimeZone;

        try
        {
            return Convert(value, format, effectiveCulture, effectiveTimeZone);
        }
        catch (FormatException ex)
        {
            return $"[Format error: {ex.Message}]";
        }
    }

    /// <summary>
    /// Converts a value to a localized, formatted date/time string.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="format">The format string to use.</param>
    /// <param name="culture">The culture to use for localization.</param>
    /// <returns>The converted string value.</returns>
    public override object? Convert(object? value, string? format, CultureInfo culture) => Convert(value, format, culture, TimeZone);

    /// <summary>
    /// Converts a value to a localized, formatted date/time string, with a custom time zone.
    /// </summary>
    /// <param name="value">The value to convert (DateTime, DateTimeOffset, DateOnly, TimeSpan, TimeOnly).</param>
    /// <param name="format">The format string to use.</param>
    /// <param name="culture">The culture to use for localization.</param>
    /// <param name="customTimeZone">The custom time zone to use for conversion.</param>
    /// <returns>The converted string value.</returns>
    public string? Convert(object? value, string? format, CultureInfo culture, TimeZoneInfo? customTimeZone)
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

        if (!dateToConvert.HasValue) return null;

        var effectiveDate = dateTimeConverterKind switch
        {
            DateTimeConverterKind.Current => GlobalizationServices.Current.FromUtc(dateToConvert.Value),
            DateTimeConverterKind.Local => dateToConvert.Value.ToLocalTime(),
            DateTimeConverterKind.Utc => dateToConvert.Value.ToUniversalTime(),
            _ => customTimeZone is not null ? GlobalizationServices.Current.Convert(dateToConvert.Value, TimeZone, customTimeZone) : dateToConvert.Value
        };

        var translatedFormat = !string.IsNullOrEmpty(format) ? format.TranslateDatePattern(culture) : null;
        return effectiveDate.ToString(translatedFormat, culture);
    }
}
