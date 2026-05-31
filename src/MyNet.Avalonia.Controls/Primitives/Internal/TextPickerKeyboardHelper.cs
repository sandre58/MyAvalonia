// -----------------------------------------------------------------------
// <copyright file="TextPickerKeyboardHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Input;

namespace MyNet.Avalonia.Controls.Primitives.Internal;

internal static class TextPickerKeyboardHelper
{
    public static TextPickerKeyResult Resolve(Key key, bool isDropDownOpen, bool hasSelectedValue, KeyModifiers modifiers) => isDropDownOpen
        ? key switch
        {
            Key.Enter => new(TextPickerKeyAction.CommitPreview),
            Key.Escape => new(TextPickerKeyAction.Rollback),
            _ => default
        }
        : modifiers != KeyModifiers.None || !hasSelectedValue
            ? default
            : key switch
            {
                Key.Down => new(TextPickerKeyAction.IncrementByOffset, -1),
                Key.Up => new(TextPickerKeyAction.IncrementByOffset, 1),
                Key.PageDown => new(TextPickerKeyAction.IncrementLargeByOffset, -1),
                Key.PageUp => new(TextPickerKeyAction.IncrementLargeByOffset, 1),
                _ => default
            };
}
