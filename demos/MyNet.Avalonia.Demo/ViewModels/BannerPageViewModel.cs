// -----------------------------------------------------------------------
// <copyright file="BannerPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog.ClassProviders;
using MyNet.Utilities;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class BannerPageViewModel : ControlCatalogViewModel
{
    public BannerPageViewModel()
        : base(nameof(Banner), [
            new ControlThemeBuilder()
                    .AddShapes("shape-circle")
                    .AddVariants("variant-solid", "variant-light", "variant-outlined", "variant-text", "shadow-surface")
                    .AddDefaultRoles()
                    .AddDefaultSizes()
        ]) => Playground.ClassProviders.AddRange([PositionClassProvider, HeaderAlignmentClassProvider]);

    public ClassProvider PositionClassProvider { get; } = new("position-top");

    public ClassProvider HeaderAlignmentClassProvider { get; } = new("align-header-left");
}
