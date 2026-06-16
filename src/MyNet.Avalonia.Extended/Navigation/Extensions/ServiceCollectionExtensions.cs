// -----------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MyNet.UI.Locators;
using MyNet.UI.Navigation;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Extended.Navigation;
#pragma warning restore IDE0130

/// <summary>
/// Registers Avalonia navigation integration services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the Avalonia navigation page host, middleware, and gesture bridge.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The same service collection for chaining.</returns>
    /// <remarks>
    /// Requires <see cref="MyNet.UI.Navigation.ServiceCollectionExtensions.AddNavigation"/> and
    /// <see cref="MyNet.UI.Locators.ServiceCollectionExtensions.AddViewLocators"/> to be registered first.
    /// Call <see cref="UseAvaloniaNavigation"/> during startup after building the service provider.
    /// Set <see cref="Assists.NavigationPageHostAssist.IsHostProperty"/> on the shell <see cref="Avalonia.Controls.NavigationPage"/>.
    /// </remarks>
    public static IServiceCollection AddAvaloniaNavigation(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddViewLocators();
        services.AddNavigation();
        services.RemoveAll<INavigationService>();
        services.AddSingleton<NavigationService>();
        services.AddSingleton<INavigationService, SingletonNavigationService>();
        services.TryAddSingleton<IAvaloniaPageFactory, AvaloniaPageFactory>();
        services.TryAddSingleton<IAvaloniaNavigationPageHost, AvaloniaNavigationPageHost>();
        services.TryAddSingleton<AvaloniaNavigationGestureBridge>();
        services.TryAddSingleton<AvaloniaNavigationResetBridge>();
        services.AddNavigationMiddleware<AvaloniaNavigationPageMiddleware>();

        return services;
    }

    /// <summary>
    /// Configures shared navigation dependencies used by Avalonia attached properties.
    /// </summary>
    /// <param name="services">The built service provider.</param>
    public static IServiceProvider UseAvaloniaNavigation(this IServiceProvider services)
    {
        ArgumentNullException.ThrowIfNull(services);
        AvaloniaNavigationBootstrap.Configure(services);

        return services;
    }
}
