// -----------------------------------------------------------------------
// <copyright file="LocalizationMarkupConverterFactory.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Converters;
using MyNet.Globalization.Localization.Translation;
using MyNet.Text.TextCasing;

namespace MyNet.Avalonia.MarkupExtensions;

/// <summary>
/// Factory for creating <see cref="StringConverter"/> instances based on specified letter casing, display style, and quantity translation options. This factory centralizes the logic for determining the appropriate converter configuration for localization scenarios in markup extensions.
/// </summary>
internal static class LocalizationMarkupConverterFactory
{
    /// <summary>
    /// Creates a <see cref="StringConverter"/> instance based on the provided letter casing, display style, and quantity translation options. If the display style is default and quantity information is not required, it returns a cached converter for the specified casing. Otherwise, it creates a new converter with the appropriate translation options.
    /// </summary>
    /// <param name="casing">The letter casing to apply to the converted string.</param>
    /// <param name="style">The display style to use for the conversion.</param>
    /// <param name="quantityFromValue">Indicates whether quantity information should be derived from the value.</param>
    /// <returns>A configured <see cref="StringConverter"/> instance.</returns>
    public static StringConverter Create(LetterCasing casing, DisplayStyle style, bool quantityFromValue)
        => style == DisplayStyle.Default && !quantityFromValue
        ? StringConverter.Converters[casing]
        : new(casing, new TranslationOptionsBuilder().WithStyle(style).Build())
        {
            QuantityFromValue = quantityFromValue
        };
}
