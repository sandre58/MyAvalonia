// -----------------------------------------------------------------------
// <copyright file="ThemeResourceKeyFactory.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using MyNet.Utilities;

namespace MyNet.Avalonia.Theme.Theming;

/// <summary>
/// Provides factory methods and constants for generating resource keys and patterns for theming in Avalonia.
/// </summary>
public static class ThemeResourceKeyFactory
{
    /// <summary>
    /// The prefix used for all resource keys.
    /// </summary>
    public const string ResourcePrefix = "MyNet";

    /// <summary>
    /// The pattern for resource keys (e.g., "MyNet.Type.Primary").
    /// </summary>
    public const string ResourcePattern = $"{ResourcePrefix}.{{0}}.{{1}}";

    /// <summary>
    /// The pattern for icon markup extension usage.
    /// </summary>
    public const string IconPattern = "{{my:Icon {0}}}";

    /// <summary>
    /// The pattern for icon path markup.
    /// </summary>
    public const string IconPathPattern = $"<PathIcon Data=\"{{{{StaticResource {ResourcePrefix}.{GeometryKey}.{{0}}}}}}\" />";

    /// <summary>
    /// The key for color resources.
    /// </summary>
    public const string ColorKey = "Color";

    /// <summary>
    /// The key for brush resources.
    /// </summary>
    public const string BrushKey = "Brush";

    /// <summary>
    /// The key for opacity resources.
    /// </summary>
    public const string OpacityKey = "Opacity";

    /// <summary>
    /// The key for shadow resources.
    /// </summary>
    public const string ShadowKey = "Shadow";

    /// <summary>
    /// The key for corners resources.
    /// </summary>
    public const string CornersKey = "Corners";

    /// <summary>
    /// The key for geometry resources.
    /// </summary>
    public const string GeometryKey = "Geometry";

    /// <summary>
    /// The key for themes.
    /// </summary>
    public const string ThemeKey = "Theme";

    /// <summary>
    /// The key for surface resources.
    /// </summary>
    public const string SurfaceKey = "Surface";

    /// <summary>
    /// The key for control resources.
    /// </summary>
    public const string ControlKey = "Control";

    /// <summary>
    /// The key for font size resources.
    /// </summary>
    public const string FontSizeKey = "Font.Size";

    /// <summary>
    /// The key for font weight resources.
    /// </summary>
    public const string FontWeightKey = "Font.Weight";

    /// <summary>
    /// The key for animation resources.
    /// </summary>
    public const string AnimationKey = "Animation";

    /// <summary>
    /// The key for round resources.
    /// </summary>
    public const string RoundKey = "Round";

    /// <summary>
    /// The key for spacing resources.
    /// </summary>
    public const string SpacingKey = "Spacing";

    /// <summary>
    /// The key for surface inverse resources.
    /// </summary>
    public const string InverseSurfaceKey = "Surface.Inverse";

    /// <summary>
    /// The key for foreground inverse resources.
    /// </summary>
    public const string InverseForegroundKey = "Foreground.Inverse";

    /// <summary>
    /// Represents the resource key for the primary foreground color.
    /// </summary>
    public const string PrimaryForeground = "Foreground.Primary";

    /// <summary>
    /// Gets a resource key using the specified type and name.
    /// </summary>
    /// <param name="type">The resource type (e.g., Type, Brush).</param>
    /// <param name="name">The resource name.</param>
    /// <returns>The formatted resource key.</returns>
    private static string BuildResourceKey(string type, string name) => ResourcePattern.FormatWith(type, name);

    /// <summary>
    /// Gets a color resource key for the specified name.
    /// </summary>
    /// <param name="name">The color name.</param>
    /// <returns>The formatted color resource key.</returns>
    public static string Color(string name) => BuildResourceKey(ColorKey, name);

    /// <summary>
    /// Gets a brush resource key for the specified name.
    /// </summary>
    /// <param name="name">The brush name.</param>
    /// <returns>The formatted brush resource key.</returns>
    public static string Brush(string name) => BuildResourceKey(BrushKey, name);

