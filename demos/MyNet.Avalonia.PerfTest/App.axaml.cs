using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using MyNet.Avalonia.PerfTest.ViewModels;
using MyNet.Avalonia.PerfTest.Views;

namespace MyNet.Avalonia.PerfTest;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Ensure MainWindow and ViewModel are created on UI thread
            Dispatcher.UIThread.Post(() =>
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(),
                };
                desktop.MainWindow.Show();
            });
        }

        base.OnFrameworkInitializationCompleted();
    }
}
