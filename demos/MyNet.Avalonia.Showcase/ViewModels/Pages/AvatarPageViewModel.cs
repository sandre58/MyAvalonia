// -----------------------------------------------------------------------
// <copyright file="AvatarPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Material.Icons;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Showcase.Extensions;
using MyNet.Avalonia.Showcase.Resources;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ThemeBuilder.Definitions;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.Avalonia.Theme.Classes;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class AvatarPageViewModel() : ShowcaseViewModel(nameof(Avatar), [
    new ControlThemeBuilder()
        .WithContent(ContentControl.ContentProperty, ContentProviderType.Icon)
        .AddShapes(CssClass.ShapeCircle)
        .AddStandardSizes()
        .AddStandardVariants()
        .AddVariant(CssClass.ShadowControl)
        .AddAllRoles()
        .AddStandardSizes()
        .AddProperty(Avatar.ShowBackgroundProperty, true, x => x.DisplayName(nameof(SettingsResources.ShowBackground)))
        .AddClassToggle(new("show-image"), true, x => x.DisplayName(nameof(SettingsResources.ShowImage)))
])
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.AccountBox;
}
