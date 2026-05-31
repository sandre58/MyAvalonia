// -----------------------------------------------------------------------
// <copyright file="TextPickerParseResult.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace MyNet.Avalonia.Controls.Primitives.Internal;

internal enum TextPickerParseStatus
{
    Empty,
    Success,
    InvalidValue,
    FormatError,
}

internal readonly record struct TextPickerParseResult<T>(TextPickerParseStatus Status, T? Value = default, Exception? Error = null);
