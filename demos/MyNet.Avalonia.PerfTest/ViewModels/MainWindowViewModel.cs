using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using MyNet.Avalonia.PerfTest.Services;

namespace MyNet.Avalonia.PerfTest.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private ViewModelBase? _currentPage;
    private string _navigationTime = "0 ms";
    private string _memoryUsage = "0 KB";
    private readonly ObservableCollection<PerformanceResult> _performanceHistory = new();

    public MainWindowViewModel()
    {
        NavigateToHomeCommand = new RelayCommand(NavigateToHome);
        NavigateToDataGridCommand = new RelayCommand(NavigateToDataGrid);
        NavigateToComplexLayoutCommand = new RelayCommand(NavigateToComplexLayout);
        NavigateToListCommand = new RelayCommand(NavigateToList);
        NavigateToFormsCommand = new RelayCommand(NavigateToForms);

        CurrentPage = new HomeViewModel();
    }

    public ViewModelBase? CurrentPage
    {
        get => _currentPage;
        set => SetProperty(ref _currentPage, value);
    }

    public string NavigationTime
    {
        get => _navigationTime;
        set => SetProperty(ref _navigationTime, value);
    }

    public string MemoryUsage
    {
        get => _memoryUsage;
        set => SetProperty(ref _memoryUsage, value);
    }

    public ObservableCollection<PerformanceResult> PerformanceHistory => _performanceHistory;

    public ICommand NavigateToHomeCommand { get; }
    public ICommand NavigateToDataGridCommand { get; }
    public ICommand NavigateToComplexLayoutCommand { get; }
    public ICommand NavigateToListCommand { get; }
    public ICommand NavigateToFormsCommand { get; }

    private void NavigateTo(Func<ViewModelBase> createViewModel, string pageName)
    {
        var stopwatch = Stopwatch.StartNew();
        var memoryBefore = GC.GetTotalMemory(true);

        // Change page
        CurrentPage = createViewModel();

        stopwatch.Stop();
        var memoryAfter = GC.GetTotalMemory(false);
        var memoryDelta = memoryAfter - memoryBefore;

        // Create result
        var result = new PerformanceResult
        {
            OperationName = $"Navigate to {pageName}",
            ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
            MemoryUsedBytes = memoryDelta,
            Timestamp = DateTime.Now
        };

        // Update UI
        NavigationTime = $"{result.ElapsedMilliseconds} ms";
        MemoryUsage = Services.PerformanceMonitor.FormatBytes(result.MemoryUsedBytes);

        _performanceHistory.Insert(0, result);

        // Keep only last 20 results
        while (_performanceHistory.Count > 20)
        {
            _performanceHistory.RemoveAt(_performanceHistory.Count - 1);
        }

        Debug.WriteLine($"[PERF] {result}");
    }

    private void NavigateToHome() => NavigateTo(() => new HomeViewModel(), "Home");
    private void NavigateToDataGrid() => NavigateTo(() => new DataGridViewModel(), "DataGrid");
    private void NavigateToComplexLayout() => NavigateTo(() => new ComplexLayoutViewModel(), "Complex Layout");
    private void NavigateToList() => NavigateTo(() => new ListViewModel(), "List");
    private void NavigateToForms() => NavigateTo(() => new FormsViewModel(), "Forms");
}

// Simple RelayCommand implementation
public class RelayCommand : ICommand
{
    private readonly Action _execute;
    private readonly Func<bool>? _canExecute;

    public RelayCommand(Action execute, Func<bool>? canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;

    public void Execute(object? parameter) => _execute();

    public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}
