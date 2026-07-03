// -----------------------------------------------------------------------
// <copyright file="RatingPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Layout;
using Material.Icons;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Controls.Enums;
using MyNet.Avalonia.Showcase.Resources;
using MyNet.Avalonia.Showcase.ThemeBuilder;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders.Editors;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.Avalonia.Theme.Assists;
using MyNet.UI.Commands;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class RatingPageViewModel(ICommandFactory commands) : ShowcaseViewModel(nameof(Rating), commands, [
    new ControlThemeBuilder()
        .AddItemsAllRoles()
        .AddStandardSizes()
        .AddProperty(Rating.MaxRatingProperty, 5, x => x.DisplayName(nameof(SettingsResources.Maximum)).Of<IntNumericUpDownEditor>(editor => editor.WithRange(1, 10)))
        .AddEnumProperty<RatingPrecision, ComboBoxEditor>(Rating.PrecisionProperty, RatingPrecision.Integer, x => x.DisplayName(nameof(SettingsResources.Precision)))
        .AddProperty(Rating.IsReadOnlyProperty, false, x => x.DisplayName(nameof(SettingsResources.IsReadOnly)))
        .AddProperty(Rating.IsClearableProperty, true, x => x.DisplayName(nameof(SettingsResources.IsClearable)))
        .AddProperty(Rating.ClearOnReselectProperty, false, x => x.DisplayName(nameof(SettingsResources.ClearOnReselect)))
        .AddEnumProperty<Orientation, ListBoxEditor>(
            Rating.OrientationProperty,
            Orientation.Horizontal,
            x => x.DisplayName(nameof(SettingsResources.Orientation)),
            configureChoice: (x, y) => y.WithIcon(Enum.Parse<MaterialIconKind>($"Border{x}")))
        .AddProperty(ItemsAssist.SpacingProperty, 4d, x => x.DisplayName(nameof(SettingsResources.Spacing)).Of<SliderEditor>(editor => editor.WithRange(0, 24)))
        .AddProperty(Rating.ItemSizeProperty, 24d, x => x.DisplayName(nameof(SettingsResources.SymbolSize)).Of<SliderEditor>(editor => editor.WithRange(12, 48)))
])
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.Star;
}
