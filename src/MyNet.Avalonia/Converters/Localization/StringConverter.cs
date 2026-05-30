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
using MyNet.Collections;
using MyNet.Globalization.Facade;
using MyNet.Globalization.Localization.Translation;
using MyNet.Humanizer.Facade;
using MyNet.Primitives;
using MyNet.Text;
using MyNet.Text.TextCasing;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Converters;
#pragma warning restore IDE0130

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
    public static StringConverter ToUpper { get; } = Converters[LetterCasing.Upper];

    /// <summary>
    /// Gets a converter that applies lower case to the result.
    /// </summary>
    public static StringConverter ToLower { get; } = Converters[LetterCasing.Lower];

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
        RegisterTypeConverter<ISmartEnum>((enumValue, _, _, abbreviate, culture) => ConvertSmartEnum(enumValue, abbreviate, culture));

        // Color types
        RegisterTypeConverter<Color>((color, _, _, _, _) => ConvertColor(color));
        RegisterTypeConverter<SolidColorBrush>((brush, _, _, _, _) => ConvertColor(brush.Color));

        // Date/Time types
        RegisterTypeConverter<DateTime>((date, format, _, _, culture) => ConvertDateTime(date, format, culture, GlobalizationServices.Current.CurrentTimeZone));
        RegisterTypeConverter<DateTimeOffset>((date, format, _, _, culture) => ConvertDateTime(date, format, culture, GlobalizationServices.Current.CurrentTimeZone));
        RegisterTypeConverter<DateOnly>((date, format, _, _, culture) => ConvertDateTime(date, format, culture, GlobalizationServices.Current.CurrentTimeZone));
        RegisterTypeConverter<TimeOnly>((time, format, _, _, culture) => ConvertDateTime(time, format, culture, GlobalizationServices.Current.CurrentTimeZone));

        // TimeSpan
        RegisterTypeConverter<TimeSpan>((timespan, format, _, _, culture) => ConvertTimeSpan(timespan, format, culture));

        // Array
        RegisterTypeConverter<Array>((array, _, _, _, _) => ConvertArray(array));

        // Localizable
        RegisterTypeConverter<Localizable>((localizable, format, _, abbreviate, culture) => ConvertString(localizable.Key, localizable.Filename, format, abbreviate, culture));

        RegisterTypeConverter<CultureInfo>((cultureInfo, format, _, abbreviate, culture) =>
            ConvertString(GetCultureDisplayName(cultureInfo, culture), null, format, abbreviate, culture));

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

    /// <inheritdoc />
    public override object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        var value = values.GetByIndex(0);
        if (value is not null && IsDateTimeValue(value))
        {
            var format = values.GetByIndex(1) as string ?? parameter as string ?? Format;
            var effectiveCulture = ResolveCulture(values, culture);
            var timeZone = values.OfType<TimeZoneInfo>().FirstOrDefault() ?? GlobalizationServices.Current.CurrentTimeZone;
            return ConvertDateTime(value, format, effectiveCulture, timeZone)?.ApplyCase(Casing);
        }

        return base.Convert(values, targetType, parameter, culture);
    }

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

        var translatedFormat = abbreviate ? format.Translate(x => x.WithStyle(DisplayStyle.Abbreviation), culture) : format.Translate(culture);
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

        var translatedFormat = pluralize ? format.Translate((decimal)value, abbreviate ? DisplayStyle.Abbreviation : DisplayStyle.Default, culture) : abbreviate ? format.Translate(x => x.WithStyle(DisplayStyle.Abbreviation), culture) : format.Translate(culture);
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
    private static string ConvertSmartEnum(ISmartEnum value, bool abbreviate, CultureInfo culture)
        => value.Humanize(new() { Style = abbreviate ? DisplayStyle.Abbreviation : DisplayStyle.Default }, culture);

    /// <summary>
    /// Converts an <see cref="Enum"/> value to a localized string.
    /// </summary>
    private static string ConvertEnum(Enum value, bool abbreviate, CultureInfo culture) => value.Humanize(new() { Style = abbreviate ? DisplayStyle.Abbreviation : DisplayStyle.Default }, culture);

    /// <summary>
    /// Converts a <see cref="TimeSpan"/> value to a localized string, optionally extracting a part.
    /// </summary>
    private static string? ConvertTimeSpan(TimeSpan value, string? format, CultureInfo culture)
    {
        var translatedTimeSpan = value.Humanize(x => x.UseUnits(TimeUnit.Day, TimeUnit.Year).MaxComponents(1), culture);

        return !int.TryParse(format, out var index) ? translatedTimeSpan : translatedTimeSpan.Split(" ").GetByIndex(index - 1);
    }

    /// <summary>
    /// Converts a <see cref="Color"/> value to a string representation.
    /// </summary>
    private static string ConvertColor(Color value) => value.ToName() == value.ToHex() ? value.ToHex() : $"{value.ToName()}";

    /// <summary>
    /// Converts a date/time value using the application time zone and localized date patterns.
    /// </summary>
    private static string? ConvertDateTime(object? value, string? format, CultureInfo culture, TimeZoneInfo? timeZone = null) =>
        DateTimeConverter.ToCurrent.Convert(value, format, culture, timeZone);

    private static bool IsDateTimeValue(object value) =>
        value is DateTime or DateTimeOffset or DateOnly or TimeOnly;

    /// <summary>
    /// Converts an array of strings to a single string.
    /// </summary>
    private static string ConvertArray(Array value) => string.Join(" ", value.OfType<string>());

    /// <summary>
    /// Gets a culture display name localized for the requested culture without mutating thread state.
    /// </summary>
    private static string GetCultureDisplayName(CultureInfo cultureInfo, CultureInfo displayCulture)
    {
        if (displayCulture.Equals(CultureInfo.CurrentUICulture))
            return cultureInfo.DisplayName;

        var translatedName = cultureInfo.Name.Translate(displayCulture);
        if (!string.Equals(translatedName, cultureInfo.Name, StringComparison.Ordinal))
            return translatedName;

        return cultureInfo.NativeName;
    }
}
