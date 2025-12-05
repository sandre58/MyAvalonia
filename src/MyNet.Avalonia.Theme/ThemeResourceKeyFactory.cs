// -----------------------------------------------------------------------
// <copyright file="ThemeResourceKeyFactory.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Utilities;

namespace MyNet.Avalonia.Theme;

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
    /// The key for geometry resources.
    /// </summary>
    public const string GeometryKey = "Geometry";

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
    /// Gets a pattern for a resource key of the specified type.
    /// </summary>
    /// <param name="type">The resource type.</param>
    /// <returns>The formatted pattern string.</returns>
    public static string Pattern(string type) => BuildResourceKey(type, "{0}");
}
