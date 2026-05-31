// -----------------------------------------------------------------------
// <copyright file="ThemeControlsHost.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Threading;
using MyNet.Avalonia.Theme.Runtime;

namespace MyNet.Avalonia.Theme.Controls;

/// <summary>
/// Entry point for wiring control themes into <see cref="MyNet.Avalonia.Theme.MyTheme"/>.
/// Call <see cref="Register"/> once at application startup, before <c>MyTheme</c> loads resources
/// (typically in <c>App.Initialize</c>, before <c>AvaloniaXamlLoader.Load</c>).
/// </summary>
public static class ThemeControlsHost
{
    private static int _registered;

    /// <summary>
    /// Registers resource merging and control-specific utility classes. Safe to call multiple times.
    /// </summary>
    public static void Register()
    {
        if (Interlocked.CompareExchange(ref _registered, 1, 0) != 0)
            return;

        ThemeComposition.RegisterCatalogMerger(ThemeControlsResourceLoader.Merge);
        ThemeControlsClassBootstrapper.Initialize();
    }
}
