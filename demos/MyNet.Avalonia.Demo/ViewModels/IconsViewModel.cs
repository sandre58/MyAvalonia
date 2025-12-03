// -----------------------------------------------------------------------
// <copyright file="IconsViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Media;
using Avalonia.Threading;
using DynamicData;
using MyNet.Avalonia.Enums;
using MyNet.Avalonia.Theme;
using MyNet.Avalonia.Theme.Extensions;
using MyNet.Humanizer;
using MyNet.UI.Navigation.Models;
using MyNet.UI.ViewModels.List;
using MyNet.UI.ViewModels.List.Filtering;
using MyNet.UI.ViewModels.List.Filtering.Filters;
using MyNet.UI.ViewModels.List.Paging;
using MyNet.UI.ViewModels.List.Sorting;
using MyNet.UI.ViewModels.Workspace;
using MyNet.Utilities;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class IconsViewModel : NavigableWorkspaceViewModel
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2213:Disposable fields should be disposed", Justification = "Disposed in Cleanup method")]
    private readonly SourceList<IconBuilderData> _allIcons = new();

    public ListViewModel<IconBuilderData> Icons { get; }

    public IconsViewModel() => Icons = new ListViewModel<IconBuilderData>(_allIcons.Connect(), new IconsControllerProvider())
    {
        CanPage = true
    };

    protected override bool CanRefreshOnNavigatedTo(NavigationContext navigationContext) => !IsLoaded;

    protected override async Task RefreshCoreAsync()
    {
        // Get all icon names on background thread (no UI access)
        var iconNames = await Task.Run(() => Enum.GetValues<IconData>().Select(x => x.ToString()).ToList()).ConfigureAwait(false);

        // Create IconBuilderData on UI thread progressively
        var newIcons = new List<IconBuilderData>();

        await Dispatcher.UIThread.InvokeAsync(async () =>
        {
            // Create and add items in batches to keep UI responsive
            const int batchSize = 50;
            for (var i = 0; i < iconNames.Count; i += batchSize)
            {
                foreach (var iconName in iconNames.Skip(i).Take(batchSize))
                {
                    // ToGeometry() must be called on UI thread
                    if (Enum.TryParse<IconData>(iconName, out var iconEnum))
                    {
                        var iconData = new IconBuilderData(iconName, iconEnum.ToGeometry());
                        _allIcons.Add(iconData);
                    }
                }

                // Small delay to keep UI responsive
                if (i + batchSize < iconNames.Count)
                {
                    await Task.Delay(1).ConfigureAwait(true);
                }
            }

            MarkAsLoaded();
        },
        DispatcherPriority.Background).ConfigureAwait(false);
    }

    protected override void Cleanup()
    {
        _allIcons.Dispose();
        base.Cleanup();
    }
}

internal sealed class IconsControllerProvider : ListParametersProvider
{
    public override IFiltersViewModel ProvideFilters() => new StringFilterViewModel(nameof(IconBuilderData.Name));

    public override ISortingViewModel ProvideSorting() => new SortingViewModel(nameof(IconBuilderData.Name));

    public override IPagingViewModel ProvidePaging() => new PagingViewModel(150);
}

internal sealed class IconBuilderData(string name, Geometry? geometry)
{
    public static readonly ICollection<string> CodePatterns = [
        "{0}",
        ThemeResourceKeyFactory.Pattern(ThemeResourceKeyFactory.GeometryKey),
        ThemeResourceKeyFactory.IconPathPattern,
        ThemeResourceKeyFactory.IconPattern
    ];

    public string Name { get; } = name;

    public string DisplayName { get; } = name.Humanize().ToTitle();

    public Geometry? Geometry { get; } = geometry;

    public ObservableCollection<string> CodeBlocks { get; } = [.. CodePatterns.Select(x => x.FormatWith(name))];
}
