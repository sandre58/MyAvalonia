// -----------------------------------------------------------------------
// <copyright file="AvaloniaNavigationBootstrap.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace MyNet.Avalonia.Extended.Navigation;

/// <summary>
/// Shared navigation dependencies for Avalonia attached properties.
/// </summary>
internal static class AvaloniaNavigationBootstrap
{
    private static IAvaloniaNavigationPageHost? _host;
    private static AvaloniaNavigationGestureBridge? _gestureBridge;
    private static bool _isConfigured;

    /// <summary>
    /// Configures shared dependencies from the service provider.
    /// </summary>
    /// <param name="services">The application service provider.</param>
    internal static void Configure(IServiceProvider services)
    {
        ArgumentNullException.ThrowIfNull(services);

        if (_isConfigured)
            return;

        _host = services.GetRequiredService<IAvaloniaNavigationPageHost>();
        _gestureBridge = services.GetRequiredService<AvaloniaNavigationGestureBridge>();
        _ = services.GetRequiredService<AvaloniaNavigationResetBridge>();
        _isConfigured = true;
    }

    /// <summary>
    /// Attaches the navigation page control to the configured host and gesture bridge.
    /// </summary>
    /// <param name="navigationPage">The navigation page host control.</param>
    internal static void AttachNavigationPage(NavigationPage navigationPage)
    {
        if (!_isConfigured)
            throw new InvalidOperationException("Call UseAvaloniaNavigation before attaching NavigationPage.");

        _host!.Attach(navigationPage);
        _gestureBridge!.Attach(navigationPage);
    }
}
