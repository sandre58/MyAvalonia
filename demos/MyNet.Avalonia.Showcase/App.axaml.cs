// -----------------------------------------------------------------------
// <copyright file="App.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using MyNet.Avalonia.Showcase.Composition;
using MyNet.Avalonia.Showcase.ViewModels;
using MyNet.Avalonia.Showcase.ViewModels.Pages;
using MyNet.Avalonia.Showcase.Views;
using MyNet.Avalonia.Theme;
using MyNet.UI.Navigation;

namespace MyNet.Avalonia.Showcase;

public sealed class App : Application
{
    private ServiceProvider? _services;

    /// <inheritdoc/>
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        MyTheme.Current.EnsureLoaded();
    }

    /// <inheritdoc/>
    public override void OnFrameworkInitializationCompleted()
    {
        var composition = new AppComposition(GetTopLevel);
        _services = composition.Build();

        var mainViewModel = CreateMainViewModel();

        switch (ApplicationLifetime)
        {
            case IClassicDesktopStyleApplicationLifetime desktop:
                {
                    var mainWindow = new MainWindow { DataContext = mainViewModel };
                    desktop.MainWindow = mainWindow;
                    mainWindow.Show();
                    break;
                }

            case ISingleViewApplicationLifetime singleView:
                singleView.MainView = new MainView { DataContext = mainViewModel };
                break;
        }

        _ = _services.GetRequiredService<INavigationClient>().NavigateToAsync<HomePageViewModel>();

        base.OnFrameworkInitializationCompleted();
    }

    private static Window? GetTopLevel() => (Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;

    private MainViewModel CreateMainViewModel()
    {
        var mainViewModel = _services!.GetRequiredService<MainViewModel>();
        var providers = PagesCatalog.GetProviders();
        mainViewModel.AddMenuItem([.. providers.Select(x => PagesCatalog.CreateMenuItem(x, _services))]);
        return mainViewModel;
    }
}
