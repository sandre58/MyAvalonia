// -----------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using MyNet.Geography;
using MyNet.Geography.Resources;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Geography;
#pragma warning restore IDE0130

/// <summary>
/// Registers common MyNet Avalonia Extended services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers MyNet Avalonia Extended services, including:
    /// - Geography localization services for Avalonia, which provide localized names and resources for geographical entities
    /// - Geography flags services for Avalonia, which provide flag images for countries and regions.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The same service collection for chaining.</returns>
    public static IServiceCollection AddMyNetAvaloniaGeography(this IServiceCollection services)
        => services.AddGeographyLocalization()
            .AddGeographyFlags();
}
