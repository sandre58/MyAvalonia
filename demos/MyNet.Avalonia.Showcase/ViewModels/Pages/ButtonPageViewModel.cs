// -----------------------------------------------------------------------
// <copyright file="ButtonPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Material.Icons;
using MyNet.Avalonia.Showcase.ThemeBuilder;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.Avalonia.Theme.Classes;
using MyNet.UI.Commands;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class ButtonPageViewModel(ICommandFactory commands) : ShowcaseViewModel(nameof(Button), commands, [
    ThemeProfiles.TextButton()
        .AddVariants(CssClass.ShadowControl),

    ThemeProfiles.RoundedIconButton(),

    ThemeProfiles.IconButton("Icon")
])
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.ButtonPointer;
}
