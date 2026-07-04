// -----------------------------------------------------------------------
// <copyright file="DisplayTextResolver.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Media;
using MyNet.Collections;
using MyNet.Globalization.Facade;
using MyNet.Globalization.Localization.Translation;
using MyNet.Humanizer.Facade;
using MyNet.Observable;
using MyNet.Primitives;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Converters;
#pragma warning restore IDE0130

/// <summary>
/// Resolves values to localized display strings using a registered type converter pipeline.
/// Shared by <see cref="StringConverter"/> and non-binding consumers such as in-popup item search.
/// </summary>
public static class DisplayTextResolver
{
    private static readonly ConcurrentDictionary<Type, Func<object, string?, TranslationOptions, CultureInfo, string?>> TypeConverters = new();

    static DisplayTextResolver() => RegisterDefaultTypeConverters();

    /// <summary>
    /// Gets a value indicating whether a display converter is registered for <paramref name="type"/>.
    /// </summary>
    public static bool IsRegisteredType(Type type) => FindTypeConverter(type) is not null;

    /// <summary>
    /// Converts <paramref name="value"/> using the registered type pipeline, or falls back to translating <see cref="object.ToString"/>.
    /// </summary>
    public static string? Convert(object value, TranslationOptions options, CultureInfo culture, string? format = null)
    {
        ArgumentNullException.ThrowIfNull(value);

        var customConverter = FindTypeConverter(value.GetType());
        if (customConverter is not null)
            return customConverter(value, format, options, culture);

        return ConvertString(value.ToString().OrEmpty(), null, format, options, culture);
    }

    /// <summary>
    /// Converts <paramref name="value"/> only when a type converter is registered; otherwise returns <see langword="false"/>.
    /// </summary>
    public static bool TryConvertRegistered(object value, CultureInfo culture, out string? text)
    {
        ArgumentNullException.ThrowIfNull(value);

        var customConverter = FindTypeConverter(value.GetType());
        if (customConverter is null)
        {
            text = null;
            return false;
        }

        text = customConverter(value, null, TranslationOptionsPresets.Default, culture);
        return true;
    }

    /// <summary>
    /// Registers a display converter for <typeparamref name="T"/>.
    /// </summary>
    public static void RegisterTypeConverter<T>(Func<T, string?, TranslationOptions, CultureInfo, string?> converter) =>
        TypeConverters[typeof(T)] = (obj, format, options, culture) => converter((T)obj, format, options, culture);

    /// <summary>
    /// Registers a display converter for <paramref name="type"/>.
    /// </summary>
    public static void RegisterTypeConverter(Type type, Func<object, string?, TranslationOptions, CultureInfo, string?> converter) =>
        TypeConverters[type] = converter;

    internal static bool IsDateTimeValue(object value) => value is DateTime or DateTimeOffset or DateOnly or TimeOnly;

    internal static string? ConvertDateTime(object? value, string? format, CultureInfo culture, TimeZoneInfo? timeZone = null) =>
        DateTimeConverter.ToCurrent.Convert(value, format, culture, timeZone);

