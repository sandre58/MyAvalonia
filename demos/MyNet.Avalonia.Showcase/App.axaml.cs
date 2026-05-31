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
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Material.Icons;
using Microsoft.Extensions.DependencyInjection;
using MyNet.Avalonia;
using MyNet.Avalonia.Clipboard;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Extended;
using MyNet.Avalonia.Extended.Busy;
using MyNet.Avalonia.Extended.Commands;
using MyNet.Avalonia.Extended.Navigation;
using MyNet.Avalonia.Extended.Schedulers;
using MyNet.Avalonia.Extended.Services;
using MyNet.Avalonia.Extended.Theming;
using MyNet.Avalonia.Extended.Toasting;
using MyNet.Avalonia.Showcase.Pages;
using MyNet.Avalonia.Showcase.Resources;
using MyNet.Avalonia.Showcase.ViewModels;
using MyNet.Avalonia.Showcase.ViewModels.Base;
using MyNet.Avalonia.Showcase.ViewModels.Pages;
using MyNet.Avalonia.Showcase.Views;
using MyNet.Avalonia.Theme;
using MyNet.Avalonia.Theme.Controls;
using MyNet.Avalonia.Theme.Diagnostics;
using MyNet.Avalonia.Theme.Themes;
using MyNet.Avalonia.Theme.Theming.Core;
using MyNet.Globalization;
using MyNet.Humanizer;
using MyNet.UI.Commands;
using MyNet.UI.Loading;
using MyNet.UI.Locators;
using MyNet.UI.Navigation;
using MyNet.UI.Notifications;
using MyNet.UI.Services;
using MyNet.UI.Theming;
using MyNet.UI.Toasting;
using MyNet.Utilities;
using MyNet.Utilities.Geography.Extensions;
using MyNet.Utilities.Logging;
using MyNet.Utilities.Logging.NLog;
using PropertyChanged;
using NavigationService = MyNet.Avalonia.Extended.Navigation.NavigationService;
using Scheduler = MyNet.UI.Threading.Scheduler;

namespace MyNet.Avalonia.Showcase;

[DoNotNotify]
public class App : Application
{
    /// <summary>
    /// Initializes the application by loading the XAML markup for the App class. This method is called during the application startup process to set up the application's resources and UI components defined in the App.xaml file.
    /// </summary>
    public override void Initialize()
    {
        ThemeControlsHost.Register();
        AvaloniaXamlLoader.Load(this);
    }

    /// <summary>
    /// Initializes the application framework and configures the main window or view based on the application's
    /// lifetime.
    /// </summary>
    /// <remarks>This method registers required services and view models, builds the service provider, and
    /// initializes application resources. It determines whether to display a main window or a main view depending on
    /// the application's lifetime type. Override this method to customize application startup behavior.</remarks>
    // ReSharper disable once AsyncVoidMethod
    public override async void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Show a static splash screen immediately while we initialize on the UI thread.
            var splash = new SplashScreenViewModel();
            var splashWindow = new SplashWindow { DataContext = splash };
            desktop.MainWindow = splashWindow;
            splashWindow.Show();

            // Yield once so the splash window actually renders before the freeze.
            await Task.Delay(50).ConfigureAwait(true);

            var vm = Prepare();

