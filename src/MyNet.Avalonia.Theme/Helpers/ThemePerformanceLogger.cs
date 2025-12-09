// -----------------------------------------------------------------------
// <copyright file="ThemePerformanceLogger.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Disposables;
using MyNet.Utilities.Logging;

namespace MyNet.Avalonia.Theme.Helpers;

internal static class ThemePerformanceLogger
{
    public static bool EnablePerformanceLogs { get; set; }

    public static IDisposable MeasureTime(string title = "", TimeSpan? maxBeforeWarning = null, TimeSpan? maxBeforeError = null)
    {
        if (!EnablePerformanceLogs)
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
}
