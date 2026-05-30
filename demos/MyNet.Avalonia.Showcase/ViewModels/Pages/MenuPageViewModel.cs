// -----------------------------------------------------------------------
// <copyright file="MenuPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Material.Icons;
using MyNet.Avalonia.Showcase.ViewModels.Playground;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class MenuPageViewModel() : ShowcaseViewModel(nameof(Menu),
[
    new()
])
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.Menu;
}
