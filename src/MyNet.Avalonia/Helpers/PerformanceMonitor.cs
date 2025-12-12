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

#pragma warning disable CS0162 // Unreachable code detected
namespace MyNet.Avalonia.Helpers;

/// <summary>
/// Provides utilities for logging and measuring performance in theme-related operations.
/// Allows conditional logging and timing of code blocks to help diagnose performance issues in theming logic.
/// </summary>
public static class PerformanceMonitor
{
    /// <summary>
    /// Gets or sets a value indicating whether performance logs are enabled.
    /// When false, all logging and timing operations are disabled.
    /// </summary>
    private const bool IsEnabled =
#if DEBUG
        false;
#else
        false;
#endif

    /// <summary>
    /// Measures the execution time of a code block and logs the result if performance logging is enabled.
    /// Returns a disposable that should be used in a <c>using</c> statement to delimit the measured scope.
    /// </summary>
    /// <param name="title">A descriptive title for the log entry. If empty, the caller's method name is used.</param>
    /// <param name="maxBeforeWarning">Optional threshold for warning-level logs.</param>
    /// <param name="maxBeforeError">Optional threshold for error-level logs.</param>
    /// <returns>A disposable that ends the timing when disposed.</returns>
    public static IDisposable Measure(string title = "", TimeSpan? maxBeforeWarning = null, TimeSpan? maxBeforeError = null)
    {
        if (!IsEnabled)
        {
            return Disposable.Empty;
        }

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
    /// Logs a debug message if performance logging is enabled.
    /// </summary>
    /// <param name="message">The message to log.</param>
    public static void Debug(string message) => IsEnabled.IfTrue(() => LogManager.Debug(message));
}
#pragma warning restore CS0162 // Unreachable code detected
