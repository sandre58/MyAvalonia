using System;
using System.Diagnostics;

namespace MyNet.Avalonia.PerfTest.Services;

public static class PerformanceMonitor
{
    private static readonly Stopwatch _stopwatch = new();
    private static long _lastMemory;

    public static void StartMeasure(string operationName)
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        _lastMemory = GC.GetTotalMemory(false);
        _stopwatch.Restart();
        
        Debug.WriteLine($"[PERF] Starting: {operationName}");
    }

    public static PerformanceResult StopMeasure(string operationName)
    {
        _stopwatch.Stop();
        var currentMemory = GC.GetTotalMemory(false);
        var memoryDelta = currentMemory - _lastMemory;

        var result = new PerformanceResult
        {
            OperationName = operationName,
            ElapsedMilliseconds = _stopwatch.ElapsedMilliseconds,
            MemoryUsedBytes = memoryDelta,
            Timestamp = DateTime.Now
        };

        Debug.WriteLine($"[PERF] Completed: {operationName}");
        Debug.WriteLine($"[PERF]   Time: {result.ElapsedMilliseconds}ms");
        Debug.WriteLine($"[PERF]   Memory: {result.MemoryUsedKB:F2} KB");
        Debug.WriteLine($"[PERF]   Gen0: {GC.CollectionCount(0)}, Gen1: {GC.CollectionCount(1)}, Gen2: {GC.CollectionCount(2)}");

        return result;
    }

    public static string FormatBytes(long bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB" };
        double len = bytes;
        int order = 0;
        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len = len / 1024;
        }
        return $"{len:0.##} {sizes[order]}";
    }
}

public class PerformanceResult
{
    public string OperationName { get; set; } = string.Empty;
    public long ElapsedMilliseconds { get; set; }
    public long MemoryUsedBytes { get; set; }
    public double MemoryUsedKB => MemoryUsedBytes / 1024.0;
    public double MemoryUsedMB => MemoryUsedBytes / (1024.0 * 1024.0);
    public string MemoryFormatted => PerformanceMonitor.FormatBytes(MemoryUsedBytes);
    public DateTime Timestamp { get; set; }

    public override string ToString()
    {
        return $"{OperationName}: {ElapsedMilliseconds}ms, Memory: {PerformanceMonitor.FormatBytes(MemoryUsedBytes)}";
    }
}
