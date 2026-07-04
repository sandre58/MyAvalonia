// -----------------------------------------------------------------------
// <copyright file="ItemsSearchFilterMode.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.Avalonia.Controls;

/// <summary>
/// Defines how item text is matched against the search query.
/// </summary>
public enum ItemsSearchFilterMode
{
    /// <summary>
    /// The item text contains the query.
    /// </summary>
    Contains,

    /// <summary>
    /// The item text starts with the query.
    /// </summary>
    StartsWith,

    /// <summary>
    /// The item text equals the query.
    /// </summary>
    Equals,
}
