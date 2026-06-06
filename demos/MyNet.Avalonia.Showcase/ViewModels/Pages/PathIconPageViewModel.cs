// -----------------------------------------------------------------------
// <copyright file="PathIconPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Material.Icons;
using MyNet.Avalonia.Showcase.Extensions;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.UI.Commands;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class PathIconPageViewModel(ICommandFactory commands) : ShowcaseViewModel("PathIcon", commands, [
    new ControlThemeBuilder()
        .AddStandardSizes()
        .AddVariants("kind-glyph")
])
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.VectorSquare;
}
