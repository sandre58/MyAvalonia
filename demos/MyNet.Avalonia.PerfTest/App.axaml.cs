// -----------------------------------------------------------------------
// <copyright file="App.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using MyNet.Avalonia.Theme;
using MyNet.Avalonia.Theme.Controls;
using MyNet.Avalonia.PerfTest.ViewModels;
using MyNet.Avalonia.PerfTest.Views;

namespace MyNet.Avalonia.PerfTest;

public partial class App : Application
{
    public override void Initialize()
    {
        ThemeControlsHost.Register();
        AvaloniaXamlLoader.Load(this);
        MyTheme.Current.EnsureLoaded();
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            ThemeControlsHost.AttachCatalog(this);
#if DEBUG
            ThemeControlsHost.EnsureCatalogAttached(this);
#endif

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
}
