// -----------------------------------------------------------------------
// <copyright file="NavigationCommands.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Windows.Input;
using MyNet.UI.Commands;
using MyNet.UI.Navigation;
using MyNet.UI.Navigation.Models;

namespace MyNet.Avalonia.Extended.Commands;

/// <summary>
/// Creates navigation commands bound to an <see cref="INavigationService"/> instance.
/// </summary>
/// <param name="navigationService">The navigation service to bind commands to.</param>
/// <param name="commandFactory">The command factory used to create command instances.</param>
public sealed class NavigationCommands(INavigationService navigationService, ICommandFactory commandFactory)
{
    /// <summary>
    /// Gets the command that navigates back in the navigation journal.
    /// </summary>
    public ICommand GoBackCommand { get; } = commandFactory.Create(
        () => navigationService.GoBackAsync(),
        () => navigationService.CanGoBack);

    /// <summary>
    /// Gets the command that navigates forward in the navigation journal.
    /// </summary>
    public ICommand GoForwardCommand { get; } = commandFactory.Create(
        () => navigationService.GoForwardAsync(),
        () => navigationService.CanGoForward);

    /// <summary>
    /// Gets the command that navigates to a non-null <see cref="INavigationPage"/>.
    /// </summary>
    public ICommand NavigateCommand { get; } = commandFactory.CreateRequired<INavigationPage>(
        page => navigationService.NavigateToAsync(page));

    /// <summary>
    /// Subscribes to navigation state changes and refreshes command availability.
    /// </summary>
    public void SubscribeToNavigationStateChanges() => navigationService.StateChanged += OnNavigationStateChanged;

    /// <summary>
    /// Unsubscribes from navigation state changes.
    /// </summary>
    public void UnsubscribeFromNavigationStateChanges() => navigationService.StateChanged -= OnNavigationStateChanged;

    private void OnNavigationStateChanged(object? sender, NavigationStateChangedEventArgs e) => RaiseCanExecuteChanged();

    private void RaiseCanExecuteChanged()
    {
        (GoBackCommand as IRaiseCanExecuteChanged)?.RaiseCanExecuteChanged();
        (GoForwardCommand as IRaiseCanExecuteChanged)?.RaiseCanExecuteChanged();
    }
}
