// -----------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MyNet.Avalonia.Extended.Toasting.Settings;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Extended.Toasting;
#pragma warning restore IDE0130

/// <summary>
/// Registers Avalonia toast rendering services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="AvaloniaToastHost"/> to render <see cref="MyNet.UI.Toasting.IToastManager"/> toasts.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="topLevelProvider">Resolves the host top level, typically the main window.</param>
    /// <param name="configureOptions">Optional callback to customize host layout options.</param>
    /// <returns>The same service collection for chaining.</returns>
    /// <remarks>
    /// Requires <see cref="MyNet.UI.Toasting.ServiceCollectionExtensions.AddToasting"/>,
    /// <see cref="MyNet.UI.Notifications.ServiceCollectionExtensions.AddNotifications"/>, and
    /// <see cref="Schedulers.ServiceCollectionExtensions.AddAvaloniaScheduler"/> to be registered first.
    /// Resolve <see cref="AvaloniaToastHost"/> during startup to attach the visual host.
    /// </remarks>
    public static IServiceCollection AddAvaloniaToasting(
        this IServiceCollection services,
        Func<TopLevel?> topLevelProvider,
        Action<AvaloniaToastHostOptions>? configureOptions = null)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(topLevelProvider);

        var options = new AvaloniaToastHostOptions();
        configureOptions?.Invoke(options);

        services.TryAddSingleton(options);
        services.TryAddSingleton(sp => new AvaloniaToastHost(
            topLevelProvider,
            sp.GetRequiredService<MyNet.UI.Toasting.IToastManager>(),
            sp.GetRequiredService<AvaloniaToastHostOptions>()));

        return services;
    }
}
