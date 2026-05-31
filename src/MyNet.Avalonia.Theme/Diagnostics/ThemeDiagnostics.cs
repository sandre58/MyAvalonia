// -----------------------------------------------------------------------
// <copyright file="ThemeDiagnostics.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace MyNet.Avalonia.Theme.Diagnostics;

/// <summary>
/// Centralized switches for theme performance diagnostics (Showcase, tests, environment).
/// </summary>
public static class ThemeDiagnostics
{
    /// <summary>
    /// Environment variable name. Set to <c>1</c> or <c>true</c> to enable theme performance tracing at startup.
    /// </summary>
    public const string PerformanceEnvironmentVariable = "MYNET_THEME_PERF";

    /// <summary>
    /// Gets a value indicating whether performance diagnostics are enabled from the environment variable.
    /// </summary>
    public static bool IsEnvironmentEnabled
    {
        get
        {
            var value = Environment.GetEnvironmentVariable(PerformanceEnvironmentVariable);
            return value is "1" or "true" or "TRUE";
        }
    }

    /// <summary>
    /// Applies <see cref="PerformanceMonitor"/> categories from the environment when set.
    /// </summary>
    public static void ConfigureFromEnvironment()
    {
        if (IsEnvironmentEnabled)
            EnableDefaultCategories();
    }

    /// <summary>
    /// Enables or disables the default Showcase diagnostic categories.
    /// </summary>
    public static void ApplyShowcaseSettings(bool enabled)
    {
        if (enabled)
            EnableDefaultCategories();
        else
            PerformanceMonitor.Disable();
    }

    /// <summary>
    /// Enables the default diagnostic categories used by Showcase and environment configuration.
    /// </summary>
    public static void EnableDefaultCategories()
        => PerformanceMonitor.Enable(PerformanceCategory.Theme, PerformanceCategory.Brushes);
}
