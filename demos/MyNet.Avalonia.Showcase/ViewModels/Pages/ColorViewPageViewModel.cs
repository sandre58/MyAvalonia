// -----------------------------------------------------------------------
// <copyright file="ColorViewPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using Material.Icons;
using MyNet.Avalonia.Controls.ColorPalettes;
using MyNet.Avalonia.Showcase.Extensions;
using MyNet.Avalonia.Showcase.Resources;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders.Editors;
using MyNet.Avalonia.Showcase.ViewModels.Playground;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class ColorViewPageViewModel() : ShowcaseViewModel(nameof(ColorView),
[
    new ControlThemeBuilder()
        .AddThemeRoles()
        .AddValueAction((x, y) =>
            {
                if (y is not IEnumerable values)
                    return;

                var result = values.OfType<string>().ToList();
                x.SetCurrentValue(ColorView.IsColorSpectrumVisibleProperty, result.Contains("spectrum"));
                x.SetCurrentValue(ColorView.IsColorPaletteVisibleProperty, result.Contains("palette"));
                x.SetCurrentValue(ColorView.IsColorComponentsVisibleProperty, result.Contains("components"));
            },
            new ObservableCollection<string> { "spectrum", "palette", "components" },
            x => x.DisplayName(nameof(SettingsResources.Tabs))
                .Of<ListBoxEditor>(editor => editor.AddChoice("spectrum", builder => builder.DisplayName(nameof(SettingsResources.Spectrum)).WithIcon(MaterialIconKind.Palette))
                    .AddChoice("palette", builder => builder.DisplayName(nameof(SettingsResources.Palette)).WithIcon(MaterialIconKind.PaletteSwatchVariant))
                    .AddChoice("components", builder => builder.DisplayName(nameof(SettingsResources.Components)).WithIcon(MaterialIconKind.TuneVariant))
                    .AllowMultipleSelection()))
        .AddProperty(ColorView.IsColorPreviewVisibleProperty, true, x => x.DisplayName(nameof(SettingsResources.IsColorPreviewVisible)))
        .AddProperty(ColorView.IsAccentColorsVisibleProperty, true, x => x.DisplayName(nameof(SettingsResources.IsAccentColorsVisible)))
        .AddEnumProperty<ColorSpectrumShape, ListBoxEditor>(ColorView.ColorSpectrumShapeProperty,
            ColorSpectrumShape.Box,
            x => x.DisplayName(nameof(SettingsResources.Shape)),
            configureChoice: (x, y) =>
            {
                switch (x)
                {
                    case ColorSpectrumShape.Box:
                        y.WithIcon(MaterialIconKind.CheckboxBlank);
                        break;
                    case ColorSpectrumShape.Ring:
                        y.WithIcon(MaterialIconKind.CheckboxBlankCircle);
                        break;
                }
            })
        .AddProperty(ColorView.IsColorSpectrumSliderVisibleProperty, true, x => x.DisplayName(nameof(SettingsResources.IsColorSpectrumSliderVisible)))
        .AddProperty(ColorView.IsAlphaVisibleProperty, true, x => x.DisplayName(nameof(SettingsResources.IsAlphaVisible)))
        .AddProperty(ColorView.PaletteProperty, Palettes[7], x => x.DisplayName(nameof(SettingsResources.Palette))
            .Of<ComboBoxEditor>(editor => editor.AddChoices(Palettes, (palette, y) => y.DisplayName(palette.GetType().Name))))
        .AddProperty(ColorView.PaletteColumnCountProperty, 16, x => x.DisplayName(nameof(SettingsResources.PaletteColumnCount)).Of<IntNumericUpDownEditor>(editor => editor.WithRange(1, 30)))
        .AddEnumProperty<ColorModel, ListBoxEditor>(ColorView.ColorModelProperty, ColorModel.Rgba, x => x.DisplayName(nameof(SettingsResources.Model)))
        .AddProperty(ColorView.IsHexInputVisibleProperty, true, x => x.DisplayName(nameof(SettingsResources.IsHexInputVisible)))
        .AddValueAction((x, y) =>
            {
                if (y is not IEnumerable values)
                    return;

                var result = values.OfType<string>().ToList();
                x.SetCurrentValue(ColorView.IsComponentSliderVisibleProperty, result.Contains("slider"));
                x.SetCurrentValue(ColorView.IsComponentTextInputVisibleProperty, result.Contains("numeric"));
            },
            new ObservableCollection<string> { "slider", "numeric" },
            x => x.DisplayName(nameof(SettingsResources.Components))
                .Of<ListBoxEditor>(editor => editor.AddChoice("slider", builder => builder.DisplayName(nameof(SettingsResources.Slider)))
                    .AddChoice("numeric", builder => builder.DisplayName(nameof(SettingsResources.Numeric)))
                    .AllowMultipleSelection())),

    new ControlThemeBuilder("Simple")
        .AddThemeRoles()
        .AddProperty(ColorView.IsColorPreviewVisibleProperty, true, x => x.DisplayName(nameof(SettingsResources.IsColorPreviewVisible)))
        .AddProperty(ColorView.IsAccentColorsVisibleProperty, false, x => x.DisplayName(nameof(SettingsResources.IsAccentColorsVisible)))
        .AddEnumProperty<ColorSpectrumShape, ListBoxEditor>(ColorView.ColorSpectrumShapeProperty,
            ColorSpectrumShape.Box,
            x => x.DisplayName(nameof(SettingsResources.Shape)),
            configureChoice: (x, y) =>
            {
                switch (x)
                {
                    case ColorSpectrumShape.Box:
                        y.WithIcon(MaterialIconKind.CheckboxBlank);
                        break;
                    case ColorSpectrumShape.Ring:
                        y.WithIcon(MaterialIconKind.CheckboxBlankCircle);
                        break;
                }
            })
        .AddProperty(ColorView.IsAlphaVisibleProperty, true, x => x.DisplayName(nameof(SettingsResources.IsAlphaVisible)))
        .AddEnumProperty<ColorModel, ListBoxEditor>(ColorView.ColorModelProperty, ColorModel.Hsva, x => x.DisplayName(nameof(SettingsResources.Model)))
        .AddProperty(ColorView.IsHexInputVisibleProperty, true, x => x.DisplayName(nameof(SettingsResources.IsHexInputVisible)))
        .AddValueAction((x, y) =>
            {
                if (y is not IEnumerable values)
                    return;

                var result = values.OfType<string>().ToList();
                x.SetCurrentValue(ColorView.IsComponentSliderVisibleProperty, result.Contains("slider"));
                x.SetCurrentValue(ColorView.IsComponentTextInputVisibleProperty, result.Contains("numeric"));
            },
            new ObservableCollection<string> { "slider" },
            x => x.DisplayName(nameof(SettingsResources.Components))
                .Of<ListBoxEditor>(editor => editor.AddChoice("slider", builder => builder.DisplayName(nameof(SettingsResources.Slider)))
                    .AddChoice("numeric", builder => builder.DisplayName(nameof(SettingsResources.Numeric)))
                    .AllowMultipleSelection()))
])
{
    public static readonly ImmutableList<IColorPalette> Palettes =
    [.. new List<Type>
        {
            typeof(FlatColorPalette),
            typeof(FlatHalfColorPalette),
            typeof(FluentColorPalette),
            typeof(MaterialColorPalette),
            typeof(MaterialHalfColorPalette),
            typeof(SixteenColorPalette),
            typeof(LightColorPalette),
            typeof(DarkColorPalette),
            typeof(StandardColorPalette)
        }.Select(x => (IColorPalette)Activator.CreateInstance(x)!)];

    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.Palette;
}
