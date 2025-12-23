// -----------------------------------------------------------------------
// <copyright file="ButtonPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using MyNet.Avalonia.Demo.Controls;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class ButtonPageViewModel : AutoBuildPageViewModel
{
    public ButtonPageViewModel()
        : base(nameof(Button), [
            new ControlThemeBuilder()
            .AddLayouts("Circle")
            .AddStyles("Light", "Outlined", "Text", "Shadow")
            .AddCartesianStyles("Light", "Outlined", "Text")
            .AddDefaultRoles()
            .AddSizes("Small", "Medium", "Large"),

            new ControlThemeBuilder("Rounded", ContentType.Icon)
            .AddStyles("Light", "Outlined", "Text", "Shadow")
            .AddCartesianStyles("Light", "Outlined", "Text")
            .AddDefaultRoles()
            .AddSizes("Small", "Medium", "Large"),

            new ControlThemeBuilder("Icon", ContentType.Geometry)
            .AddDefaultRoles()
            .AddSizes("Small", "Medium", "Large")
        ])
    { }
}
