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
using MyNet.Globalization.Facade;
using MyNet.Primitives;
using MyNet.Primitives.Exceptions;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Converters;
#pragma warning restore IDE0130

/// <summary>
/// Converts a collection of validation errors into localized display messages.
/// </summary>
/// <remarks>
/// Handles <see cref="TranslatableException"/> (resource key + parameters), standard <see cref="Exception"/>,
/// and falls back to string translation for other error objects.
/// Returns a <see cref="List{T}"/> of strings suitable for <see cref="Avalonia.Controls.ContentControl.Content"/>.
/// </remarks>
public class ValidationErrorMessageConverter : IValueConverter
{
    /// <summary>
    /// Gets the default singleton instance.
    /// </summary>
    public static readonly ValidationErrorMessageConverter Default = new();

    /// <inheritdoc/>
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
                    messages.Add(translatableException.ResourceKey.Translate().FormatWith(culture, translatableException.Parameters ?? []));
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

    /// <inheritdoc/>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotSupportedException();
}
