// -----------------------------------------------------------------------
// <copyright file="StringConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Media;
using MyNet.Avalonia.Extensions;
using MyNet.Humanizer;
using MyNet.Utilities;
using MyNet.Utilities.Units;

namespace MyNet.Avalonia.Converters;

/// <summary>
/// Converts values to localized, formatted, pluralized, and cased strings for UI display.
/// Supports enums, colors, dates, timespans, numbers, and custom formatting.
/// </summary>
/// <remarks>
/// This converter is designed for advanced localization scenarios in Avalonia applications.
/// It supports pluralization, abbreviation, casing, and custom format strings, and can be used in both single and multi-value bindings.
/// You can also specify a custom culture for localization and register custom type converters.
/// </remarks>
/// <remarks>
/// Initializes a new instance of the <see cref="StringConverter"/> class.
/// </remarks>
/// <param name="casing">The casing to apply to the result.</param>
/// <param name="pluralize">Whether to apply pluralization rules.</param>
/// <param name="abbreviate">Whether to use abbreviated translations.</param>
/// <param name="culture">A custom culture to use for localization. If null, uses the provided culture parameter.</param>
public class StringConverter(LetterCasing casing, bool pluralize = false, bool abbreviate = false, CultureInfo? culture = null) : LocalizableConverter(casing, culture)
{
    private static readonly ConcurrentDictionary<Type, Func<object, string?, bool, bool, CultureInfo, string?>> TypeConverters = new();

    public static readonly ReadOnlyDictionary<LetterCasing, StringConverter> Converters = Enum.GetValues<LetterCasing>().ToDictionary(c => c, c => new StringConverter(c)).AsReadOnly();

    /// <summary>
    /// Gets a converter that applies upper case to the result.
    /// </summary>
    public static StringConverter ToUpper { get; } = Converters[LetterCasing.AllCaps];

    /// <summary>
    /// Gets a converter that applies lower case to the result.
    /// </summary>
    public static StringConverter ToLower { get; } = Converters[LetterCasing.LowerCase];

    /// <summary>
    /// Gets a converter that applies title case to the result.
    /// </summary>
    public static StringConverter ToTitle { get; } = Converters[LetterCasing.Title];

    /// <summary>
    /// Gets the default converter (normal casing).
    /// </summary>
    public static StringConverter Default { get; } = Converters[LetterCasing.Normal];

    /// <summary>
    /// Gets or sets a value indicating whether to apply pluralization rules.
    /// </summary>
    public bool Pluralize { get; set; } = pluralize;

    /// <summary>
    /// Gets or sets a value indicating whether to use abbreviated translations for enums and enumerations.
    /// </summary>
    public bool Abbreviate { get; set; } = abbreviate;

    static StringConverter() => RegisterDefaultTypeConverters();

