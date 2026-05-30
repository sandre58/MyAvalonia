// -----------------------------------------------------------------------
// <copyright file="SliderPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using Avalonia.Layout;
using Material.Icons;
using MyNet.Avalonia.Controls.Enums;
using MyNet.Avalonia.Showcase.Extensions;
using MyNet.Avalonia.Showcase.Resources;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders.Editors;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.Avalonia.Theme.Assists;
using MyNet.Avalonia.Theme.Classes;
using MyNet.Avalonia.Theme.Classes.Enums;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class SliderPageViewModel() : ShowcaseViewModel(nameof(Slider),
[
    new ControlThemeBuilder()
        .AddVariants(ControlVariant.Solid, ControlVariant.Light, ControlVariant.Outlined)
        .AddVariant(CssClass.ShadowControl)
        .AddDefaultRoles()
        .AddEnumProperty<Orientation, ListBoxEditor>(
            Slider.OrientationProperty,
            Orientation.Horizontal,
            x => x.DisplayName(nameof(SettingsResources.Orientation)),
            configureChoice: (x, y) => y.WithIcon(Enum.Parse<MaterialIconKind>($"Border{x}")))
        .AddProperty(Slider.IsDirectionReversedProperty, false, x => x.DisplayName(nameof(SettingsResources.IsDirectionReversed)))
        .AddEnumProperty<TickPlacement, ComboBoxEditor>(Slider.TickPlacementProperty, TickPlacement.None, x => x.DisplayName(nameof(SettingsResources.TickPlacement)))
        .AddEnumProperty<TickMode, ComboBoxEditor>(SliderAssist.TickModeProperty, TickMode.Tick, x => x.DisplayName(nameof(SettingsResources.TickMode)))
        .AddProperty(Slider.TickFrequencyProperty, 20, x => x.DisplayName(nameof(SettingsResources.TickFrequency)).Of<IntNumericUpDownEditor>(editor => editor.WithRange(0, 100)))
        .AddProperty(SliderAssist.TickLengthProperty, 5, x => x.DisplayName(nameof(SettingsResources.TickLength)).Of<IntNumericUpDownEditor>(editor => editor.WithRange(1, 20)))
        .AddProperty(Slider.IsSnapToTickEnabledProperty, false, x => x.DisplayName(nameof(SettingsResources.IsSnapToTickEnabled)))
        .AddProperty(global::Avalonia.Controls.Primitives.RangeBase.MinimumProperty, 0, x => x.DisplayName(nameof(SettingsResources.Minimum)).Of<IntNumericUpDownEditor>(editor => editor.WithRange(-1000, 1000)))
        .AddProperty(global::Avalonia.Controls.Primitives.RangeBase.MaximumProperty, 100, x => x.DisplayName(nameof(SettingsResources.Maximum)).Of<IntNumericUpDownEditor>(editor => editor.WithRange(-1000, 1000)))
        .AddProperty(SliderAssist.ShowValueOnMouseOverProperty, true, x => x.DisplayName(nameof(SettingsResources.ShowValueOnMouseOver)))
])
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.TuneVariant;
}
