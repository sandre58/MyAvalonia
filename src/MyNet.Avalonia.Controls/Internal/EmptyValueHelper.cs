// -----------------------------------------------------------------------
// <copyright file="EmptyValueHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace MyNet.Avalonia.Controls.Internal;

internal static class EmptyValueHelper
{
    internal static bool IsEmptyLike(object? value) => value is null || value switch
    {
        string str => string.IsNullOrEmpty(str),
        double dbl => double.IsNaN(dbl),
        Array arr => arr.Length == 0,
        DateTime date => date == DateTime.MinValue,
        _ => false
    };
}
