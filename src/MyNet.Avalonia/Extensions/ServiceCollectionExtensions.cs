// -----------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using MyNet.Avalonia.Colors;
using MyNet.Globalization;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// Registers MyNet.Avalonia core services and translation resources.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Contributes Avalonia core translation resources and the shared <see cref="IColorRegistry"/>.
    /// </summary>
    /// <remarks>
    /// The host must also call <c>AddGlobalization()</c>, <c>AddLocalization()</c>, <c>AddInflection()</c>,
    /// <c>AddHumanizer()</c>, then <c>UseGlobalization()</c>, <c>UseLocalization()</c>, and <c>UseDisplayText()</c>
    /// on the built <see cref="IServiceProvider"/>.
    /// </remarks>
    public static IServiceCollection AddMyNetAvalonia(this IServiceCollection services)
    {
        services.AddTranslationResource(ColorRegistry.ResourceName, ColorResources.ResourceManager);
        services.AddSingleton<IColorRegistry>(ColorRegistry.Instance);
        return services;
    }
}
