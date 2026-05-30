// -----------------------------------------------------------------------
// <copyright file="PageResolver.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using MyNet.UI.Extensions;
using MyNet.UI.Locators;
using MyNet.UI.Navigation.Models;

namespace MyNet.Avalonia.Extended.Navigation;

/// <summary>
/// Provides functionality to resolve navigation page types to their corresponding Avalonia page types for navigation
/// purposes.
/// </summary>
/// <remarks>This class enables custom navigation scenarios by allowing the registration of navigation page types
/// with specific Avalonia pages, facilitating the correct instantiation of pages during navigation.</remarks>
/// <param name="viewResolver">The view resolver used to map navigation page types to Avalonia page types.</param>
public class PageResolver(IViewResolver viewResolver) : IPageResolver
{
    /// <summary>
    /// Registers a mapping between a navigation page type and its corresponding Avalonia page type for use in
    /// navigation resolution.
    /// </summary>
    /// <remarks>Use this method to enable custom navigation scenarios by associating a navigation page type
    /// with a specific Avalonia page. This allows the navigation framework to resolve and instantiate the correct
    /// Avalonia page when navigating to the registered navigation page type.</remarks>
    /// <typeparam name="TPage">The type of the navigation page to register. Must implement the INavigationPage interface.</typeparam>
    /// <typeparam name="TAvaloniaPage">The Avalonia page type to associate with the specified navigation page. Must derive from Page.</typeparam>
    public void Register<TPage, TAvaloniaPage>()
        where TPage : INavigationPage
        where TAvaloniaPage : Page => viewResolver.Register<TPage, TAvaloniaPage>();

    /// <summary>
    /// Register a mapping between a navigation page type and an Avalonia page type. This method allows you to specify
    /// the relationship between the two types for navigation purposes.
    /// </summary>
    /// <param name="pageType">The type of the navigation page.</param>
    /// <param name="avaloniaPageType">The type of the Avalonia page.</param>
    public void Register(Type pageType, Type avaloniaPageType) => viewResolver.Register(pageType, avaloniaPageType);

    /// <summary>
    /// Resolve the page to display for the given navigation page. This method uses the view resolver to find the
    /// corresponding Avalonia page type and creates an instance of it.
    /// </summary>
    /// <param name="page">The navigation page to resolve.</param>
    /// <returns>The resolved page.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the page cannot be resolved.</exception>
    public Page Resolve(INavigationPage page)
    {
        var avaloniaPageType = viewResolver.Resolve(page.GetType());

        var view = Activator.CreateInstance(avaloniaPageType)
            ?? throw new InvalidOperationException($"Cannot create an instance of '{avaloniaPageType}'.");

        var avaloniaPage = view as Page ?? new ContentPage { Content = view };

        avaloniaPage.DataContext = page;

        return avaloniaPage;
    }
}
