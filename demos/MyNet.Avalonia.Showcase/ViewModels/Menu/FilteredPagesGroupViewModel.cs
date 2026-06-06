// -----------------------------------------------------------------------
// <copyright file="FilteredPagesGroupViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Material.Icons;
using MyNet.Observable;
using MyNet.Observable.Behaviors.Metadata.Attributes;
using MyNet.UI.Navigation.Models;

namespace MyNet.Avalonia.Showcase.ViewModels.Menu;

/// <summary>
/// Group menu entry showing only pages that match the active menu filter.
/// </summary>
internal sealed class FilteredPagesGroupViewModel(PagesGroupViewModel source, IReadOnlyList<IMenuItemViewModel> pages)
    : ObservableObject, IMenuItemViewModel
{
    /// <inheritdoc/>
    [UpdateOnCultureChanged]
    public string Title => source.Title;

    /// <inheritdoc/>
    public MaterialIconKind Icon => source.Icon;

    /// <inheritdoc/>
    public bool IsGroup => true;

    /// <inheritdoc/>
    public bool IsSectionHeader => false;

    /// <inheritdoc/>
    public INavigationPage? NavigationTarget => null;

    /// <inheritdoc/>
    public IReadOnlyList<IMenuItemViewModel> Items { get; } = pages;
}
