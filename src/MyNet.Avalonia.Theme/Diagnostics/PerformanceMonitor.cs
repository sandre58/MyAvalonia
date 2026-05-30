// -----------------------------------------------------------------------
// <copyright file="PerformanceMonitor.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Disposables;
using MyNet.Utilities;
using MyNet.Utilities.Logging;

namespace MyNet.Avalonia.Theme.Diagnostics;

/// <summary>
/// Conditional performance logging for theme and UI operations.
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

public static class PerformanceMonitor
{
    /// <summary>
    /// Gets or sets categories enabled for performance monitoring.
    /// </summary>
    public static PerformanceCategory EnabledCategories { get; set; } =
#if DEBUG
        PerformanceCategory.None;
#else
        PerformanceCategory.None;
#endif

    /// <summary>
    /// Measures the execution time of a code block when its category is enabled.
    /// </summary>
    public static IDisposable Measure(string title = "", TimeSpan? maxBeforeWarning = null, TimeSpan? maxBeforeError = null, PerformanceCategory category = PerformanceCategory.All)
    {
        if ((EnabledCategories & category) == 0)
            return Disposable.Empty;

        if (string.IsNullOrEmpty(title))
        {
            var st = new StackTrace(new StackFrame(1));
            var method = st.GetFrame(0)?.GetMethod();

            if (method != null)
            {
                title = $"{method.DeclaringType}.{method.Name}({string.Join(", ", method.GetParameters().Select(x => x.Name))})";
            }
        }

        return LogManager.MeasureTime(new PerformanceLoggerSettings(false, false, x => maxBeforeWarning.HasValue && x >= maxBeforeWarning.Value
                ? PerformanceTraceLevel.Warning
                : maxBeforeError.HasValue && x >= maxBeforeError.Value ? PerformanceTraceLevel.Error : PerformanceTraceLevel.Debug),
            title);
    }

    /// <summary>
    /// Logs a debug message when performance logging is enabled.
    /// </summary>
    public static void Debug(string message, PerformanceCategory category = PerformanceCategory.All)
        => ((EnabledCategories & category) != 0).IfTrue(() =>
        {
            LogManager.Debug(message);
            System.Diagnostics.Debug.WriteLine(message);
        });

    /// <summary>
    /// Logs a warning message when performance logging is enabled.
    /// </summary>
    public static void Warning(string message, PerformanceCategory category = PerformanceCategory.All)
        => ((EnabledCategories & category) != 0).IfTrue(() =>
        {
            LogManager.Warning(message);
            System.Diagnostics.Debug.WriteLine(message);
        });
}
