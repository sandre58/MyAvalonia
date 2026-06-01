// -----------------------------------------------------------------------
// <copyright file="ButtonPageViewModel.cs" company="Stéphane ANDRE">
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
using MyNet.UI.Commands;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class ButtonPageViewModel(ICommandFactory commands) : ShowcaseViewModel(nameof(Button), commands, [
    new ControlThemeBuilder()
        .WithContent(ContentControl.ContentProperty, ContentProviderType.Text)
        .AddShapes(CssClass.ShapeCircle)
        .AddDefaultVariants()
        .AddVariants(CssClass.ShadowControl)
        .AddDefaultSizes()
        .AddDefaultRoles(),

    new ControlThemeBuilder("Rounded")
        .WithContent(ContentControl.ContentProperty, ContentProviderType.Icon)
        .AddDefaultVariants()
        .AddVariants(CssClass.ShadowControl)
        .AddDefaultSizes()
        .AddDefaultRoles(),

    new ControlThemeBuilder("Icon")
        .WithContent(ContentControl.ContentProperty, ContentProviderType.Icon)
        .AddDefaultSizes()
        .AddDefaultRoles()
])
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.ButtonPointer;
}
