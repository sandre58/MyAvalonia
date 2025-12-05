// -----------------------------------------------------------------------
// <copyright file="ThicknessDirection.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.Avalonia.Theme.Enums;

/// <summary>
/// Enumerates directions for applying thickness (such as margin or padding) in the theming system.
/// Each value represents a specific side or combination of sides for consistent layout adjustments.
/// </summary>
public enum ThicknessDirection
{
    /// <summary>
    /// No thickness applied.
    /// </summary>
    None = 0,

    /// <summary>
    /// Thickness applied to all sides.
    /// </summary>
    All = 1,

    /// <summary>
    /// Thickness applied to left and right sides.
    /// </summary>
    Horizontal = 2,

    /// <summary>
    /// Thickness applied to top and bottom sides.
    /// </summary>
    Vertical = 3,

    /// <summary>
    /// Thickness applied to the left side only.
    /// </summary>
    Left = 4,

    /// <summary>
    /// Thickness applied to the top side only.
    /// </summary>
    Top = 5,

    /// <summary>
    /// Thickness applied to the right side only.
    /// </summary>
    Right = 6,

    /// <summary>
    /// Thickness applied to the bottom side only.
    /// </summary>
    Bottom = 7
}
