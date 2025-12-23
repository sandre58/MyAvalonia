// -----------------------------------------------------------------------
// <copyright file="TabControlPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using MyNet.Avalonia.Demo.Controls;
using MyNet.Avalonia.Theme.Palettes;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class TabControlPageViewModel : AutoBuildPageViewModel
{
    public TabControlPageViewModel()
        : base(nameof(TabControl), [
            new ControlThemeBuilder()
            .AddLayouts("Header", "Header Inverse")
            .AddStyles("Solid", "Light", "Outlined")
            .AddCartesianStyles("Solid", "Light", "Outlined")
            .AddCartesianStyles("Circle", "Solid")
            .AddRoles([ThemeRole.Accent, ThemeRole.Inverse]),

            new ControlThemeBuilder("Indicator")
            .AddRoles([ThemeRole.Accent, ThemeRole.Inverse])
        ])
    { }
}
