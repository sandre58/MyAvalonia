// -----------------------------------------------------------------------
// <copyright file="LoggingBootstrap.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.IO;
using NLog;

namespace MyNet.Avalonia.Showcase.Composition.Logging;

/// <summary>
/// Loads and shuts down NLog for desktop hosts.
/// </summary>
public static class LoggingBootstrap
{
    private static bool _configured;

    /// <summary>
    /// Gets the logging mode used by <see cref="AppComposition"/>.
    /// Defaults to <see cref="LoggingMode.Debug"/> for portable hosts and design-time.
    /// </summary>
    public static LoggingMode LoggingMode { get; private set; } = LoggingMode.Debug;

    /// <summary>
    /// Loads NLog configuration from the host <c>config/NLog.config</c> when present.
    /// </summary>
    /// <param name="configFilePath">Optional absolute path to NLog.config.</param>
    public static void ConfigureNLog(string? configFilePath = null)
    {
        if (_configured)
            return;

        configFilePath ??= Path.Combine(AppContext.BaseDirectory, "config", "NLog.config");
        if (File.Exists(configFilePath))
            LogManager.Setup().LoadConfigurationFromFile(configFilePath);

        _configured = true;
    }

    /// <summary>Configures NLog file/console logging for the desktop host.</summary>
    public static void ConfigureForDesktop(string? nlogConfigPath = null)
    {
        LoggingMode = LoggingMode.NLog;
        ConfigureNLog(nlogConfigPath);
    }

    /// <summary>Configures debug-only logging for browser, mobile, and other portable hosts.</summary>
    public static void ConfigureForPortableHost() => LoggingMode = LoggingMode.Debug;

    /// <summary>Flushes pending log entries and releases NLog resources.</summary>
    public static void Shutdown()
    {
        if (!_configured)
            return;

        LogManager.Shutdown();
        _configured = false;
    }
}
