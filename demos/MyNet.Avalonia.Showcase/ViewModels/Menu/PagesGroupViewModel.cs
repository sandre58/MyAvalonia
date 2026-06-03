// -----------------------------------------------------------------------
// <copyright file="PagesGroupViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Material.Icons;
using MyNet.Globalization.Facade;
using MyNet.Observable.Behaviors.Metadata.Attributes;
using MyNet.UI.Navigation.Models;

namespace MyNet.Avalonia.Showcase.ViewModels.Menu;

/// <summary>
/// View model for a grouped set of showcase pages in the navigation menu.
/// </summary>
internal sealed class PagesGroupViewModel : ObservableObject, IMenuItemViewModel
{
    private readonly ObservableCollection<IMenuItemViewModel> _pages = [];

    public PagesGroupViewModel(string resourceKey, MaterialIconKind icon)
    {
        Title = resourceKey;
        Icon = icon;
        Pages = new(_pages);
    }

    /// <inheritdoc/>
    [UpdateOnCultureChanged]
    public string Title => field.Translate();

    /// <inheritdoc/>
    public MaterialIconKind Icon { get; }

    /// <inheritdoc/>
    public bool IsGroup => true;

    /// <inheritdoc/>
    public bool IsSeparator => false;

    /// <inheritdoc/>
    public INavigationPage? NavigationTarget => null;

    /// <summary>Gets child pages in this group (resolved from DI when opened).</summary>
    public ReadOnlyObservableCollection<IMenuItemViewModel> Pages { get; }

    /// <inheritdoc/>
    public IReadOnlyList<IMenuItemViewModel> Items => Pages;

    /// <summary>Adds lazy menu entries for the given page view model types.</summary>
    public void AddPages(IEnumerable<Type> viewModelTypes, IServiceProvider services)
    {
        foreach (var viewModelType in viewModelTypes)
            _pages.Add(new LazyPageMenuItem(viewModelType, services));
    }
}
