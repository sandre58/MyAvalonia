// -----------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MyNet.Avalonia.Extended.Commands;
using MyNet.Avalonia.Extended.Schedulers;
using MyNet.UI.Commands;
using MyNet.UI.Threading;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Extended;
#pragma warning restore IDE0130

/// <summary>
/// Registers Avalonia command and scheduler services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="AvaloniaSchedulerProvider"/> and <see cref="AvaloniaCommandFactory"/>.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The same service collection for chaining.</returns>
    public static IServiceCollection AddAvaloniaCommands(this IServiceCollection services)
    {
        services.TryAddSingleton<ISchedulerProvider, AvaloniaSchedulerProvider>();
        services.TryAddSingleton<ICommandFactory, AvaloniaCommandFactory>();
        return services;
    }
}
