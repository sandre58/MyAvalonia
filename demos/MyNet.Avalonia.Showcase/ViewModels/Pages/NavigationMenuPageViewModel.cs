// -----------------------------------------------------------------------
// <copyright file="NavigationMenuPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Material.Icons;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Showcase.Extensions;
using MyNet.Avalonia.Showcase.Resources;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders.Editors;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.UI.Commands;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class NavigationMenuPageViewModel(ICommandFactory commands) : ShowcaseViewModel(nameof(NavigationMenu), commands, [
    new ControlThemeBuilder()
        .AddDefaultRoles()
        .AddProperty(NavigationMenu.CollapseWidthProperty, 50, x => x.DisplayName(nameof(SettingsResources.CollapseWidth)).Of<SliderEditor>(editor => editor.WithRange(40, 50)))
        .AddProperty(NavigationMenu.ExpandWidthProperty, 300, x => x.DisplayName(nameof(SettingsResources.ExpandWidth)).Of<SliderEditor>(editor => editor.WithRange(200, 300)))
        .AddProperty(NavigationMenu.SubMenuIndentProperty, 24, x => x.DisplayName(nameof(SettingsResources.SubMenuIndent)).Of<SliderEditor>(editor => editor.WithRange(0, 24)))
])
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.Navigation;
}
