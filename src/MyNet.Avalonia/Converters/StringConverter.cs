// -----------------------------------------------------------------------
// <copyright file="StringConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using MyNet.Avalonia.Extensions;
using MyNet.Humanizer;
using MyNet.Utilities;
using MyNet.Utilities.Units;

namespace MyNet.Avalonia.Converters;

/// <summary>
/// Converts string values.
/// </summary>
public class StringConverter(LetterCasing casing, bool pluralize = false, bool abbreviate = false)
            : IValueConverter, IMultiValueConverter
{
    public static StringConverter ToUpper { get; } = new(LetterCasing.AllCaps);

    public static StringConverter ToLower { get; } = new(LetterCasing.LowerCase);

    public static StringConverter ToTitle { get; } = new(LetterCasing.Title);

    public static StringConverter Default { get; } = new(LetterCasing.Normal);

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return null;
        }

        // Value
        var result = value switch
        {
            SolidColorBrush brush => brush.Color.ToName() == brush.Color.ToHex() ? brush.Color.ToHex() : $"{brush.Color.ToName()}",
            Color color => color.ToName() == color.ToHex() ? color.ToHex() : $"{color.ToName()}",
            Enum enumValue => enumValue.Humanize(abbreviate, culture),
            IEnumeration enumValue => enumValue.Humanize(abbreviate, culture),
            TimeSpan timespan => timespan.Humanize(1, TimeUnit.Year, TimeUnit.Day, culture: culture),
            _ => value.ToString()
        };

        // Format
        if (parameter is not string p)
            return result?.ApplyCase(casing);
        if (double.TryParse(result, out var res) && !string.IsNullOrEmpty(result))
        {
            if (double.IsNaN(res)) return null;

            var format = pluralize ? p.TranslateWithCount(res, abbreviate, culture) : abbreviate ? p.TranslateAbbreviated(culture) : p.Translate(culture);
            result = res.ToString(format, culture);
        }
        else
        {
            switch (value)
            {
                case string str when !string.IsNullOrEmpty(str):
                    {
                        var format = abbreviate ? p.TranslateAbbreviated(culture) : p.Translate(culture);
                        var translation = str.Translate(culture);
                        result = format.FormatWith(culture, translation);
                        break;
                    }

                case DateTimeOffset:
                case DateTime:
                case DateOnly:
                case TimeOnly:
                    result = DateTimeConverter.Default.Convert(value, targetType, p, culture).ToString();
                    break;

                case TimeSpan when int.TryParse(p, out var number):
                    {
                        var split = result?.Split(" ");
                        if (split != null && number <= split.Length)
                            result = split[number - 1];
                        break;
                    }

                default:
                    break;
            }
        }

        // Casing
        return result?.ApplyCase(casing);
    }

    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture) => Convert(values.Count > 0 ? values[0] : null, targetType, values.Count > 1 ? values[1] : null, culture);

    public virtual object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotSupportedException();
}
