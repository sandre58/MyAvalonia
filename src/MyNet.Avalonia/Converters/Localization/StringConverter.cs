// -----------------------------------------------------------------------
// <copyright file="StringConverter.cs" company="Stéphane ANDRE">
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
using MyNet.Globalization.Localization.Translation;
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

    /// <summary>
    /// Registers a display converter for <typeparamref name="T"/>.
    /// </summary>
    public static void RegisterTypeConverter<T>(Func<T, string?, TranslationOptions, CultureInfo, string?> converter) =>
        DisplayTextResolver.RegisterTypeConverter(converter);

    /// <summary>
    /// Registers a display converter for <paramref name="type"/>.
    /// </summary>
    public static void RegisterTypeConverter(Type type, Func<object, string?, TranslationOptions, CultureInfo, string?> converter) =>
        DisplayTextResolver.RegisterTypeConverter(type, converter);

    /// <inheritdoc />
    public override object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        var value = values.GetByIndex(0);
        if (value is not null && DisplayTextResolver.IsDateTimeValue(value))
        {
            var format = values.GetByIndex(1) as string ?? parameter as string ?? Format;
            var effectiveCulture = ResolveCulture(values, culture);
            var timeZone = values.OfType<TimeZoneInfo>().FirstOrDefault() ?? GlobalizationServices.Current.CurrentTimeZone;
            return DisplayTextResolver.ConvertDateTime(value, format, effectiveCulture, timeZone)?.ApplyCase(Casing);
        }

        return base.Convert(values, targetType, parameter, culture);
    }

    /// <inheritdoc />
    public override object? Convert(object? value, string? format, CultureInfo culture)
    {
        if (value == null)
            return null;

        var options = ResolveOptions(value);
        var result = DisplayTextResolver.Convert(value, options, culture, format);
        return result?.ApplyCase(Casing);
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
}
