// -----------------------------------------------------------------------
// <copyright file="LoggingExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyNet.Avalonia.Showcase.Composition.Logging;
using MyNet.Utilities.Logging;
using NLog.Extensions.Logging;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Showcase.Composition;
#pragma warning restore IDE0130

/// <summary>
/// Registers Microsoft.Extensions.Logging for the showcase and wires MyNet static logging.
/// </summary>
internal static class LoggingExtensions
{
    /// <summary>
    /// Adds showcase logging services according to <see cref="ShowcaseApp.LoggingMode"/>.
    /// </summary>
    public static IServiceCollection AddShowcaseLogging(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddLogging(logging =>
        {
            logging.ClearProviders();
#if DEBUG
            logging.SetMinimumLevel(LogLevel.Debug);
#else
            logging.SetMinimumLevel(LogLevel.Information);
#endif
            logging.AddFilter("Microsoft", LogLevel.Warning);
            logging.AddFilter("System", LogLevel.Warning);

            switch (LoggingBootstrap.LoggingMode)
            {
                case LoggingMode.NLog:
                    logging.AddNLog();
                    break;
                case LoggingMode.Debug:
                    logging.AddDebug();
                    break;
                default:
                    throw new InvalidOperationException($"Unsupported logging mode: {LoggingBootstrap.LoggingMode}");
            }
        });

        return services;
    }

    /// <summary>
    /// Connects <see cref="Log.Factory"/> to the built DI container.
    /// </summary>
    public static IServiceProvider UseShowcaseLogging(this IServiceProvider services)
    {
        ArgumentNullException.ThrowIfNull(services);
        Log.Factory = services.GetRequiredService<ILoggerFactory>();
        return services;
    }
}
