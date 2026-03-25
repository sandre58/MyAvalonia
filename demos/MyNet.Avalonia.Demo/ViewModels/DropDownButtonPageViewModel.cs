// -----------------------------------------------------------------------
// <copyright file="DropDownButtonPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog.ClassProviders;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog.ContentProviders;
using MyNet.Avalonia.Theme.Classes.Enums;
using MyNet.Utilities;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class DropDownButtonPageViewModel : ControlCatalogViewModel
{
    public DropDownButtonPageViewModel()
        : base(nameof(DropDownButton),
            [
                new ControlThemeBuilder()
                    .AddShapes("shape-circle")
                    .AddVariants("variant-solid", "variant-light", "variant-outlined", "variant-text", "shadow-control")
                    .AddDefaultSizes()
                    .AddDefaultRoles(),

                new ControlThemeBuilder("Rounded", defaultContentType: ContentProviderType.Icon)
                    .AddVariants("variant-solid", "variant-light", "variant-outlined", "variant-text", "shadow-control")
                    .AddDefaultSizes()
                    .AddDefaultRoles()
            ]) => Playground.ClassProviders.AddRange([PlacementClassProvider]);

    /// <inheritdoc/>
    public override IconData Icon => IconData.FormSelect;

    public ClassProvider PlacementClassProvider { get; } = new("position-right");
}
