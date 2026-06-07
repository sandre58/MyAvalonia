// -----------------------------------------------------------------------
// <copyright file="MultiComboBoxPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Material.Icons;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Showcase.Resources;
using MyNet.Avalonia.Showcase.ThemeBuilder;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders.Editors;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.Avalonia.Theme.Classes.Enums;
using MyNet.UI.Commands;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class MultiComboBoxPageViewModel(ICommandFactory commands) : ShowcaseViewModel(nameof(MultiComboBox), commands, [
    new ControlThemeBuilder()
        .AddVariants(ControlVariant.Solid, ControlVariant.Outlined)
        .AddThemeRoles()
        .AddDefaultSizes()
        .AddProperty(MultiComboBox.ShowSelectAllProperty, true, x => x.DisplayName(nameof(SettingsResources.ShowSelectAll)))
        .AddProperty(MultiComboBox.MaxDropDownHeightProperty, 300d, x => x.DisplayName(nameof(SettingsResources.MaxDropDownHeight)).Of<IntNumericUpDownEditor>(editor => editor.WithRange(120, 600)))
        .AddProperty(MultiComboBox.PlaceholderTextProperty, string.Empty, x => x.DisplayName(nameof(SettingsResources.ShowPlaceholderText))),

    new ControlThemeBuilder("Underline")
        .AddThemeRoles()
        .AddDefaultSizes()
        .AddProperty(MultiComboBox.ShowSelectAllProperty, true, x => x.DisplayName(nameof(SettingsResources.ShowSelectAll)))
        .AddProperty(MultiComboBox.MaxDropDownHeightProperty, 300d, x => x.DisplayName(nameof(SettingsResources.MaxDropDownHeight)).Of<IntNumericUpDownEditor>(editor => editor.WithRange(120, 600)))
])
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.FormSelect;
}
