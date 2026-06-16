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
    /// Leading with background, title spanning two rows — dense stat or settings tile.
    /// </summary>
    Compact,

    /// <summary>
    /// Leading without background, title on a single row — feature or capability tile.
    /// </summary>
    Tile
}
