// -----------------------------------------------------------------------
// <copyright file="LoaderPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Material.Icons;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Controls.Enums;
using MyNet.Avalonia.Showcase.Resources;
using MyNet.Avalonia.Showcase.ThemeBuilder;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders.Editors;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.UI.Commands;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class LoaderPageViewModel(ICommandFactory commands) : ShowcaseViewModel(nameof(Loader), commands, [
    new ControlThemeBuilder()
        .AddEnumProperty<LoaderAnimation, ListBoxEditor>(
            Loader.AnimationProperty,
            LoaderAnimation.Circular,
            x => x.DisplayName(nameof(SettingsResources.Animation)),
            configureChoice: (animation, choice) => choice.WithIcon(animation switch
            {
                LoaderAnimation.Circular => MaterialIconKind.Loading,
                LoaderAnimation.Ring => MaterialIconKind.CircleOutline,
                LoaderAnimation.Dots => MaterialIconKind.DotsHorizontal,
                LoaderAnimation.Bars => MaterialIconKind.ChartBar,
                LoaderAnimation.Pulse => MaterialIconKind.CircleOpacity,
                _ => MaterialIconKind.Loading
            }))
        .AddProperty(Loader.IsActiveProperty, true, x => x.DisplayName(nameof(SettingsResources.IsActive)).Of<ToggleSwitchEditor>())
        .AddStandardSizes()
        .AddAllRoles()
])
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.Loading;
}
