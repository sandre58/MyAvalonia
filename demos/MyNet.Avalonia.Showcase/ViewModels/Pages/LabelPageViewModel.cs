// -----------------------------------------------------------------------
// <copyright file="LabelPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Material.Icons;
using MyNet.Avalonia.Showcase.Extensions;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ThemeBuilder.Definitions;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.Avalonia.Theme.Classes;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class LabelPageViewModel() : ShowcaseViewModel(nameof(Label),
[
    new ControlThemeBuilder()
        .WithContent(ContentControl.ContentProperty, ContentProviderType.Text)
        .AddVariants("opacity-high", "opacity-medium", "opacity-low", "text-helper", "text-watermark")
        .AddAllRoles()
        .AddSizes("font-xs", "font-sm", "font-md", "font-lg", "font-xl", "font-h6", "font-h5", "font-h4", "font-h3", "font-h2", "font-h1"),

    new ControlThemeBuilder()
        .WithKind("badge")
        .WithContent(ContentControl.ContentProperty, ContentProviderType.Text)
        .AddShapes(CssClass.ShapeCircle)
        .AddVariants("variant-light", "variant-outlined", "variant-text", "shadow-control")
        .AddAllRoles()
        .AddDefaultRoles(),

    new ControlThemeBuilder()
        .WithKind("code")
        .WithContent(ContentControl.ContentProperty, ContentProviderType.Text)
])
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.TagText;
}
