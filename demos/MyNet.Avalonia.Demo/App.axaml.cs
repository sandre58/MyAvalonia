// -----------------------------------------------------------------------
// <copyright file="App.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Concurrency;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using MyNet.Avalonia.Clipboard;
using MyNet.Avalonia.Demo.Pages;
using MyNet.Avalonia.Demo.Resources;
using MyNet.Avalonia.Demo.ViewModels;
using MyNet.Avalonia.Demo.Views;
using MyNet.Avalonia.Extended.Busy;
using MyNet.Avalonia.Extended.Clipboard;
using MyNet.Avalonia.Extended.Commands;
using MyNet.Avalonia.Extended.Navigation;
using MyNet.Avalonia.Extended.Schedulers;
using MyNet.Avalonia.Extended.Services;
using MyNet.Avalonia.Extended.Theming;
using MyNet.Avalonia.Extended.Toasting;
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
using MyNet.Utilities;
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
    /// <summary>
    /// Initializes the application by loading the XAML markup for the App class. This method is called during the application startup process to set up the application's resources and UI components defined in the App.xaml file.
    /// </summary>
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    /// <summary>
    /// Initializes the application framework and configures the main window or view based on the application's
    /// lifetime.
    /// </summary>
    /// <remarks>This method registers required services and view models, builds the service provider, and
    /// initializes application resources. It determines whether to display a main window or a main view depending on
    /// the application's lifetime type. Override this method to customize application startup behavior.</remarks>
    public override void OnFrameworkInitializationCompleted()
    {
        var viewModelTypes = ProvidePages();

        // Register all the services needed for the application to run
        var collection = new ServiceCollection();
        RegisterServices(collection);
        RegisterPageViewModels(collection, viewModelTypes.Keys);

        // Creates a ServiceProvider containing services from the provided IServiceCollection
        var services = collection.BuildServiceProvider();

        InitializeServices(services);
        InitializeTheme(services);
        InitializeResources();

        var vm = ViewModelManager.Get<MainViewModel>();
        RegisterPages(services, vm, viewModelTypes);

        switch (ApplicationLifetime)
        {
            case IClassicDesktopStyleApplicationLifetime desktop:
                desktop.MainWindow = new MainWindow { DataContext = vm };
                break;
            case ISingleViewApplicationLifetime singleView:
                singleView.MainView = new MainView { DataContext = vm };
                break;
        }

        NavigationManager.NavigateTo<HomePageViewModel>();

        base.OnFrameworkInitializationCompleted();
    }

    /// <summary>
    /// Registers the services required for the application to run. This method adds various services to the provided
    /// <see cref="ServiceCollection"/>.
    /// </summary>
    /// <param name="collection">The service collection to register services with.</param>
    private void RegisterServices(ServiceCollection collection)
        => collection.AddSingleton<ILogger, Logger>()
            .AddSingleton<IViewResolver, ViewResolver>()
            .AddSingleton<IViewModelLocator, ViewModelLocator>()
            .AddSingleton<IPageResolver, PageResolver>()
            .AddSingleton<IThemeBrushService>(MyTheme.Current)
            .AddSingleton<IThemeBaseRegistry, ThemeVariantsRegistry>()
            .AddSingleton<IThemeService, ThemeService>()
            .AddSingleton<INotificationsManager, NotificationsManager>()
            .AddSingleton<INavigationService, Extended.Navigation.NavigationService>()
            .AddSingleton<IToasterService>(new ToasterService(() => (ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow))
            .AddSingleton<IClipboardService>(new ClipboardService(() => (ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow))
            .AddScoped<IBusyServiceFactory, BusyServiceFactory>()
            .AddScoped<IScheduler, AvaloniaScheduler>(_ => AvaloniaScheduler.Current)
            .AddScoped<ICommandFactory, AvaloniaCommandFactory>()
            .AddScoped<IAppCommandsService, AppCommandsService>();

    /// <summary>
    /// Provides a mapping of ViewModel types to their corresponding Page types.
    /// </summary>
    /// <remarks>This method is used to facilitate the navigation between different pages in the application
    /// by linking ViewModels to their respective UI representations.</remarks>
    /// <returns>A dictionary where the key is a ViewModel type and the value is the associated Page type.</returns>
    private static Dictionary<Type, Type> ProvidePages() => new()
    {
        [typeof(HomePageViewModel)] = typeof(HomePage),
        [typeof(ThemePageViewModel)] = typeof(ThemePage),
        [typeof(IconsPageViewModel)] = typeof(IconsPage),
        [typeof(AvatarPageViewModel)] = typeof(AvatarPage),
        [typeof(BadgePageViewModel)] = typeof(BadgePage),
        [typeof(BannerPageViewModel)] = typeof(BannerPage),
        [typeof(BorderPageViewModel)] = typeof(BorderPage),
        [typeof(ButtonPageViewModel)] = typeof(ButtonPage),
        [typeof(CalendarPageViewModel)] = typeof(CalendarPage),
        [typeof(CarouselPageViewModel)] = typeof(CarouselPage),
        [typeof(CheckBoxPageViewModel)] = typeof(CheckBoxPage),
        [typeof(ClockPageViewModel)] = typeof(ClockPage),
        [typeof(ClockSelectorPageViewModel)] = typeof(ClockSelectorPage),
        [typeof(ColorViewPageViewModel)] = typeof(ColorViewPage),
        [typeof(DataGridPageViewModel)] = typeof(DataGridPage),
        [typeof(DialogPageViewModel)] = typeof(DialogPage),
        [typeof(DrawerPageViewModel)] = typeof(DrawerPage),
        [typeof(DropDownButtonPageViewModel)] = typeof(DropDownButtonPage),
        [typeof(EllipsePageViewModel)] = typeof(EllipsePage),
        [typeof(ExpanderPageViewModel)] = typeof(ExpanderPage),
        [typeof(FieldsPageViewModel)] = typeof(FieldsPage),
        [typeof(FormPageViewModel)] = typeof(FormPage),
        [typeof(GridSplitterPageViewModel)] = typeof(GridSplitterPage),
        [typeof(HeaderedContentControlPageViewModel)] = typeof(HeaderedContentControlPage),
        [typeof(HyperLinkButtonPageViewModel)] = typeof(HyperLinkButtonPage),
        [typeof(LabelPageViewModel)] = typeof(LabelPage),
        [typeof(ListBoxPageViewModel)] = typeof(ListBoxPage),
        [typeof(MenuPageViewModel)] = typeof(MenuPage),
        [typeof(NavigationMenuPageViewModel)] = typeof(NavigationMenuPage),
        [typeof(NotificationPageViewModel)] = typeof(NotificationPage),
        [typeof(OutlinedIconPageViewModel)] = typeof(OutlinedIconPage),
        [typeof(PaginationPageViewModel)] = typeof(PaginationPage),
        [typeof(ProgressBarPageViewModel)] = typeof(ProgressBarPage),
        [typeof(RadioButtonPageViewModel)] = typeof(RadioButtonPage),
        [typeof(SelectableTextBlockPageViewModel)] = typeof(SelectableTextBlockPage),
        [typeof(SliderPageViewModel)] = typeof(SliderPage),
        [typeof(SplitButtonPageViewModel)] = typeof(SplitButtonPage),
        [typeof(SplitViewPageViewModel)] = typeof(SplitViewPage),
        [typeof(TabControlPageViewModel)] = typeof(TabControlPage),
        [typeof(TextBlockPageViewModel)] = typeof(TextBlockPage),
        [typeof(TimeViewPageViewModel)] = typeof(TimeViewPage),
        [typeof(ToggleButtonPageViewModel)] = typeof(ToggleButtonPage),
        [typeof(ToggleSplitButtonPageViewModel)] = typeof(ToggleSplitButtonPage),
        [typeof(ToggleSwitchPageViewModel)] = typeof(ToggleSwitchPage),
        [typeof(TreeViewPageViewModel)] = typeof(TreeViewPage)
    };

    /// <summary>
    /// Registers the specified view model types with the provided service collection as singleton services.
    /// </summary>
    /// <remarks>This method always registers the MainViewModel as a singleton in addition to any types
    /// provided in the viewModelTypes parameter.</remarks>
    /// <param name="collection">The service collection to which the view models will be registered.</param>
    /// <param name="viewModelTypes">An enumerable collection of view model types to register as singletons.</param>
    private static void RegisterPageViewModels(ServiceCollection collection, IEnumerable<Type> viewModelTypes)
    {
        collection.AddSingleton<MainViewModel>();
        viewModelTypes.ForEach(x => collection.AddSingleton(x));
    }

    /// <summary>
    /// Registers the pages with the view resolver and adds them to the main view model's navigation pages collection.
    /// </summary>
    /// <param name="services">The service provider to resolve dependencies.</param>
    /// <param name="mainViewModel">The main view model to which the pages will be added.</param>
    /// <param name="pages">A dictionary mapping view model types to their corresponding page types.</param>
    private static void RegisterPages(ServiceProvider services, MainViewModel mainViewModel, Dictionary<Type, Type> pages)
    {
        var viewResolver = services.GetRequiredService<IViewResolver>();
        pages.ForEach(x => viewResolver.Register(x.Key, x.Value));
        mainViewModel.AddPages([.. pages.Keys.Select(x => (INavigationPage)services.GetRequiredService(x))]);
    }

    /// <summary>
    /// Initializes core application services and configures essential components required for application startup.
    /// </summary>
    /// <remarks>This method must be called during application startup to ensure that logging, view models,
    /// theming, navigation, clipboard, and other infrastructure services are properly configured and available
    /// throughout the application's lifetime.</remarks>
    /// <param name="services">The service provider used to resolve and supply dependencies for service initialization. Cannot be null.</param>
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

    /// <summary>
    /// Initializes the application's resources by calling the initialization methods of the Extended and Controls resource bootstrappers, and registering various resource managers with the translation service for localization support. This method ensures that all necessary resources are loaded and available for use throughout the application, enabling proper localization and theming functionality.
    /// </summary>
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

    /// <summary>
    /// Initializes the application's theme by registering various theme variants with the theme base registry. This method ensures that the application has access to different themes, such as dark, light, high contrast, and a custom dark blue theme, allowing users to switch between them based on their preferences or accessibility needs.
    /// </summary>
    /// <param name="services">The service provider to resolve dependencies.</param>
    private static void InitializeTheme(ServiceProvider services)
    {
        var registry = services.GetRequiredService<IThemeBaseRegistry>();
        registry.Register(registry.Dark);
        registry.Register(registry.Light);
        registry.Register(registry.HighContrast);
        registry.Register(new ThemeBase(ThemeVariantProvider.DarkBlue, true, false));
    }
}
