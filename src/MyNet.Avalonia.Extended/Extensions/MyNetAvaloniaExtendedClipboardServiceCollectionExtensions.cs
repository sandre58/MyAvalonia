// -----------------------------------------------------------------------
// <copyright file="MyNetAvaloniaExtendedClipboardServiceCollectionExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using MyNet.Avalonia.Clipboard;
using MyNet.Avalonia.Extended.Clipboard;

namespace MyNet.Avalonia.Extended;

/// <summary>
/// Registers Avalonia clipboard services.
/// </summary>
public static class MyNetAvaloniaExtendedClipboardServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="IClipboardService"/> backed by the host <see cref="TopLevel"/> clipboard.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="topLevelProvider">Resolves the host top level, typically the main window.</param>
    /// <param name="enableFeedback">When true, registers toast feedback after copy operations.</param>
    public static IServiceCollection AddMyNetAvaloniaClipboard(
        this IServiceCollection services,
        Func<TopLevel?> topLevelProvider,
        bool enableFeedback = true)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(topLevelProvider);

        if (enableFeedback)
            services.AddSingleton<IClipboardFeedback, ToastClipboardFeedback>();

        services.AddSingleton<IClipboardService>(sp =>
        {
            IClipboardFeedback? feedback = enableFeedback ? sp.GetRequiredService<IClipboardFeedback>() : null;
            return new ClipboardService(topLevelProvider, feedback);
        });

        return services;
    }
}
