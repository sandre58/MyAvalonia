// -----------------------------------------------------------------------
// <copyright file="App.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using MyNet.Avalonia.Showcase.Composition;
using MyNet.Avalonia.Showcase.Views;
using MyNet.Avalonia.Theme;

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
        _services = new AppComposition(GetTopLevel).Build();
        var mainViewModel = AppComposition.ConfigureMainViewModel(_services);

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
                mainViewModel.ShowShellChromeInView = true;
                singleView.MainView = new MainView { DataContext = mainViewModel };
                break;
        }

        AppComposition.NavigateToDefaultPage(_services);

        base.OnFrameworkInitializationCompleted();
    }

    private static TopLevel? GetTopLevel() => Current?.ApplicationLifetime switch
    {
        IClassicDesktopStyleApplicationLifetime desktop => desktop.MainWindow,
        ISingleViewApplicationLifetime { MainView: { } mainView } => TopLevel.GetTopLevel(mainView),
        _ => null
    };
}
