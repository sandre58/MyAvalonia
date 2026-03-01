// -----------------------------------------------------------------------
// <copyright file="IThemeRoleResolver.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Media;

namespace MyNet.Avalonia.Theme.Palettes;

public sealed class ThemeRoleResolver : IThemeRoleResolver
{
    public RoleResolutionResult Resolve(ThemeRole role, ThemeContext context, string? defaultPath = null)
    {
        switch (role)
        {
            case ThemeRole.Default:
                if (context == ThemeContext.Contrast)
                    return new(RoleResolutionKind.ParentForeground);

                return new(RoleResolutionKind.Path, defaultPath);

            case ThemeRole.Contrast:
                return new(RoleResolutionKind.ParentForeground);

            case ThemeRole.Inverse:
                return new(RoleResolutionKind.Inverse);

            case ThemeRole.Custom:
                return new(RoleResolutionKind.Path, defaultPath);

            default:
                return new(RoleResolutionKind.Path, role.ToString());
        }
    }
}
