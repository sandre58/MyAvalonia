// -----------------------------------------------------------------------
// <copyright file="ResourcesBootstrapper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Resources;

namespace MyNet.Avalonia;

public static class ResourcesBootstrapper
{
    private static bool _isInitialized;

    /// <summary>
    /// Initializes application-wide translation and globalization services. This method ensures that required resources
    /// and event handlers are set up before localization features are used.
    /// </summary>
    /// <remarks>Call this method once during application startup to register translation resources,
    /// initialize the humanizer, and subscribe to culture change events. Subsequent calls have no effect.</remarks>
    public static void Initialize()
    {
        if (_isInitialized) return;

        ColorResourcesLocator.Initialize();
        Humanizer.ResourceLocator.Initialize();

        _isInitialized = true;
    }
}
