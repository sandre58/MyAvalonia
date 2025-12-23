// -----------------------------------------------------------------------
// <copyright file="ExpanderPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using MyNet.Avalonia.Demo.Controls;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class ExpanderPageViewModel : AutoBuildPageViewModel
{
    public ExpanderPageViewModel()
        : base(nameof(Expander), [
            new ControlThemeBuilder()
            .AddStyles("Light", "Outlined", "Text", "Shadow", "Headered")
            .AddCartesianStyles("Headered", "HeaderShadow")
            .AddCartesianStyles("Outlined", "Solid")
            .AddCartesianStyles("Light", "Outlined", "Text")
            .AddCartesianStyles("Light", "Outlined", "Headered")
            .AddThemeRoles(),

            new ControlThemeBuilder("Button")
            .AddLayouts("Circle")
            .AddStyles("Light", "Outlined", "Text", "Shadow")
            .AddCartesianStyles("Light", "Outlined", "Text")
            .AddThemeRoles()
            .AddSizes("Small", "Medium", "Large")
        ])
    { }
}
