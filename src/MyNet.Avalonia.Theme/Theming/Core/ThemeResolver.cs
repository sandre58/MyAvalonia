// -----------------------------------------------------------------------
// <copyright file="ThemeResolver.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.Avalonia.Theme.Theming.Core;

/// <summary>
/// Resolves brush source information from role and context.
/// </summary>
internal sealed class ThemeResolver : IThemeResolver
{
    /// <inheritdoc />
    public ThemeBrushResolution Resolve(ThemeRole? role, ThemeContext? context, string? resourceKey = null)
        => role.HasValue
            ? role switch
            {
                ThemeRole.Default => new ThemeBrushResolution(ThemeBrushResolutionKind.UseDirectBrush, UseContrast: false),
                ThemeRole.Contrast => new ThemeBrushResolution(ThemeBrushResolutionKind.UseForeground),
                ThemeRole.Inverse => new ThemeBrushResolution(ThemeBrushResolutionKind.UseKey, ThemeResourceKeyFactory.InverseSurfaceKey),
                _ => new ThemeBrushResolution(ThemeBrushResolutionKind.UseKey, role.ToString())
            }
            : context.HasValue
            ? context switch
            {
                ThemeContext.Default when !string.IsNullOrEmpty(resourceKey) => new ThemeBrushResolution(ThemeBrushResolutionKind.UseKey, resourceKey),
                ThemeContext.Contrast => new ThemeBrushResolution(ThemeBrushResolutionKind.UseForeground, OpacityKey: resourceKey),
                _ => new ThemeBrushResolution(ThemeBrushResolutionKind.UseDirectBrush)
            }
            : !string.IsNullOrEmpty(resourceKey)
            ? new ThemeBrushResolution(ThemeBrushResolutionKind.UseKey, resourceKey)
            : new ThemeBrushResolution(ThemeBrushResolutionKind.UseDirectBrush);
}
