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
    ParseAndApply
}

internal static class TextPickerCommitHelper
{
    public static TextPickerTextCommitKind ResolveCommitAction(string? text, string? selectedValueText, bool hasSelectedValue) => string.IsNullOrWhiteSpace(text)
        ? hasSelectedValue ? TextPickerTextCommitKind.ClearValue : TextPickerTextCommitKind.NoOp
        : hasSelectedValue && selectedValueText == text ? TextPickerTextCommitKind.NoOp : TextPickerTextCommitKind.ParseAndApply;

    public static bool ShouldApplyParsedValue<T>(T? parsedValue, T? currentValue) =>
        parsedValue?.Equals(currentValue) == false;
}
