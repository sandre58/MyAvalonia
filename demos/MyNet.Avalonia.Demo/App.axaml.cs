// -----------------------------------------------------------------------
// <copyright file="App.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reflection;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using MyNet.Avalonia.Clipboard;
using MyNet.Avalonia.Demo.Resources;
using MyNet.Avalonia.Demo.ViewModels;
using MyNet.Avalonia.Demo.Views;
using MyNet.Avalonia.Extended.Busy;
using MyNet.Avalonia.Extended.Clipboard;
using MyNet.Avalonia.Extended.Commands;
using MyNet.Avalonia.Extended.Schedulers;
using MyNet.Avalonia.Extended.Services;
using MyNet.Avalonia.Extended.Theming;
using MyNet.Avalonia.Extended.Toasting;
using MyNet.Avalonia.Extended.WarmUp;
using MyNet.Avalonia.Theme;
using MyNet.Avalonia.Theme.Themes;
using MyNet.Avalonia.Theme.Theming.Core;
using MyNet.UI.Commands;
using MyNet.UI.Loading;
using MyNet.UI.Locators;
using MyNet.UI.Navigation;
using MyNet.UI.Navigation.Models;
using MyNet.UI.Notifications;
using MyNet.UI.Services;
using MyNet.UI.Theming;
using MyNet.UI.Toasting;
using MyNet.Utilities.Geography.Extensions;
using MyNet.Utilities.Localization;
using MyNet.Utilities.Logging;
using MyNet.Utilities.Logging.NLog;
using PropertyChanged;
using Scheduler = MyNet.UI.Threading.Scheduler;

namespace MyNet.Avalonia.Demo;

[DoNotNotify]
public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);

#if DEBUG
        this.AttachDeveloperTools();
#endif
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Register all the services needed for the application to run
        var collection = new ServiceCollection();
        RegisterServices(collection);
        var pageTypes = RegisterPageViewModels(collection);

        // Creates a ServiceProvider containing services from the provided IServiceCollection
        var services = collection.BuildServiceProvider();

        InitializeServices(services);

        InitializeTheme(services);
        InitializeResources();

        var vm = ViewModelManager.Get<MainViewModel>();
        switch (ApplicationLifetime)
        {
            case IClassicDesktopStyleApplicationLifetime desktop:
                desktop.MainWindow = new MainWindow { DataContext = vm };

                desktop.MainWindow.Opened += async (_, _) =>
                {
                    var warmUpService = services.GetRequiredService<IWarmUpService>();

                    await warmUpService.WarmUpAsync(pageTypes, delayMs: 800).ConfigureAwait(false);
                };
                break;
            case ISingleViewApplicationLifetime singleView:
                singleView.MainView = new MainView { DataContext = vm };
                break;
        }

        _ = NavigationManager.NavigateTo<HomePageViewModel>();

        base.OnFrameworkInitializationCompleted();
    }

    private void RegisterServices(ServiceCollection collection)
        => collection.AddSingleton<ILogger, Logger>()
            .AddSingleton<IViewModelLocator, ViewModelLocator>()
            .AddSingleton<IWarmUpService, ViewModelWarmUpService>()
            .AddSingleton<IThemeBrushService>(MyTheme.Current)
            .AddSingleton<IThemeBaseRegistry, ThemeVariantsRegistry>()
            .AddSingleton<IThemeService, ThemeService>()
            .AddSingleton<INotificationsManager, NotificationsManager>()
            .AddSingleton<INavigationService, NavigationService>()
            .AddSingleton<IToasterService>(new ToasterService(() => (ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow))
            .AddSingleton<IClipboardService>(new ClipboardService(() => (ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow))
            // .AddSingleton<IDialogService, OverlayDialogService>()
            .AddScoped<IBusyServiceFactory, BusyServiceFactory>()
            // .AddScoped<IMessageBoxFactory, MessageBoxFactory>()
            .AddScoped<IScheduler, AvaloniaScheduler>(_ => AvaloniaScheduler.Current)
            .AddScoped<ICommandFactory, AvaloniaCommandFactory>()
            .AddScoped<IAppCommandsService, AppCommandsService>();

    private static Type[] RegisterPageViewModels(ServiceCollection collection)
    {
        collection.AddSingleton<MainViewModel>();

        var types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t is { IsClass: true, IsAbstract: false } && t.IsAssignableTo(typeof(INavigationPage))).ToArray();
        foreach (var viewModelType in types)
        {
            collection.AddSingleton(viewModelType);
        }

        return types;
    }

    private static void InitializeServices(ServiceProvider services)
    {
        // Logging
        Logger.LoadConfiguration($"{Directory.GetCurrentDirectory()}/config/NLog.config");

        var viewModelLocator = services.GetRequiredService<IViewModelLocator>();
        var busyFactory = services.GetRequiredService<IBusyServiceFactory>();
        LogManager.Initialize(services.GetRequiredService<ILogger>());
        ViewModelManager.Initialize(null!, viewModelLocator);
        ThemeManager.Initialize(services.GetRequiredService<IThemeService>(), services.GetRequiredService<IThemeBaseRegistry>());
        NavigationManager.Initialize(services.GetRequiredService<INavigationService>(), viewModelLocator);
        ToasterManager.Initialize(services.GetRequiredService<IToasterService>());
        ClipboardManager.Initialize(services.GetRequiredService<IClipboardService>());
        BusyManager.Initialize(busyFactory);
        AppBusyManager.Initialize(busyFactory);
        CommandsManager.Initialize(services.GetRequiredService<ICommandFactory>());
        Scheduler.Initialize(services.GetRequiredService<IScheduler>());
    }

    private static void InitializeResources()
    {
        Extended.ResourcesBootstrapper.Initialize();
        Controls.ResourcesBootstrapper.Initialize();
        TranslationService.RegisterResources(nameof(CountryResources), CountryResources.ResourceManager);
        TranslationService.RegisterResources(nameof(CommonResources), CommonResources.ResourceManager);
        TranslationService.RegisterResources(nameof(MenuResources), MenuResources.ResourceManager);
        TranslationService.RegisterResources(nameof(SettingsResources), SettingsResources.ResourceManager);
        TranslationService.RegisterResources(nameof(NotificationPageResources), NotificationPageResources.ResourceManager);
        TranslationService.RegisterResources(nameof(FormResources), FormResources.ResourceManager);
        TranslationService.RegisterResources(nameof(DataGridPageResources), DataGridPageResources.ResourceManager);
        TranslationService.RegisterResources(nameof(MenuPageResources), MenuPageResources.ResourceManager);
        TranslationService.RegisterResources(nameof(NavigationMenuPageResources), NavigationMenuPageResources.ResourceManager);
        TranslationService.RegisterResources(nameof(DialogsPageResources), DialogsPageResources.ResourceManager);
        TranslationService.RegisterResources(nameof(ThemePageResources), ThemePageResources.ResourceManager);
    }

    private static void InitializeTheme(ServiceProvider services)
    {
        var registry = services.GetRequiredService<IThemeBaseRegistry>();
        registry.Register(registry.Dark);
        registry.Register(registry.Light);
        registry.Register(registry.HighContrast);
        registry.Register(new ThemeBase(ThemeVariantProvider.DarkBlue, true, false));
    }
}
