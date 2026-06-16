// -----------------------------------------------------------------------
// <copyright file="AvaloniaPageFactory.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Avalonia.Controls;
using MyNet.UI.Locators.Factories;
using MyNet.UI.Navigation.Models;

namespace MyNet.Avalonia.Extended.Navigation;

/// <summary>
/// Creates Avalonia pages from navigation page instances using <see cref="IViewFactory"/>.
/// </summary>
/// <param name="viewFactory">The view factory used to resolve views from page types.</param>
public sealed class AvaloniaPageFactory(IViewFactory viewFactory) : IAvaloniaPageFactory
{
    private readonly Dictionary<INavigationPage, Page> _pages = new(ReferenceEqualityComparer.Instance);

    /// <inheritdoc />
    public Page Create(INavigationPage page)
    {
        ArgumentNullException.ThrowIfNull(page);

        if (_pages.TryGetValue(page, out var cached))
            return cached;

        var view = viewFactory.CreateView(page.GetType());

        Page avaloniaPage;
        if (view is Page pageView)
        {
            pageView.DataContext = page;
            avaloniaPage = pageView;
        }
        else
        {
            avaloniaPage = new ContentPage
            {
                Content = view,
                DataContext = page
            };
        }

        _pages[page] = avaloniaPage;
        return avaloniaPage;
    }

    /// <inheritdoc />
    public void Clear() => _pages.Clear();
}
