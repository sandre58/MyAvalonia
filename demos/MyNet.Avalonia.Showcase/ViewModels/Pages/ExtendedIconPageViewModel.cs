// -----------------------------------------------------------------------
// <copyright file="ExtendedIconPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Material.Icons;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Showcase.ThemeBuilder;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.Avalonia.Theme.Classes.Enums;
using MyNet.UI.Commands;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class ExtendedIconPageViewModel(ICommandFactory commands) : ShowcaseViewModel(nameof(ExtendedIcon), commands, [
    new ControlThemeBuilder()
        .AddVariants(ControlVariant.Light, ControlVariant.Outlined)
        .AddVariants("kind-glyph")
        .AddAllRoles()
        .AddStandardSizes()
])
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.Shape;
}
