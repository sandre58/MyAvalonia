// -----------------------------------------------------------------------
// <copyright file="CarouselPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog.ClassProviders;
using MyNet.Avalonia.Theme.Palettes;
using MyNet.Utilities;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class CarouselPageViewModel : ControlCatalogViewModel
{
    public CarouselPageViewModel()
        : base(nameof(Carousel),
            [
                new ControlThemeBuilder(),

                new ControlThemeBuilder("Full")
                    .AddRoles([ThemeRole.Default, ThemeRole.Accent, ThemeRole.Contrast])
            ]) => Playground.ClassProviders.AddRange([TypeClassProvider, IndicatorClassProvider]);

    public ClassProvider TypeClassProvider { get; } = new("variant-dots");

    public ClassProvider IndicatorClassProvider { get; } = new("indicator-center");
}
