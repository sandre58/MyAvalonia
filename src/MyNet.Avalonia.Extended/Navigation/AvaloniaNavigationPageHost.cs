// -----------------------------------------------------------------------
// <copyright file="AvaloniaNavigationPageHost.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Threading;

namespace MyNet.Avalonia.Extended.Navigation;

/// <summary>
/// Synchronizes navigation middleware operations with an Avalonia <see cref="NavigationPage"/> stack.
/// </summary>
public sealed class AvaloniaNavigationPageHost : IAvaloniaNavigationPageHost
{
    private readonly List<Page> _pendingPages = [];
    private NavigationPage? _navigationPage;
    private int _programmaticPopCount;

    /// <inheritdoc />
    public bool IsAttached => _navigationPage is not null;

    /// <inheritdoc />
    public void Attach(NavigationPage navigationPage)
    {
        ArgumentNullException.ThrowIfNull(navigationPage);

        if (ReferenceEquals(_navigationPage, navigationPage))
            return;

        _navigationPage = navigationPage;
        FlushPending();
    }

    /// <inheritdoc />
    public void Push(Page view)
    {
        ArgumentNullException.ThrowIfNull(view);

        if (_navigationPage is null)
        {
            _pendingPages.Add(view);
            return;
        }

        Post(() => _navigationPage.PushAsync(view));
    }

    /// <inheritdoc />
    public void Pop()
    {
        if (_navigationPage is null)
        {
            if (_pendingPages.Count > 0)
                _pendingPages.RemoveAt(_pendingPages.Count - 1);

            return;
        }

        Post(() =>
        {
            _programmaticPopCount++;
            _navigationPage.PopAsync();
        });
    }

    /// <inheritdoc />
    public void Clear()
    {
        _pendingPages.Clear();

        if (_navigationPage is null)
            return;

        Post(() =>
        {
            while (_navigationPage.NavigationStack.Count > 0)
            {
                _programmaticPopCount++;
                _navigationPage.PopAsync();
            }
        });
    }

    /// <inheritdoc />
    public bool TryConsumeProgrammaticPop()
    {
        if (_programmaticPopCount <= 0)
            return false;

        _programmaticPopCount--;
        return true;
    }

    private void FlushPending()
    {
        if (_pendingPages.Count == 0)
            return;

        Post(() =>
        {
            foreach (var page in _pendingPages)
                _navigationPage!.PushAsync(page);

            _pendingPages.Clear();
        });
    }

    private static void Post(Action action)
    {
        if (Dispatcher.UIThread.CheckAccess())
            action();
        else
            Dispatcher.UIThread.Post(action);
    }
}
