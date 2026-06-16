// -----------------------------------------------------------------------
// <copyright file="IAvaloniaPageFactory.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using MyNet.UI.Navigation.Models;

namespace MyNet.Avalonia.Extended.Navigation;

/// <summary>
/// Creates Avalonia <see cref="Page"/> instances for navigation pages.
/// </summary>
public interface IAvaloniaPageFactory
{
    /// <summary>
    /// Creates the Avalonia page to display for the given navigation page.
    /// </summary>
    /// <param name="page">The navigation page.</param>
    /// <returns>The Avalonia page bound to <paramref name="page"/>.</returns>
    Page Create(INavigationPage page);

    /// <summary>
    /// Clears cached page instances.
    /// </summary>
    void Clear();
}
