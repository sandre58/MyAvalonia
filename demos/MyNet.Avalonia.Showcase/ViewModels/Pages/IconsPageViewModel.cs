// -----------------------------------------------------------------------
// <copyright file="IconsPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Material.Icons;
using MyNet.Avalonia.Controls.Helpers;
using MyNet.Avalonia.Showcase.ViewModels.Base;
using MyNet.Avalonia.Theme.Theming;
using MyNet.Observable.Collections;
using MyNet.Observable.Collections.Filters;
using MyNet.Observable.Collections.Selection;
using MyNet.Primitives;
using MyNet.UI.Commands;
using MyNet.UI.ViewModels.List.Factories;
using MyNet.UI.ViewModels.List.Paging;
using MyNet.UI.ViewModels.List.Selection;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class IconsPageViewModel : PageViewModel
{
    public IconsPageViewModel(ICommandFactory commands)
    {
        Icons = new(
            ExtendedCollection.FromReadOnly(IconsHelper.Groups.Select(x => new MaterialIconKindWrapper(x))),
            new() { Paging = new PagingViewModel(100) });

        MoveToPageCommand = commands.CreateRequired<int>(page => Icons.Paging!.MoveToPage(page));
    }

    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.TagFaces;

    public IconsMaterialListViewModel Icons { get; }

    public ICommand MoveToPageCommand { get; }

    public string? SearchText
    {
        get;
        set
        {
            if (!SetProperty(ref field, value))
                return;

            Icons.ApplySearch(value);
        }
    }
}

internal sealed class IconsMaterialListViewModel(
    ExtendedCollection<MaterialIconKindWrapper> source,
    ListViewModelOptions<MaterialIconKindWrapper>? options)
    : SelectableListViewModel<MaterialIconKindWrapper>(source, SelectionMode.Single, options)
{
    public void ApplySearch(string? searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
        {
            DataProvider.ClearFilter();
        }
        else
        {
            var filter = FilterBuilder<MaterialIconKindWrapper>.Create()
                .Where(icon =>
                    icon.DisplayName.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                    || icon.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                    || icon.Aliases.Any(alias => alias.Contains(searchText, StringComparison.OrdinalIgnoreCase)))
                .Build();

            if (filter is not null)
                DataProvider.SetFilter(filter);
            else
                DataProvider.ClearFilter();
        }

        RequestPipelineRefresh();
    }
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

    public string[] CodeBlocks { get; } = [.. group.Aliases.SelectMany(x => CodePatterns.Select(y => y.FormatWithInvariant(x)))];
}