    /// <summary>
    /// Gets a geometry resource key for the specified name.
    /// </summary>
    /// <param name="name">The geometry name.</param>
    /// <returns>The formatted geometry resource key.</returns>
    public static string Geometry(string name) => BuildResourceKey(GeometryKey, name);

    /// <summary>
    /// Gets an opacity resource key for the specified name.
    /// </summary>
    /// <param name="name">The opacity name.</param>
    /// <returns>The formatted opacity resource key.</returns>
    public static string Opacity(string name) => BuildResourceKey(OpacityKey, name);

    /// <summary>
    /// Gets an shadow resource key for the specified name.
    /// </summary>
    /// <param name="name">The shadow name.</param>
    /// <returns>The formatted shadow resource key.</returns>
    public static string Shadow(string name) => BuildResourceKey(ShadowKey, name);

    /// <summary>
    /// Gets an corners resource key for the specified name.
    /// </summary>
    /// <param name="name">The corners name.</param>
    /// <returns>The formatted corners resource key.</returns>
    public static string Corners(string name) => BuildResourceKey(CornersKey, name);

    /// <summary>
    /// Gets a font size resource key for the specified name.
    /// </summary>
    /// <param name="name">The font size name.</param>
    /// <returns>The formatted font size resource key.</returns>
    public static string FontSize(string name) => BuildResourceKey(FontSizeKey, name);

    /// <summary>
    /// Gets a font weight resource key for the specified name.
    /// </summary>
    /// <param name="name">The font weight name.</param>
    /// <returns>The formatted font weight resource key.</returns>
    public static string FontWeight(string name) => BuildResourceKey(FontWeightKey, name);

    /// <summary>
    /// Gets an animation resource key for the specified name.
    /// </summary>
    /// <param name="name">The animation name.</param>
    /// <returns>The formatted animation resource key.</returns>
    public static string Animation(string name) => BuildResourceKey(AnimationKey, name);

    /// <summary>
    /// Gets a spacing resource key for the specified name.
    /// </summary>
    /// <param name="name">The spacing name.</param>
    /// <returns>The formatted spacing resource key.</returns>
    public static string Spacing(string name) => BuildResourceKey(SpacingKey, name);

    /// <summary>
    /// Gets a theme key for the specified name.
    /// </summary>
    /// <param name="control">The control type.</param>
    /// <param name="name">The control layout name.</param>
    /// <returns>The formatted theme key.</returns>
    public static string Theme(string control, string? name = null) => BuildResourceKey(ThemeKey, !string.IsNullOrEmpty(name) ? $"{control}.{name}" : control);

    /// <summary>
    /// Gets a pattern for a resource key of the specified type.
    /// </summary>
    /// <param name="type">The resource type.</param>
    /// <returns>The formatted pattern string.</returns>
    public static string Pattern(string type) => BuildResourceKey(type, "{0}");

    /// <summary>
    /// Determines the appropriate contrasted color for a given color key based on theme conventions.
    /// </summary>
    /// <param name="colorKey">The color key to find a contrasted color for.</param>
    /// <returns>The contrasted color if found; otherwise, null.</returns>
    public static string? ContrastedColor(string colorKey)
    {
        // Define the contrasted color mappings
        var contrastedColorKey = colorKey switch
        {
            // Surface.Inverse uses Foreground.Inverse as contrast
            InverseSurfaceKey => InverseForegroundKey,

            // Foreground.Inverse uses Foreground.Primary for contrast
            InverseForegroundKey => PrimaryForeground,

            // All other Surface levels use Foreground.Primary
            var k when k.StartsWith($"{SurfaceKey}.", StringComparison.OrdinalIgnoreCase) => PrimaryForeground,

            // Control borders might need foreground for contrast
            var k when k.StartsWith($"{ControlKey}.", StringComparison.OrdinalIgnoreCase) => PrimaryForeground,

            // Overlay and other elements has no specified contrasted color
            _ => null
        };

        return !string.IsNullOrEmpty(contrastedColorKey) ? Color(contrastedColorKey) : null;
    }
}
