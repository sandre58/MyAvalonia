// -----------------------------------------------------------------------
// <copyright file="IconsViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Media;
using MyNet.Avalonia.Enums;
using MyNet.Avalonia.Theme;
using MyNet.Avalonia.Theme.Extensions;
using MyNet.Humanizer;
using MyNet.UI.ViewModels.List;
using MyNet.UI.ViewModels.List.Filtering;
using MyNet.UI.ViewModels.List.Filtering.Filters;
using MyNet.UI.ViewModels.List.Paging;
using MyNet.UI.ViewModels.List.Sorting;
using MyNet.Utilities;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class IconsViewModel : SelectionListViewModel<IconBuilderData>
{
    public static readonly ICollection<string> CodePatterns = [
        "{0}",

        ThemeResources.GetPattern(ThemeResources.GeometryKey),

        ThemeResources.IconPathPattern,

        ThemeResources.IconPattern
    ];

    private static readonly IList<IconBuilderData> PathIcons = [.. Enum.GetValues<IconData>()
        .Select(x => new IconBuilderData(x.ToString(), x.ToGeometry()))];

    public IconsViewModel()
        : base(PathIcons, new IconsControllerProvider(), MyNet.UI.Selection.SelectionMode.Single) => CanPage = true;
}

internal sealed class IconsControllerProvider : ListParametersProvider
{
    public override IFiltersViewModel ProvideFilters() => new StringFilterViewModel(nameof(IconBuilderData.Name));

    public override ISortingViewModel ProvideSorting() => new SortingViewModel(nameof(IconBuilderData.Name));

    public override IPagingViewModel ProvidePaging() => new PagingViewModel(150);
}

internal sealed class IconBuilderData(string name, Geometry? geometry)
{
    public string Name { get; } = name;

    public string DisplayName { get; } = name.Humanize().ToTitle();

    public Geometry? Geometry { get; } = geometry;

    public ObservableCollection<string> CodeBlocks { get; } = [.. IconsViewModel.CodePatterns.Select(x => x.FormatWith(name))];
}
