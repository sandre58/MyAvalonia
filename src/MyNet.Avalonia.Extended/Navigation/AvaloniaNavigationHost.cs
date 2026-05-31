// -----------------------------------------------------------------------
// <copyright file="AvaloniaNavigationHost.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using Avalonia.Threading;
using MyNet.UI.Navigation;
using MyNet.UI.Navigation.Models;

namespace MyNet.Avalonia.Extended.Navigation;

/// <summary>
/// Synchronizes <see cref="INavigationService"/> state with an Avalonia <see cref="NavigationPage"/>.
/// </summary>
public sealed class AvaloniaNavigationHost : IDisposable
{
    private readonly INavigationService _navigationService;
    private readonly INavigationJournal _journal;
    private readonly IAvaloniaPageFactory _pageFactory;
    private NavigationPage? _navigationPage;
    private int _programmaticPopCount;
    private bool _isDisposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="AvaloniaNavigationHost"/> class.
    /// </summary>
    /// <param name="navigationService">The application navigation service.</param>
    /// <param name="journal">The navigation journal.</param>
    /// <param name="pageFactory">The Avalonia page factory.</param>
    public AvaloniaNavigationHost(
        INavigationService navigationService,
        INavigationJournal journal,
        IAvaloniaPageFactory pageFactory)
    {
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        _journal = journal ?? throw new ArgumentNullException(nameof(journal));
        _pageFactory = pageFactory ?? throw new ArgumentNullException(nameof(pageFactory));

        _navigationService.StateChanged += OnNavigationStateChanged;
    }

    /// <summary>
    /// Attaches an Avalonia <see cref="NavigationPage"/> and rebuilds the visual stack from the journal.
    /// </summary>
    /// <param name="navigationPage">The navigation page host control.</param>
    public void Attach(NavigationPage navigationPage)
    {
        ArgumentNullException.ThrowIfNull(navigationPage);

        if (ReferenceEquals(_navigationPage, navigationPage))
            return;

        DetachNavigationPage();
        _navigationPage = navigationPage;
        _navigationPage.Popped += OnPagePopped;
        RebuildVisualStack();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (_isDisposed)
            return;

        _isDisposed = true;
        _navigationService.StateChanged -= OnNavigationStateChanged;
        DetachNavigationPage();
    }

    private void DetachNavigationPage()
    {
        if (_navigationPage is null)
            return;

        _navigationPage.Popped -= OnPagePopped;
        _navigationPage = null;
    }

    private void OnNavigationStateChanged(object? sender, NavigationStateChangedEventArgs e) => SyncVisualStack();

    private void OnPagePopped(object? sender, NavigationEventArgs e)
    {
        if (_programmaticPopCount > 0)
        {
            _programmaticPopCount--;
            return;
        }

        _ = _navigationService.GoBackAsync();
    }

    private void SyncVisualStack()
    {
        if (_navigationPage is null)
            return;

        var targetDepth = _journal.BackStack.Count + (_navigationService.CurrentContext is not null ? 1 : 0);
        Post(() => SyncVisualStackCore(targetDepth));
    }

    private void SyncVisualStackCore(int targetDepth)
    {
        if (_navigationPage is null || _isDisposed)
            return;

        while (_navigationPage.NavigationStack.Count > targetDepth)
        {
            _programmaticPopCount++;
            _navigationPage.PopAsync();
        }

        while (_navigationPage.NavigationStack.Count < targetDepth)
        {
            var page = ResolvePageAt(_navigationPage.NavigationStack.Count);

            if (page is null)
                break;

            _navigationPage.PushAsync(_pageFactory.Create(page));
        }
    }

    private void RebuildVisualStack()
    {
        if (_navigationPage is null)
            return;

        Post(() =>
        {
            if (_navigationPage is null || _isDisposed)
                return;

            _navigationPage.Popped -= OnPagePopped;

            while (_navigationPage.NavigationStack.Count > 0)
            {
                _programmaticPopCount++;
                _navigationPage.PopAsync();
            }

            _programmaticPopCount = 0;

            for (var index = 0; index < _journal.BackStack.Count; index++)
            {
                var context = _journal.BackStack[_journal.BackStack.Count - 1 - index];
                _navigationPage.PushAsync(_pageFactory.Create(context.To));
            }

            if (_navigationService.CurrentContext is { } currentContext)
                _navigationPage.PushAsync(_pageFactory.Create(currentContext.To));

            _navigationPage.Popped += OnPagePopped;
        });
    }

    private INavigationPage? ResolvePageAt(int index)
    {
        if (_navigationService.CurrentContext is null)
            return null;

        if (index < _journal.BackStack.Count)
            return _journal.BackStack[_journal.BackStack.Count - 1 - index].To;

        return _navigationService.CurrentContext.To;
    }

    private static void Post(Action action)
    {
        if (Dispatcher.UIThread.CheckAccess())
            action();
        else
            Dispatcher.UIThread.Post(action);
    }
}