            // Switch to the main window.
            var mainWindow = new MainWindow { DataContext = vm };
            desktop.MainWindow = mainWindow;
            mainWindow.Show();
            splashWindow.Close();
        }
        else
        {
            var vm = Prepare();

            if (ApplicationLifetime is ISingleViewApplicationLifetime singleView)
                singleView.MainView = new MainView { DataContext = vm };
        }

        NavigationManager.NavigateTo<HomePageViewModel>();

        base.OnFrameworkInitializationCompleted();
    }

    /// <summary>
    /// Prepares the application by registering services, view models, and pages, initializing resources and themes, and returning the main view model for the application. This method is responsible for setting up the application's dependency injection container, configuring logging, theming, navigation, and other core services required for the application to function properly. It also registers the pages with the view resolver and adds them to the main view model's navigation menu. The returned main view model is then used as the data context for the main window or view of the application.
    /// </summary>
    private MainViewModel Prepare()
    {
        var pagesProviders = ProvidePages();
        var viewModelTypes = pagesProviders.SelectMany(x => x.GetPageAssociations()).Select(x => x.ViewModelType).ToList();

        ThemeDiagnostics.ConfigureFromEnvironment();
        MyTheme.Current.EnsureLoaded();

        var collection = new ServiceCollection();
        RegisterServices(collection);
        RegisterPageViewModels(collection, viewModelTypes);

        var services = collection.BuildServiceProvider();

        services.UseGlobalization();
        services.UseLocalization();
        services.UseDisplayText();

        InitializeServices(services);
        InitializeTheme(services);

        var vm = ViewModelManager.Get<MainViewModel>();
        RegisterPages(services, vm, pagesProviders);
        return vm;
    }

    /// <summary>
    /// Registers the services required for the application to run. This method adds various services to the provided
    /// <see cref="ServiceCollection"/>.
    /// </summary>
    /// <param name="collection">The service collection to register services with.</param>
    private void RegisterServices(ServiceCollection collection)
    {
        collection.AddGlobalization()
            .AddLocalization()
            .AddInflection()
            .AddHumanizer()
            .AddMyNetAvalonia()
            .AddMyNetAvaloniaControls()
            .AddMyNetAvaloniaExtended()
            .AddAvaloniaTheming()
            .AddAvaloniaClipboard(() => (ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow)
            .AddMyNetAvaloniaShowcaseResources();

        collection.AddSingleton<ILogger, Logger>()
            .AddSingleton<IViewResolver, ViewResolver>()
            .AddSingleton<IViewModelLocator, ViewModelLocator>()
            .AddSingleton<IPageResolver, PageResolver>()
            .AddSingleton<IThemeBrushService>(MyTheme.Current)
            .AddNotifications()
            .AddSingleton<INavigationService, NavigationService>()
            .AddSingleton<IToasterService>(new ToasterService(() => (ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow))
            .AddScoped<IBusyServiceFactory, BusyServiceFactory>()
            .AddScoped<IScheduler, AvaloniaScheduler>(_ => AvaloniaScheduler.Current)
            .AddScoped<ICommandFactory, AvaloniaCommandFactory>()
            .AddScoped<IAppCommandsService, AppCommandsService>();
    }

    /// <summary>
    /// Provides a mapping of ViewModel types to their corresponding Page types.
    /// </summary>
    /// <remarks>This method is used to facilitate the navigation between different pages in the application
    /// by linking ViewModels to their respective UI representations.</remarks>
    /// <returns>A dictionary where the key is a ViewModel type and the value is the associated Page type.</returns>
    private static List<IPagesProvider> ProvidePages() =>
    [

        // General pages
        new PageAssociation(typeof(HomePageViewModel), typeof(HomePage)),
        new PageAssociation(typeof(ThemePageViewModel), typeof(ThemePage)),
        new PageAssociation(typeof(IconsPageViewModel), typeof(IconsPage)),

        // Text & Labels group
        new PagesGroup(nameof(MenuResources.Texts), MaterialIconKind.FormatText, [
            new(typeof(LabelPageViewModel), typeof(LabelPage)),
            new(typeof(SelectableTextBlockPageViewModel), typeof(SelectableTextBlockPage)),
            new(typeof(TextBlockPageViewModel), typeof(TextBlockPage))
        ]),

        // Buttons group
        new PagesGroup(nameof(MenuResources.Buttons), MaterialIconKind.GestureTapButton, [
            new(typeof(ButtonPageViewModel), typeof(ButtonPage)),
            new(typeof(ButtonSpinnerPageViewModel), typeof(ButtonSpinnerPage)),
            new(typeof(CheckBoxPageViewModel), typeof(CheckBoxPage)),
            new(typeof(DropDownButtonPageViewModel), typeof(DropDownButtonPage)),
            new(typeof(HyperLinkButtonPageViewModel), typeof(HyperLinkButtonPage)),
            new(typeof(RadioButtonPageViewModel), typeof(RadioButtonPage)),
            new(typeof(SplitButtonPageViewModel), typeof(SplitButtonPage)),
            new(typeof(ToggleButtonPageViewModel), typeof(ToggleButtonPage)),
            new(typeof(ToggleSplitButtonPageViewModel), typeof(ToggleSplitButtonPage)),
            new(typeof(ToggleSwitchPageViewModel), typeof(ToggleSwitchPage))
        ]),

        // Input group
        new PagesGroup(nameof(MenuResources.Inputs), MaterialIconKind.FormTextbox, [
            new(typeof(ColorViewPageViewModel), typeof(ColorViewPage)),
            new(typeof(CalendarPageViewModel), typeof(CalendarPage)),
            new(typeof(ClockPageViewModel), typeof(ClockPage)),
            new(typeof(ClockSelectorPageViewModel), typeof(ClockSelectorPage)),
            new(typeof(FieldsPageViewModel), typeof(FieldsPage)),
            new(typeof(SliderPageViewModel), typeof(SliderPage)),
            new(typeof(TimeViewPageViewModel), typeof(TimeViewPage)),
        ]),

        // Containers & Banners group
        new PagesGroup(nameof(MenuResources.Containers), MaterialIconKind.ViewCarousel, [
            new(typeof(AvatarPageViewModel), typeof(AvatarPage)),
            new(typeof(BadgePageViewModel), typeof(BadgePage)),
            new(typeof(BannerPageViewModel), typeof(BannerPage)),
            new(typeof(CarouselPageViewModel), typeof(CarouselPage)),
            new(typeof(ExpanderPageViewModel), typeof(ExpanderPage)),
            new(typeof(FormPageViewModel), typeof(FormPage)),
            new(typeof(GridSplitterPageViewModel), typeof(GridSplitterPage)),
            new(typeof(HeaderedContentControlPageViewModel), typeof(HeaderedContentControlPage)),
            new(typeof(SplitViewPageViewModel), typeof(SplitViewPage)),
            new(typeof(TabControlPageViewModel), typeof(TabControlPage))
        ]),

        // Data, Lists & Trees group
        new PagesGroup(nameof(MenuResources.DataAndLists), MaterialIconKind.Table, [
            new(typeof(DataGridPageViewModel), typeof(DataGridPage)),
            new(typeof(ListBoxPageViewModel), typeof(ListBoxPage)),
            new(typeof(TreeViewPageViewModel), typeof(TreeViewPage)),
        ]),

        // Navigation group
        new PagesGroup(nameof(MenuResources.Navigation), MaterialIconKind.BookOpenPageVariantOutline, [
            new(typeof(ContentPagePageViewModel), typeof(ContentPagePage)),
            new(typeof(CarouselPagePageViewModel), typeof(CarouselPagePage)),
            new(typeof(DrawerPagePageViewModel), typeof(DrawerPagePage)),
            new(typeof(MenuPageViewModel), typeof(MenuPage)),
            new(typeof(NavigationMenuPageViewModel), typeof(NavigationMenuPage)),
            new(typeof(PaginationPageViewModel), typeof(PaginationPage)),
            new(typeof(TabbedPagePageViewModel), typeof(TabbedPagePage))
        ]),

        // Dialogs & Feedback group
        new PagesGroup(nameof(MenuResources.DialogsAndFeeback), MaterialIconKind.MessageAlertOutline, [
            new(typeof(DialogPageViewModel), typeof(DialogPage)),
            new(typeof(NotificationPageViewModel), typeof(NotificationPage)),
            new(typeof(ProgressBarPageViewModel), typeof(ProgressBarPage))
        ]),

        // Shapes & Visuals group
        new PagesGroup(nameof(MenuResources.ShapesAndVisuals), MaterialIconKind.Shape, [
            new(typeof(BorderPageViewModel), typeof(BorderPage)),
            new(typeof(EllipsePageViewModel), typeof(EllipsePage)),
            new(typeof(ExtendedIconPageViewModel), typeof(ExtendedIconPage)),
        ])
    ];

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
    /// <param name="pagesProvider">A dictionary mapping view model types to their corresponding page types.</param>
    private static void RegisterPages(IServiceProvider services, MainViewModel mainViewModel, List<IPagesProvider> pagesProvider)
    {
        var viewResolver = services.GetRequiredService<IViewResolver>();
        pagesProvider.SelectMany(x => x.GetPageAssociations()).ForEach(x => viewResolver.Register(x.ViewModelType, x.ViewType));
        mainViewModel.AddMenuItem([.. pagesProvider.Select(x => CreateMenuItemViewModel(x, services))]);
    }

    /// <summary>
    /// Creates an instance of <see cref="IMenuItemViewModel"/> based on the provided <see cref="IPagesProvider"/>. If the provider is a <see cref="PageAssociation"/>, it resolves the corresponding view model from the service provider and returns it as a <see cref="PageViewModel"/>. If the provider is a <see cref="PagesGroup"/>, it creates a new <see cref="PagesGroupViewModel"/> with the specified resource key and icon, and adds the associated pages to the group by resolving their view models from the service provider. This method allows for dynamic creation of menu items based on the structure defined in the pages provider, enabling both individual page associations and grouped page collections in the application's navigation menu.
    /// </summary>
    /// <param name="pagesProvider">The pages provider, which can be a single page association or a group of pages.</param>
    /// <param name="services">The service provider used to resolve ViewModel instances.</param>
    /// <returns>An instance of <see cref="IMenuItemViewModel"/> corresponding to the given pages provider.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the type of pages provider is not supported.</exception>
    private static IMenuItemViewModel CreateMenuItemViewModel(IPagesProvider pagesProvider, IServiceProvider services)
    {
        switch (pagesProvider)
        {
            case PageAssociation pageAssociation:
                return (PageViewModel)services.GetRequiredService(pageAssociation.ViewModelType);

            case PagesGroup pagesGroup:
                var group = new PagesGroupViewModel(pagesGroup.ResourceKey, pagesGroup.Icon);
                group.AddPages([.. pagesGroup.Associations.Select(x => (PageViewModel)services.GetRequiredService(x.ViewModelType))]);
                return group;

            default:
                throw new ArgumentOutOfRangeException(nameof(pagesProvider), pagesProvider, null);
        }
    }

    /// <summary>
    /// Initializes core application services and configures essential components required for application startup.
    /// </summary>
    /// <remarks>This method must be called during application startup to ensure that logging, view models,
    /// theming, navigation, clipboard, and other infrastructure services are properly configured and available
    /// throughout the application's lifetime.</remarks>
    /// <param name="services">The service provider used to resolve and supply dependencies for service initialization. Cannot be null.</param>
    private static void InitializeServices(IServiceProvider services)
    {
        // Logging
        Logger.LoadConfiguration($"{Directory.GetCurrentDirectory()}/config/NLog.config");

        var viewModelLocator = services.GetRequiredService<IViewModelLocator>();
        var busyFactory = services.GetRequiredService<IBusyServiceFactory>();
        LogManager.Initialize(services.GetRequiredService<ILogger>());
        ViewModelManager.Initialize(null!, viewModelLocator);
        services.UseThemeManager();
        NavigationManager.Initialize(services.GetRequiredService<INavigationService>(), viewModelLocator);
        ToasterManager.Initialize(services.GetRequiredService<IToasterService>());
        services.UseClipboard();
        BusyManager.Initialize(busyFactory);
        AppBusyManager.Initialize(busyFactory);
        CommandsManager.Initialize(services.GetRequiredService<ICommandFactory>());
        Scheduler.Initialize(services.GetRequiredService<IScheduler>());
    }

    /// <summary>
    /// Initializes the application's theme by registering various theme variants with the theme base registry. This method ensures that the application has access to different themes, such as dark, light, high contrast, and a custom dark blue theme, allowing users to switch between them based on their preferences or accessibility needs.
    /// </summary>
    /// <param name="services">The service provider to resolve dependencies.</param>
    private static void InitializeTheme(IServiceProvider services)
    {
        var registry = services.GetRequiredService<IThemeBaseRegistry>();
        registry.Register(new ThemeBase(ThemeVariantProvider.DarkBlue, true, false));
    }

    private sealed record PagesGroup(string? ResourceKey, MaterialIconKind Icon, IList<PageAssociation> Associations) : IPagesProvider
    {
        public IEnumerable<PageAssociation> GetPageAssociations() => Associations;
    }

    private sealed record PageAssociation(Type ViewModelType, Type ViewType) : IPagesProvider
    {
        public IEnumerable<PageAssociation> GetPageAssociations() => [this];
    }

    private interface IPagesProvider
    {
        IEnumerable<PageAssociation> GetPageAssociations();
    }
}
