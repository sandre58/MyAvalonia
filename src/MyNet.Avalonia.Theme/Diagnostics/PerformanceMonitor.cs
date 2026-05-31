// -----------------------------------------------------------------------
// <copyright file="PerformanceMonitor.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using System.Reactive.Disposables;
using System.Runtime.CompilerServices;

namespace MyNet.Avalonia.Theme.Diagnostics;

/// <summary>
/// Severity of a performance measurement log entry.
/// </summary>
public enum PerformanceTraceLevel
{
    Debug,
    Warning,
    Error
}

/// <summary>
/// Categories for conditional performance monitoring in the theme pipeline.
/// </summary>
[Flags]
public enum PerformanceCategory
{
    None = 0,
    Brushes = 1,
    Pages = 2,
    Theme = 4,
    Controls = 8,
    Utilities = 16,
    All = Brushes | Pages | Theme | Controls | Utilities
}

/// <summary>
/// Lightweight, opt-in performance tracing for theme and UI operations.
/// Disabled by default; enable categories at startup (for example in DEBUG builds).
/// </summary>
public static class PerformanceMonitor
{
    /// <summary>
    /// Gets or sets categories enabled for performance monitoring.
    /// </summary>
    public static PerformanceCategory EnabledCategories { get; set; } = PerformanceCategory.None;

    /// <summary>
    /// Gets or sets optional sink invoked after each completed measurement (in addition to <see cref="Trace"/> output).
    /// </summary>
    public static Action<PerformanceMeasurement>? MeasurementRecorded { get; set; }

    /// <summary>
    /// Returns whether the given category is currently monitored.
    /// </summary>
    public static bool IsEnabled(PerformanceCategory category)
        => category != PerformanceCategory.None && (EnabledCategories & category) != 0;

    /// <summary>
    /// Enables one or more categories. Pass nothing to enable <see cref="PerformanceCategory.All"/>.
    /// </summary>
    public static void Enable(params PerformanceCategory[] categories) => EnabledCategories = categories.Length == 0
        ? PerformanceCategory.All
        : EnabledCategories | Combine(categories);

    /// <summary>
    /// Disables one or more categories. Pass nothing to disable all monitoring.
    /// </summary>
    public static void Disable(params PerformanceCategory[] categories) => EnabledCategories = categories.Length == 0
        ? PerformanceCategory.None
        : EnabledCategories & ~Combine(categories);

#if DEBUG
    /// <summary>
    /// Enables all categories when compiled in DEBUG (no-op in Release).
    /// </summary>
    public static void EnableAllForDebugging() => EnabledCategories = PerformanceCategory.All;
#endif

    /// <summary>
    /// Measures the execution time of a code block when its category is enabled.
    /// </summary>
    /// <param name="title">Optional label; when omitted, uses caller member and file name.</param>
    /// <param name="maxBeforeWarning">Elapsed time at or above which the entry is logged as a warning.</param>
    /// <param name="maxBeforeError">Elapsed time at or above which the entry is logged as an error.</param>
    /// <param name="category">Category filter for this measurement.</param>
    /// <param name="memberName">Caller member (filled automatically).</param>
    /// <param name="filePath">Caller file path (filled automatically).</param>
    public static IDisposable Measure(
        string title = "",
        TimeSpan? maxBeforeWarning = null,
        TimeSpan? maxBeforeError = null,
        PerformanceCategory category = PerformanceCategory.All,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filePath = "")
    {
        if (!IsEnabled(category))
            return Disposable.Empty;

        var label = string.IsNullOrEmpty(title)
            ? $"{Path.GetFileNameWithoutExtension(filePath)}.{memberName}"
            : title;

        return new MeasureScope(label, maxBeforeWarning, maxBeforeError, category);
    }

    /// <summary>
    /// Logs a debug message when performance logging is enabled for the category.
    /// </summary>
    public static void Debug(string message, PerformanceCategory category = PerformanceCategory.All)
        => Write(PerformanceTraceLevel.Debug, message, category);

    /// <summary>
    /// Logs a warning message when performance logging is enabled for the category.
    /// </summary>
    public static void Warning(string message, PerformanceCategory category = PerformanceCategory.All)
        => Write(PerformanceTraceLevel.Warning, message, category);

    /// <summary>
    /// Logs an error message when performance logging is enabled for the category.
    /// </summary>
    public static void Error(string message, PerformanceCategory category = PerformanceCategory.All)
        => Write(PerformanceTraceLevel.Error, message, category);

    private static PerformanceTraceLevel ResolveLevel(TimeSpan elapsed, TimeSpan? maxBeforeWarning, TimeSpan? maxBeforeError) => maxBeforeError.HasValue && elapsed >= maxBeforeError.Value
        ? PerformanceTraceLevel.Error
        : maxBeforeWarning.HasValue && elapsed >= maxBeforeWarning.Value ? PerformanceTraceLevel.Warning : PerformanceTraceLevel.Debug;

    private static void Write(PerformanceTraceLevel level, string message, PerformanceCategory category)
    {
        if (!IsEnabled(category))
            return;

        Trace.WriteLine(FormatLine(level, message));
    }

    private static void Record(PerformanceMeasurement measurement)
    {
        Write(measurement.Level, FormatMeasurement(measurement), measurement.Category);
        MeasurementRecorded?.Invoke(measurement);
    }

    private static string FormatMeasurement(PerformanceMeasurement measurement)
        => $"{measurement.Title}: {FormatDuration(measurement.Elapsed)}";

    private static string FormatLine(PerformanceTraceLevel level, string message)
        => level switch
        {
            PerformanceTraceLevel.Warning => $"[PERF:WARN] {message}",
            PerformanceTraceLevel.Error => $"[PERF:ERROR] {message}",
            _ => $"[PERF] {message}"
        };

    private static string FormatDuration(TimeSpan elapsed)
        => elapsed.TotalMilliseconds >= 1000
            ? $"{elapsed.TotalSeconds:F2} s"
            : $"{elapsed.TotalMilliseconds:F1} ms";

    private static PerformanceCategory Combine(ReadOnlySpan<PerformanceCategory> categories)
    {
        var combined = PerformanceCategory.None;
        foreach (var category in categories)
            combined |= category;

        return combined;
    }

    private sealed class MeasureScope(string title, TimeSpan? maxBeforeWarning, TimeSpan? maxBeforeError, PerformanceCategory category)
        : IDisposable
    {
        private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

        public void Dispose()
        {
            _stopwatch.Stop();
            var elapsed = _stopwatch.Elapsed;
            var level = ResolveLevel(elapsed, maxBeforeWarning, maxBeforeError);
            Record(new(title, elapsed, level, category));
        }
    }
}

/// <summary>
/// Result of a completed <see cref="PerformanceMonitor.Measure"/> scope.
/// </summary>
/// <param name="Title">Operation label.</param>
/// <param name="Elapsed">Elapsed time.</param>
/// <param name="Level">Trace level after threshold evaluation.</param>
/// <param name="Category">Category filter used for the measurement.</param>
public readonly record struct PerformanceMeasurement(
    string Title,
    TimeSpan Elapsed,
    PerformanceTraceLevel Level,
    PerformanceCategory Category);
