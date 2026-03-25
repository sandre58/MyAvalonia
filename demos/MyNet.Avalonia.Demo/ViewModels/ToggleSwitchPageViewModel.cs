// -----------------------------------------------------------------------
// <copyright file="ToggleSwitchPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog.ContentProviders;
using MyNet.Avalonia.Theme.Classes.Enums;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class ToggleSwitchPageViewModel() : ControlCatalogViewModel(nameof(ToggleSwitch),
    [
        new ControlThemeBuilder()
                .AddDefaultSizes()
                .AddDefaultRoles(),

        new ControlThemeBuilder("Inner", defaultContentType: ContentProviderType.Icon)
                .AddDefaultSizes()
                .AddDefaultRoles(),

        new ControlThemeBuilder("Alternate", defaultContentType: ContentProviderType.None)
                .AddDefaultRoles(),

        new ControlThemeBuilder("Button")
                .AddShapes("shape-circle")
                .AddVariants("variant-solid", "variant-light", "variant-outlined", "variant-text")
                .AddDefaultSizes()
                .AddDefaultRoles(),

        new ControlThemeBuilder("Button.Rounded", defaultContentType: ContentProviderType.Icon)
                .AddVariants("variant-solid", "variant-light", "variant-outlined", "variant-text")
                .AddDefaultSizes()
                .AddDefaultRoles(),

        new ControlThemeBuilder("Button.Icon", defaultContentType: ContentProviderType.Icon)
                .AddDefaultSizes()
                .AddDefaultRoles()
    ])
{
    /// <inheritdoc/>
    public override IconData Icon => IconData.ToggleSwitch;
}
