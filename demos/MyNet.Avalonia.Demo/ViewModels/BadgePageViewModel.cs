// -----------------------------------------------------------------------
// <copyright file="BadgePageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Demo.Controls;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class BadgePageViewModel : AutoBuildPageViewModel
{
    public BadgePageViewModel()
        : base(nameof(Badge), [
            new ControlThemeBuilder()
            .AddLayouts("Circle")
            .AddStyles("Light", "Outlined", "Shadow")
            .AddCartesianStyles("Light", "Outlined")
            .AddDefaultRoles()
            .AddSizes("Medium", "Large")
        ])
    { }
}
