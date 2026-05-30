// -----------------------------------------------------------------------
// <copyright file="App.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Microsoft.Extensions.DependencyInjection;
using MyNet.Avalonia.PerfTest.ViewModels;
using MyNet.Avalonia.PerfTest.Views;
using MyNet.Utilities.Logging;
using MyNet.Utilities.Logging.NLog;

namespace MyNet.Avalonia.PerfTest;

public partial class App : Application
{
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        var collection = new ServiceCollection();
        RegisterServices(collection);

        // Creates a ServiceProvider containing services from the provided IServiceCollection
        var services = collection.BuildServiceProvider();

        InitializeServices();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Ensure MainWindow and ViewModel are created on UI thread
            Dispatcher.UIThread.Post(() =>
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel()
                };
                desktop.MainWindow.Show();
            });
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static void RegisterServices(ServiceCollection collection) => collection.AddSingleton<ILogger, Logger>();

    private static void InitializeServices() => Logger.LoadConfiguration($"{Directory.GetCurrentDirectory()}/config/NLog.config");
}
