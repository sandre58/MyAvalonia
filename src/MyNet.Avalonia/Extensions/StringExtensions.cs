// -----------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.Avalonia.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Add a prefix before value, separated by period.
    /// </summary>
    /// <param name="value">Value.</param>
    /// <param name="prefix">Prefix to add.</param>
    /// <param name="separator">Separator to use between prefix and value.</param>
    /// <returns>prefix.value.</returns>
    public static string WithPrefix(this string value, string? prefix = null, string? separator = "-") => !string.IsNullOrWhiteSpace(prefix) ? $"{prefix}{separator}{value}" : value;
}
