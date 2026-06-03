// -----------------------------------------------------------------------
// <copyright file="CarouselPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Avalonia.Layout;
using Material.Icons;
using MyNet.Avalonia.Showcase.Resources;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders.Editors;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.Avalonia.Theme.Classes;
using MyNet.Avalonia.Theme.Theming.Core;
using MyNet.Humanizer.Facade;
using MyNet.UI.Commands;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class CarouselPageViewModel(ICommandFactory commands) : ShowcaseViewModel(nameof(Carousel), commands, [
    new(),

    new ControlThemeBuilder("Full")
        .AddRoles(ThemeRole.Default, ThemeRole.Accent, ThemeRole.Contrast)
        .AddClass(CssClass.Variant("dots"),
            x => x.DisplayName(nameof(SettingsResources.Type))
                .Of<ListBoxEditor>(editor => editor.AddChoice(CssClass.Variant("dots"), builder => builder.DisplayName(nameof(SettingsResources.Dots)).WithIcon(MaterialIconKind.DotsHorizontal))
                    .AddChoice(CssClass.Variant("columnar"), builder => builder.DisplayName(nameof(SettingsResources.Columnar)).WithIcon(MaterialIconKind.ViewColumn))
                    .AddChoice(CssClass.Variant("lines"), builder => builder.DisplayName(nameof(SettingsResources.Lines)).WithIcon(MaterialIconKind.DragHorizontalVariant))))
        .AddClass(CssClass.Indicator(nameof(HorizontalAlignment.Center)),
            x => x.DisplayName(nameof(SettingsResources.Position))
                .Of<ListBoxEditor>(editor => editor.AddChoice(CssClass.Indicator(nameof(HorizontalAlignment.Left)), builder => builder.DisplayName(() => HorizontalAlignment.Left.Humanize()).WithIcon(MaterialIconKind.GamepadCircleLeft))
                    .AddChoice(CssClass.Indicator(nameof(HorizontalAlignment.Center)), builder => builder.DisplayName(() => HorizontalAlignment.Center.Humanize()).WithIcon(MaterialIconKind.ImageFilterCenterFocus))
                    .AddChoice(CssClass.Indicator(nameof(HorizontalAlignment.Right)), builder => builder.DisplayName(() => HorizontalAlignment.Right.Humanize()).WithIcon(MaterialIconKind.GamepadCircleRight))))
])
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.ViewCarousel;
}
