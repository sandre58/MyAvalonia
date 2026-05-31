// -----------------------------------------------------------------------
// <copyright file="AvaloniaPageFactory.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
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
    /// <inheritdoc />
    public Page Create(INavigationPage page)
    {
        ArgumentNullException.ThrowIfNull(page);

        var view = viewFactory.CreateView(page.GetType());

        if (view is Page avaloniaPage)
        {
            avaloniaPage.DataContext = page;
            return avaloniaPage;
        }

        return new ContentPage
        {
            Content = view,
            DataContext = page,
        };
    }
}
