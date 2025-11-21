// -----------------------------------------------------------------------
// <copyright file="PerformanceMonitor.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Reactive.Disposables;
using MyNet.Utilities;
using MyNet.Utilities.Logging;

namespace MyNet.Avalonia.Demo.Helpers;

/// <summary>
/// Utility class for monitoring performance in the application.
/// Writes performance metrics to Debug output.
/// </summary>
public static class PerformanceMonitor
{
    private const bool IsEnabled =
#if DEBUG
        true;
#else
        false;
#endif

    private static PerformanceLoggerSettings CreateLoggerSettings() => new(false, false, x => x.TotalMilliseconds switch
    {
        < 50 => PerformanceTraceLevel.Debug,
        < 200 => PerformanceTraceLevel.Warning,
        _ => PerformanceTraceLevel.Error
    });

    /// <summary>
    /// Creates a disposable performance timer that measures until disposed.
    /// </summary>
    /// <param name="actionName">Name of the action being measured.</param>
    /// <returns>Disposable timer.</returns>
    public static IDisposable Measure(string actionName) => IsEnabled ? LogManager.MeasureTime(CreateLoggerSettings(), $"[PERF] {actionName}") : Disposable.Empty;
}
