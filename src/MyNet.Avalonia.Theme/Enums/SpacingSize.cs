// -----------------------------------------------------------------------
// <copyright file="SpacingSize.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.Avalonia.Theme.Enums;

/// <summary>
/// Enumerates standard thickness sizes (such as margin or padding) for use in the theming system.
/// Each value represents a predefined thickness value for consistent layout spacing.
/// </summary>
public enum SpacingSize
{
    /// <summary>
    /// No spacing.
    /// </summary>
    None = 0,

    /// <summary>
    /// Small spacing.
    /// </summary>
    Xxs = 1,

    /// <summary>
    /// Small spacing.
    /// </summary>
    Xs = 2,

    /// <summary>
    /// Default (medium) spacing.
    /// </summary>
    Sm = 5,

    /// <summary>
    /// Medium spacing.
    /// </summary>
    Md = 10,

    /// <summary>
    /// Large spacing.
    /// </summary>
    Lg = 16,

    /// <summary>
    /// Extra large spacing.
    /// </summary>
    Xl = 24,

    /// <summary>
    /// Huge spacing.
    /// </summary>
    Xxl = 32,

    /// <summary>
    /// Huge spacing.
    /// </summary>
    Xxxl = 48
}
