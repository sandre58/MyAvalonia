// -----------------------------------------------------------------------
// <copyright file="ControlPalette.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Avalonia.Media;

namespace MyNet.Avalonia.Theme.Theming.Palettes;

/// <summary>
/// Defines the base color palette for surfaces, backgrounds, borders, and other UI elements in a theme variant.
/// Colors in this palette adapt to the Light or Dark theme variant and provide consistent styling for common UI components.
/// </summary>
public class ControlPalette
{
    // Surfaces

    /// <summary>
    /// Gets the elevated surface color for modal overlays (busy indicator, overlay dialogs).
    /// Lighter than the application surface in dark themes.
    /// </summary>
    public required Color SurfaceOverlay { get; init; }

    /// <summary>
    /// Gets the elevated surface color for contextual popups (menus, dropdowns, flyouts).
    /// </summary>
    public required Color SurfacePopup { get; init; }

    /// <summary>
    /// Gets the root application background color (window client area).
    /// </summary>
    public required Color SurfaceApplication { get; init; }

    /// <summary>
    /// Gets the background color for surface level 0 (chrome / header).
    /// </summary>
    public required Color SurfaceLevel0 { get; init; }

    /// <summary>
    /// Gets the background color for surface level 1 (container / card).
    /// </summary>
    public required Color SurfaceLevel1 { get; init; }

    /// <summary>
    /// Gets the background color for surface level 2 (control / input).
    /// </summary>
    public required Color SurfaceLevel2 { get; init; }

    /// <summary>
    /// Gets the inverse surface color (Tooltip).
    /// </summary>
    public required Color SurfaceInverse { get; init; }

    /// <summary>
    /// Gets the border color for all surfaces.
    /// </summary>
    public required Color SurfaceBorder { get; init; }

    // Foreground

    /// <summary>
    /// Gets the primary foreground color.
    /// </summary>
    public required Color ForegroundPrimary { get; init; }

    /// <summary>
    /// Gets the secondary foreground color.
    /// </summary>
    public required Color ForegroundSecondary { get; init; }

    /// <summary>
    /// Gets the tertiary foreground color.
    /// </summary>
    public required Color ForegroundTertiary { get; init; }

    /// <summary>
    /// Gets the inverse foreground color.
    /// </summary>
    public required Color ForegroundInverse { get; init; }

    // Divider

    /// <summary>
    /// Gets the divider color.
    /// </summary>
    public required Color Divider { get; init; }

    // Controls

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

    // Overlay

    /// <summary>
    /// Gets the background color for overlays.
    /// </summary>
    public required Color OverlayBackground { get; init; }

    // Specifics

    /// <summary>
    /// Gets the background color for the close button when hovered.
    /// </summary>
    public required Color ButtonCloseHover { get; init; }

    // Validation

    /// <summary>
    /// Gets the color used to indicate validation errors.
    /// </summary>
    public required Color ValidationError { get; init; }

    /// <summary>
    /// Converts the base theme palette to a read-only dictionary suitable for use as resource dictionary keys and values.
    /// </summary>
    /// <returns>A dictionary containing all base theme colors with their corresponding resource keys.</returns>
    public IReadOnlyDictionary<string, object> ToResourceDictionary() => new Dictionary<string, object>
        {
            // Surfaces
            { "Surface.Overlay", SurfaceOverlay },
            { "Surface.Popup", SurfacePopup },
            { "Surface.Application", SurfaceApplication },
            { "Surface.Inverse", SurfaceInverse },
            { "Surface.Border", SurfaceBorder },
            { "Surface.Level0", SurfaceLevel0 },
            { "Surface.Level1", SurfaceLevel1 },
            { "Surface.Level2", SurfaceLevel2 },

            // Foreground
            { "Foreground.Primary", ForegroundPrimary },
            { "Foreground.Secondary", ForegroundSecondary },
            { "Foreground.Tertiary", ForegroundTertiary },
            { "Foreground.Inverse", ForegroundInverse },

            // Divider
            { "Divider", Divider },

            // Controls
            { "Control.Border", ControlBorder },
            { "Control.Border.Hover", ControlBorderHover },
            { "Control.Border.Focus", ControlBorderFocus },

            // Overlay
            { "Overlay.Background", OverlayBackground },

            // Specifics
            { "Button.Close.Hover", ButtonCloseHover },

            // Validation
            { "Validation.Error", ValidationError }
        };

    /// <summary>
    /// Creates a ThemePalette instance from a resource dictionary.
    /// </summary>
    /// <param name="dictionary">The resource dictionary containing color definitions.</param>
    /// <returns>A new ThemePalette instance.</returns>
    public static ControlPalette FromResourceDictionary(IReadOnlyDictionary<string, object> dictionary)
    {
        var defaultColor = global::Avalonia.Media.Colors.Gray;
        return new()
        {
            // Surfaces
            SurfaceLevel0 = (Color)dictionary.GetValueOrDefault("Surface.Level0", defaultColor),
            SurfaceOverlay = (Color)dictionary.GetValueOrDefault("Surface.Overlay", defaultColor),
            SurfacePopup = (Color)dictionary.GetValueOrDefault("Surface.Popup", defaultColor),
            SurfaceApplication = (Color)dictionary.GetValueOrDefault("Surface.Application", defaultColor),
            SurfaceLevel1 = (Color)dictionary.GetValueOrDefault("Surface.Level1", defaultColor),
            SurfaceLevel2 = (Color)dictionary.GetValueOrDefault("Surface.Level2", defaultColor),
            SurfaceInverse = (Color)dictionary.GetValueOrDefault("Surface.Inverse", defaultColor),
            SurfaceBorder = (Color)dictionary.GetValueOrDefault("Surface.Border", defaultColor),

            // Foreground
            ForegroundPrimary = (Color)dictionary.GetValueOrDefault("Foreground.Primary", defaultColor),
            ForegroundSecondary = (Color)dictionary.GetValueOrDefault("Foreground.Secondary", defaultColor),
            ForegroundTertiary = (Color)dictionary.GetValueOrDefault("Foreground.Tertiary", defaultColor),
            ForegroundInverse = (Color)dictionary.GetValueOrDefault("Foreground.Inverse", defaultColor),

            // Divider
            Divider = (Color)dictionary.GetValueOrDefault("Divider", defaultColor),

            // Controls
            ControlBorder = (Color)dictionary.GetValueOrDefault("Control.Border", defaultColor),
            ControlBorderHover = (Color)dictionary.GetValueOrDefault("Control.Border.Hover", defaultColor),
            ControlBorderFocus = (Color)dictionary.GetValueOrDefault("Control.Border.Focus", defaultColor),

            // Overlay
            OverlayBackground = (Color)dictionary.GetValueOrDefault("Overlay.Background", defaultColor),

            // Specifics
            ButtonCloseHover = (Color)dictionary.GetValueOrDefault("Button.Close.Hover", defaultColor),

            // Validation
            ValidationError = (Color)dictionary.GetValueOrDefault("Validation.Error", defaultColor)
        };
    }
}
