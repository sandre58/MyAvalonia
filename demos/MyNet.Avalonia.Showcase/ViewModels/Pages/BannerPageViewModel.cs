// -----------------------------------------------------------------------
// <copyright file="BannerPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Material.Icons;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Controls.Enums;
using MyNet.Avalonia.Showcase.Resources;
using MyNet.Avalonia.Showcase.ThemeBuilder;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders.Editors;
using MyNet.Avalonia.Showcase.ThemeBuilder.Definitions;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.Avalonia.Theme.Classes;
using MyNet.Humanizer.Facade;
using MyNet.UI.Commands;
using MyNet.UI.Resources;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class BannerPageViewModel(ICommandFactory commands) : ShowcaseViewModel(nameof(Banner), commands, [
    new ControlThemeBuilder()
        .WithContent(HeaderedContentControl.HeaderProperty, ContentProviderType.Text)
        .AddShapes(CssClass.ShapeCircle)
        .AddStandardVariants()
        .AddVariants(CssClass.ShadowSurface)
        .AddDefaultRoles()
        .AddDefaultSizes()
        .AddProperty(Banner.CanCloseProperty, true, x => x.DisplayName(nameof(SettingsResources.ShowCloseButton)))
        .AddEnumClass<Position, ListBoxEditor>(Position.Top, x => x.DisplayName(nameof(SettingsResources.Layout)), configureChoice: (x, y) => y.WithIcon(Enum.Parse<MaterialIconKind>($"Dock{x}")))
        .AddClass(CssClass.HeaderAlignment(nameof(Position.Left)),
            x => x.DisplayName(nameof(SettingsResources.HeaderPosition))
                .Of<ListBoxEditor>(editor => editor.AddChoice(CssClass.HeaderAlignment(nameof(Position.Left)), builder => builder.DisplayName(() => Position.Left.Humanize()).WithIcon(MaterialIconKind.GamepadCircleLeft))
                    .AddChoice(CssClass.HeaderAlignment(nameof(Position.Top)), builder => builder.DisplayName(() => Position.Top.Humanize()).WithIcon(MaterialIconKind.GamepadCircleUp))
                    .AddChoice(CssClass.HeaderAlignment(nameof(Position.Right)), builder => builder.DisplayName(() => Position.Right.Humanize()).WithIcon(MaterialIconKind.GamepadCircleRight))
                    .AddChoice(CssClass.HeaderAlignment(nameof(Position.Bottom)), builder => builder.DisplayName(() => Position.Bottom.Humanize()).WithIcon(MaterialIconKind.GamepadCircleDown))
                    .AddChoice(CssClass.HeaderAlignment($"{CssSuffix.Middle}-{nameof(HorizontalAlignment.Center)}"), builder => builder.DisplayName(() => SettingsResources.Center).WithIcon(MaterialIconKind.ImageFilterCenterFocus))))
        .AddAction<Banner>(x => x.IsVisible = true, x => x.DisplayName(nameof(UiResources.Restore)).WithIcon(MaterialIconKind.Restore))
])
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.InformationBox;
}
