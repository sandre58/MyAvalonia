// -----------------------------------------------------------------------
// <copyright file="AppComposition.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Extended;
using MyNet.Avalonia.Extended.Theming;
using MyNet.Avalonia.Showcase.Resources;
using MyNet.Avalonia.Showcase.ViewModels;
using MyNet.Avalonia.Showcase.ViewModels.Pages;
using MyNet.Avalonia.Theme;
using MyNet.Avalonia.Theme.Diagnostics;
using MyNet.Avalonia.Theme.Themes;
using MyNet.Avalonia.Theme.Theming.Core;
using MyNet.Collections;
using MyNet.Fakers;
using MyNet.Globalization;
using MyNet.Globalization.Culture;
using MyNet.UI;
using MyNet.UI.Locators.Conventions;
using MyNet.UI.Navigation;
using MyNet.UI.Theming;

namespace MyNet.Avalonia.Showcase.Composition;

/// <summary>
/// Builds DI, navigation, and the showcase menu for <see cref="App"/>.
/// </summary>
internal sealed class AppComposition(Func<TopLevel?> topLevelProvider)
{
    /// <summary>Builds the service provider and registers all showcase services.</summary>
    public ServiceProvider Build()
    {
        ThemeDiagnostics.ConfigureFromEnvironment();

        var providers = PagesCatalog.GetProviders();
        var viewModelTypes = providers.SelectMany(x => x.GetPageAssociations()).Select(x => x.ViewModelType).ToList();

        var collection = new ServiceCollection();
        RegisterServices(collection);
        RegisterTranslations(collection);
        RegisterPageViewModels(collection, viewModelTypes);

        var services = collection.BuildServiceProvider();
        services.UseShowcaseLogging();
        InitializeServices(services);
        InitializeTheme(services);
        InitializePageMappings(services);

        return services;
    }

    /// <summary>Resolves <see cref="MainViewModel"/> and registers showcase menu items.</summary>
    public static MainViewModel ConfigureMainViewModel(IServiceProvider services)
    {
        ArgumentNullException.ThrowIfNull(services);

        var mainViewModel = services.GetRequiredService<MainViewModel>();
        mainViewModel.AddMenuItem([.. PagesCatalog.CreateMenuItems(services)]);
        return mainViewModel;
    }

    /// <summary>Navigates to the default landing page.</summary>
    public static void NavigateToDefaultPage(IServiceProvider services)
        => _ = services.GetRequiredService<INavigationClient>().NavigateToAsync<HomePageViewModel>();

    private void RegisterServices(IServiceCollection collection)
        => collection.AddFakers()
            .AddUi([SupportedCultures.English, SupportedCultures.French])
            .AddMyNetAvaloniaColors()
            .AddMyNetAvaloniaControls()
            .AddMyNetAvaloniaExtended(topLevelProvider)
            .AddSingleton<IThemeBrushService>(MyTheme.Current)
            .AddShowcaseLogging();

    /// <summary>
    /// Contributes showcase demo translation resources to the catalog.
    /// </summary>
    private static void RegisterTranslations(IServiceCollection services)
        => services.AddTranslationResource(nameof(CommonResources), CommonResources.ResourceManager)
            .AddTranslationResource(nameof(MenuResources), MenuResources.ResourceManager)
            .AddTranslationResource(nameof(SettingsResources), SettingsResources.ResourceManager)
            .AddTranslationResource(nameof(NotificationPageResources), NotificationPageResources.ResourceManager)
            .AddTranslationResource(nameof(FormResources), FormResources.ResourceManager)
            .AddTranslationResource(nameof(DataGridPageResources), DataGridPageResources.ResourceManager)
            .AddTranslationResource(nameof(MenuPageResources), MenuPageResources.ResourceManager)
            .AddTranslationResource(nameof(NavigationMenuPageResources), NavigationMenuPageResources.ResourceManager)
            .AddTranslationResource(nameof(DialogsPageResources), DialogsPageResources.ResourceManager)
            .AddTranslationResource(nameof(ThemePageResources), ThemePageResources.ResourceManager)
            .AddTranslationResource(nameof(ControlThemeResources), ControlThemeResources.ResourceManager)
            .AddTranslationResource(nameof(HomePageResources), HomePageResources.ResourceManager)
            .AddTranslationResource(nameof(CardPageResources), CardPageResources.ResourceManager);

    private static void RegisterPageViewModels(IServiceCollection collection, IEnumerable<Type> viewModelTypes)
    {
        collection.AddSingleton<MainViewModel>();
        viewModelTypes.ForEach(x => collection.AddSingleton(x));
    }

    private static void InitializeServices(IServiceProvider services)
    {
        services.UseUi();
        services.UseFakers();
        services.UseMyNetAvaloniaClipboard();
        services.UseMyNetAvaloniaExtended();
    }

    private static void InitializeTheme(IServiceProvider services)
    {
        var registry = services.GetRequiredService<IThemeBaseRegistry>();
        registry.Register(new ThemeBase(ThemeVariantProvider.DarkBlue, true, false));
    }

    private static void InitializePageMappings(IServiceProvider services)
    {
        var providers = PagesCatalog.GetProviders();
        var typeResolver = services.GetRequiredService<ITypeResolver>();

        foreach (var association in providers.SelectMany(x => x.GetPageAssociations()))
            typeResolver.Register(association.ViewModelType, association.ViewType);
    }
}
