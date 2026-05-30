// -----------------------------------------------------------------------
// <copyright file="LocalizableConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;
using MyNet.Collections;
using MyNet.Text.TextCasing;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Converters;
#pragma warning restore IDE0130

/// <summary>
/// Abstract base class for converters that produce localized, formatted, pluralized, and cased strings for UI display.
/// Supports custom casing, formatting, and culture selection. Used as a base for advanced localization converters in Avalonia.
/// </summary>
/// <remarks>
/// This class provides a flexible foundation for localization-aware value converters. It supports pluralization, abbreviation, casing, and custom format strings, and can be used in both single and multi-value bindings. Derived classes must implement the <see cref="Convert(object?, string?, CultureInfo)"/> method.
/// </remarks>
/// <example>
/// <code>
/// // Example usage in a derived converter:
/// public class MyCustomConverter : LocalizableConverter
/// {
///     public MyCustomConverter() : base(LetterCasing.Normal, new CultureInfo("fr-FR")) { }
///     public override object? Convert(object? value, string? format, CultureInfo culture)
///     {
///         // Custom conversion logic
///     }
/// }
/// </code>
/// </example>
public abstract class LocalizableConverter(LetterCasing casing, CultureInfo? culture = null) : IValueConverter, IMultiValueConverter
{
    /// <summary>
    /// Gets or sets the casing to apply to the result (e.g., normal, upper, lower, title).
    /// </summary>
    public LetterCasing Casing { get; set; } = casing;

    /// <summary>
    /// Gets or sets the format string to use for formatting the value.
    /// </summary>
    public string? Format { get; set; }

    /// <summary>
    /// Gets or sets a custom culture to use for localization. If null, uses the provided culture parameter.
    /// </summary>
    public CultureInfo? Culture { get; set; } = culture;

    /// <summary>
    /// Converts a value to a localized, formatted string.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="targetType">The target type.</param>
    /// <param name="parameter">The format string or additional parameter.</param>
    /// <param name="culture">The culture to use for localization.</param>
    /// <returns>The converted string value.</returns>
    /// <exception cref="FormatException">Thrown if the format string is invalid.</exception>
    public virtual object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var format = parameter as string ?? Format;
        var effectiveCulture = Culture ?? culture;

        try
        {
            return Convert(value, format, effectiveCulture);
        }
        catch (FormatException ex)
        {
            // Return a fallback value or error string if format is invalid
            return $"[Format error: {ex.Message}]";
        }
    }

    /// <summary>
    /// Converts multiple values to a localized, formatted string.
    /// </summary>
    /// <param name="values">The values to convert.</param>
    /// <param name="targetType">The target type.</param>
    /// <param name="parameter">The format string or additional parameter.</param>
    /// <param name="culture">The culture to use for localization.</param>
    /// <returns>The converted string value.</returns>
    public virtual object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        var value = values.GetByIndex(0);
        var format = values.GetByIndex(1) as string ?? parameter as string ?? Format;
        var effectiveCulture = values.OfType<CultureInfo>().LastOrDefault() ?? Culture ?? culture;

        try
        {
            return Convert(value, format, effectiveCulture);
        }
        catch (FormatException ex)
        {
            return $"[Format error: {ex.Message}]";
        }
    }

    /// <summary>
    /// Converts a value to a localized, formatted string with a specific format and culture.
    /// Must be implemented by derived classes.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="format">The format string to use.</param>
    /// <param name="culture">The culture to use for localization.</param>
    /// <returns>The converted string value.</returns>
    /// <exception cref="FormatException">Thrown if the format string is invalid.</exception>
    public abstract object? Convert(object? value, string? format, CultureInfo culture);

    /// <summary>
    /// Not supported. Throws <see cref="NotSupportedException"/>.
    /// </summary>
    /// <param name="value">The value to convert back.</param>
    /// <param name="targetType">The target type.</param>
    /// <param name="parameter">The format string or additional parameter.</param>
    /// <param name="culture">The culture to use for localization.</param>
    /// <returns>Never returns; always throws.</returns>
    public virtual object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotSupportedException();
}
