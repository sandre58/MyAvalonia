// -----------------------------------------------------------------------
// <copyright file="ExpanderPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
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

internal sealed class ExpanderPageViewModel(ICommandFactory commands) : ShowcaseViewModel(nameof(Expander), commands, [
    new ControlThemeBuilder()
        .WithContent(HeaderedContentControl.HeaderProperty, ContentProviderType.Text)
        .AddAllVariants()
        .AddVariant(CssClass.Underline)
        .AddVariant(CssClass.ShadowSurface)
        .AddVariant(CssClass.VariantHeader)
        .AddDefaultVariants()
        .AddVariant(CssClass.ShadowHeader)
        .AddDefaultRoles()
        .AddDefaultSizes()
        .AddSizes("header-xs", "header-sm", "header-md", "header-lg", "header-xl", "header-h6", "header-h5", "header-h4", "header-h3", "header-h2", "header-h1")
        .AddEnumProperty<ExpandDirection, ListBoxEditor>(Expander.ExpandDirectionProperty, ExpandDirection.Down, x => x.DisplayName(nameof(SettingsResources.Direction)), configureChoice: (x, y) => y.WithIcon(Enum.Parse<MaterialIconKind>($"Chevron{x}")), onValueChanged: (x, y) =>
        {
            switch ((ExpandDirection?)y)
            {
                case ExpandDirection.Down:
                    x.VerticalAlignment = VerticalAlignment.Top;
                    x.HorizontalAlignment = HorizontalAlignment.Center;
                    x.Width = 300;
                    x.Height = double.NaN;
                    break;

                case ExpandDirection.Up:
                    x.VerticalAlignment = VerticalAlignment.Bottom;
                    x.HorizontalAlignment = HorizontalAlignment.Center;
                    x.Width = 300;
                    x.Height = double.NaN;
                    break;

                case ExpandDirection.Left:
                    x.VerticalAlignment = VerticalAlignment.Center;
                    x.HorizontalAlignment = HorizontalAlignment.Right;
                    x.Width = double.NaN;
                    x.Height = 300;
                    break;

                case ExpandDirection.Right:
                    x.VerticalAlignment = VerticalAlignment.Center;
                    x.HorizontalAlignment = HorizontalAlignment.Left;
                    x.Width = double.NaN;
                    x.Height = 300;
                    break;
            }
        })
        .AddClass(CssClass.HeaderAlignment(nameof(Position.Left)),
            x => x.DisplayName(nameof(SettingsResources.HeaderPosition))
                .Of<ListBoxEditor>(editor => editor.AddChoice(CssClass.HeaderAlignment(nameof(Position.Left)), builder => builder.DisplayName(() => Position.Left.Humanize()).WithIcon(MaterialIconKind.GamepadCircleLeft))
                    .AddChoice(CssClass.HeaderAlignment(nameof(Position.Top)), builder => builder.DisplayName(() => Position.Top.Humanize()).WithIcon(MaterialIconKind.GamepadCircleUp))
                    .AddChoice(CssClass.HeaderAlignment(nameof(Position.Right)), builder => builder.DisplayName(() => Position.Right.Humanize()).WithIcon(MaterialIconKind.GamepadCircleRight))
                    .AddChoice(CssClass.HeaderAlignment(nameof(Position.Bottom)), builder => builder.DisplayName(() => Position.Bottom.Humanize()).WithIcon(MaterialIconKind.GamepadCircleDown))
                    .AddChoice(CssClass.HeaderAlignment($"{CssSuffix.Middle}-{nameof(HorizontalAlignment.Center)}"), builder => builder.DisplayName(() => SettingsResources.Center).WithIcon(MaterialIconKind.ImageFilterCenterFocus)))),

    new ControlThemeBuilder("Button")
        .WithContent(HeaderedContentControl.HeaderProperty, ContentProviderType.Text)
        .AddShapes(CssClass.ShapeCircle)
        .AddDefaultVariants()
        .AddVariants(CssClass.ShadowControl)
        .AddDefaultSizes()
        .AddDefaultRoles()
        .AddEnumProperty<ExpandDirection, ListBoxEditor>(Expander.ExpandDirectionProperty, ExpandDirection.Down, x => x.DisplayName(nameof(SettingsResources.Direction)), configureChoice: (x, y) => y.WithIcon(Enum.Parse<MaterialIconKind>($"Chevron{x}")))
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
    public override MaterialIconKind Icon => MaterialIconKind.ArrowExpand;
}
