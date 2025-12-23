// -----------------------------------------------------------------------
// <copyright file="ToggleButtonPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls.Primitives;
using MyNet.Avalonia.Demo.Controls;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class ToggleButtonPageViewModel : AutoBuildPageViewModel
{
    public ToggleButtonPageViewModel()
        : base(nameof(ToggleButton), [
            new ControlThemeBuilder()
            .AddLayouts("Circle")
            .AddStyles("Light", "Outlined", "Text")
            .AddCartesianStyles("Light", "Outlined", "Text")
            .AddDefaultRoles()
            .AddSizes("Small", "Medium", "Large"),

            new ControlThemeBuilder("Rounded", ContentType.Icon)
            .AddStyles("Light", "Outlined", "Text")
            .AddCartesianStyles("Light", "Outlined", "Text")
            .AddDefaultRoles()
            .AddSizes("Small", "Medium", "Large"),

            new ControlThemeBuilder("Icon", ContentType.Geometry)
            .AddDefaultRoles()
            .AddSizes("Small", "Medium", "Large")
        ])
    { }
}
