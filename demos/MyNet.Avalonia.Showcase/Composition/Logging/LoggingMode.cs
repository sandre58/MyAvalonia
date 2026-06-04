// -----------------------------------------------------------------------
// <copyright file="LoggingMode.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.Avalonia.Showcase.Composition.Logging;

/// <summary>
/// Logging backend selected by the Avalonia host (desktop file logging vs portable debug output).
/// </summary>
public enum LoggingMode
{
    /// <summary>Microsoft.Extensions.Logging debug provider (browser, mobile, design-time).</summary>
    Debug,

    /// <summary>NLog with <c>NLog.config</c> (desktop).</summary>
    NLog
}
