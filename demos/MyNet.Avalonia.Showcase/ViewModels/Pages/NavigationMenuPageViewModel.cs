// -----------------------------------------------------------------------
// <copyright file="NavigationMenuPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.ObjectModel;
using Material.Icons;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Showcase.Resources;
using MyNet.Avalonia.Showcase.ThemeBuilder;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders.Editors;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.UI.Commands;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class NavigationMenuPageViewModel(ICommandFactory commands) : ShowcaseViewModel(nameof(NavigationMenu), commands, [
    new ControlThemeBuilder()
        .AddDefaultRoles()
        .AddProperty(NavigationMenu.CollapseWidthProperty, 50, x => x.DisplayName(nameof(SettingsResources.CollapseWidth)).Of<SliderEditor>(editor => editor.WithRange(40, 50)))
        .AddProperty(NavigationMenu.ExpandWidthProperty, 300, x => x.DisplayName(nameof(SettingsResources.ExpandWidth)).Of<SliderEditor>(editor => editor.WithRange(200, 300)))
        .AddProperty(NavigationMenu.SubMenuIndentProperty, 24, x => x.DisplayName(nameof(SettingsResources.SubMenuIndent)).Of<SliderEditor>(editor => editor.WithRange(0, 24)))
])
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.Navigation;

    public ObservableCollection<NavigationMenuDemoItem> BoundItems { get; } =
    [
        new() { Header = "Workspace", IsSectionHeader = true },
        new()
        {
            Header = "Projects",
            Icon = MaterialIconKind.FolderOutline,
            Children =
            [
                new() { Header = "Active", Icon = MaterialIconKind.PlayCircleOutline },
                new() { Header = "Archived", Icon = MaterialIconKind.ArchiveOutline }
            ]
        },
        new() { Header = "Team", IsSectionHeader = true },
        new() { Header = "Members", Icon = MaterialIconKind.AccountGroupOutline },
        new() { Header = "Settings", Icon = MaterialIconKind.CogOutline }
    ];

    public NavigationMenuDemoItem? SelectedBoundItem
    {
        get;
        set => SetProperty(ref field, value);
    }

    public string? SelectedBoundItemLabel => SelectedBoundItem?.Header;
}
