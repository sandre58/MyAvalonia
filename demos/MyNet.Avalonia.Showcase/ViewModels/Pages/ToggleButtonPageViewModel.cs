// -----------------------------------------------------------------------
// <copyright file="ToggleButtonPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Material.Icons;
using MyNet.Avalonia.Showcase.Extensions;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ThemeBuilder.Definitions;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.Avalonia.Theme.Classes;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class ToggleButtonPageViewModel() : ShowcaseViewModel(nameof(ToggleButton),
    [
        new ControlThemeBuilder()
                .WithContent(ContentControl.ContentProperty, ContentProviderType.Text)
                .AddShapes(CssClass.ShapeCircle)
                .AddStandardVariants()
                .AddVariant(CssClass.ShadowControl)
                .AddDefaultSizes()
                .AddDefaultRoles(),

        new ControlThemeBuilder("Rounded")
                .WithContent(ContentControl.ContentProperty, ContentProviderType.Icon)
                .AddStandardVariants()
                .AddVariant(CssClass.ShadowControl)
                .AddDefaultSizes()
                .AddDefaultRoles(),

        new ControlThemeBuilder("Icon")
                .WithContent(ContentControl.ContentProperty, ContentProviderType.Icon)
                .AddDefaultSizes()
                .AddDefaultRoles()
    ])
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.ToggleSwitchVariant;
}
