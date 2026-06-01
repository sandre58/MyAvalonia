// -----------------------------------------------------------------------
// <copyright file="AvaloniaNavigationResetBridge.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.UI.Navigation;

namespace MyNet.Avalonia.Extended.Navigation;

/// <summary>
/// Clears the Avalonia navigation page stack when the navigation service is reset.
/// </summary>
public sealed class AvaloniaNavigationResetBridge
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AvaloniaNavigationResetBridge"/> class.
    /// </summary>
    /// <param name="navigationService">The application navigation service.</param>
    /// <param name="host">The Avalonia navigation page host.</param>
    public AvaloniaNavigationResetBridge(
        INavigationService navigationService,
        IAvaloniaNavigationPageHost host)
        => navigationService.StateChanged += (_, e) => OnNavigationStateChanged(e, host);

    private static void OnNavigationStateChanged(NavigationStateChangedEventArgs e, IAvaloniaNavigationPageHost host)
    {
        if (e.CurrentContext is null)
            host.Clear();
    }
}
