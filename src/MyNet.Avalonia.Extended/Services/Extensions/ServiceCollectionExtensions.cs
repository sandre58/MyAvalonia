// -----------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MyNet.UI.Services;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Extended.Services;
#pragma warning restore IDE0130

/// <summary>
/// Registers Avalonia application services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="AppCommandsService"/> as <see cref="IAppCommandsService"/>.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The same service collection for chaining.</returns>
    public static IServiceCollection AddAvaloniaAppCommands(this IServiceCollection services)
    {
        services.TryAddSingleton<IAppCommandsService, AppCommandsService>();
        return services;
    }
}
