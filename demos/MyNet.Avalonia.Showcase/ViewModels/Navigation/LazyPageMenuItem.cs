// -----------------------------------------------------------------------
// <copyright file="LazyPageMenuItem.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Material.Icons;
using Microsoft.Extensions.DependencyInjection;
using MyNet.Avalonia.Showcase.ViewModels.Base;
using MyNet.Globalization.Facade;
using MyNet.UI.Navigation.Models;

namespace MyNet.Avalonia.Showcase.ViewModels.Navigation;

/// <summary>
/// Menu entry that resolves the page view model from DI only when the page is opened.
/// </summary>
internal sealed class LazyPageMenuItem : IMenuItemViewModel
{
    private readonly Type _viewModelType;
    private readonly IServiceProvider _services;
    private PageViewModel? _page;

    public LazyPageMenuItem(Type viewModelType, IServiceProvider services)
    {
        _viewModelType = viewModelType ?? throw new ArgumentNullException(nameof(viewModelType));
        _services = services ?? throw new ArgumentNullException(nameof(services));
    }

    /// <summary>Gets the resolved page view model (singleton from DI).</summary>
    public PageViewModel Page => _page ??= (PageViewModel)_services.GetRequiredService(_viewModelType);

    /// <inheritdoc/>
    public INavigationPage NavigationTarget => Page;

    /// <inheritdoc/>
    public string? Title => _page?.Title ?? CreateTitleFromTypeName(_viewModelType.Name);

    /// <inheritdoc/>
    public MaterialIconKind Icon => _page?.Icon ?? MaterialIconKind.CircleOffOutline;

    /// <inheritdoc/>
    public bool IsGroup => false;

    private static string CreateTitleFromTypeName(string name)
    {
        foreach (var suffix in new[] { "PageViewModel", "ViewModel", "Page" })
        {
            if (name.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
            {
                name = name[..^suffix.Length];
                break;
            }
        }

        return name.Translate();
    }
}
