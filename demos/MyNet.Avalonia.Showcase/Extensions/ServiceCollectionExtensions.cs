// -----------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using MyNet.Avalonia.Showcase.Resources;
using MyNet.Globalization;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Showcase;
#pragma warning restore IDE0130

/// <summary>
/// Registers showcase-specific translation resources.
/// </summary>
internal static class ServiceCollectionExtensions
{
    /// <summary>
    /// Contributes showcase demo translation resources to the catalog.
    /// </summary>
    public static IServiceCollection AddMyNetAvaloniaShowcaseResources(this IServiceCollection services)
    {
        services.AddTranslationResource(nameof(CommonResources), CommonResources.ResourceManager);
        services.AddTranslationResource(nameof(MenuResources), MenuResources.ResourceManager);
        services.AddTranslationResource(nameof(SettingsResources), SettingsResources.ResourceManager);
        services.AddTranslationResource(nameof(NotificationPageResources), NotificationPageResources.ResourceManager);
        services.AddTranslationResource(nameof(FormResources), FormResources.ResourceManager);
        services.AddTranslationResource(nameof(DataGridPageResources), DataGridPageResources.ResourceManager);
        services.AddTranslationResource(nameof(MenuPageResources), MenuPageResources.ResourceManager);
        services.AddTranslationResource(nameof(NavigationMenuPageResources), NavigationMenuPageResources.ResourceManager);
        services.AddTranslationResource(nameof(DialogsPageResources), DialogsPageResources.ResourceManager);
        services.AddTranslationResource(nameof(ThemePageResources), ThemePageResources.ResourceManager);
        services.AddTranslationResource(nameof(ControlThemeResources), ControlThemeResources.ResourceManager);
        return services;
    }
}
