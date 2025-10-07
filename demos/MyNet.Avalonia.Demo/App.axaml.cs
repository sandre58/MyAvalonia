// -----------------------------------------------------------------------
// <copyright file="App.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.IO;
using System.Reactive.Concurrency;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using MyNet.Avalonia.Clipboard;
using MyNet.Avalonia.Demo.Pages;
using MyNet.Avalonia.Demo.Resources;
using MyNet.Avalonia.Demo.Services;
using MyNet.Avalonia.Demo.ViewModels;
using MyNet.Avalonia.Demo.ViewModels.Dialogs;
using MyNet.Avalonia.Demo.Views;
using MyNet.Avalonia.Demo.Views.Dialogs;
using MyNet.Avalonia.Theme;
using MyNet.Avalonia.UI.Busy;
using MyNet.Avalonia.UI.Clipboard;
using MyNet.Avalonia.UI.Commands;
using MyNet.Avalonia.UI.Dialogs;
using MyNet.Avalonia.UI.Schedulers;
using MyNet.Avalonia.UI.Services;
using MyNet.Avalonia.UI.Theming;
using MyNet.Avalonia.UI.Toasting;
using MyNet.UI.Commands;
using MyNet.UI.Extensions;
using MyNet.UI.Loading;
using MyNet.UI.Locators;
using MyNet.UI.Navigation;
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
    private static void RegisterViewModels(ServiceCollection collection)
        => collection.AddSingleton<MainViewModel>();

    private static void InitializeServices(ServiceProvider services)
    {
        // Logging
        Utilities.Logging.NLog.Logger.LoadConfiguration($"{Directory.GetCurrentDirectory()}/config/NLog.config");

        var viewModelLocator = services.GetRequiredService<IViewModelLocator>();
        var busyFactory = services.GetRequiredService<IBusyServiceFactory>();
        LogManager.Initialize(services.GetRequiredService<ILogger>());
        ViewModelManager.Initialize(services.GetRequiredService<IViewModelResolver>(), viewModelLocator);
        ViewManager.Initialize(services.GetRequiredService<IViewResolver>(), services.GetRequiredService<IViewLocator>());
        ThemeManager.Initialize(services.GetRequiredService<IThemeService>());
        NavigationManager.Initialize(services.GetRequiredService<INavigationService>(), viewModelLocator);
        ToasterManager.Initialize(services.GetRequiredService<IToasterService>());
        ClipboardManager.Initialize(services.GetRequiredService<IClipboardService>());
        DrawerManager.Initialize(services.GetRequiredService<IViewResolver>(), services.GetRequiredService<IViewLocator>());
        //DialogManager.Initialize(dialogService, messageBoxFactory, services.GetRequiredService<IViewResolver>(), services.GetRequiredService<IViewLocator>(), viewModelLocator);
        WindowDialogManager.Initialize(services.GetRequiredService<IViewResolver>(), services.GetRequiredService<IViewLocator>(), viewModelLocator);

        BusyManager.Initialize(busyFactory);
        AppBusyManager.Initialize(busyFactory);
        CommandsManager.Initialize(services.GetRequiredService<ICommandFactory>());
        MyNet.UI.Threading.Scheduler.Initialize(services.GetRequiredService<IScheduler>());
    }

    private static void RegisterViewAndViewModels(ServiceProvider services)
    {
        // Register the views for the view models to improve performances (avoiding reflection at runtime)
        RegisterViewAndViewModel<LoginDialogViewModel, LoginDialogView>(services);
        RegisterViewAndViewModel<DataGridsViewModel, DataGridsPage>(services);
        RegisterViewAndViewModel<DialogsViewModel, DialogsPage>(services);
        RegisterViewAndViewModel<DrawersViewModel, DrawersPage>(services);
        RegisterViewAndViewModel<IconsViewModel, IconsPage>(services);
        RegisterViewAndViewModel<MainViewModel, MainView>(services);
        RegisterViewAndViewModel<ThemeViewModel, ThemePage>(services);
    }

    private static void RegisterViewAndViewModel<TViewModel, TView>(ServiceProvider services)
    {
        var viewResolver = services.GetRequiredService<IViewResolver>();
        var viewModelResolver = services.GetRequiredService<IViewResolver>();

        viewResolver.Register<TViewModel, TView>();
        viewModelResolver.Register<TViewModel, TView>();
    }

    private static void InitializeResources()
    {
        UI.ResourceLocator.Initialize();
        Controls.ResourceLocator.Initialize();
        TranslationService.RegisterResources(nameof(CountryResources), CountryResources.ResourceManager);
        TranslationService.RegisterResources(nameof(DemoResources), DemoResources.ResourceManager);
    }

    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        // Register all the services needed for the application to run
        var collection = new ServiceCollection();
        RegisterServices(collection);
        RegisterViewModels(collection);

        // Creates a ServiceProvider containing services from the provided IServiceCollection
        var services = collection.BuildServiceProvider();

        InitializeServices(services);
        RegisterViewAndViewModels(services);

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

        _ = NavigationManager.NavigateTo<HomePage>();

        base.OnFrameworkInitializationCompleted();
    }

    private void RegisterServices(ServiceCollection collection)
        => collection.AddSingleton<ILogger, Utilities.Logging.NLog.Logger>()
                     .AddSingleton<IViewModelResolver, ViewModelResolver>()
                     .AddSingleton<IViewModelLocator, ViewModelLocator>(x => new ViewModelLocator(x))
                     .AddSingleton<IViewLocator, ViewLocator>()
                     .AddSingleton<IViewResolver, ViewResolver>()
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
                     .AddScoped<IAppCommandsService, AppCommandsService>()
                     ;
}
