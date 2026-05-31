// -----------------------------------------------------------------------
// <copyright file="ThemeChangeCoordinator.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using MyNet.Utilities.Deferring;

namespace MyNet.Avalonia.Theme.Runtime;

/// <summary>
/// Coalesces theme change notifications and increments <see cref="MyTheme.ThemeVersion"/> when updates are published.
/// </summary>
internal sealed class ThemeChangeCoordinator
{
    private readonly object _sender;
    private readonly Action _incrementThemeVersion;
    private readonly DeferredAction _deferrer;

    public ThemeChangeCoordinator(object sender, Action incrementThemeVersion)
    {
        _sender = sender;
        _incrementThemeVersion = incrementThemeVersion;
        _deferrer = new(Publish);
    }

    public event EventHandler? ThemeChanged;

    public bool IsDeferred => _deferrer.IsDeferred;

    public IDisposable Defer() => _deferrer.Defer();

    public void NotifyChange() => Publish();

    private void Publish()
    {
        if (_deferrer.IsDeferred)
            return;

        _incrementThemeVersion();
        ThemeChanged?.Invoke(_sender, EventArgs.Empty);
    }
}
