// -----------------------------------------------------------------------
// <copyright file="GridSplitterPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Material.Icons;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.UI.Commands;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class GridSplitterPageViewModel(ICommandFactory commands) : ShowcaseViewModel(nameof(GridSplitter), commands, [
    new()
])
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.ArrowSplitVertical;
}
