using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;

namespace MyNet.Avalonia.PerfTest.Services;

public static class PerformanceMonitor
{
    private static readonly Stopwatch _stopwatch = new();
    private static long _lastMemory;
    private static int _gen0, _gen1, _gen2;

    public static void StartMeasure(string operationName)
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        _gen0 = GC.CollectionCount(0);
        _gen1 = GC.CollectionCount(1);
        _gen2 = GC.CollectionCount(2);
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
            Gen0Collections = GC.CollectionCount(0) - _gen0,
            Gen1Collections = GC.CollectionCount(1) - _gen1,
            Gen2Collections = GC.CollectionCount(2) - _gen2,
            Timestamp = DateTime.Now
        };

        Debug.WriteLine($"[PERF] Completed: {operationName}");
        Debug.WriteLine($"[PERF]   Time: {result.ElapsedMilliseconds}ms");
        Debug.WriteLine($"[PERF]   Memory: {result.MemoryUsedKB:F2} KB");
        Debug.WriteLine($"[PERF]   GC: Gen0={result.Gen0Collections}, Gen1={result.Gen1Collections}, Gen2={result.Gen2Collections}");

        return result;
    }

    /// <summary>
    /// Measures the actual render time by waiting for a layout pass to complete after the action.
    /// </summary>
    public static async Task<PerformanceResult> MeasureRenderAsync(string operationName, Control target, Action action)
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        var gen0 = GC.CollectionCount(0);
        var gen1 = GC.CollectionCount(1);
        var gen2 = GC.CollectionCount(2);
        var memBefore = GC.GetTotalMemory(false);
        var sw = Stopwatch.StartNew();

        // Execute the action (e.g., navigation)
        action();

        // Wait for 2 layout passes to ensure rendering is complete
        await WaitForLayoutPassAsync(target);
        await WaitForLayoutPassAsync(target);

        sw.Stop();
        var memAfter = GC.GetTotalMemory(false);

        var result = new PerformanceResult
        {
            OperationName = operationName,
            ElapsedMilliseconds = sw.ElapsedMilliseconds,
            MemoryUsedBytes = memAfter - memBefore,
            Gen0Collections = GC.CollectionCount(0) - gen0,
            Gen1Collections = GC.CollectionCount(1) - gen1,
            Gen2Collections = GC.CollectionCount(2) - gen2,
            Timestamp = DateTime.Now
        };

        Debug.WriteLine($"[PERF] {result}");
        return result;
    }

    /// <summary>
    /// Runs a full benchmark: navigates to each page multiple times and returns all results.
    /// </summary>
    public static async Task<List<PerformanceResult>> RunFullBenchmarkAsync(
        Control target,
        IReadOnlyList<(string Name, Action Navigate)> pages,
        int iterations = 3)
    {
        var results = new List<PerformanceResult>();

        // Warmup
        foreach (var (name, navigate) in pages)
        {
            navigate();
            await WaitForLayoutPassAsync(target);
        }

        for (var i = 0; i < iterations; i++)
        {
            foreach (var (name, navigate) in pages)
            {
                var result = await MeasureRenderAsync($"[{i + 1}/{iterations}] {name}", target, navigate);
                results.Add(result);

                // Small delay to stabilize
                await Task.Delay(100);
            }
        }

        return results;
    }

    private static Task WaitForLayoutPassAsync(Control target)
    {
        var tcs = new TaskCompletionSource();
        void OnLayoutUpdated(object? sender, EventArgs e)
        {
            target.LayoutUpdated -= OnLayoutUpdated;
            tcs.TrySetResult();
        }
        target.LayoutUpdated += OnLayoutUpdated;

        // Fallback timeout in case no layout pass occurs
        Dispatcher.UIThread.Post(() =>
        {
            Task.Delay(50).ContinueWith(_ => tcs.TrySetResult());
        });

        return tcs.Task;
    }

    public static string FormatBytes(long bytes)
    {
        string[] sizes = ["B", "KB", "MB", "GB"];
        double len = bytes;
        var order = 0;
        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len /= 1024;
        }
        return $"{len:0.##} {sizes[order]}";
    }
}

public class PerformanceResult
{
    public string OperationName { get; set; } = string.Empty;
    public long ElapsedMilliseconds { get; set; }
    public long MemoryUsedBytes { get; set; }
    public int Gen0Collections { get; set; }
    public int Gen1Collections { get; set; }
    public int Gen2Collections { get; set; }
    public double MemoryUsedKB => MemoryUsedBytes / 1024.0;
    public double MemoryUsedMB => MemoryUsedBytes / (1024.0 * 1024.0);
    public string MemoryFormatted => PerformanceMonitor.FormatBytes(MemoryUsedBytes);
    public string GcInfo => $"Gen0={Gen0Collections} Gen1={Gen1Collections} Gen2={Gen2Collections}";
    public DateTime Timestamp { get; set; }

    public override string ToString()
        => $"{OperationName}: {ElapsedMilliseconds}ms, Memory: {MemoryFormatted}, GC: {GcInfo}";
}
