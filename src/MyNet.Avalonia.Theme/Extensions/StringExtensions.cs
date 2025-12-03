// -----------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.Avalonia.Theme.Extensions;

internal static class StringExtensions
{
    /// <summary>
    /// Add a prefix before value, separated by period.
    /// </summary>
    /// <param name="value">Value.</param>
    /// <param name="prefix">Prefix to add.</param>
    /// <returns>prefix.value.</returns>
    internal static string WithPrefix(this string value, string? prefix = null) => !string.IsNullOrWhiteSpace(prefix) ? $"{prefix}.{value}" : value;
}
