// -----------------------------------------------------------------------
// <copyright file="AvaloniaNavigationGestureBridge.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using MyNet.UI.Navigation;

namespace MyNet.Avalonia.Extended.Navigation;

/// <summary>
/// Maps user-initiated <see cref="NavigationPage"/> back gestures to <see cref="INavigationService.GoBackAsync"/>.
/// </summary>
public sealed class AvaloniaNavigationGestureBridge(
    INavigationService navigationService,
    IAvaloniaNavigationPageHost host) : IDisposable
{
    private NavigationPage? _navigationPage;
    private bool _isDisposed;

    /// <summary>
    /// Attaches back-gesture handling to the given navigation page.
    /// </summary>
    /// <param name="navigationPage">The navigation page host control.</param>
    public void Attach(NavigationPage navigationPage)
    {
        ArgumentNullException.ThrowIfNull(navigationPage);

        if (ReferenceEquals(_navigationPage, navigationPage))
            return;

        Detach();
        _navigationPage = navigationPage;
        _navigationPage.Popped += OnPagePopped;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (_isDisposed)
            return;

        _isDisposed = true;
        Detach();
    }

    private void Detach()
    {
        if (_navigationPage is null)
            return;

        _navigationPage.Popped -= OnPagePopped;
        _navigationPage = null;
    }

    private void OnPagePopped(object? sender, NavigationEventArgs e)
    {
        if (host.TryConsumeProgrammaticPop())
            return;

        _ = navigationService.GoBackAsync();
    }
}
