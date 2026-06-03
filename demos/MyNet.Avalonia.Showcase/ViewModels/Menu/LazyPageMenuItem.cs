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
/// Menu entry that resolves the page view model from DI only when the page is opened.
/// </summary>
internal sealed class LazyPageMenuItem(Type viewModelType, IServiceProvider services) : ObservableObject, IMenuItemViewModel
{
    private static readonly IReadOnlyList<IMenuItemViewModel> EmptyItems = [];
    private readonly Type _viewModelType = viewModelType ?? throw new ArgumentNullException(nameof(viewModelType));
    private readonly IServiceProvider _services = services ?? throw new ArgumentNullException(nameof(services));
    private PageViewModel? _page;

    /// <summary>Gets the resolved page view model (singleton from DI).</summary>
    public PageViewModel Page => _page ??= (PageViewModel)_services.GetRequiredService(_viewModelType);

    /// <inheritdoc/>
    public INavigationPage NavigationTarget => Page;

    /// <inheritdoc/>
    [UpdateOnCultureChanged]
    public string Title { get => field.Translate(); } = MenuPageTitleKeys.For(viewModelType);

    /// <inheritdoc/>
    public MaterialIconKind Icon => _page?.Icon ?? MaterialIconKind.CircleOffOutline;

    /// <inheritdoc/>
    public bool IsGroup => false;

    /// <inheritdoc/>
    public bool IsSeparator => false;

    /// <inheritdoc/>
    public IReadOnlyList<IMenuItemViewModel> Items => EmptyItems;
}
