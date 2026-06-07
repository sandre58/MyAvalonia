// -----------------------------------------------------------------------
// <copyright file="BadgePageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls.Primitives;
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
using MyNet.UI.Commands;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class BadgePageViewModel(ICommandFactory commands) : ShowcaseViewModel(nameof(Badge), commands, [
    new ControlThemeBuilder()
        .WithContent(HeaderedContentControl.HeaderProperty, ContentProviderType.Text)
        .AddShapes(CssClass.ShapeCircle)
        .AddAllVariants()
        .AddVariant(CssClass.ShadowControl)
        .AddAllRoles()
        .AddDefaultSizes()
        .AddEnumProperty<CornerPosition, ListBoxEditor>(Badge.CornerPositionProperty, CornerPosition.TopRight, x => x.DisplayName(nameof(SettingsResources.Position)), configureChoice: (x, y) => y.WithIcon(Enum.Parse<MaterialIconKind>($"Arrow{x}")))
        .AddProperty(Badge.OffsetXProperty, 0, x => x.DisplayName(nameof(SettingsResources.OffsetX)).Of<SliderEditor>(editor => editor.WithRange(-10, 10)))
        .AddProperty(Badge.OffsetYProperty, 0, x => x.DisplayName(nameof(SettingsResources.OffsetY)).Of<SliderEditor>(editor => editor.WithRange(-10, 10)))
        .AddProperty(Badge.IsRoundedProperty, false, x => x.DisplayName(nameof(SettingsResources.IsRounded)))
])
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.CheckboxBlankBadge;
}
