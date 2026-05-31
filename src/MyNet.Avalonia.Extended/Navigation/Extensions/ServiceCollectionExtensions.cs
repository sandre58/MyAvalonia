// -----------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MyNet.Avalonia.Extended.Assists;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Extended.Navigation;
#pragma warning restore IDE0130

/// <summary>
/// Registers Avalonia navigation integration services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the Avalonia navigation host and page factory.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The same service collection for chaining.</returns>
    /// <remarks>
    /// Requires <see cref="MyNet.UI.Navigation.ServiceCollectionExtensions.AddNavigation"/> and
    /// <see cref="MyNet.UI.Locators.ServiceCollectionExtensions.AddViewLocators"/> to be registered first.
    /// Call <see cref="AvaloniaNavigationContext.Configure"/> during startup after building the service provider.
    /// Attach <see cref="Assists.NavigationAssist"/> to menu and <see cref="NavigationPage"/> controls in XAML.
    /// </remarks>
    public static IServiceCollection AddAvaloniaNavigation(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.TryAddSingleton<IAvaloniaPageFactory, AvaloniaPageFactory>();
        services.TryAddSingleton<AvaloniaNavigationHost>();

        return services;
    }

    /// <summary>
    /// Configures shared navigation dependencies used by Avalonia attached properties.
    /// </summary>
    /// <param name="services">The built service provider.</param>
    public static void UseAvaloniaNavigation(this IServiceProvider services)
    {
        ArgumentNullException.ThrowIfNull(services);
        AvaloniaNavigationContext.Configure(services);
    }
}
