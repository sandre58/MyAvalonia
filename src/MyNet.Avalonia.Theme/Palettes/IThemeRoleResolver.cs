// -----------------------------------------------------------------------
// <copyright file="IThemeRoleResolver.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Media;

namespace MyNet.Avalonia.Theme.Palettes;

public interface IThemeRoleResolver
{
    RoleResolutionResult Resolve(ThemeRole role, ThemeContext context, IBrush? parentForeground, string? defaultPath = null);
}

public readonly record struct RoleResolutionResult(RoleResolutionKind Kind, string? Path = null, IBrush? DirectBrush = null);

public enum RoleResolutionKind
{
    Path,
    ParentForeground,
    Inverse,
    DirectBrush
}
