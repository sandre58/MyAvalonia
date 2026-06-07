// -----------------------------------------------------------------------
// <copyright file="ProgressBarPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using Avalonia.Layout;
using Material.Icons;
using MyNet.Avalonia.Controls.Enums;
using MyNet.Avalonia.Showcase.Resources;
using MyNet.Avalonia.Showcase.ThemeBuilder;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders.Editors;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.Avalonia.Theme.Classes;
using MyNet.Avalonia.Theme.Classes.Enums;
using MyNet.Humanizer.Facade;
using MyNet.UI.Commands;
using MyNet.UI.Resources;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class ProgressBarPageViewModel(ICommandFactory commands) : ShowcaseViewModel(nameof(ProgressBar), commands, [
    new ControlThemeBuilder()
        .AddVariants(ControlVariant.Solid, ControlVariant.Light, ControlVariant.Outlined)
        .AddVariant(CssClass.ShadowControl)
        .AddDefaultSizes()
        .AddDefaultRoles()
        .AddEnumProperty<Orientation, ListBoxEditor>(
            ProgressBar.OrientationProperty,
            Orientation.Horizontal,
            x => x.DisplayName(nameof(SettingsResources.Orientation)),
            configureChoice: (x, y) => y.WithIcon(Enum.Parse<MaterialIconKind>($"Border{x}")))
        .AddProperty(global::Avalonia.Controls.Primitives.RangeBase.ValueProperty, 0, x => x.DisplayName(nameof(SettingsResources.Value)))
        .AddProperty(ProgressBar.IsIndeterminateProperty, false, x => x.DisplayName(nameof(SettingsResources.IsIndeterminate)))
        .AddClass(new("none"),
            x => x.DisplayName(nameof(SettingsResources.ValuePosition))
                .Of<ListBoxEditor>(editor => editor.AddChoice(new CssClass("none"), builder => builder.DisplayName(UiResources.None).WithIcon(MaterialIconKind.CircleOffOutline))
                    .AddChoice(CssClass.Position(nameof(Position.Left)), builder => builder.DisplayName(() => Position.Left.Humanize()).WithIcon(MaterialIconKind.GamepadCircleLeft))
                    .AddChoice(CssClass.Position(nameof(HorizontalAlignment.Center)), builder => builder.DisplayName(() => SettingsResources.Center).WithIcon(MaterialIconKind.ImageFilterCenterFocus))
                    .AddChoice(CssClass.Position(nameof(Position.Right)), builder => builder.DisplayName(() => Position.Right.Humanize()).WithIcon(MaterialIconKind.GamepadCircleRight))),
            onValueChanged: (x, y) => ((ProgressBar)x).ShowProgressText = y?.ToString() != "none"),

    new ControlThemeBuilder("Circular")
        .AddVariants(ControlVariant.Light)
        .AddVariant(CssClass.ShadowControl)
        .AddDefaultSizes()
        .AddDefaultRoles()
        .AddProperty(global::Avalonia.Controls.Primitives.RangeBase.ValueProperty, 0, x => x.DisplayName(nameof(SettingsResources.Value)))
        .AddProperty(ProgressBar.IsIndeterminateProperty, false, x => x.DisplayName(nameof(SettingsResources.IsIndeterminate)))
        .AddProperty(ProgressBar.ShowProgressTextProperty, false, x => x.DisplayName(nameof(SettingsResources.ShowValue)))
])
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.ProgressCheck;
}