    private static void RegisterDefaultTypeConverters()
    {
        RegisterTypeConverter<string>((value, format, options, culture) => ConvertString(value, null, format, options, culture));

        RegisterTypeConverter<byte>((value, format, options, culture) => ConvertNumeric(value, format, options, culture));
        RegisterTypeConverter<int>((value, format, options, culture) => ConvertNumeric(value, format, options, culture));
        RegisterTypeConverter<long>((value, format, options, culture) => ConvertNumeric(value, format, options, culture));
        RegisterTypeConverter<float>((value, format, options, culture) => ConvertNumeric(value, format, options, culture));
        RegisterTypeConverter<double>(ConvertNumeric);

        RegisterTypeConverter<Enum>((enumValue, _, options, culture) => ConvertEnum(enumValue, options, culture));
        RegisterTypeConverter<ISmartEnum>((enumValue, _, options, culture) => ConvertSmartEnum(enumValue, options, culture));

        RegisterTypeConverter<Color>((color, _, _, _) => ConvertColor(color));
        RegisterTypeConverter<SolidColorBrush>((brush, _, _, _) => ConvertColor(brush.Color));

        RegisterTypeConverter<DateTime>((date, format, _, culture) => ConvertDateTime(date, format, culture, GlobalizationServices.Current.CurrentTimeZone));
        RegisterTypeConverter<DateTimeOffset>((date, format, _, culture) => ConvertDateTime(date, format, culture, GlobalizationServices.Current.CurrentTimeZone));
        RegisterTypeConverter<DateOnly>((date, format, _, culture) => ConvertDateTime(date, format, culture, GlobalizationServices.Current.CurrentTimeZone));
        RegisterTypeConverter<TimeOnly>((time, format, _, culture) => ConvertDateTime(time, format, culture, GlobalizationServices.Current.CurrentTimeZone));

        RegisterTypeConverter<TimeSpan>((timespan, format, _, culture) => ConvertTimeSpan(timespan, format, culture));
        RegisterTypeConverter<Array>((array, _, _, _) => ConvertArray(array));

        RegisterTypeConverter<IObservableValue<string>>((value, format, options, culture) =>
            ConvertString(value.Value.OrEmpty(), null, format, options, culture));

        RegisterTypeConverter<CultureInfo>((cultureInfo, format, options, culture) =>
            ConvertString(GetCultureDisplayName(cultureInfo, culture), null, format, options, culture));

        RegisterTypeConverter<TextBlock>((value, format, options, culture) =>
            ConvertString(value.Text.OrEmpty(), null, format, options, culture));

        RegisterTypeConverter<TextBox>((value, format, options, culture) =>
            ConvertString(value.Text.OrEmpty(), null, format, options, culture));
    }

    private static Func<object, string?, TranslationOptions, CultureInfo, string?>? FindTypeConverter(Type type) =>
        TypeConverters.TryGetValue(type, out var converter)
            ? converter
            : TypeConverters.FirstOrDefault(x => x.Key.IsAssignableFrom(type)).Value;

    private static string ConvertString(string key, string? filename, string? format, TranslationOptions options, CultureInfo culture)
    {
        if (string.IsNullOrEmpty(key))
            return key;

        var translation = string.IsNullOrEmpty(filename)
            ? key.Translate(options, culture)
            : key.Translate(options, filename, culture);

        if (string.IsNullOrEmpty(format))
            return translation;

        var translatedFormat = format.Translate(options, culture);

        try
        {
            return translatedFormat.FormatWith(culture, translation);
        }
        catch (FormatException ex)
        {
            throw new FormatException($"Invalid format string: {translatedFormat}", ex);
        }
    }

    private static string? ConvertNumeric(double value, string? format, TranslationOptions options, CultureInfo culture)
    {
        if (double.IsNaN(value))
            return null;

        if (string.IsNullOrEmpty(format))
            return value.ToString(culture);

        var translatedFormat = format.Translate(options, culture);

        try
        {
            return value.ToString(translatedFormat, culture);
        }
        catch (FormatException ex)
        {
            throw new FormatException($"Invalid format string: {translatedFormat}", ex);
        }
    }

    private static string ConvertSmartEnum(ISmartEnum value, TranslationOptions options, CultureInfo culture) =>
        value.Humanize(new() { Style = options.Style }, culture);

    private static string ConvertEnum(Enum value, TranslationOptions options, CultureInfo culture) =>
        value.Humanize(new() { Style = options.Style }, culture);

    private static string? ConvertTimeSpan(TimeSpan value, string? format, CultureInfo culture)
    {
        var translatedTimeSpan = value.Humanize(x => x.UseUnits(TimeUnit.Day, TimeUnit.Year).MaxComponents(1), culture);
        return !int.TryParse(format, out var index) ? translatedTimeSpan : translatedTimeSpan.Split(" ").GetByIndex(index - 1);
    }

    private static string ConvertColor(Color value) => value.ToName() == value.ToHex() ? value.ToHex() : $"{value.ToName()}";

    private static string ConvertArray(Array value) => string.Join(" ", value.OfType<string>());

    private static string GetCultureDisplayName(CultureInfo cultureInfo, CultureInfo displayCulture)
    {
        if (displayCulture.Equals(CultureInfo.CurrentUICulture))
            return cultureInfo.DisplayName;

        var translatedName = cultureInfo.Name.Translate(TranslationOptionsPresets.Default, displayCulture);
        return !string.Equals(translatedName, cultureInfo.Name, StringComparison.Ordinal) ? translatedName : cultureInfo.NativeName;
    }
}
