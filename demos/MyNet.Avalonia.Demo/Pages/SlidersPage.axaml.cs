// -----------------------------------------------------------------------
// <copyright file="SlidersPage.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media;
using MyNet.Avalonia.Controls.Assists;
using MyNet.Avalonia.Demo.Helpers;
using MyNet.Avalonia.Extensions;
using MyNet.Utilities;
using MyNet.Utilities.Generator;

namespace MyNet.Avalonia.Demo.Pages;

internal sealed partial class SlidersPage : AutoBuildPage
{
    public SlidersPage() => InitializeComponent();

    protected override Control CreateControl(ControlData data)
    {
        var item = new Slider
        {
            Value = RandomGenerator.Int(0, 100),
            [!Slider.IsSnapToTickEnabledProperty] = IsSnapToTickEnabled[!ToggleButton.IsCheckedProperty],
            [!Slider.OrientationProperty] = Orientation[!SelectingItemsControl.SelectedValueProperty],
            [!Slider.IsDirectionReversedProperty] = IsDirectionReversed[!ToggleButton.IsCheckedProperty],
            [!Slider.TickPlacementProperty] = TickPlacement[!SelectingItemsControl.SelectedValueProperty],
            Width = 250,
            [!SliderAssist.ShowValueOnMouseOverProperty] = ShowValue[!ToggleButton.IsCheckedProperty],
            [!SliderAssist.TickModeProperty] = TickMode[!SelectingItemsControl.SelectedValueProperty]
        };

        _ = item.TryBind(SliderAssist.TickLengthProperty, new Binding(nameof(NumericUpDown.Value))
        {
            Source = TickLength,
            Converter = new FuncValueConverter<decimal?, double?>(x => !x.HasValue ? 0 : (double?)x)
        });
        _ = item.TryBind(RangeBase.MinimumProperty, new Binding(nameof(NumericUpDown.Value))
        {
            Source = Minimum,
            Converter = new FuncValueConverter<decimal?, double?>(x => !x.HasValue ? 0 : (double?)x)
        });
        _ = item.TryBind(RangeBase.MaximumProperty, new Binding(nameof(NumericUpDown.Value))
        {
            Source = Maximum,
            Converter = new FuncValueConverter<decimal?, double?>(x => !x.HasValue ? 0 : (double?)x)
        });

        _ = item.TryBind(Slider.TickFrequencyProperty, new Binding(nameof(NumericUpDown.Value))
        {
            Source = TickFrequency,
            Converter = new FuncValueConverter<decimal?, double?>(x => !x.HasValue ? 0 : (double?)x)
        });

        return item;
    }

    protected override IEnumerable<ControlThemeData> ProvideThemes()
        => [
            new ControlThemeData(defaultStyleDisplay: DefaultStyleDisplay.WithColors)
            .AddThemeColors(false)
            .AddCustomControls(() =>
            {
                var verticalSliders = CreateColorSliders();
                verticalSliders.ForEach(x =>
                {
                    x.Height = 300;
                    x.Orientation = global::Avalonia.Layout.Orientation.Vertical;
                });

                var sliders = CreateColorSliders();
                sliders.ForEach(x => x.Width = 300);

                return [.. verticalSliders, .. sliders];
            })
        ];

    private static Slider[] CreateColorSliders()
    {
        var hue = new ColorSlider { HsvColor = new HsvColor(RandomGenerator.Color().ToColor() ?? Colors.White), ColorModel = ColorModel.Hsva, ColorComponent = ColorComponent.Component1 };

        var saturation = new ColorSlider { HsvColor = new HsvColor(RandomGenerator.Color().ToColor() ?? Colors.White), ColorModel = ColorModel.Hsva, ColorComponent = ColorComponent.Component2 };

        var value = new ColorSlider { HsvColor = new HsvColor(RandomGenerator.Color().ToColor() ?? Colors.White), ColorModel = ColorModel.Hsva, ColorComponent = ColorComponent.Component3 };

        var red = new ColorSlider { Color = RandomGenerator.Color().ToColor() ?? Colors.White, ColorModel = ColorModel.Rgba, ColorComponent = ColorComponent.Component1 };

        var green = new ColorSlider { Color = RandomGenerator.Color().ToColor() ?? Colors.White, ColorModel = ColorModel.Rgba, ColorComponent = ColorComponent.Component2 };

        var blue = new ColorSlider { Color = RandomGenerator.Color().ToColor() ?? Colors.White, ColorModel = ColorModel.Rgba, ColorComponent = ColorComponent.Component3 };

        var alpha = new ColorSlider { Color = RandomGenerator.Color().ToColor() ?? Colors.White, ColorModel = ColorModel.Rgba, ColorComponent = ColorComponent.Alpha };

        return [hue, saturation, value, red, green, blue, alpha];
    }

    private void Orientation_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        => BuildHelper.ExecuteOnChildren<Slider>(Root, x =>
        {
            switch (Orientation.SelectedIndex)
            {
                case 0:
                    x.Width = 250;
                    x.Height = 80;
                    break;

                case 1:
                    x.Width = 80;
                    x.Height = 250;
                    break;
            }
        });
}
