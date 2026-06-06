// -----------------------------------------------------------------------
// <copyright file="TagBoxPageViewModel.cs" company="Stéphane ANDRE">
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
using MyNet.Avalonia.Theme.Classes;
using MyNet.Avalonia.Theme.Classes.Enums;
using MyNet.UI.Commands;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class TagBoxPageViewModel(ICommandFactory commands) : ShowcaseViewModel(nameof(TagBox), commands, [
    new ControlThemeBuilder()
        .AddVariants(ControlVariant.Solid, ControlVariant.Outlined)
        .AddThemeRoles()
        .AddDefaultSizes()
        .AddProperty(TagBox.MaxCountProperty, int.MaxValue, x => x.DisplayName(nameof(SettingsResources.MaxCount)).Of<IntNumericUpDownEditor>(editor => editor.WithRange(1, 20)))
        .AddProperty(TagBox.AllowDuplicatesProperty, true, x => x.DisplayName(nameof(SettingsResources.AllowDuplicates)))
        .AddProperty(TagBox.SeparatorProperty, string.Empty, x => x.DisplayName(nameof(SettingsResources.Separator)))
        .AddProperty(TagBox.PlaceholderTextProperty, string.Empty, x => x.DisplayName(nameof(SettingsResources.ShowPlaceholderText)))
        .AddEnumProperty<LostFocusBehavior, ListBoxEditor>(TagBox.LostFocusBehaviorProperty, LostFocusBehavior.Add, x => x.DisplayName(nameof(SettingsResources.LostFocusBehavior))),

    new ControlThemeBuilder("Underline")
        .AddThemeRoles()
        .AddDefaultSizes()
        .AddProperty(TagBox.MaxCountProperty, int.MaxValue, x => x.DisplayName(nameof(SettingsResources.MaxCount)).Of<IntNumericUpDownEditor>(editor => editor.WithRange(1, 20)))
        .AddProperty(TagBox.AllowDuplicatesProperty, true, x => x.DisplayName(nameof(SettingsResources.AllowDuplicates)))
        .AddEnumProperty<LostFocusBehavior, ListBoxEditor>(TagBox.LostFocusBehaviorProperty, LostFocusBehavior.Add, x => x.DisplayName(nameof(SettingsResources.LostFocusBehavior)))
])
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.TagMultiple;
}
