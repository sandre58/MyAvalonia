// -----------------------------------------------------------------------
// <copyright file="PageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Threading;
using System.Threading.Tasks;
using Material.Icons;
using MyNet.Globalization.Facade;
using MyNet.UI.Navigation.Models;

namespace MyNet.Avalonia.Showcase.ViewModels.Base;

/// <summary>
/// Base view model for showcase pages (menu title + navigation marker).
/// </summary>
internal abstract class PageViewModel : ObservableObject, IMenuItemViewModel, INavigationPage
{
    /// <inheritdoc/>
    public string Title => CreateTitle();

    /// <summary>
    /// Creates a user-friendly title from the runtime type name.
    /// </summary>
    protected virtual string CreateTitle()
    {
        var name = GetType().Name;
        foreach (var suffix in new[] { "PageViewModel", "ViewModel", "Page" })
        {
            if (name.EndsWith(suffix, System.StringComparison.OrdinalIgnoreCase))
            {
                name = name[..^suffix.Length];
                break;
            }
        }

        return name.Translate();
    }

    /// <inheritdoc/>
    public virtual MaterialIconKind Icon { get; } = MaterialIconKind.CircleOffOutline;

    /// <inheritdoc/>
    public bool IsGroup => false;

    /// <inheritdoc/>
    public virtual Task OnNavigatingToAsync(NavigationContext context, CancellationToken cancellationToken) => Task.CompletedTask;

    /// <inheritdoc/>
    public virtual Task OnNavigatedAsync(NavigationContext context, CancellationToken cancellationToken) => Task.CompletedTask;

    /// <inheritdoc/>
    public virtual Task OnNavigatingFromAsync(NavigationContext context, CancellationToken cancellationToken) => Task.CompletedTask;
}