    /// <summary>
    /// Registers default type converters for built-in types.
    /// </summary>
    private static void RegisterDefaultTypeConverters()
    {
        // String
        RegisterTypeConverter<string>((value, format, _, abbreviate, culture) => ConvertString(value, null, format, abbreviate, culture));

        // Numeric types
        RegisterTypeConverter<byte>((value, format, pluralize, abbreviate, culture) => ConvertDouble(value, format, pluralize, abbreviate, culture));
        RegisterTypeConverter<int>((value, format, pluralize, abbreviate, culture) => ConvertDouble(value, format, pluralize, abbreviate, culture));
        RegisterTypeConverter<long>((value, format, pluralize, abbreviate, culture) => ConvertDouble(value, format, pluralize, abbreviate, culture));
        RegisterTypeConverter<float>((value, format, pluralize, abbreviate, culture) => ConvertDouble(value, format, pluralize, abbreviate, culture));
        RegisterTypeConverter<double>(ConvertDouble);

        // Enum types
        RegisterTypeConverter<Enum>((enumValue, _, _, abbreviate, culture) => ConvertEnum(enumValue, abbreviate, culture));
        RegisterTypeConverter<IEnumeration>((enumValue, _, _, abbreviate, culture) => ConvertEnumeration(enumValue, abbreviate, culture));

        // Color types
        RegisterTypeConverter<Color>((color, _, _, _, _) => ConvertColor(color));
        RegisterTypeConverter<SolidColorBrush>((brush, _, _, _, _) => ConvertColor(brush.Color));

        // Date/Time types
        RegisterTypeConverter<DateTime>((date, format, _, _, culture) => ConvertDateTime(date, format, culture));
        RegisterTypeConverter<DateTimeOffset>((date, format, _, _, culture) => ConvertDateTime(date, format, culture));
        RegisterTypeConverter<DateOnly>((date, format, _, _, culture) => ConvertDateTime(date, format, culture));
        RegisterTypeConverter<TimeOnly>((time, format, _, _, culture) => ConvertDateTime(time, format, culture));

        // TimeSpan
        RegisterTypeConverter<TimeSpan>((timespan, format, _, _, culture) => ConvertTimeSpan(timespan, format, culture));

        // Array
        RegisterTypeConverter<Array>((array, _, _, _, _) => ConvertArray(array));

        // Localizable
        RegisterTypeConverter<Localizable>((localizable, format, _, abbreviate, culture) => ConvertString(localizable.Key, localizable.Filename, format, abbreviate, culture));

        // Controls
        RegisterTypeConverter<TextBlock>((value, format, _, _, culture) => ConvertString(value.Text.OrEmpty(), null, format, false, culture));
        RegisterTypeConverter<TextBox>((value, format, _, _, culture) => ConvertString(value.Text.OrEmpty(), null, format, false, culture));
    }

    /// <summary>
    /// Registers a custom type converter for a specific type.
    /// </summary>
    /// <typeparam name="T">The type to register a converter for.</typeparam>
    /// <param name="converter">The converter function that takes (value, format, culture) and returns a string.</param>
    public static void RegisterTypeConverter<T>(Func<T, string?, bool, bool, CultureInfo, string?> converter) => TypeConverters[typeof(T)] = (obj, format, pluralize, abbreviate, culture) => converter((T)obj, format, pluralize, abbreviate, culture);

    /// <summary>
    /// Registers a custom type converter for a specific type.
    /// </summary>
    /// <param name="type">The type to register a converter for.</param>
    /// <param name="converter">The converter function that takes (value, format, culture) and returns a string.</param>
    public static void RegisterTypeConverter(Type type, Func<object, string?, bool, bool, CultureInfo, string?> converter) => TypeConverters[type] = converter;

    /// <summary>
    /// Finds a registered type converter for a given type, including base types and interfaces.
    /// </summary>
    /// <param name="type">The type to find a converter for.</param>
    /// <returns>The converter function if found, otherwise null.</returns>
    private static Func<object, string?, bool, bool, CultureInfo, string?>? FindTypeConverter(Type type) =>
        TypeConverters.TryGetValue(type, out var converter)
            ? converter
            : TypeConverters.FirstOrDefault(x => x.Key.IsAssignableFrom(type)).Value;

    /// <summary>
    /// Converts a value to a localized, formatted string with a specific format.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="format">The format string to use.</param>
    /// <param name="culture">The culture to use for localization.</param>
    /// <returns>The converted string value.</returns>
    /// <exception cref="FormatException">Thrown if the format string is invalid.</exception>
    public override object? Convert(object? value, string? format, CultureInfo culture)
    {
        if (value == null) return null;

        // Check for custom type converter first (including inheritance)
        var valueType = value.GetType();
        var customConverter = FindTypeConverter(valueType);
        if (customConverter != null)
        {
            var result = customConverter(value, format, Pluralize, Abbreviate, culture);
            return result?.ApplyCase(Casing);
        }

        var fallback = ConvertString(value.ToString().OrEmpty(), null, format, Abbreviate, culture);

        return fallback.ApplyCase(Casing);
    }

