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
    /// Gets a value indicating whether the view is already on the navigation stack or pending to be shown.
    /// </summary>
    /// <param name="view">The Avalonia page to look for.</param>
    /// <returns><see langword="true"/> when the view is already known to the host.</returns>
    bool Contains(Page view);

    /// <summary>
    /// Gets the number of pages above <paramref name="view"/> on the navigation stack.
    /// </summary>
    /// <param name="view">The Avalonia page to locate.</param>
    /// <returns>
    /// The distance from the top of the stack, <c>0</c> when <paramref name="view"/> is current,
    /// or <c>-1</c> when it is not on the stack.
    /// </returns>
    int GetStackDistance(Page view);

    /// <summary>
    /// Brings an existing page to the front by popping pages above it.
    /// </summary>
    /// <param name="view">The Avalonia page to activate.</param>
    /// <param name="distance">The number of pages above <paramref name="view"/>.</param>
    void PopTo(Page view, int distance);

    /// <summary>
    /// Pops the top view from the navigation stack.
    /// </summary>
    void Pop();

    /// <summary>
    /// Skips the next <paramref name="count"/> Avalonia pops triggered by journal back navigation.
    /// </summary>
    /// <param name="count">The number of pops to suppress.</param>
    void SuppressAvaloniaBackPops(int count);

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
