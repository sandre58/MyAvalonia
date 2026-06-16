// -----------------------------------------------------------------------
// <copyright file="SingletonNavigationService.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;
using MyNet.UI.Navigation;
using MyNet.UI.Navigation.Models;

namespace MyNet.Avalonia.Extended.Navigation;

/// <summary>
/// Prevents duplicate navigation entries by reactivating existing pages in the Avalonia stack.
/// </summary>
public sealed class SingletonNavigationService(
    NavigationService inner,
    IAvaloniaNavigationPageHost host,
    IAvaloniaPageFactory pageFactory) : INavigationService
{
    /// <inheritdoc />
    public event EventHandler<NavigationStateChangedEventArgs>? StateChanged
    {
        add => inner.StateChanged += value;
        remove => inner.StateChanged -= value;
    }

    /// <inheritdoc />
    public bool CanGoBack => inner.CanGoBack;

    /// <inheritdoc />
    public bool CanGoForward => inner.CanGoForward;

    /// <inheritdoc />
    public NavigationContext? CurrentContext => inner.CurrentContext;

    /// <inheritdoc />
    public async Task<NavigationResult> NavigateToAsync(
        INavigationPage page,
        INavigationParameters? parameters = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(page);

        if (ReferenceEquals(CurrentContext?.To, page))
            return new(NavigationStatus.Succeeded);

        var view = pageFactory.Create(page);

        if (!host.Contains(view))
            return await inner.NavigateToAsync(page, parameters, cancellationToken).ConfigureAwait(false);

        var distance = host.GetStackDistance(view);
        if (distance <= 0)
            return new(NavigationStatus.Succeeded);

        host.PopTo(view, distance);
        host.SuppressAvaloniaBackPops(distance);

        NavigationResult? lastResult = null;
        for (var i = 0; i < distance; i++)
        {
            lastResult = await inner.GoBackAsync(cancellationToken).ConfigureAwait(false);
            if (lastResult.Status is not NavigationStatus.Succeeded)
                return lastResult;
        }

        return lastResult ?? new(NavigationStatus.Succeeded);
    }

    /// <inheritdoc />
    public Task<NavigationResult> GoBackAsync(CancellationToken cancellationToken = default)
        => inner.GoBackAsync(cancellationToken);

    /// <inheritdoc />
    public Task<NavigationResult> GoForwardAsync(CancellationToken cancellationToken = default)
        => inner.GoForwardAsync(cancellationToken);

    /// <inheritdoc />
    public Task ResetAsync() => inner.ResetAsync();
}
