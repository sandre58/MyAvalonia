// -----------------------------------------------------------------------
// <copyright file="ExtendedIconPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Material.Icons;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Showcase.Extensions;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.Avalonia.Theme.Classes.Enums;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class ExtendedIconPageViewModel() : ShowcaseViewModel(nameof(ExtendedIcon), [
    new ControlThemeBuilder()
        .AddVariants(ControlVariant.Light, ControlVariant.Outlined)
        .AddAllRoles()
        .AddStandardSizes()
])
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.Shape;
}
