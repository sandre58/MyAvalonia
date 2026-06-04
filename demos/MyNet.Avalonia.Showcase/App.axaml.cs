// -----------------------------------------------------------------------
// <copyright file="App.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyNet.Avalonia.Showcase.Composition;
using MyNet.Avalonia.Showcase.Composition.Logging;
using MyNet.Avalonia.Showcase.Views;
using MyNet.Avalonia.Theme;

namespace MyNet.Avalonia.Showcase;

public sealed partial class App : Application
{
    private ServiceProvider? _services;
    private ILogger<App>? _logger;

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
        _logger = _services.GetRequiredService<ILogger<App>>();
        if (_logger.IsEnabled(LogLevel.Information))
            LogShowcaseApplicationStartingLoggingMode(LoggingBootstrap.LoggingMode);

        var mainViewModel = AppComposition.ConfigureMainViewModel(_services);

        switch (ApplicationLifetime)
        {
            case IClassicDesktopStyleApplicationLifetime desktop:
                {
                    desktop.Exit += OnApplicationExit;
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

    private void OnApplicationExit(object? sender, EventArgs e) => DisposeServices();

    private void DisposeServices()
    {
        if (_services is null)
            return;

        if (_logger?.IsEnabled(LogLevel.Information) == true)
            LogShowcaseApplicationShuttingDown();
        _services.Dispose();
        _services = null;
        _logger = null;
    }

    private static TopLevel? GetTopLevel() => Current?.ApplicationLifetime switch
    {
        IClassicDesktopStyleApplicationLifetime desktop => desktop.MainWindow,
        ISingleViewApplicationLifetime { MainView: { } mainView } => TopLevel.GetTopLevel(mainView),
        _ => null
    };

    [LoggerMessage(LogLevel.Information, "Showcase application starting ({LoggingMode})")]
    partial void LogShowcaseApplicationStartingLoggingMode(LoggingMode loggingMode);

    [LoggerMessage(LogLevel.Information, "Showcase application shutting down")]
    partial void LogShowcaseApplicationShuttingDown();
}
