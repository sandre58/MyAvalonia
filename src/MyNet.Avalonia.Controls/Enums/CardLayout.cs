// -----------------------------------------------------------------------
// <copyright file="CardLayout.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.Avalonia.Controls.Enums;

/// <summary>
/// Defines the body arrangement of a <see cref="Card"/>.
/// </summary>
public enum CardLayout
{
    /// <summary>
    /// Icon or avatar on the left, title stack and optional content on the right.
    /// </summary>
    Horizontal,

    /// <summary>
    /// Leading centered above a centered title stack — shortcut tiles and feature grids.
    /// </summary>
    Vertical,

    /// <summary>
    /// Emphasizes <see cref="Card.Title"/> as a primary metric with a muted subtitle.
    /// </summary>
    Stat,

    /// <summary>
    /// <see cref="RegionControl.Leading"/> spans the full width on top — articles, products, previews.
    /// </summary>
    MediaTop,

    /// <summary>
    /// <see cref="RegionControl.Leading"/> in a fixed left column — contacts, files, rich list rows.
    /// </summary>
    MediaLeft,
}
