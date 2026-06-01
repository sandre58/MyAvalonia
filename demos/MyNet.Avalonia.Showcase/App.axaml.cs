// -----------------------------------------------------------------------
// <copyright file="App.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Material.Icons;
using Microsoft.Extensions.DependencyInjection;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Extended;
using MyNet.Avalonia.Extended.Clipboard;
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
using MyNet.Fakers;
using MyNet.Globalization;
using MyNet.Humanizer;
using MyNet.UI.Commands;
using MyNet.UI.Loading;
using MyNet.UI.Locators;
using MyNet.UI.Locators.Conventions;
using MyNet.UI.Navigation;
using MyNet.UI.Notifications;
using MyNet.UI.Services;
using MyNet.UI.Theming;
using MyNet.UI.Toasting;
using MyNet.UI.ViewModels;

namespace MyNet.Avalonia.Showcase;

public class App : Application
{
    /// <inheritdoc/>
    public override void Initialize()
    {
        ThemeControlsHost.Register();
        AvaloniaXamlLoader.Load(this);
    }

    /// <inheritdoc/>
    public override async void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var splash = new SplashScreenViewModel(new ApplicationInfo());
            var splashWindow = new SplashWindow { DataContext = splash };
            desktop.MainWindow = splashWindow;
            splashWindow.Show();

            await Task.Delay(50).ConfigureAwait(true);

            var vm = Prepare();

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

        base.OnFrameworkInitializationCompleted();
    }

    private static MainViewModel Prepare()
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
        services.UseFakers();

        InitializeServices(services);
        InitializeTheme(services);

        var vm = services.GetRequiredService<MainViewModel>();
        RegisterPages(services, vm, pagesProviders);
        _ = services.GetRequiredService<INavigationClient>().NavigateToAsync<HomePageViewModel>();
        return vm;
    }

    private static void RegisterServices(ServiceCollection collection)
    {
        collection.AddGlobalization()
            .AddLocalization()
            .AddInflection()
            .AddHumanizer()
            .AddMyNetAvalonia()
            .AddMyNetAvaloniaControls()
            .AddMyNetAvaloniaExtended()
            .AddAvaloniaTheming()
            .AddAvaloniaClipboard(() => (Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow)
            .AddAvaloniaAppCommands()
            .AddAvaloniaScheduler()
            .AddNotifications()
            .AddSingleton<IToastFactory, ShowcaseDemoToastFactory>()
            .AddToasting()
            .AddAvaloniaToasting(() => (Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow)
            .AddBusy()
            .AddNavigation()
            .AddViewLocators()
            .AddAvaloniaNavigation()
            .AddShell()
            .AddResources();

        collection.AddFakers();
        collection.AddSingleton<IApplicationInfo, ApplicationInfo>()
            .AddSingleton<IThemeBrushService>(MyTheme.Current)
            .AddScoped<ICommandFactory, AvaloniaCommandFactory>();
    }

    private static List<IPagesProvider> ProvidePages() =>
    [
        new PageAssociation(typeof(HomePageViewModel), typeof(HomePage)),
        new PageAssociation(typeof(ThemePageViewModel), typeof(ThemePage)),
        new PageAssociation(typeof(IconsPageViewModel), typeof(IconsPage)),

        new PagesGroup(nameof(MenuResources.Texts), MaterialIconKind.FormatText, [
            new(typeof(LabelPageViewModel), typeof(LabelPage)),
            new(typeof(SelectableTextBlockPageViewModel), typeof(SelectableTextBlockPage)),
            new(typeof(TextBlockPageViewModel), typeof(TextBlockPage))
        ]),

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

        new PagesGroup(nameof(MenuResources.Inputs), MaterialIconKind.FormTextbox, [
            new(typeof(ColorViewPageViewModel), typeof(ColorViewPage)),
            new(typeof(CalendarPageViewModel), typeof(CalendarPage)),
            new(typeof(ClockPageViewModel), typeof(ClockPage)),
            new(typeof(ClockSelectorPageViewModel), typeof(ClockSelectorPage)),
            new(typeof(FieldsPageViewModel), typeof(FieldsPage)),
            new(typeof(SliderPageViewModel), typeof(SliderPage)),
            new(typeof(TimeViewPageViewModel), typeof(TimeViewPage))
        ]),

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

        new PagesGroup(nameof(MenuResources.DataAndLists), MaterialIconKind.Table, [
            new(typeof(DataGridPageViewModel), typeof(DataGridPage)),
            new(typeof(ListBoxPageViewModel), typeof(ListBoxPage)),
            new(typeof(TreeViewPageViewModel), typeof(TreeViewPage))
        ]),

        new PagesGroup(nameof(MenuResources.Navigation), MaterialIconKind.BookOpenPageVariantOutline, [
            new(typeof(ContentPagePageViewModel), typeof(ContentPagePage)),
            new(typeof(CarouselPagePageViewModel), typeof(CarouselPagePage)),
            new(typeof(DrawerPagePageViewModel), typeof(DrawerPagePage)),
            new(typeof(MenuPageViewModel), typeof(MenuPage)),
            new(typeof(NavigationMenuPageViewModel), typeof(NavigationMenuPage)),
            new(typeof(PaginationPageViewModel), typeof(PaginationPage)),
            new(typeof(TabbedPagePageViewModel), typeof(TabbedPagePage))
        ]),

        new PagesGroup(nameof(MenuResources.DialogsAndFeeback), MaterialIconKind.MessageAlertOutline, [
            new(typeof(DialogPageViewModel), typeof(DialogPage)),
            new(typeof(NotificationPageViewModel), typeof(NotificationPage)),
            new(typeof(ProgressBarPageViewModel), typeof(ProgressBarPage))
        ]),

        new PagesGroup(nameof(MenuResources.ShapesAndVisuals), MaterialIconKind.Shape, [
            new(typeof(BorderPageViewModel), typeof(BorderPage)),
            new(typeof(EllipsePageViewModel), typeof(EllipsePage)),
            new(typeof(ExtendedIconPageViewModel), typeof(ExtendedIconPage))
        ])
    ];

    private static void RegisterPageViewModels(ServiceCollection collection, IEnumerable<Type> viewModelTypes)
    {
        collection.AddSingleton<MainViewModel>();
        viewModelTypes.ForEach(x => collection.AddSingleton(x));
    }

    private static void RegisterPages(IServiceProvider services, MainViewModel mainViewModel, List<IPagesProvider> pagesProvider)
    {
        var typeResolver = services.GetRequiredService<ITypeResolver>();
        pagesProvider.SelectMany(x => x.GetPageAssociations()).ForEach(x => typeResolver.Register(x.ViewModelType, x.ViewType));
        mainViewModel.AddMenuItem([.. pagesProvider.Select(x => CreateMenuItemViewModel(x, services))]);
    }

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

    private static void InitializeServices(IServiceProvider services)
    {
        services.UseThemeManager();
        services.UseAvaloniaNavigation();
        _ = services.GetRequiredService<AvaloniaToastHost>();
        services.UseClipboard();
    }

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
