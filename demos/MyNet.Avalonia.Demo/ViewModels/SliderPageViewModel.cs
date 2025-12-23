// -----------------------------------------------------------------------
// <copyright file="SliderPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using MyNet.Avalonia.Demo.Controls;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class SliderPageViewModel : AutoBuildPageViewModel
{
    public SliderPageViewModel()
        : base(nameof(Slider), [
            new ControlThemeBuilder()
            .AddThemeRoles()
        ])
    { }
}
