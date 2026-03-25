// -----------------------------------------------------------------------
// <copyright file="MenuPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog;
using MyNet.Avalonia.Theme.Classes.Enums;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class MenuPageViewModel : ControlCatalogViewModel
{
    public MenuPageViewModel()
    : base(nameof(Menu),
        [
            new ControlThemeBuilder()
        ])
    { }

    /// <inheritdoc/>
    public override IconData Icon => IconData.Menu;
}
