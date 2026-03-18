// -----------------------------------------------------------------------
// <copyright file="IconsPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Media;
using DynamicData;
using DynamicData.Binding;
using MyNet.Avalonia.Theme.Classes.Enums;
using MyNet.Avalonia.Theme.Theming;
using MyNet.Humanizer;
using MyNet.Observable.Collections.Providers;
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
    private readonly ObservableCollection<IconBuilderData> _allIcons = [];

    public ListViewModel<IconBuilderData> Icons { get; }

    public IconsPageViewModel() => Icons = new ListViewModel<IconBuilderData>(_allIcons.ToObservableChangeSet(), new IconsControllerProvider())
    {
        CanPage = true
    };

    protected override bool CanRefreshOnNavigatedTo(NavigationContext navigationContext) => !IsLoaded;

    protected override async Task RefreshCoreAsync()
    {
        var icons = Enum.GetValues<IconData>();
        _allIcons.Set(icons.Select(static icon => new IconBuilderData(icon.ToString())).Take(5));
        //await Dispatcher.UIThread.InvokeAsync(async () =>
        //{
        //    _allIcons.Edit(static list => list.Clear());

        //    const int batchSize = 400;
        //    for (var i = 0; i < icons.Length; i += batchSize)
        //    {
        //        var batch = icons
        //            .Skip(i)
        //            .Take(batchSize)
        //            .Select(static icon => new IconBuilderData(icon.ToString()))
        //            .ToList();

        //        _allIcons.Edit(list => list.AddRange(batch));

        //        if (i + batchSize < icons.Length)
        //        {
        //            await Task.Yield();
        //        }
        //    }

        //    MarkAsLoaded();
        //},
        //DispatcherPriority.Background).ConfigureAwait(false);
    }
}

internal sealed class IconsProvider : ISourceProvider<IconBuilderData>
{
    public ReadOnlyObservableCollection<IconBuilderData> Source => throw new NotImplementedException();

    public IObservable<IChangeSet<IconBuilderData>> Connect() => throw new NotImplementedException();
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
