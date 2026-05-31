// -----------------------------------------------------------------------
// <copyright file="TextPickerValidationHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace MyNet.Avalonia.Controls.Primitives.Internal;

internal static class TextPickerValidationHelper
{
    public static TextPickerParseResult<T> Parse<T>(
        string? text,
        Func<string, T?> convert,
        Func<T?, bool> isValid)
    {
        if (string.IsNullOrWhiteSpace(text))
            return new(TextPickerParseStatus.Empty);

        try
        {
            var value = convert(text);
            return isValid(value)
                ? new(TextPickerParseStatus.Success, value)
                : new(TextPickerParseStatus.InvalidValue);
        }
        catch (FormatException ex)
        {
            return new(TextPickerParseStatus.FormatError, Error: ex);
        }
    }
}
