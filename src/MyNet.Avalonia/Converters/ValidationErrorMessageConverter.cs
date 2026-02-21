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
        if (value is not IEnumerable errors)
            return value;

        var messages = new List<string>();

        foreach (var error in errors)
        {
            switch (error)
            {
                case TranslatableException translatableException:
                    messages.Add(TranslationService.Current.Translate(translatableException.ResourceKey).FormatWith(translatableException.Parameters ?? []));
                    break;
                case Exception exception:
                    messages.Add(exception.Message);
                    break;
                default:
                    messages.Add(error?.ToString()?.Translate() ?? string.Empty);
                    break;
            }
        }

        return messages;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
