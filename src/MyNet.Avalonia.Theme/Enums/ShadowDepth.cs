// -----------------------------------------------------------------------
// <copyright file="ShadowDepth.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.Avalonia.Theme.Enums;

/// <summary>
/// Specifies the available shadow depth levels for elevation and visual hierarchy in the theme.
/// Used to control the intensity and spread of drop shadows for UI elements.
/// </summary>
public enum ShadowDepth
{
    /// <summary>
    /// No shadow (flat surface).
    /// </summary>
    Depth0,

    /// <summary>
    /// Shadow depth level 1 (lowest elevation).
    /// </summary>
    Depth1,

    /// <summary>
    /// Shadow depth level 2.
    /// </summary>
    Depth2,

    /// <summary>
    /// Shadow depth level 3.
    /// </summary>
    Depth3,

    /// <summary>
    /// Shadow depth level 4.
    /// </summary>
    Depth4,

    /// <summary>
    /// Shadow depth level 5 (highest elevation).
    /// </summary>
    Depth5,

    /// <summary>
    /// Centered shadow depth level 1 (lowest elevation, centered effect).
    /// </summary>
    CenterDepth1,

    /// <summary>
    /// Centered shadow depth level 2.
    /// </summary>
    CenterDepth2,

    /// <summary>
    /// Centered shadow depth level 3.
    /// </summary>
    CenterDepth3,

    /// <summary>
    /// Centered shadow depth level 4.
    /// </summary>
    CenterDepth4,

    /// <summary>
    /// Centered shadow depth level 5 (highest elevation, centered effect).
    /// </summary>
    CenterDepth5
}
