// -----------------------------------------------------------------------
// <copyright file="CarouselPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using MyNet.Avalonia.Demo.Controls;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class CarouselPageViewModel : AutoBuildPageViewModel
{
    public CarouselPageViewModel()
        : base(nameof(Carousel), [
            new ControlThemeBuilder(),

            new ControlThemeBuilder("Full").AddThemeRoles()
        ])
    { }
}
