// -----------------------------------------------------------------------
// <copyright file="BannerPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Demo.Controls;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class BannerPageViewModel : AutoBuildPageViewModel
{
    public BannerPageViewModel()
        : base(nameof(Banner), [
            new ControlThemeBuilder()
            .AddStyles("Light", "Outlined", "Shadow")
            .AddCartesianStyles("Light", "Outlined")
            .AddDefaultRoles()
        ])
    { }
}
