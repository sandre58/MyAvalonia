// -----------------------------------------------------------------------
// <copyright file="ValidationErrorMessageConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;
using MyNet.Utilities;
using MyNet.Utilities.Exceptions;
using MyNet.Utilities.Localization;

namespace MyNet.Avalonia.Converters;

public class ValidationErrorMessageConverter : IValueConverter
{
    public static readonly ValidationErrorMessageConverter Default = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is IEnumerable errors)
        {
            var messages = new List<string>();

            foreach (var error in errors)
            {
                if (error is TranslatableException translatableException)
                {
                    messages.Add(TranslationService.Current.Translate(translatableException.ResourceKey).FormatWith(translatableException.Parameters ?? []));
                }
                else if (error is Exception exception)
                {
                    messages.Add(exception.Message);
                }
                else
                {
                    messages.Add(error?.ToString()?.Translate() ?? string.Empty);
                }
            }

            return messages;
        }

        return value;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
