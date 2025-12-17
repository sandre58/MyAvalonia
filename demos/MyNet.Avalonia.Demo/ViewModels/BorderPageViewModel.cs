// -----------------------------------------------------------------------
// <copyright file="BorderPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using MyNet.Avalonia.Demo.Controls;

namespace MyNet.Avalonia.Demo.ViewModels;

internal class BorderPageViewModel : ControlPageViewModelBase
{
    public BorderPageViewModel()
        : base(nameof(Border), [
            new ControlThemeBuilder("Card")
            .AddAllRoles()
            .AddStyles("Light", "Outlined")
            .AddCartesianStyles("Outlined", "Light")
            .AddCartesianStyles("Outlined", "Solid")
            .AddCartesianStyles("Solid", "Shadow Hover")
            .AddCartesianStyles("Outlined", "Shadow Hover")
        ])
    { }
}
