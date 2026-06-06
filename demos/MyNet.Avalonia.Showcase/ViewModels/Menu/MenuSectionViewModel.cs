// -----------------------------------------------------------------------
// <copyright file="MenuSectionViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Material.Icons;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Showcase.Resources;
using MyNet.Globalization.Facade;
using MyNet.Observable;
using MyNet.Observable.Behaviors.Metadata.Attributes;
using MyNet.UI.Navigation.Models;

namespace MyNet.Avalonia.Showcase.ViewModels.Menu;

/// <summary>
/// Non-navigable section header mapped to <see cref="NavigationMenuItem.IsSectionHeader"/> (title + optional icon).
/// </summary>
internal sealed class MenuSectionViewModel : ObservableObject, IMenuItemViewModel
{
    private static readonly IReadOnlyList<IMenuItemViewModel> EmptyItems = [];

    /// <summary>Gets the section before the control catalog groups.</summary>
    public static MenuSectionViewModel ComponentsCatalog { get; } = new(nameof(MenuResources.ComponentsCatalog), MaterialIconKind.Widgets);

    private MenuSectionViewModel(string titleResourceKey, MaterialIconKind icon)
    {
        Title = titleResourceKey;
        Icon = icon;
    }

    /// <inheritdoc/>
    [UpdateOnCultureChanged]
    public string Title => field.Translate();

    /// <inheritdoc/>
    public MaterialIconKind Icon { get; }

    /// <inheritdoc/>
    public bool IsGroup => false;

    /// <inheritdoc/>
    public bool IsSectionHeader => true;

    /// <inheritdoc/>
    public INavigationPage? NavigationTarget => null;

    /// <inheritdoc/>
    public IReadOnlyList<IMenuItemViewModel> Items => EmptyItems;
}
