// -----------------------------------------------------------------------
// <copyright file="SplitViewPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using MyNet.Avalonia.Demo.Controls;
using MyNet.Avalonia.Theme.Palettes;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class SplitViewPageViewModel : AutoBuildPageViewModel
{
    public SplitViewPageViewModel()
        : base(nameof(SplitView), [
            new ControlThemeBuilder()
            .AddStyles("Light", "Outlined", "Shadow")
            .AddCartesianStyles("Light", "Outlined")
            .AddCartesianStyles("Outlined", "Solid")
            .AddThemeRoles().AddRoles(ThemeRole.Dark)
        ])
    { }
}
