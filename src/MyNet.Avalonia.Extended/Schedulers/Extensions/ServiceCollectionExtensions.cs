// -----------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Reactive.Concurrency;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MyNet.Globalization.Culture;
using MyNet.UI.Threading;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Extended.Schedulers;
#pragma warning restore IDE0130

/// <summary>
/// Registers Avalonia command and scheduler services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers Avalonia schedulers services.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The same service collection for chaining.</returns>
    public static IServiceCollection AddAvaloniaScheduler(this IServiceCollection services)
    {
        services.TryAddSingleton<AvaloniaScheduler>(static sp => new(sp.GetRequiredService<ICultureContext>()));

        services.TryAddSingleton<IScheduler>(static sp => sp.GetRequiredService<AvaloniaScheduler>());
        services.TryAddSingleton<ISchedulerProvider, AvaloniaSchedulerProvider>();
        return services;
    }
}
