// -----------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using MyNet.Avalonia.Extended.Services;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Extended;
#pragma warning restore IDE0130

/// <summary>
/// Registers common MyNet Avalonia Extended services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers core extended services shared across Avalonia host applications.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The same service collection for chaining.</returns>
    public static IServiceCollection AddMyNetAvaloniaExtended(this IServiceCollection services)
        => services.AddAvaloniaAppCommands();
}
