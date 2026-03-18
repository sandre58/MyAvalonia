// -----------------------------------------------------------------------
// <copyright file="ThicknessDirection.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.Avalonia.Theme.Classes.Enums;

/// <summary>
/// Enumerates directions for applying thickness (such as margin or padding) in the theming system.
/// Each value represents a specific side or combination of sides for consistent layout adjustments.
/// </summary>
public enum ThicknessDirection
{
    /// <summary>
    /// No thickness applied.
    /// </summary>
    None,

    /// <summary>
    /// Thickness applied to all sides.
    /// </summary>
    All,

    /// <summary>
    /// Thickness applied to left and right sides.
    /// </summary>
    Horizontal,

    /// <summary>
    /// Thickness applied to top and bottom sides.
    /// </summary>
    Vertical,

    /// <summary>
    /// Thickness applied to the left side only.
    /// </summary>
    Left,

    /// <summary>
    /// Thickness applied to the top side only.
    /// </summary>
    Top,

    /// <summary>
    /// Thickness applied to the right side only.
    /// </summary>
    Right,

    /// <summary>
    /// Thickness applied to the bottom side only.
    /// </summary>
    Bottom
}
