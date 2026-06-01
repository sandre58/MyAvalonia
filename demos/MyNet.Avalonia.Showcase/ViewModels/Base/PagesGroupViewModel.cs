// -----------------------------------------------------------------------
// <copyright file="PagesGroupViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using DynamicData;
using Material.Icons;
using MyNet.Globalization.Facade;
using MyNet.Observable.Behaviors.Metadata.Attributes;

namespace MyNet.Avalonia.Showcase.ViewModels.Base;

/// <summary>
/// View model for a grouped set of showcase pages in the navigation menu.
/// </summary>
internal sealed class PagesGroupViewModel : ObservableObject, IMenuItemViewModel
{
    [SuppressMessage("Usage", "CA2213:Disposable fields should be disposed", Justification = "Disposed with the view model.")]
    private readonly string _titleResourceKey;
    private readonly ObservableCollection<PageViewModel> _pages = [];

    public PagesGroupViewModel(string resourceKey, MaterialIconKind icon)
    {
        _titleResourceKey = resourceKey;
        Icon = icon;
        Pages = new(_pages);
    }

    /// <inheritdoc/>
    [UpdateOnCultureChanged]
    public string Title => _titleResourceKey.Translate();

    /// <inheritdoc/>
    public MaterialIconKind Icon { get; }

    /// <inheritdoc/>
    public bool IsGroup => true;

    /// <summary>Gets child pages in this group.</summary>
    public ReadOnlyObservableCollection<PageViewModel> Pages { get; }

    /// <summary>Adds pages to the group.</summary>
    public void AddPages(params PageViewModel[] pages) => _pages.AddRange(pages);
}
