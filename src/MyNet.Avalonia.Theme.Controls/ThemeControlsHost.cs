// -----------------------------------------------------------------------
// <copyright file="ThemeControlsHost.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Styling;
using MyNet.Avalonia.Theme.Controls.Classes;

namespace MyNet.Avalonia.Theme.Controls;

/// <summary>
/// Entry point for wiring control themes into the application.
/// </summary>
public static class ThemeControlsHost
{
    private static int _registered;

    private static int _catalogAttached;

    private static bool _utilityClassesInitialized;

    /// <summary>
    /// Registers utility classes used by control themes.
    /// Safe to call multiple times.
    /// </summary>
    public static void Register()
    {
        if (Interlocked.CompareExchange(ref _registered, 1, 0) != 0)
            return;

        RegisterUtilityClasses();
    }

    /// <summary>
    /// Loads and attaches the precompiled control-theme catalog to <paramref name="application"/>.
    /// Call after <see cref="MyNet.Avalonia.Theme.MyTheme"/> has completed <c>EnsureLoaded()</c>.
    /// Safe to call multiple times.
    /// </summary>
    /// <param name="application">Application instance (typically <c>this</c> from <c>App</c>).</param>
    public static void AttachCatalog(Application application)
    {
        ArgumentNullException.ThrowIfNull(application);

        if (Interlocked.CompareExchange(ref _catalogAttached, 1, 0) != 0)
            return;

        application.Styles.Add(new ThemeControlsCatalog());
    }

    /// <summary>
    /// Ensures the catalog is attached; throws when missing after startup.
    /// </summary>
    /// <param name="application">Optional application; uses <see cref="Application.Current"/> when null.</param>
    /// <exception cref="InvalidOperationException">Catalog styles are not attached.</exception>
    public static void EnsureCatalogAttached(Application? application = null)
    {
        if (Volatile.Read(ref _catalogAttached) != 0)
            return;

        application ??= Application.Current
            ?? throw new InvalidOperationException("No Avalonia Application is available.");

        foreach (var item in application.Styles)
        {
            if (item is ThemeControlsCatalog)
                return;
        }

        throw new InvalidOperationException(
            $"Control themes are not loaded. Call {nameof(AttachCatalog)}(application) after MyTheme.Current.EnsureLoaded().");
    }

    private static void RegisterUtilityClasses()
    {
        if (_utilityClassesInitialized)
            return;

        _utilityClassesInitialized = true;

        IconClassRegistry.Register();
        LayoutClassRegistry.Register();
    }
}
