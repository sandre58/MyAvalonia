// -----------------------------------------------------------------------
// <copyright file="AvaloniaNavigationContext.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Microsoft.Extensions.DependencyInjection;
using MyNet.UI.Commands;
using MyNet.UI.Locators;
using MyNet.UI.Locators.Conventions;

namespace MyNet.Avalonia.Extended.Navigation;

/// <summary>
/// Shared navigation dependencies for Avalonia attached properties.
/// </summary>
internal static class AvaloniaNavigationContext
{
    private static IViewModelLocator? _viewModelLocator;
    private static ITypeResolver? _typeResolver;
    private static AvaloniaNavigationHost? _navigationHost;

    /// <summary>
    /// Gets the view model locator.
    /// </summary>
    internal static IViewModelLocator ViewModelLocator =>
        _viewModelLocator ?? throw new InvalidOperationException("Call Configure before using navigation assists.");

    /// <summary>
    /// Gets the type resolver.
    /// </summary>
    internal static ITypeResolver TypeResolver =>
        _typeResolver ?? throw new InvalidOperationException("Call Configure before using navigation assists.");

    /// <summary>
    /// Gets the command factory used by navigation assists.
    /// </summary>
    internal static ICommandFactory CommandFactory { get; private set; } = RelayCommandFactory.Default;

    /// <summary>
    /// Gets the Avalonia navigation host.
    /// </summary>
    internal static AvaloniaNavigationHost NavigationHost =>
        _navigationHost ?? throw new InvalidOperationException("Call Configure before using navigation assists.");

    /// <summary>
    /// Configures shared dependencies from the service provider.
    /// </summary>
    /// <param name="services">The application service provider.</param>
    internal static void Configure(IServiceProvider services)
    {
        ArgumentNullException.ThrowIfNull(services);

        _viewModelLocator = services.GetRequiredService<IViewModelLocator>();
        _typeResolver = services.GetRequiredService<ITypeResolver>();
        CommandFactory = services.GetService<ICommandFactory>() ?? RelayCommandFactory.Default;
        _navigationHost = services.GetRequiredService<AvaloniaNavigationHost>();
    }
}
