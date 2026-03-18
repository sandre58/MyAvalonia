using Avalonia.Controls;
using Avalonia.Interactivity;
using MyNet.Avalonia.PerfTest.ViewModels;

namespace MyNet.Avalonia.PerfTest.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object? sender, RoutedEventArgs e)
    {
        if (DataContext is MainWindowViewModel vm)
        {
            var contentControl = this.FindControl<ContentControl>("PageHost");
            if (contentControl is not null)
                vm.SetMeasureTarget(contentControl);
        }
    }
}