    /// <summary>
    /// Converts a string key to a localized and formatted string.
    /// </summary>
    /// <param name="key">The resource key.</param>
    /// <param name="filename">The resource filename (optional).</param>
    /// <param name="format">The format string (optional).</param>
    /// <param name="abbreviate">Whether to abbreviate the output.</param>
    /// <param name="culture">The culture to use.</param>
    /// <returns>The localized and formatted string.</returns>
    private static string ConvertString(string key, string? filename, string? format, bool abbreviate, CultureInfo culture)
    {
        if (string.IsNullOrEmpty(key)) return key;

        var translation = !string.IsNullOrEmpty(filename) ? key.Translate(filename, culture) : key.Translate(culture);

        if (string.IsNullOrEmpty(format)) return translation;

        var translatedFormat = abbreviate ? format.TranslateAbbreviated(culture) : format.Translate(culture);
        try
        {
            return translatedFormat.FormatWith(culture, translation);
        }
        catch (FormatException ex)
        {
            throw new FormatException($"Invalid format string: {translatedFormat}", ex);
        }
    }

    /// <summary>
    /// Converts a double value to a localized and formatted string.
    /// </summary>
    /// <param name="value">The double value.</param>
    /// <param name="format">The format string.</param>
    /// <param name="pluralize">Whether to pluralize the output.</param>
    /// <param name="abbreviate">Whether to abbreviate the output.</param>
    /// <param name="culture">The culture to use.</param>
    /// <returns>The formatted string.</returns>
    private static string? ConvertDouble(double value, string? format, bool pluralize, bool abbreviate, CultureInfo culture)
    {
        if (double.IsNaN(value)) return null;
        if (string.IsNullOrEmpty(format)) return value.ToString(culture);

        var translatedFormat = pluralize ? format.TranslateWithCount(value, abbreviate, culture) : abbreviate ? format.TranslateAbbreviated(culture) : format.Translate(culture);
        try
        {
            return value.ToString(translatedFormat, culture);
        }
        catch (FormatException ex)
        {
            throw new FormatException($"Invalid format string: {translatedFormat}", ex);
        }
    }

    /// <summary>
    /// Converts an <see cref="IEnumeration"/> value to a localized string.
    /// </summary>
    private static string? ConvertEnumeration(IEnumeration value, bool abbreviate, CultureInfo culture) => value.Humanize(abbreviate, culture);

    /// <summary>
    /// Converts an <see cref="Enum"/> value to a localized string.
    /// </summary>
    private static string? ConvertEnum(Enum value, bool abbreviate, CultureInfo culture) => value.Humanize(abbreviate, culture);

    /// <summary>
    /// Converts a <see cref="TimeSpan"/> value to a localized string, optionally extracting a part.
    /// </summary>
    private static string? ConvertTimeSpan(TimeSpan value, string? format, CultureInfo culture)
    {
        var translatedTimeSpan = value.Humanize(1, TimeUnit.Year, TimeUnit.Day, culture: culture);

        return !int.TryParse(format, out var index) ? translatedTimeSpan : translatedTimeSpan.Split(" ").GetByIndex(index - 1);
    }

    /// <summary>
    /// Converts a <see cref="Color"/> value to a string representation.
    /// </summary>
    private static string ConvertColor(Color value) => value.ToName() == value.ToHex() ? value.ToHex() : $"{value.ToName()}";

    /// <summary>
    /// Converts a date/time value to a localized string.
    /// </summary>
    private static string? ConvertDateTime(object? value, string? format, CultureInfo culture) => DateTimeConverter.Default.Convert(value, format, culture)?.ToString();

    /// <summary>
    /// Converts an array of strings to a single string.
    /// </summary>
    private static string ConvertArray(Array value) => string.Join(" ", value.OfType<string>());
}

/// <summary>
/// Represents a localizable resource key and optional filename.
/// </summary>
public record Localizable(string Key, string? Filename);
