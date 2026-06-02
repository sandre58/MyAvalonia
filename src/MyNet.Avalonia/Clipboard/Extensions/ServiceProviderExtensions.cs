// -----------------------------------------------------------------------
// <copyright file="ServiceProviderExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Microsoft.Extensions.DependencyInjection;
using MyNet.Avalonia.Bindings;
using MyNet.Avalonia.Clipboard;
using MyNet.Globalization;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// Connects the DI clipboard service to the static XAML facade.
/// </summary>
public static class ServiceProviderExtensions
{
    /// <summary>
    /// Initializes globalization facades and reconnects Avalonia culture bindings to the configured service.
    /// </summary>
    public static IServiceProvider UseAvaloniaGlobalization(this IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);

        serviceProvider.UseGlobalization();
        GlobalizationBindingSource.ReconnectToCurrentService();

        return serviceProvider;
    }

    /// <summary>
    /// Initializes <see cref="ClipboardManager"/> from the built service provider.
    /// </summary>
    /// <remarks>
    /// The host must register <see cref="IClipboardService"/> first, for example via
    /// <c>AddMyNetAvaloniaClipboard()</c>.
    /// </remarks>
    public static IServiceProvider UseAvaloniaClipboard(this IServiceProvider services)
    {
        ClipboardManager.Configure(services.GetRequiredService<IClipboardService>());
        return services;
    }
}
