// -----------------------------------------------------------------------
// <copyright file="HyperLinkButtonPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Material.Icons;
using MyNet.Avalonia.Showcase.Extensions;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ThemeBuilder.Definitions;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.Avalonia.Theme.Classes.Enums;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class HyperLinkButtonPageViewModel() : ShowcaseViewModel(nameof(HyperlinkButton),
[
    new ControlThemeBuilder()
        .WithContent(ContentControl.ContentProperty, ContentProviderType.Text)
        .AddVariants(ControlVariant.Text)
        .AddDefaultRoles()
])
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.LinkVariant;
}
