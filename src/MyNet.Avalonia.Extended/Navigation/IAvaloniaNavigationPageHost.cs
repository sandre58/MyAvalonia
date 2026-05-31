// -----------------------------------------------------------------------
// <copyright file="IAvaloniaNavigationPageHost.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;

namespace MyNet.Avalonia.Extended.Navigation;

/// <summary>
/// Hosts navigation views inside an Avalonia <see cref="NavigationPage"/> stack.
/// </summary>
public interface IAvaloniaNavigationPageHost
{
    /// <summary>
    /// Gets a value indicating whether a <see cref="NavigationPage"/> is attached.
    /// </summary>
    bool IsAttached { get; }

    /// <summary>
    /// Attaches the Avalonia navigation page control.
    /// </summary>
    /// <param name="navigationPage">The navigation page host control.</param>
    void Attach(NavigationPage navigationPage);

    /// <summary>
    /// Pushes a view onto the navigation stack.
    /// </summary>
    /// <param name="view">The Avalonia page to display.</param>
    void Push(Page view);

    /// <summary>
    /// Pops the top view from the navigation stack.
    /// </summary>
    void Pop();

    /// <summary>
    /// Clears the navigation stack and any pending views.
    /// </summary>
    void Clear();

    /// <summary>
    /// Consumes one programmatic pop notification so gesture back does not trigger service navigation.
    /// </summary>
    /// <returns><see langword="true"/> when the pop was initiated by the host.</returns>
    bool TryConsumeProgrammaticPop();
}
