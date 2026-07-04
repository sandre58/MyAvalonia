// -----------------------------------------------------------------------
// <copyright file="MultiComboBoxPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Styling;
using Material.Icons;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Showcase.Resources;
using MyNet.Avalonia.Showcase.ThemeBuilder;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders.Editors;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.Avalonia.Theme.Classes.Enums;
using MyNet.Humanizer.Facade;
using MyNet.UI.Commands;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class MultiComboBoxPageViewModel(ICommandFactory commands) : ShowcaseViewModel(nameof(MultiComboBox), commands, [
    new ControlThemeBuilder()
        .AddVariants(ControlVariant.Solid, ControlVariant.Outlined)
        .AddThemeRoles()
        .AddDefaultSizes()
        .AddProperty(MultiComboBox.ShowSelectAllProperty, true, x => x.DisplayName(nameof(SettingsResources.ShowSelectAll)))
        .AddProperty(MultiComboBox.MaxDropDownHeightProperty, 300d, x => x.DisplayName(nameof(SettingsResources.MaxDropDownHeight)).Of<IntNumericUpDownEditor>(editor => editor.WithRange(120, 600)))
        .AddProperty(MultiComboBox.PlaceholderTextProperty, string.Empty, x => x.DisplayName(nameof(SettingsResources.ShowPlaceholderText)))
        .AddProperty(MultiComboBox.IsSearchEnabledProperty, true, x => x.DisplayName(nameof(SettingsResources.IsSearchEnabled)))
        .AddProperty(MultiComboBox.SearchFilterModeProperty, ItemsSearchFilterMode.Contains, x => x.DisplayName(nameof(SettingsResources.SearchFilterMode)).Of<ListBoxEditor>(editor =>
            editor.AddChoices(Enum.GetValues<ItemsSearchFilterMode>(), (value, y) => y.DisplayName(() => value.Humanize()))))
        .AddProperty(MultiComboBox.SearchIsCaseSensitiveProperty, false, x => x.DisplayName(nameof(SettingsResources.SearchIsCaseSensitive)).Of<ToggleSwitchEditor>())
        .AddValueAction(
            ApplySearchTextBoxTheme,
            "Bordered",
            x => x.DisplayName(nameof(SettingsResources.SearchTextBoxTheme)).Of<ListBoxEditor>(editor => editor
                .AddChoice("Bordered", builder => builder.DisplayName(nameof(SettingsResources.SearchTextBoxThemeBordered)))
                .AddChoice("Clean", builder => builder.DisplayName(nameof(SettingsResources.SearchTextBoxThemeClean))))),

    new ControlThemeBuilder("Underline")
        .AddThemeRoles()
        .AddDefaultSizes()
        .AddProperty(MultiComboBox.ShowSelectAllProperty, true, x => x.DisplayName(nameof(SettingsResources.ShowSelectAll)))
        .AddProperty(MultiComboBox.MaxDropDownHeightProperty, 300d, x => x.DisplayName(nameof(SettingsResources.MaxDropDownHeight)).Of<IntNumericUpDownEditor>(editor => editor.WithRange(120, 600)))
        .AddProperty(MultiComboBox.IsSearchEnabledProperty, true, x => x.DisplayName(nameof(SettingsResources.IsSearchEnabled)))
        .AddProperty(MultiComboBox.SearchFilterModeProperty, ItemsSearchFilterMode.Contains, x => x.DisplayName(nameof(SettingsResources.SearchFilterMode)).Of<ListBoxEditor>(editor =>
            editor.AddChoices(Enum.GetValues<ItemsSearchFilterMode>(), (value, y) => y.DisplayName(() => value.Humanize()))))
        .AddProperty(MultiComboBox.SearchIsCaseSensitiveProperty, false, x => x.DisplayName(nameof(SettingsResources.SearchIsCaseSensitive)).Of<ToggleSwitchEditor>())
        .AddValueAction(
            ApplySearchTextBoxTheme,
            "Clean",
            x => x.DisplayName(nameof(SettingsResources.SearchTextBoxTheme)).Of<ListBoxEditor>(editor => editor
                .AddChoice("Clean", builder => builder.DisplayName(nameof(SettingsResources.SearchTextBoxThemeClean)))
                .AddChoice("Bordered", builder => builder.DisplayName(nameof(SettingsResources.SearchTextBoxThemeBordered)))))
])
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.FormSelect;

    private static void ApplySearchTextBoxTheme(Control control, object? value)
    {
        if (control is not MultiComboBox multiComboBox)
            return;

        var themeKey = value as string == "Clean"
            ? "MyNet.Theme.TextBox.Embedded.Popup.Search"
            : "MyNet.Theme.TextBox.Embedded.Popup.Search.Outlined";

        if (Application.Current?.TryGetResource(themeKey, null, out var resource) == true && resource is ControlTheme theme)
            multiComboBox.SearchTextBoxTheme = theme;
    }
}
