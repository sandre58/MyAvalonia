// -----------------------------------------------------------------------
// <copyright file="ToggleButtonPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls.Primitives;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog.ContentProviders;
using MyNet.Avalonia.Theme.Classes.Enums;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class ToggleButtonPageViewModel() : ControlCatalogViewModel(nameof(ToggleButton),
    [
        new ControlThemeBuilder()
                .AddShapes("shape-circle")
                .AddVariants("variant-light", "variant-outlined", "variant-text", "shadow-control")
                .AddDefaultSizes()
                .AddDefaultRoles(),

        new ControlThemeBuilder("Rounded", defaultContentType: ContentProviderType.Icon)
                .AddVariants("variant-light", "variant-outlined", "variant-text", "shadow-control")
                .AddDefaultSizes()
                .AddDefaultRoles(),

        new ControlThemeBuilder("Icon", defaultContentType: ContentProviderType.Icon)
                .AddDefaultSizes()
                .AddDefaultRoles()
    ])
{
    /// <inheritdoc/>
    public override IconData Icon => IconData.ToggleSwitchVariant;
}
