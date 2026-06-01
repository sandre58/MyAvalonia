// -----------------------------------------------------------------------
// <copyright file="ToggleSwitchPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Material.Icons;
using MyNet.Avalonia.Showcase.Extensions;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ThemeBuilder.Definitions;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.Avalonia.Theme.Classes;
using MyNet.UI.Commands;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class ToggleSwitchPageViewModel(ICommandFactory commands) : ShowcaseViewModel(nameof(ToggleSwitch), commands, [
    new ControlThemeBuilder()
            .AddDefaultSizes()
            .AddDefaultRoles(),

        new ControlThemeBuilder("Inner")
                .WithContent(ContentControl.ContentProperty, ContentProviderType.Icon)
                .AddDefaultSizes()
                .AddDefaultRoles(),

        new ControlThemeBuilder("Alternate")
                .AddDefaultRoles(),

        new ControlThemeBuilder("Button")
                .AddShapes(CssClass.ShapeCircle)
                .AddStandardVariants()
                .AddDefaultSizes()
                .AddDefaultRoles(),

        new ControlThemeBuilder("Button.Rounded")
                .WithContent(ContentControl.ContentProperty, ContentProviderType.Icon)
                .AddStandardVariants()
                .AddDefaultSizes()
                .AddDefaultRoles(),

        new ControlThemeBuilder("Button.Icon")
                .WithContent(ContentControl.ContentProperty, ContentProviderType.Icon)
                .AddDefaultSizes()
                .AddDefaultRoles()
    ])
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.ToggleSwitch;
}
