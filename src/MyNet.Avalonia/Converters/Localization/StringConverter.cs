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
/// Converts values to localized, formatted, and cased strings for UI display.
/// </summary>
public class StringConverter(LetterCasing casing, TranslationOptions? translationOptions = null, CultureInfo? culture = null)
    : LocalizableConverter(casing, culture)
{
    private static readonly ConcurrentDictionary<Type, Func<object, string?, TranslationOptions, CultureInfo, string?>> TypeConverters = new();

    public static readonly ReadOnlyDictionary<LetterCasing, StringConverter> Converters =
        Enum.GetValues<LetterCasing>().ToDictionary(c => c, c => new StringConverter(c)).AsReadOnly();

    public static StringConverter ToUpper { get; } = Converters[LetterCasing.Upper];

    public static StringConverter ToLower { get; } = Converters[LetterCasing.Lower];

    public static StringConverter ToTitle { get; } = Converters[LetterCasing.Title];

    public static StringConverter Default { get; } = Converters[LetterCasing.Normal];

    /// <summary>
    /// Gets or sets translation options applied to resource keys and format keys.
    /// </summary>
    public TranslationOptions TranslationOptions { get; set; } = translationOptions ?? TranslationOptionsPresets.Default;

    /// <summary>
    /// Gets or sets a value indicating whether the bound numeric value is passed as <see cref="TranslationOptions.Quantity"/>.
    /// </summary>
    public bool QuantityFromValue { get; set; }

    static StringConverter() => RegisterDefaultTypeConverters();

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

        RegisterTypeConverter<Localizable>((localizable, format, options, culture) =>
            ConvertString(localizable.Key, localizable.Filename, format, options, culture));

        RegisterTypeConverter<CultureInfo>((cultureInfo, format, options, culture) =>
            ConvertString(GetCultureDisplayName(cultureInfo, culture), null, format, options, culture));

        RegisterTypeConverter<TextBlock>((value, format, options, culture) =>
            ConvertString(value.Text.OrEmpty(), null, format, options, culture));

        RegisterTypeConverter<TextBox>((value, format, options, culture) =>
            ConvertString(value.Text.OrEmpty(), null, format, options, culture));
    }

    public static void RegisterTypeConverter<T>(Func<T, string?, TranslationOptions, CultureInfo, string?> converter) => TypeConverters[typeof(T)] = (obj, format, options, culture) => converter((T)obj, format, options, culture);

    public static void RegisterTypeConverter(Type type, Func<object, string?, TranslationOptions, CultureInfo, string?> converter) => TypeConverters[type] = converter;

    private static Func<object, string?, TranslationOptions, CultureInfo, string?>? FindTypeConverter(Type type)
        => TypeConverters.TryGetValue(type, out var converter)
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

    /// <inheritdoc />
    public override object? Convert(object? value, string? format, CultureInfo culture)
    {
        if (value == null)
            return null;

        var options = ResolveOptions(value);
        var valueType = value.GetType();
        var customConverter = FindTypeConverter(valueType);
        if (customConverter != null)
        {
            var result = customConverter(value, format, options, culture);
            return result?.ApplyCase(Casing);
        }

        var fallback = ConvertString(value.ToString().OrEmpty(), null, format, options, culture);
        return fallback.ApplyCase(Casing);
    }

    private TranslationOptions ResolveOptions(object? value)
    {
        return !QuantityFromValue || !tryGetQuantity(value, out var quantity)
            ? TranslationOptions
            : new TranslationOptionsBuilder()
                .WithStyle(TranslationOptions.Style)
                .WithQuantity(quantity, TranslationOptions.QuantityRenderingMode)
                .Build();

        static bool tryGetQuantity(object? value, out decimal quantity)
        {
            quantity = 0;

            if (value is null)
                return false;

            try
            {
                quantity = System.Convert.ToDecimal(value, CultureInfo.InvariantCulture);
                return true;
            }
            catch (InvalidCastException)
            {
                return false;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }

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

    private static string ConvertSmartEnum(ISmartEnum value, TranslationOptions options, CultureInfo culture) => value.Humanize(new() { Style = options.Style }, culture);

    private static string ConvertEnum(Enum value, TranslationOptions options, CultureInfo culture) => value.Humanize(new() { Style = options.Style }, culture);

    private static string? ConvertTimeSpan(TimeSpan value, string? format, CultureInfo culture)
    {
        var translatedTimeSpan = value.Humanize(x => x.UseUnits(TimeUnit.Day, TimeUnit.Year).MaxComponents(1), culture);
        return !int.TryParse(format, out var index) ? translatedTimeSpan : translatedTimeSpan.Split(" ").GetByIndex(index - 1);
    }

    private static string ConvertColor(Color value) => value.ToName() == value.ToHex() ? value.ToHex() : $"{value.ToName()}";

    private static string? ConvertDateTime(object? value, string? format, CultureInfo culture, TimeZoneInfo? timeZone = null) => DateTimeConverter.ToCurrent.Convert(value, format, culture, timeZone);

    private static bool IsDateTimeValue(object value) => value is DateTime or DateTimeOffset or DateOnly or TimeOnly;

    private static string ConvertArray(Array value) => string.Join(" ", value.OfType<string>());

    private static string GetCultureDisplayName(CultureInfo cultureInfo, CultureInfo displayCulture)
    {
        if (displayCulture.Equals(CultureInfo.CurrentUICulture))
            return cultureInfo.DisplayName;

        var translatedName = cultureInfo.Name.Translate(TranslationOptionsPresets.Default, displayCulture);
        return !string.Equals(translatedName, cultureInfo.Name, StringComparison.Ordinal) ? translatedName : cultureInfo.NativeName;
    }
}

/// <summary>
/// Represents a localizable resource key and optional filename for <see cref="StringConverter"/>.
/// </summary>
/// <param name="Key">The resource key to translate.</param>
/// <param name="Filename">The optional .resx filename (without extension).</param>
/// <example>
/// Bind a <see cref="Localizable"/> value through <see cref="StringConverter"/> to translate a keyed resource.
/// </example>
public record Localizable(string Key, string? Filename);
