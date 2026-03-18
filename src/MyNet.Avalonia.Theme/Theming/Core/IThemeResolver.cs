// -----------------------------------------------------------------------
// <copyright file="IThemeResolver.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.Avalonia.Theme.Theming.Core;

/// <summary>
/// Defines an interface for resolving the source of a theme brush.
/// This resolver determines which source must be used, without returning or transforming any brush instance.
/// </summary>
public interface IThemeResolver
{
    /// <summary>
    /// Resolves which source should be used to obtain a brush.
    /// </summary>
    /// <param name="role">The theme role when role-based resolution is requested.</param>
    /// <param name="context">The theme context when context-based resolution is requested.</param>
    /// <param name="resourceKey">The brush key provided by the caller.</param>
    /// <returns>A resolution descriptor indicating where the brush should come from.</returns>
    ThemeBrushResolution Resolve(ThemeRole? role, ThemeContext? context, string? resourceKey = null);
}

/// <summary>
/// Indicates the source selected by <see cref="IThemeResolver"/>.
/// </summary>
public enum ThemeBrushResolutionKind
{
    UseDirectBrush,

    UseForeground,

    UseKey
}

/// <summary>
/// Describes the source information required to materialize a brush.
/// </summary>
/// <param name="Kind">The selected source kind.</param>
/// <param name="BrushKey">The resource key to use when <see cref="Kind"/> is <see cref="ThemeBrushResolutionKind.UseKey"/>.</param>
/// <param name="OpacityKey">The key used to resolve opacity for context contrast scenarios.</param>
/// <param name="UseContrast">Indicates whether the brush should be used in a contrast context.</param>
public sealed record ThemeBrushResolution(ThemeBrushResolutionKind Kind, string? BrushKey = null, string? OpacityKey = null, bool UseContrast = true);
