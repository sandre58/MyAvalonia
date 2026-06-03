// -----------------------------------------------------------------------
// <copyright file="ToggleButtonPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls.Primitives;
using Material.Icons;
using MyNet.Avalonia.Showcase.ThemeBuilder;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.UI.Commands;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class ToggleButtonPageViewModel(ICommandFactory commands) : ShowcaseViewModel(nameof(ToggleButton), commands, [
    ThemeProfiles.StandardTextButton(),

    ThemeProfiles.RoundedIconButton(),

    ThemeProfiles.IconButton("Icon")
])
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.ToggleSwitchVariant;
}
