// -----------------------------------------------------------------------
// <copyright file="LazyPageMenuItem.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Material.Icons;
using Microsoft.Extensions.DependencyInjection;
using MyNet.Avalonia.Showcase.ViewModels.Base;
using MyNet.Globalization.Facade;
using MyNet.Observable;
using MyNet.Observable.Behaviors.Metadata.Attributes;
using MyNet.UI.Navigation.Models;

namespace MyNet.Avalonia.Showcase.ViewModels.Menu;

/// <summary>
/// Menu entry that resolves the page view model from DI only when navigation needs it.
/// </summary>
internal sealed class LazyPageMenuItem(Type viewModelType, MaterialIconKind icon, IServiceProvider services) : ObservableObject, IMenuItemViewModel
{
    private static readonly IReadOnlyList<IMenuItemViewModel> EmptyItems = [];
    private readonly IServiceProvider _services = services ?? throw new ArgumentNullException(nameof(services));

    /// <summary>Gets the view model type registered in DI for this menu entry.</summary>
    public Type ViewModelType { get; } = viewModelType ?? throw new ArgumentNullException(nameof(viewModelType));

    /// <summary>Gets the resolved page view model (singleton from DI).</summary>
    public PageViewModel Page => field ??= (PageViewModel)_services.GetRequiredService(ViewModelType);

    /// <summary>Resolves and returns the navigation page from DI.</summary>
    public INavigationPage ResolvePage() => Page;

    /// <inheritdoc/>
    public INavigationPage? NavigationTarget => null;

    /// <inheritdoc/>
    [UpdateOnCultureChanged]
    public string Title { get => field.Translate(); } = MenuPageTitleKeys.For(viewModelType);

    /// <inheritdoc/>
    public MaterialIconKind Icon { get; } = icon;

    /// <inheritdoc/>
    public bool IsGroup => false;

    /// <inheritdoc/>
    public bool IsSectionHeader => false;

    /// <inheritdoc/>
    public IReadOnlyList<IMenuItemViewModel> Items => EmptyItems;
}
