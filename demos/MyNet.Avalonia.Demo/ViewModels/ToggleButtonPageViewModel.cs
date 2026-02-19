// -----------------------------------------------------------------------
// <copyright file="ToggleButtonPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls.Primitives;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog.ContentProviders;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class ToggleButtonPageViewModel : ControlCatalogViewModel
{
    public ToggleButtonPageViewModel()
        : base(nameof(ToggleButton),
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
    }
}
