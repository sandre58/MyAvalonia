// -----------------------------------------------------------------------
// <copyright file="ThicknessSize.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.Avalonia.Theme.Enums;

/// <summary>
/// Enumerates standard thickness sizes (such as margin or padding) for use in the theming system.
/// Each value represents a predefined thickness value for consistent layout spacing.
/// </summary>
public enum ThicknessSize
{
    /// <summary>
    /// No thickness.
    /// </summary>
    None = 0,

    /// <summary>
    /// Small thickness.
    /// </summary>
    Small = 2,

    /// <summary>
    /// Default (medium) thickness.
    /// </summary>
    Default = 5,

    /// <summary>
    /// Medium thickness.
    /// </summary>
    Medium = 10,

    /// <summary>
    /// Large thickness.
    /// </summary>
    Large = 15,

    /// <summary>
    /// Extra large thickness.
    /// </summary>
    ExtraLarge = 20,

    /// <summary>
    /// Huge thickness.
    /// </summary>
    Huge = 40
}
