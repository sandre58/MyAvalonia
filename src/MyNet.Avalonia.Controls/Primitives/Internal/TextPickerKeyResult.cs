// -----------------------------------------------------------------------
// <copyright file="TextPickerKeyResult.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.Avalonia.Controls.Primitives.Internal;

internal enum TextPickerKeyAction
{
    None,
    CommitPreview,
    Rollback,
    IncrementByOffset,
    IncrementLargeByOffset
}

internal readonly record struct TextPickerKeyResult(TextPickerKeyAction Action, int Offset = 0);
