// -----------------------------------------------------------------------
// <copyright file="EmptyStatePageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Material.Icons;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Controls.Primitives;
using MyNet.Avalonia.Showcase.Resources;
using MyNet.Avalonia.Showcase.ThemeBuilder;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders.Editors;
using MyNet.Avalonia.Showcase.ThemeBuilder.Definitions;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.Avalonia.Theme.Classes;
using MyNet.UI.Commands;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class EmptyStatePageViewModel(ICommandFactory commands) : ShowcaseViewModel(nameof(EmptyState), commands, [
    new ControlThemeBuilder()
        .WithContent(EmptyState.TitleProperty, ContentProviderType.Text)
        .WithIcon(RegionControl.LeadingProperty, true)
        .AddDefaultSizes()
        .AddProperty(EmptyState.SubtitleProperty, configure: x => x.DisplayName(nameof(CardPageResources.Subtitle)).Of<TextBoxEditor>(y => y.WithValue("Add your first item to get started.")))
])
{
    public override MaterialIconKind Icon => MaterialIconKind.InboxOutline;
}
