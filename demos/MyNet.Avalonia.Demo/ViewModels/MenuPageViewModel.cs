// -----------------------------------------------------------------------
// <copyright file="MenuPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class MenuPageViewModel : ControlCatalogViewModel
{
    public MenuPageViewModel()
    : base(nameof(Menu),
        [
            new ControlThemeBuilder()
        ])
    { }
}
