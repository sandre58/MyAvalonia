using System.Collections.ObjectModel;
using MyNet.Avalonia.PerfTest.Services;

namespace MyNet.Avalonia.PerfTest.ViewModels;

public class PerformanceHistoryViewModel : ViewModelBase
{
    public ObservableCollection<PerformanceResult> History { get; }

    public PerformanceHistoryViewModel(ObservableCollection<PerformanceResult> history)
    {
        History = history;
    }
}
