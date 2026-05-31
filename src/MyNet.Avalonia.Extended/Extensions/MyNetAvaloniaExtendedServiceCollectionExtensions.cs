// -----------------------------------------------------------------------
// <copyright file="MyNetAvaloniaExtendedServiceCollectionExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using MyNet.Globalization;
using MyNet.UI.Resources;

namespace MyNet.Avalonia.Extended;

/// <summary>
/// Registers MyNet.Avalonia.Extended translation resources.
/// </summary>
public static class MyNetAvaloniaExtendedServiceCollectionExtensions
{
    /// <summary>
    /// Contributes extended UI translation resources to the catalog.
    /// </summary>
    /// <remarks>
    /// Contributes extended UI translation resources to the catalog.
    /// Include extended styles in <c>App.axaml</c> via
    /// <see cref="Theming.AvaloniaExtendedThemes.GenericStyles"/>.
    /// </remarks>
    public static IServiceCollection AddMyNetAvaloniaExtended(this IServiceCollection services)
    {
        services.AddTranslationResource(nameof(UiResources), UiResources.ResourceManager);
        services.AddTranslationResource(nameof(MessageResources), MessageResources.ResourceManager);
        services.AddTranslationResource(nameof(FormatResources), FormatResources.ResourceManager);
        return services;
    }
}
