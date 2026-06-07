// -----------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MyNet.UI.Commands;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Extended.Commands;
#pragma warning restore IDE0130

/// <summary>
/// Registers Avalonia command and scheduler services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers Avalonia schedulers and command factory services.
    /// </summary>
    /// <remarks>
    /// Requires an <see cref="ICultureContext"/> to be registered, typically via <c>AddGlobalization()</c>.
    /// </remarks>
    /// <param name="services">The service collection.</param>
    /// <returns>The same service collection for chaining.</returns>
    public static IServiceCollection AddAvaloniaCommands(this IServiceCollection services)
    {
        services.TryAddSingleton<ICommandFactory, AvaloniaCommandFactory>();
        return services;
    }
}
