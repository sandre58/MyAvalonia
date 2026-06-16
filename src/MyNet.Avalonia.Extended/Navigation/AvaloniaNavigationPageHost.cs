// -----------------------------------------------------------------------
// <copyright file="AvaloniaNavigationPageHost.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
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
    private int _skipAvaloniaBackPopCount;

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
            if (!_pendingPages.Contains(view))
                _pendingPages.Add(view);

            return;
        }

        Post(() => _navigationPage.PushAsync(view));
    }

    /// <inheritdoc />
    public bool Contains(Page view)
    {
        ArgumentNullException.ThrowIfNull(view);

        if (_pendingPages.Any(page => ReferenceEquals(page, view)))
            return true;

        return _navigationPage?.NavigationStack.Any(page => ReferenceEquals(page, view)) ?? false;
    }

    /// <inheritdoc />
    public int GetStackDistance(Page view)
    {
        ArgumentNullException.ThrowIfNull(view);

        if (_navigationPage is null)
            return _pendingPages.Any(page => ReferenceEquals(page, view)) ? 0 : -1;

        var stack = _navigationPage.NavigationStack;
        for (var i = stack.Count - 1; i >= 0; i--)
        {
            if (!ReferenceEquals(stack[i], view))
                continue;

            return stack.Count - 1 - i;
        }

        return -1;
    }

    /// <inheritdoc />
    public void PopTo(Page view, int distance)
    {
        ArgumentNullException.ThrowIfNull(view);

        if (distance <= 0 || _navigationPage is null)
            return;

        Post(() =>
        {
            _programmaticPopCount += distance;
            _ = _navigationPage.PopToPageAsync(view);
        });
    }

    /// <inheritdoc />
    public void SuppressAvaloniaBackPops(int count)
    {
        if (count > 0)
            _skipAvaloniaBackPopCount += count;
    }

    /// <inheritdoc />
    public void Pop()
    {
        if (_skipAvaloniaBackPopCount > 0)
        {
            _skipAvaloniaBackPopCount--;
            return;
        }

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
