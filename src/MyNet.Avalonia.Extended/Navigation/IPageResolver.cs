// -----------------------------------------------------------------------
// <copyright file="IPageResolver.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using MyNet.UI.Navigation.Models;

namespace MyNet.Avalonia.Extended.Navigation;

/// <summary>
/// Defines a contract for resolving the appropriate page instance to display based on a specified navigation request.
/// </summary>
/// <remarks>Implementations of this interface determine how navigation requests are mapped to concrete page
/// instances. This is useful in applications that require dynamic or customizable navigation logic, such as those
/// supporting modular page registration or runtime page selection strategies.</remarks>
public interface IPageResolver
{
    /// <summary>
    /// Resolve the page to display for the given navigation page.
    /// </summary>
    /// <param name="page">The navigation page to resolve.</param>
    /// <returns>The resolved page.</returns>
    Page Resolve(INavigationPage page);
}
