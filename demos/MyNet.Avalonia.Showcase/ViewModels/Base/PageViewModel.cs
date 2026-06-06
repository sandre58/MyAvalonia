// -----------------------------------------------------------------------
// <copyright file="PageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Material.Icons;
using MyNet.Avalonia.Showcase.ViewModels.Menu;
using MyNet.Globalization.Facade;
using MyNet.Observable;
using MyNet.Observable.Behaviors.Metadata.Attributes;
using MyNet.UI.Navigation.Models;

namespace MyNet.Avalonia.Showcase.ViewModels.Base;

/// <summary>
/// Base view model for showcase pages (menu title + navigation marker).
/// </summary>
internal abstract class PageViewModel : ObservableObject, IMenuItemViewModel, INavigationPage
{
    private static readonly IReadOnlyList<IMenuItemViewModel> EmptyItems = [];

    protected PageViewModel() => Title = MenuPageTitleKeys.For(GetType());

    /// <inheritdoc/>
    [UpdateOnCultureChanged]
    public string Title => field.Translate();

    /// <inheritdoc/>
    public virtual MaterialIconKind Icon { get; } = MaterialIconKind.CircleOffOutline;

    /// <inheritdoc/>
    public bool IsGroup => false;

    /// <inheritdoc/>
    public bool IsSectionHeader => false;

    /// <inheritdoc/>
    public INavigationPage NavigationTarget => this;

    /// <inheritdoc/>
    public IReadOnlyList<IMenuItemViewModel> Items => EmptyItems;

    /// <inheritdoc/>
    public virtual Task OnNavigatingToAsync(NavigationContext context, CancellationToken cancellationToken) => Task.CompletedTask;

    /// <inheritdoc/>
    public virtual Task OnNavigatedAsync(NavigationContext context, CancellationToken cancellationToken) => Task.CompletedTask;

    /// <inheritdoc/>
    public virtual Task OnNavigatingFromAsync(NavigationContext context, CancellationToken cancellationToken) => Task.CompletedTask;
}
