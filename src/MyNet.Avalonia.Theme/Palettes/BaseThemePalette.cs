// -----------------------------------------------------------------------
// <copyright file="BaseThemePalette.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Avalonia.Media;

namespace MyNet.Avalonia.Theme.Palettes;

/// <summary>
/// Base color palette for surfaces, backgrounds, borders, and other UI elements.
/// Colors in this palette change depending on the Light or Dark theme variant.
/// </summary>
public class BaseThemePalette
{
    /// <summary>
    /// Gets the background color for the main application surface.
    /// </summary>
    public required Color ApplicationBackground { get; init; }

    /// <summary>
    /// Gets the foreground color for the main application surface.
    /// </summary>
    public required Color ApplicationForeground { get; init; }

    /// <summary>
    /// Gets the background color for general surfaces.
    /// </summary>
    public required Color SurfaceBackground { get; init; }

    /// <summary>
    /// Gets the background color for dark surfaces.
    /// </summary>
    public required Color SurfaceBackgroundDark { get; init; }

    /// <summary>
    /// Gets the border color for surfaces.
    /// </summary>
    public required Color SurfaceBorder { get; init; }
    /// <summary>
    /// Gets the background color for controls.
    /// </summary>
    public required Color ControlBackground { get; init; }

    /// <summary>
    /// Gets the background color for light controls.
    /// </summary>
    public required Color ControlBackgroundLight { get; init; }

    /// <summary>
    /// Gets the border color for controls.
    /// </summary>
    public required Color ControlBorder { get; init; }

    /// <summary>
    /// Gets the border color for controls when hovered.
    /// </summary>
    public required Color ControlBorderHover { get; init; }

    /// <summary>
    /// Gets the border color for controls when focused.
    /// </summary>
    public required Color ControlBorderFocus { get; init; }

    /// <summary>
    /// Gets the background color for overlays.
    /// </summary>
    public required Color OverlayBackground { get; init; }

    /// <summary>
    /// Gets the background color for dialogs.
    /// </summary>
    public required Color DialogBackground { get; init; }

    /// <summary>
    /// Gets the background color for popups.
    /// </summary>
    public required Color PopupBackground { get; init; }

    /// <summary>
    /// Gets the background color for tooltips.
    /// </summary>
    public required Color ToolTipBackground { get; init; }

    /// <summary>
    /// Gets the border color for tooltips.
    /// </summary>
    public required Color ToolTipBorder { get; init; }

    /// <summary>
    /// Gets the background color for the close button when hovered.
    /// </summary>
    public required Color ButtonCloseBackgroundHover { get; init; }

    /// <summary>
    /// Converts the base theme palette to a read-only dictionary suitable for use as resource dictionary keys and values.
    /// </summary>
    /// <returns>A dictionary containing all base theme colors with their corresponding resource keys.</returns>
    public IReadOnlyDictionary<string, Color> ToResourceDictionary() => new Dictionary<string, Color>
        {
            { "Application.Background", ApplicationBackground },
            { "Application.Foreground", ApplicationForeground },
            { "Surface.Background", SurfaceBackground },
            { "Surface.Background.Dark", SurfaceBackgroundDark },
            { "Surface.Border", SurfaceBorder },
            { "Control.Background", ControlBackground },
            { "Control.Background.Light", ControlBackgroundLight },
            { "Control.Border", ControlBorder },
            { "Control.Border.Hover", ControlBorderHover },
            { "Control.Border.Focus", ControlBorderFocus },
            { "Overlay.Background", OverlayBackground },
            { "Dialog.Background", DialogBackground },
            { "Popup.Background", PopupBackground },
            { "ToolTip.Background", ToolTipBackground },
            { "ToolTip.Border", ToolTipBorder },
            { "Button.Close.Background.Hover", ButtonCloseBackgroundHover }
        };
}
