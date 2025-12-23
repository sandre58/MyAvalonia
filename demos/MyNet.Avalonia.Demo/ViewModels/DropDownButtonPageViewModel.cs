// -----------------------------------------------------------------------
// <copyright file="DropDownButtonPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Demo.Controls;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class DropDownButtonPageViewModel : AutoBuildPageViewModel
{
    public DropDownButtonPageViewModel()
        : base(nameof(DropDownButton), [
            new ControlThemeBuilder()
            .AddLayouts("Circle")
            .AddStyles("Light", "Solid", "Outlined", "Text")
            .AddCartesianStyles("Solid", "Shadow").AddCartesianStyles("Light", "Outlined", "Text")
            .AddDefaultRoles()
            .AddSizes("Small", "Medium", "Large"),

            new ControlThemeBuilder("Rounded", ContentType.Icon)
            .AddStyles("Light", "Solid", "Outlined", "Text")
            .AddCartesianStyles("Solid", "Shadow").AddCartesianStyles("Light", "Outlined", "Text")
            .AddDefaultRoles()
            .AddSizes("Small", "Medium", "Large")
        ])
    { }
}
