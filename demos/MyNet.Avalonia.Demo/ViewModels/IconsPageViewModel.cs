// -----------------------------------------------------------------------
// <copyright file="IconsPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Media;
using MyNet.Avalonia.Theme.Classes.Enums;
using MyNet.Avalonia.Theme.Theming;
using MyNet.Humanizer;
using MyNet.UI.Navigation.Models;
using MyNet.UI.ViewModels.List;
using MyNet.UI.ViewModels.List.Filtering;
using MyNet.UI.ViewModels.List.Filtering.Filters;
using MyNet.UI.ViewModels.List.Paging;
using MyNet.UI.ViewModels.List.Sorting;
using MyNet.Utilities;
using static MyNet.Avalonia.Theme.ThemeResources;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class IconsPageViewModel : PageViewModel
{
    public ListViewModel<IconBuilderData> Icons { get; }

    public IconsPageViewModel() => Icons = new ListViewModel<IconBuilderData>([.. Enum.GetValues<IconData>().Select(static icon => new IconBuilderData(icon.ToString()))], new IconsControllerProvider())
    {
        CanPage = true
    };

    /// <inheritdoc/>
    public override IconData Icon => IconData.TagFaces;

    protected override bool CanRefreshOnNavigatedTo(NavigationContext navigationContext) => !IsLoaded;
}

internal sealed class IconsControllerProvider : ListParametersProvider
{
    public override IFiltersViewModel ProvideFilters() => new StringFilterViewModel(nameof(IconBuilderData.Name));

    public override ISortingViewModel ProvideSorting() => new SortingViewModel(nameof(IconBuilderData.Name));

    public override IPagingViewModel ProvidePaging() => new PagingViewModel(100);
}

internal sealed class IconBuilderData(string name)
{
    public static readonly ICollection<string> CodePatterns = [
        "{0}",
        ThemeResourceKeyFactory.Pattern(ThemeResourceKeyFactory.GeometryKey),
        ThemeResourceKeyFactory.IconPathPattern,
        ThemeResourceKeyFactory.IconPattern
    ];

    public string Name { get; } = name;

    public string DisplayName { get; } = name.Humanize().ToTitle();

    public Geometry? Geometry => _geometry ??= Icons.Get(Name).Value;

    public ObservableCollection<string> CodeBlocks { get; } = [.. CodePatterns.Select(x => x.FormatWith(name))];

    private Geometry? _geometry;
}
