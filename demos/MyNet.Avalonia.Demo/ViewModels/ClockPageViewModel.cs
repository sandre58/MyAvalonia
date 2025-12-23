// -----------------------------------------------------------------------
// <copyright file="ClockPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Demo.Controls;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class ClockPageViewModel : AutoBuildPageViewModel
{
    public ClockPageViewModel()
        : base(nameof(Clock), [
            new ControlThemeBuilder()
            .AddStyles("Solid", "Outlined")
            .AddCartesianStyles("Solid", "Outlined", "Shadow")
            .AddThemeRoles()
        ])
    { }
}
