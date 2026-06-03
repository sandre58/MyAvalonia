// -----------------------------------------------------------------------
// <copyright file="ThemeProfiles.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using MyNet.Avalonia.Showcase.Extensions;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ThemeBuilder.Definitions;
using MyNet.Avalonia.Theme.Classes;

namespace MyNet.Avalonia.Showcase.ThemeBuilder;

/// <summary>
/// Reusable <see cref="ControlThemeBuilder"/> presets for showcase playground pages.
/// </summary>
internal static class ThemeProfiles
{
    /// <summary>
    /// Creates a builder for the default or named control theme key.
    /// </summary>
    public static ControlThemeBuilder Create(string? themeKey = null) => themeKey is null ? new() : new ControlThemeBuilder(themeKey);

    /// <summary>
    /// Text content, circular shape, default variants, sizes, and roles.
    /// </summary>
    public static ControlThemeBuilder TextButton(string? themeKey = null) =>
        Create(themeKey)
            .WithContent(ContentControl.ContentProperty, ContentProviderType.Text)
            .AddShapes(CssClass.ShapeCircle)
            .AddDefaultVariants()
            .AddDefaultSizes()
            .AddDefaultRoles();

    /// <summary>
    /// Icon content with default sizes and roles (no variant presets).
    /// </summary>
    public static ControlThemeBuilder IconButton(string themeKey) =>
        Create(themeKey)
            .WithContent(ContentControl.ContentProperty, ContentProviderType.Icon)
            .AddDefaultSizes()
            .AddDefaultRoles();

    /// <summary>
    /// Icon content, standard variants, optional control shadow, default sizes and roles.
    /// </summary>
    public static ControlThemeBuilder RoundedIconButton(string themeKey = "Rounded") =>
        Create(themeKey)
            .WithContent(ContentControl.ContentProperty, ContentProviderType.Icon)
            .AddStandardVariants()
            .AddVariant(CssClass.ShadowControl)
            .AddDefaultSizes()
            .AddDefaultRoles();

    /// <summary>
    /// Text content, circular shape, standard variants, optional control shadow, default sizes and roles.
    /// </summary>
    public static ControlThemeBuilder StandardTextButton(string? themeKey = null) =>
        Create(themeKey)
            .WithContent(ContentControl.ContentProperty, ContentProviderType.Text)
            .AddShapes(CssClass.ShapeCircle)
            .AddStandardVariants()
            .AddVariant(CssClass.ShadowControl)
            .AddDefaultSizes()
            .AddDefaultRoles();
}
