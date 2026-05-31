// -----------------------------------------------------------------------
// <copyright file="App.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using MyNet.Avalonia.PerfTest.ViewModels;
using MyNet.Avalonia.PerfTest.Views;

namespace MyNet.Avalonia.PerfTest;

public partial class App : Application
{
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
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
