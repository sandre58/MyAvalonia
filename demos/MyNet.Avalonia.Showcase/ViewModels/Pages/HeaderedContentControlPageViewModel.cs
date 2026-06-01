// -----------------------------------------------------------------------
// <copyright file="HeaderedContentControlPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Material.Icons;
using MyNet.Avalonia.Controls.Enums;
using MyNet.Avalonia.Showcase.Extensions;
using MyNet.Avalonia.Showcase.Resources;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders.Editors;
using MyNet.Avalonia.Showcase.ThemeBuilder.Definitions;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.Avalonia.Theme.Classes;
using MyNet.UI.Commands;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class HeaderedContentControlPageViewModel(ICommandFactory commands) : ShowcaseViewModel(nameof(HeaderedContentControl), commands, [
    new ControlThemeBuilder()
        .WithContent(HeaderedContentControl.HeaderProperty, ContentProviderType.Text)
        .AddAllVariants()
        .AddVariant(CssClass.Underline)
        .AddVariant(CssClass.ShadowSurface)
        .AddVariant(CssClass.VariantHeader)
        .AddDefaultVariants()
        .AddVariant(CssClass.ShadowHeader)
        .AddAllRoles()
        .AddDefaultSizes()
        .AddSizes("header-xs", "header-sm", "header-md", "header-lg", "header-xl", "header-h6", "header-h5", "header-h4", "header-h3", "header-h2", "header-h1")
        .AddEnumClass<Position, ListBoxEditor>(Position.Top, x => x.DisplayName(nameof(SettingsResources.Layout)), configureChoice: (x, y) => y.WithIcon(Enum.Parse<MaterialIconKind>($"Dock{x}")))
        .AddClass(CssClass.HeaderAlignment(nameof(Position.Left)),
            x => x.DisplayName(nameof(SettingsResources.HeaderPosition))
                .Of<ListBoxEditor>(editor => editor.AddChoice(CssClass.HeaderAlignment(nameof(Position.Left)), builder => builder.DisplayName(() => Position.Left.Humanize()).WithIcon(MaterialIconKind.GamepadCircleLeft))
                    .AddChoice(CssClass.HeaderAlignment(nameof(Position.Top)), builder => builder.DisplayName(() => Position.Top.Humanize()).WithIcon(MaterialIconKind.GamepadCircleUp))
                    .AddChoice(CssClass.HeaderAlignment(nameof(Position.Right)), builder => builder.DisplayName(() => Position.Right.Humanize()).WithIcon(MaterialIconKind.GamepadCircleRight))
                    .AddChoice(CssClass.HeaderAlignment(nameof(Position.Bottom)), builder => builder.DisplayName(() => Position.Bottom.Humanize()).WithIcon(MaterialIconKind.GamepadCircleDown))
                    .AddChoice(CssClass.HeaderAlignment($"{CssSuffix.Middle}-{nameof(HorizontalAlignment.Center)}"), builder => builder.DisplayName(() => SettingsResources.Center).WithIcon(MaterialIconKind.ImageFilterCenterFocus)))),

    new ControlThemeBuilder()
        .WithContent(HeaderedContentControl.HeaderProperty, ContentProviderType.Text)
        .WithKind("label")
        .AddVariant(CssClass.Watermark)
        .AddDefaultSizes()
        .AddSizes("header-xs", "header-sm", "header-md", "header-lg", "header-xl", "header-h6", "header-h5", "header-h4", "header-h3", "header-h2", "header-h1")
        .AddEnumClass<Position, ListBoxEditor>(Position.Top, x => x.DisplayName(nameof(SettingsResources.Layout)), configureChoice: (x, y) => y.WithIcon(Enum.Parse<MaterialIconKind>($"Dock{x}")))
        .AddClass(CssClass.HeaderAlignment(nameof(Position.Left)),
            x => x.DisplayName(nameof(SettingsResources.HeaderPosition))
                .Of<ListBoxEditor>(editor => editor.AddChoice(CssClass.HeaderAlignment(nameof(Position.Left)), builder => builder.DisplayName(() => Position.Left.Humanize()).WithIcon(MaterialIconKind.GamepadCircleLeft))
                    .AddChoice(CssClass.HeaderAlignment(nameof(Position.Top)), builder => builder.DisplayName(() => Position.Top.Humanize()).WithIcon(MaterialIconKind.GamepadCircleUp))
                    .AddChoice(CssClass.HeaderAlignment(nameof(Position.Right)), builder => builder.DisplayName(() => Position.Right.Humanize()).WithIcon(MaterialIconKind.GamepadCircleRight))
                    .AddChoice(CssClass.HeaderAlignment(nameof(Position.Bottom)), builder => builder.DisplayName(() => Position.Bottom.Humanize()).WithIcon(MaterialIconKind.GamepadCircleDown))
                    .AddChoice(CssClass.HeaderAlignment($"{CssSuffix.Middle}-{nameof(HorizontalAlignment.Center)}"), builder => builder.DisplayName(() => SettingsResources.Center).WithIcon(MaterialIconKind.ImageFilterCenterFocus))))
])
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.CardBulleted;
}
