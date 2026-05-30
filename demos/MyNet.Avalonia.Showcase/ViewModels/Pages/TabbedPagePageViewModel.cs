// -----------------------------------------------------------------------
// <copyright file="TabbedPagePageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using Material.Icons;
using MyNet.Avalonia.Showcase.Extensions;
using MyNet.Avalonia.Showcase.Resources;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders.Editors;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.Avalonia.Theme.Classes.Enums;
using MyNet.Avalonia.Theme.Helpers;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class TabbedPagePageViewModel() : ShowcaseViewModel(nameof(TabbedPage),
[
    new ControlThemeBuilder()
        .AddEnumProperty<TabPlacement, ListBoxEditor>(TabbedPage.TabPlacementProperty, TabPlacement.Top, x => x.DisplayName(nameof(SettingsResources.Layout)), configureChoice: (x, y) => _ = x == TabPlacement.Auto ? y.WithIcon(MaterialIconKind.AutoFix) : y.WithIcon(Enum.Parse<MaterialIconKind>($"Dock{x}")))
        .AddEnumValue<PageTransitionType, ComboBoxEditor>((x, y) => (x as TabbedPage)?.SetValue(TabbedPage.PageTransitionProperty, TransitionsHelper.CreatePageTransition(y.GetValueOrDefault())),
            PageTransitionType.None,
            x => x.DisplayName(SettingsResources.Transition))
])
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.Tab;
}
