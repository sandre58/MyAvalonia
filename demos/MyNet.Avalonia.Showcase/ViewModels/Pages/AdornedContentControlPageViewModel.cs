// -----------------------------------------------------------------------
// <copyright file="AdornedContentControlPageViewModel.cs" company="Stéphane ANDRE">
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

internal sealed class AdornedContentControlPageViewModel(ICommandFactory commands) : ShowcaseViewModel(nameof(AdornedContentControl), commands, [
    new ControlThemeBuilder()
        .WithIcon(AdornedContentControl.AdornmentProperty, useByDefault: true)
        .AddEnumProperty<Position, ListBoxEditor>(
            AdornedContentControl.AdornmentPositionProperty,
            configure: x => x.DisplayName(nameof(SettingsResources.Position)))
        .AddProperty(AdornedContentControl.SpacingProperty, 5, x => x.DisplayName(nameof(SettingsResources.Spacing)).Of<SliderEditor>(editor => editor.WithRange(0, 32)))
        .AddProperty(AdornedContentControl.AdornmentOpacityProperty, 0.7, x => x.DisplayName(nameof(SettingsResources.Opacity)).Of<SliderEditor>(editor => editor.WithRange(0.0m, 1.0m).WithIncrement(0.05m)))
])
{
    public override MaterialIconKind Icon => MaterialIconKind.TextBoxOutline;
}
