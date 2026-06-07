// -----------------------------------------------------------------------
// <copyright file="HyperLinkButtonPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Material.Icons;
using MyNet.Avalonia.Showcase.ThemeBuilder;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ThemeBuilder.Definitions;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.Avalonia.Theme.Classes.Enums;
using MyNet.UI.Commands;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class HyperLinkButtonPageViewModel(ICommandFactory commands) : ShowcaseViewModel(nameof(HyperlinkButton), commands, [
    new ControlThemeBuilder()
        .WithContent(ContentControl.ContentProperty, ContentProviderType.Text)
        .AddVariants(ControlVariant.Text)
        .AddDefaultSizes()
        .AddDefaultRoles()
])
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.LinkVariant;
}
