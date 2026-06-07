// -----------------------------------------------------------------------
// <copyright file="IconContentControlPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Material.Icons;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Controls.Enums;
using MyNet.Avalonia.Showcase.Resources;
using MyNet.Avalonia.Showcase.ThemeBuilder;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders.Editors;
using MyNet.Avalonia.Showcase.ThemeBuilder.Definitions;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.UI.Commands;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class IconContentControlPageViewModel(ICommandFactory commands) : ShowcaseViewModel(nameof(IconContentControl), commands, [
    new ControlThemeBuilder()
        .WithContent(ContentControl.ContentProperty, ContentProviderType.Text)
        .WithIcon(IconContentControl.IconProperty, useByDefault: true)
        .AddEnumProperty<Position, ListBoxEditor>(
            IconContentControl.IconPositionProperty,
            Position.Left,
            x => x.DisplayName(nameof(SettingsResources.Position)),
            configureChoice: (position, choice) => choice.WithIcon(position switch
            {
                Position.Left => MaterialIconKind.ChevronLeft,
                Position.Right => MaterialIconKind.ChevronRight,
                Position.Top => MaterialIconKind.ChevronUp,
                Position.Bottom => MaterialIconKind.ChevronDown,
                _ => MaterialIconKind.Help
            }))
        .AddProperty(IconContentControl.SpacingProperty, 5, x => x.DisplayName(nameof(SettingsResources.Spacing)).Of<SliderEditor>(editor => editor.WithRange(0, 32)))
        .AddProperty(IconContentControl.IconOpacityProperty, 0.7, x => x.DisplayName(nameof(SettingsResources.Opacity)).Of<SliderEditor>(editor => editor.WithRange(0.0m, 1.0m).WithIncrement(0.05m)))
])
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.TextBoxOutline;
}
