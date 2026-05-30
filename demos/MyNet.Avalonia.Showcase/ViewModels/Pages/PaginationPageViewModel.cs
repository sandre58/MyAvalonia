// -----------------------------------------------------------------------
// <copyright file="PaginationPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Material.Icons;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Showcase.Extensions;
using MyNet.Avalonia.Showcase.Resources;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ViewModels.Playground;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class PaginationPageViewModel() : ShowcaseViewModel(nameof(Pagination),
[
    new ControlThemeBuilder()
        .AddThemeRoles()
        .AddProperty(Pagination.ShowPageSizeSelectorProperty, false, x => x.DisplayName(nameof(SettingsResources.ShowPageSizeSelector)))
        .AddProperty(Pagination.ShowQuickJumpProperty, false, x => x.DisplayName(nameof(SettingsResources.ShowQuickJump))),

    new ControlThemeBuilder("Compact")
        .AddThemeRoles()
])
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.PageLayoutBody;
}
