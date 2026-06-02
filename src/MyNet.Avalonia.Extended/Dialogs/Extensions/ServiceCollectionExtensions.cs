// -----------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MyNet.Avalonia.Extended.Dialogs.Internal;
using MyNet.Avalonia.Extended.Dialogs.Presentation;
using MyNet.UI.Dialogs;
using MyNet.UI.Locators;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Extended.Dialogs;
#pragma warning restore IDE0130

/// <summary>
/// Registers Avalonia dialog presenters for <see cref="MyNet.UI.Dialogs.ContentDialogs.IContentDialogService"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers overlay and window <see cref="MyNet.UI.Dialogs.ContentDialogs.IDialogPresenter"/> implementations.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="topLevelProvider">Resolves the host top level, typically the main window.</param>
    /// <returns>The same service collection for chaining.</returns>
    /// <remarks>
    /// Requires <see cref="ServiceCollectionExtensions.AddDialogs"/> and
    /// <see cref="MyNet.UI.Locators.ServiceCollectionExtensions.AddViewLocators"/> to be registered first.
    /// Use <see cref="DialogOptions.ForOverlay"/> or <see cref="DialogOptions.ForWindow"/>
    /// to select the presentation surface.
    /// See <c>Dialogs/README.md</c> in this package for host setup and examples.
    /// </remarks>
    public static IServiceCollection AddAvaloniaDialogs(
        this IServiceCollection services,
        Func<TopLevel?> topLevelProvider)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(topLevelProvider);

        services.AddViewLocators();
        services.TryAddSingleton(new DialogHostOptions(topLevelProvider));
        services.TryAddSingleton<DialogSessionRegistry>();
        services.AddDialogs(builder =>
        {
            builder.AddPresenter<OverlayDialogPresenter>();
            builder.AddPresenter<WindowDialogPresenter>();
        });

        return services;
    }
}
