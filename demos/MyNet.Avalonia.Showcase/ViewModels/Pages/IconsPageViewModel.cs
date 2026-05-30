// -----------------------------------------------------------------------
// <copyright file="IconsPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Material.Icons;
using MyNet.Avalonia.Controls.Helpers;
using MyNet.Avalonia.Showcase.ViewModels.Base;
using MyNet.Avalonia.Theme.Theming;
using MyNet.UI.Navigation.Models;
using MyNet.UI.ViewModels.List;
using MyNet.UI.ViewModels.List.Filtering;
using MyNet.UI.ViewModels.List.Filtering.Filters;
using MyNet.UI.ViewModels.List.Paging;
using MyNet.UI.ViewModels.List.Sorting;
using MyNet.Utilities;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class IconsPageViewModel : PageViewModel
{
    public ListViewModel<MaterialIconKindWrapper> Icons { get; }

    public IconsPageViewModel()
        => Icons = new([.. IconsHelper.Groups.Select(x => new MaterialIconKindWrapper(x)).ToList()],
            new IconsControllerProvider()) { CanPage = true };

    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.TagFaces;

    protected override bool CanRefreshOnNavigatedTo(NavigationContext navigationContext) => !IsLoaded;
}

internal sealed class IconsControllerProvider : ListParametersProvider
{
    public override IFiltersViewModel ProvideFilters() => new StringFilterViewModel(nameof(MaterialIconKindWrapper.DisplayAliases));

    public override ISortingViewModel ProvideSorting() => new SortingViewModel(nameof(MaterialIconKindWrapper.Name));

    public override IPagingViewModel ProvidePaging() => new PagingViewModel(100);
}

internal sealed class MaterialIconKindWrapper(MaterialIconKindGroup group)
{
    public static readonly ICollection<string> CodePatterns =
    [
        "{0}",
        ThemeResourceKeyFactory.MaterialIconPathPattern,
        ThemeResourceKeyFactory.MaterialIconPattern
    ];

    public string Name => group.Name;

    public MaterialIconKind Kind { get; } = group.Kind;

    public string[] Aliases { get; } = group.Aliases;

    public string DisplayName { get; } = group.DisplayName;

    public string DisplayAliases { get; } = string.Join(",", group.Aliases);

    public string[] CodeBlocks { get; } = [.. group.Aliases.SelectMany(x => CodePatterns.Select(y => y.FormatWith(x)))];
}
