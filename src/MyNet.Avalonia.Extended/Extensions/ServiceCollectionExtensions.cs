// -----------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using MyNet.Avalonia.Extended.Clipboard;
using MyNet.Avalonia.Extended.Commands;
using MyNet.Avalonia.Extended.Dialogs;
using MyNet.Avalonia.Extended.Navigation;
using MyNet.Avalonia.Extended.Schedulers;
using MyNet.Avalonia.Extended.Services;
using MyNet.Avalonia.Extended.Theming;
using MyNet.Avalonia.Extended.Toasting;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Extended;
#pragma warning restore IDE0130

/// <summary>
/// Registers common MyNet Avalonia Extended services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers core extended services shared across Avalonia host applications.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="topLevelProvider">A function that provides the top-level window.</param>
    /// <returns>The same service collection for chaining.</returns>
    public static IServiceCollection AddMyNetAvaloniaExtended(this IServiceCollection services, Func<TopLevel?> topLevelProvider)
        => services.AddAvaloniaAppCommands()
            .AddAvaloniaCommands()
            .AddAvaloniaScheduler()
            .AddAvaloniaTheming()
            .AddAvaloniaClipboard(topLevelProvider)
            .AddAvaloniaToasting(topLevelProvider)
            .AddAvaloniaDialogs(topLevelProvider)
            .AddAvaloniaNavigation();

    /// <summary>
    /// Registers core extended services shared across Avalonia host applications, using the main window as the top-level provider.
    /// </summary>
    /// <param name="services">The service provider.</param>
    /// <returns>The same service provider for chaining.</returns>
    public static IServiceProvider UseMyNetAvaloniaExtended(this IServiceProvider services)
    {
        _ = services.GetService<AvaloniaToastHost>();
        return services.UseAvaloniaNavigation();
    }
}
