// -----------------------------------------------------------------------
// <copyright file="SplitButtonPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog.ClassProviders;
using MyNet.Utilities;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class SplitButtonPageViewModel : ControlCatalogViewModel
{
    public SplitButtonPageViewModel()
        : base(nameof(SplitButton),
            [
                new ControlThemeBuilder()
                    .AddShapes("shape-circle")
                    .AddVariants("variant-solid", "variant-light", "variant-outlined", "variant-text", "shadow-control")
                    .AddDefaultSizes()
                    .AddDefaultRoles()
            ]) => Playground.ClassProviders.AddRange([PlacementClassProvider]);

    public ClassProvider PlacementClassProvider { get; } = new("position-right");
}
