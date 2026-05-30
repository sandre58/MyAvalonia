// -----------------------------------------------------------------------
// <copyright file="BorderPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Material.Icons;
using MyNet.Avalonia.Showcase.Extensions;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.Avalonia.Theme.Classes;
using MyNet.Avalonia.Theme.Classes.Enums;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class BorderPageViewModel() : ShowcaseViewModel(nameof(Border),
[
    new ControlThemeBuilder()
        .WithKind(CssClass.KindCard)
        .AddVariants(ControlVariant.Light, ControlVariant.Outlined, ControlVariant.Solid)
        .AddVariant(CssClass.ShadowSurface)
        .AddVariant(new("is-hover"))
        .AddAllRoles()
])
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.CardOutline;
}
