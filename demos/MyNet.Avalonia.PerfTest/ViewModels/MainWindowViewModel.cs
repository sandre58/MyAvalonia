using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using MyNet.Avalonia.PerfTest.Services;

namespace MyNet.Avalonia.PerfTest.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private ViewModelBase? _currentPage;
    private string _navigationTime = "0 ms";
    private string _memoryUsage = "0 KB";
    private string _gcInfo = "";
    private string _benchmarkStatus = "";
    private bool _isBenchmarkRunning;
    private readonly ObservableCollection<PerformanceResult> _performanceHistory = new();

    // Reference to the visual host for render measurement
    private Control? _measureTarget;

    public MainWindowViewModel()
    {
        NavigateToHomeCommand = new RelayCommand(NavigateToHome);
        NavigateToDataGridCommand = new RelayCommand(NavigateToDataGrid);
        NavigateToComplexLayoutCommand = new RelayCommand(NavigateToComplexLayout);
        NavigateToListCommand = new RelayCommand(NavigateToList);
        NavigateToFormsCommand = new RelayCommand(NavigateToForms);
        NavigateToStyleProfilingCommand = new RelayCommand(NavigateToStyleProfiling);
        NavigateToThemeListCommand = new RelayCommand(NavigateToThemeList);
        RunBenchmarkCommand = new RelayCommand(async () => await RunBenchmarkAsync(), () => !_isBenchmarkRunning);
        ClearHistoryCommand = new RelayCommand(ClearHistory);

        CurrentPage = new HomeViewModel();
    }

    /// <summary>
    /// Sets the target control for render measurement (should be the ContentControl hosting pages).
    /// </summary>
    public void SetMeasureTarget(Control target) => _measureTarget = target;

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

    public string GcInfo
    {
        get => _gcInfo;
        set => SetProperty(ref _gcInfo, value);
    }

    public string BenchmarkStatus
    {
        get => _benchmarkStatus;
        set => SetProperty(ref _benchmarkStatus, value);
    }

    public ObservableCollection<PerformanceResult> PerformanceHistory => _performanceHistory;

    public ICommand NavigateToHomeCommand { get; }
    public ICommand NavigateToDataGridCommand { get; }
    public ICommand NavigateToComplexLayoutCommand { get; }
    public ICommand NavigateToListCommand { get; }
    public ICommand NavigateToFormsCommand { get; }
    public ICommand NavigateToStyleProfilingCommand { get; }
    public ICommand NavigateToThemeListCommand { get; }
    public ICommand RunBenchmarkCommand { get; }
    public ICommand ClearHistoryCommand { get; }

    private async void NavigateTo(Func<ViewModelBase> createViewModel, string pageName)
    {
        if (_measureTarget is not null)
        {
            var result = await PerformanceMonitor.MeasureRenderAsync(
                $"Navigate to {pageName}",
                _measureTarget,
                () => CurrentPage = createViewModel());

            UpdateResult(result);
        }
        else
        {
            // Fallback without render measurement
            var stopwatch = Stopwatch.StartNew();
            var memoryBefore = GC.GetTotalMemory(true);

            CurrentPage = createViewModel();

            stopwatch.Stop();
            var memoryAfter = GC.GetTotalMemory(false);

            var result = new PerformanceResult
            {
                OperationName = $"Navigate to {pageName}",
                ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
                MemoryUsedBytes = memoryAfter - memoryBefore,
                Timestamp = DateTime.Now
            };

            UpdateResult(result);
        }
    }

    private void UpdateResult(PerformanceResult result)
    {
        NavigationTime = $"{result.ElapsedMilliseconds} ms";
        MemoryUsage = result.MemoryFormatted;
        GcInfo = result.GcInfo;

        _performanceHistory.Insert(0, result);

        while (_performanceHistory.Count > 50)
            _performanceHistory.RemoveAt(_performanceHistory.Count - 1);

        Debug.WriteLine($"[PERF] {result}");
    }

    private async Task RunBenchmarkAsync()
    {
        if (_measureTarget is null || _isBenchmarkRunning) return;

        _isBenchmarkRunning = true;
        BenchmarkStatus = "Benchmark en cours...";

        var pages = new List<(string Name, Action Navigate)>
        {
            ("Home", () => CurrentPage = new HomeViewModel()),
            ("DataGrid", () => CurrentPage = new DataGridViewModel()),
            ("Complex Layout", () => CurrentPage = new ComplexLayoutViewModel()),
            ("List", () => CurrentPage = new ListViewModel()),
            ("Theme List", () => CurrentPage = new ThemeListViewModel()),
            ("Forms", () => CurrentPage = new FormsViewModel())
        };

        try
        {
            var results = await PerformanceMonitor.RunFullBenchmarkAsync(_measureTarget, pages, iterations: 3);

            foreach (var result in results)
            {
                _performanceHistory.Insert(0, result);
                Debug.WriteLine($"[BENCHMARK] {result}");
            }

            // Compute and log averages
            Debug.WriteLine("========== BENCHMARK SUMMARY ==========");
            foreach (var page in pages)
            {
                long totalMs = 0;
                long totalMem = 0;
                var count = 0;
                foreach (var r in results)
                {
                    if (r.OperationName.Contains(page.Name))
                    {
                        totalMs += r.ElapsedMilliseconds;
                        totalMem += r.MemoryUsedBytes;
                        count++;
                    }
                }
                if (count > 0)
                {
                    Debug.WriteLine($"[AVG] {page.Name}: {totalMs / count}ms, Memory: {PerformanceMonitor.FormatBytes(totalMem / count)}");
                }
            }
            Debug.WriteLine("========================================");

            BenchmarkStatus = $"Terminé - {results.Count} mesures";
        }
        catch (Exception ex)
        {
            BenchmarkStatus = $"Erreur: {ex.Message}";
        }
        finally
        {
            _isBenchmarkRunning = false;
        }
    }

    private void ClearHistory()
    {
        _performanceHistory.Clear();
        NavigationTime = "0 ms";
        MemoryUsage = "0 KB";
        GcInfo = "";
        BenchmarkStatus = "";
    }

    private void NavigateToHome() => NavigateTo(() => new HomeViewModel(), "Home");
    private void NavigateToDataGrid() => NavigateTo(() => new DataGridViewModel(), "DataGrid");
    private void NavigateToComplexLayout() => NavigateTo(() => new ComplexLayoutViewModel(), "Complex Layout");
    private void NavigateToList() => NavigateTo(() => new ListViewModel(), "List");
    private void NavigateToForms() => NavigateTo(() => new FormsViewModel(), "Forms");
    private void NavigateToStyleProfiling() => NavigateTo(() => new StyleProfilingViewModel(), "Style Profiling");
    private void NavigateToThemeList() => NavigateTo(() => new ThemeListViewModel(), "Theme List");
}

// Simple RelayCommand implementation
public class RelayCommand : ICommand
{
    private readonly Action? _execute;
    private readonly Func<Task>? _executeAsync;
    private readonly Func<bool>? _canExecute;

    public RelayCommand(Action execute, Func<bool>? canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public RelayCommand(Func<Task> executeAsync, Func<bool>? canExecute = null)
    {
        _executeAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
        _canExecute = canExecute;
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;

    public async void Execute(object? parameter)
    {
        if (_executeAsync is not null)
            await _executeAsync();
        else
            _execute?.Invoke();
    }

    public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}
