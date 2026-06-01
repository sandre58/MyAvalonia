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
using MyNet.Observable.Collections.Sources;
using MyNet.UI.ViewModels.List;
using MyNet.UI.ViewModels.List.Factories;
using MyNet.UI.ViewModels.List.Paging;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class IconsPageViewModel : PageViewModel
{
    public ListViewModel<MaterialIconKindWrapper> Icons { get; }

    public IconsPageViewModel()
        => Icons = new(
            SourceEngine<MaterialIconKindWrapper>.From(
                IconsHelper.Groups.Select(x => new MaterialIconKindWrapper(x)),
                readOnly: true),
            new ListViewModelOptions<MaterialIconKindWrapper> { Paging = new PagingViewModel(100) });

    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.TagFaces;
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
