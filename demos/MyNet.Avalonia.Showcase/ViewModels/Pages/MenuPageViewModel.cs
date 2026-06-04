// -----------------------------------------------------------------------
// <copyright file="MenuPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Material.Icons;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.UI.Commands;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class MenuPageViewModel(ICommandFactory commands) : ShowcaseViewModel(nameof(Menu), commands, [
    new()
])
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.Menu;
}
