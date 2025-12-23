// -----------------------------------------------------------------------
// <copyright file="ToggleSwitchPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using MyNet.Avalonia.Demo.Controls;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class ToggleSwitchPageViewModel : AutoBuildPageViewModel
{
    public ToggleSwitchPageViewModel()
        : base(nameof(ToggleSwitch), [
            new ControlThemeBuilder()
            .AddDefaultRoles()
            .AddSizes("Small", "Medium", "Large"),

            new ControlThemeBuilder("Inner", ContentType.Icon)
            .AddDefaultRoles()
            .AddSizes("Small", "Medium", "Large"),

            new ControlThemeBuilder("Alternate", ContentType.None)
            .AddDefaultRoles(),

            new ControlThemeBuilder("Button")
            .AddLayouts("Circle")
            .AddStyles("Light", "Outlined", "Text")
            .AddCartesianStyles("Light", "Outlined", "Text")
            .AddDefaultRoles()
            .AddSizes("Small", "Medium", "Large"),

            new ControlThemeBuilder("Button.Rounded", ContentType.Icon)
            .AddStyles("Light", "Outlined", "Text")
            .AddCartesianStyles("Light", "Outlined", "Text")
            .AddDefaultRoles()
            .AddSizes("Small", "Medium", "Large"),

            new ControlThemeBuilder("Button.Icon", ContentType.Geometry)
            .AddDefaultRoles()
            .AddSizes("ExtraSmall", "Small", "Medium", "Large", "ExtraLarge")
        ])
    { }
}
