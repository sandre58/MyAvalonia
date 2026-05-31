// -----------------------------------------------------------------------
// <copyright file="TextPickerCommitHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.Avalonia.Controls.Primitives.Internal;

internal enum TextPickerTextCommitKind
{
    NoOp,
    ClearValue,
    ParseAndApply,
}

internal static class TextPickerCommitHelper
{
    public static TextPickerTextCommitKind ResolveCommitAction(string? text, string? selectedValueText, bool hasSelectedValue)
    {
        if (string.IsNullOrWhiteSpace(text))
            return hasSelectedValue ? TextPickerTextCommitKind.ClearValue : TextPickerTextCommitKind.NoOp;

        if (hasSelectedValue && selectedValueText == text)
            return TextPickerTextCommitKind.NoOp;

        return TextPickerTextCommitKind.ParseAndApply;
    }

    public static bool ShouldApplyParsedValue<T>(T? parsedValue, T? currentValue) =>
        parsedValue?.Equals(currentValue) == false;
}
