// -----------------------------------------------------------------------
// <copyright file="App.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

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
using MyNet.Avalonia.Theme;
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
using PropertyChanged;

namespace MyNet.Avalonia.Demo;

[DoNotNotify]
public class App : Application
{
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        // Register all the services needed for the application to run
        var collection = new ServiceCollection();
        RegisterServices(collection);
        RegisterPageViewModels(collection);

        // Creates a ServiceProvider containing services from the provided IServiceCollection
        var services = collection.BuildServiceProvider();

        InitializeServices(services);

        InitializeResources();

        var vm = ViewModelManager.Get<MainViewModel>();
        switch (ApplicationLifetime)
        {
            case IClassicDesktopStyleApplicationLifetime desktop:
                desktop.MainWindow = new MainWindow { DataContext = vm };
                break;
            case ISingleViewApplicationLifetime singleView:
                singleView.MainView = new MainView { DataContext = vm };
                break;
        }

        _ = NavigationManager.NavigateTo<HomePageViewModel>();

        base.OnFrameworkInitializationCompleted();
    }

    private void RegisterServices(ServiceCollection collection)
        => collection.AddSingleton<ILogger, Utilities.Logging.NLog.Logger>()
                     .AddSingleton<IViewModelLocator, ViewModelLocator>()
                     .AddSingleton<IThemeService>(new ThemeService(MyTheme.Current))
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

    private static void RegisterPageViewModels(ServiceCollection collection)
    {
        collection.AddSingleton<MainViewModel>();
        foreach (var viewModelType in Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsClass && !t.IsAbstract && t.IsAssignableTo(typeof(INavigationPage))))
        {
            collection.AddSingleton(viewModelType);
        }
    }

    private static void InitializeServices(ServiceProvider services)
    {
        // Logging
        Utilities.Logging.NLog.Logger.LoadConfiguration($"{Directory.GetCurrentDirectory()}/config/NLog.config");

        var viewModelLocator = services.GetRequiredService<IViewModelLocator>();
        var busyFactory = services.GetRequiredService<IBusyServiceFactory>();
        LogManager.Initialize(services.GetRequiredService<ILogger>());
        ViewModelManager.Initialize(null!, viewModelLocator);
        ThemeManager.Initialize(services.GetRequiredService<IThemeService>());
        NavigationManager.Initialize(services.GetRequiredService<INavigationService>(), viewModelLocator);
        ToasterManager.Initialize(services.GetRequiredService<IToasterService>());
        ClipboardManager.Initialize(services.GetRequiredService<IClipboardService>());
        BusyManager.Initialize(busyFactory);
        AppBusyManager.Initialize(busyFactory);
        CommandsManager.Initialize(services.GetRequiredService<ICommandFactory>());
        UI.Threading.Scheduler.Initialize(services.GetRequiredService<IScheduler>());
    }

    private static void InitializeResources()
    {
        Extended.ResourceLocator.Initialize();
        Avalonia.Controls.ResourceLocator.Initialize();
        TranslationService.RegisterResources(nameof(CountryResources), CountryResources.ResourceManager);
        TranslationService.RegisterResources(nameof(DemoResources), DemoResources.ResourceManager);
        TranslationService.RegisterResources(nameof(FormResources), FormResources.ResourceManager);
    }
}
