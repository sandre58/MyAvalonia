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
using MyNet.Avalonia;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Extended;
using MyNet.Avalonia.Extended.Theming;
using MyNet.Avalonia.Showcase.ViewModels;
using MyNet.Avalonia.Theme;
using MyNet.Avalonia.Theme.Diagnostics;
using MyNet.Avalonia.Theme.Themes;
using MyNet.Avalonia.Theme.Theming.Core;
using MyNet.Fakers;
using MyNet.Globalization;
using MyNet.Globalization.Culture;
using MyNet.Humanizer;
using MyNet.UI.ViewModels.Shell.Chrome;
using MyNet.Observable.Validation;
using MyNet.UI.Loading;
using MyNet.UI.Locators.Conventions;
using MyNet.UI.Theming;
using MyNet.UI.ViewModels;

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
        RegisterPageViewModels(collection, viewModelTypes);

        var services = collection.BuildServiceProvider();
        InitializeServices(services);
        InitializeTheme(services);
        RegisterNavigationMappings(services);

        return services;
    }

    /// <summary>Registers view-model to view mappings for navigation.</summary>
    public static void RegisterNavigationMappings(IServiceProvider services)
    {
        var providers = PagesCatalog.GetProviders();
        var typeResolver = services.GetRequiredService<ITypeResolver>();

        foreach (var association in providers.SelectMany(x => x.GetPageAssociations()))
            typeResolver.Register(association.ViewModelType, association.ViewType);
    }

    private void RegisterServices(IServiceCollection collection)
        => collection.AddGlobalization()
            .AddLocalization()
            .AddInflection()
            .AddHumanizer()
            .AddFakers()
            .AddBusy()
            .AddShell()
            .AddTransient(static sp => new ShellCultureViewModel(
                sp.GetRequiredService<ICultureService>(),
                [SupportedCultures.English, SupportedCultures.French]))
            .AddAvaloniaColors()
            .AddMyNetAvaloniaControls()
            .AddMyNetAvaloniaExtended(topLevelProvider)
            .AddSingleton<IThemeBrushService>(MyTheme.Current)
            .AddResources();

    private static void InitializeServices(IServiceProvider services)
    {
        services.UseGlobalization();
        services.UseLocalization();
        services.UseDisplayText();
        services.UseFakers();
        services.UseThemeManager();
        services.UseAvaloniaClipboard();
        services.UseMyNetAvaloniaExtended();
        ValidationLocalization.Configure();
    }

    private static void InitializeTheme(IServiceProvider services)
    {
        var registry = services.GetRequiredService<IThemeBaseRegistry>();
        registry.Register(new ThemeBase(ThemeVariantProvider.DarkBlue, true, false));
    }

    private static void RegisterPageViewModels(IServiceCollection collection, IEnumerable<Type> viewModelTypes)
    {
        collection.AddSingleton<MainViewModel>();
        viewModelTypes.ForEach(x => collection.AddSingleton(x));
    }
}
