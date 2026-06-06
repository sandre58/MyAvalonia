// -----------------------------------------------------------------------
// <copyright file="CardLayout.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.Avalonia.Controls.Enums;

/// <summary>
/// Defines the content layout of a <see cref="Card"/>.
/// </summary>
public enum CardLayout
{
    /// <summary>
    /// Leading element on the left, title and subtitle stacked on the right.
    /// Suitable for compact stat tiles and settings entries.
    /// </summary>
    Horizontal,

    /// <summary>
    /// Leading element and title on the first row, subtitle and content below.
    /// Suitable for feature tiles and capability cards.
    /// </summary>
    Vertical
}
