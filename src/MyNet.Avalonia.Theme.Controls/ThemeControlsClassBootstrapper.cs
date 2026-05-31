// -----------------------------------------------------------------------
// <copyright file="ThemeControlsClassBootstrapper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Theme.Controls.Classes;

namespace MyNet.Avalonia.Theme.Controls;

/// <summary>
/// Registers utility classes that depend on <c>MyNet.Avalonia.Controls</c>.
/// </summary>
public static class ThemeControlsClassBootstrapper
{
    private static bool _initialized;

    /// <summary>
    /// Registers control-specific utility classes. Safe to call multiple times.
    /// </summary>
    public static void Initialize()
    {
        if (_initialized)
            return;

        _initialized = true;

        IconClassRegistry.Register();
        LayoutClassRegistry.Register();
    }
}
