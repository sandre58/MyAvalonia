// -----------------------------------------------------------------------
// <copyright file="Opacity.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.Avalonia.Theme.Palettes;

/// <summary>
/// Specifies named opacity levels for various UI states and elevation effects in a theme.
/// Used to reference standard opacity values for overlays, interactions, and elevation surfaces.
/// </summary>
public enum Opacity
{
    /// <summary>
    /// Opacity for high elevation surfaces.
    /// </summary>
    ElevationHigh,

    /// <summary>
    /// Opacity for medium elevation surfaces.
    /// </summary>
    ElevationMedium,

    /// <summary>
    /// Opacity for low elevation surfaces.
    /// </summary>
    ElevationLow,

    /// <summary>
    /// Opacity for disabled UI elements.
    /// </summary>
    Disabled,

    /// <summary>
    /// Opacity for scrim overlays (background dimming).
    /// </summary>
    Scrim,

    /// <summary>
    /// Opacity for drag state.
    /// </summary>
    Drag,

    /// <summary>
    /// Opacity for pressed state.
    /// </summary>
    Pressed,

    /// <summary>
    /// Opacity for focus state.
    /// </summary>
    Focus,

    /// <summary>
    /// Opacity for hover state.
    /// </summary>
    Hover,

    /// <summary>
    /// Opacity for overlay elements.
    /// </summary>
    Overlay
}
