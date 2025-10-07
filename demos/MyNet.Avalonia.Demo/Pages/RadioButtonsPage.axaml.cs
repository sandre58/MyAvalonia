// -----------------------------------------------------------------------
// <copyright file="RadioButtonsPage.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Avalonia.Controls;
using MyNet.Avalonia.Demo.Helpers;
using MyNet.Humanizer;
using MyNet.Utilities;
using MyNet.Utilities.Generator;

namespace MyNet.Avalonia.Demo.Pages;

internal sealed partial class RadioButtonsPage : AutoBuildPage
{
    public RadioButtonsPage() => InitializeComponent();

    protected override Control CreateControl(ControlData data)
    {
        var item = new RadioButton
        {
            HorizontalAlignment = global::Avalonia.Layout.HorizontalAlignment.Center,
            Content = data.Color.Or(data.Size.OrEmpty()).Or("Default"),
            GroupName = $"{data.Theme}_{data.Layout}_{data.Styles?.Humanize("_")}_{data.Size is not null}",
            IsChecked = RandomGenerator.ListItem([true, false, (bool?)null])
        };

        return item;
    }

    protected override IEnumerable<ControlThemeData> ProvideThemes()
        => [
            new ControlThemeData(defaultStyleDisplay: DefaultStyleDisplay.WithColors)
            .AddLayouts("Circle", "Alternate")
            .AddDefaultColors(false)
            .AddSizes("Small", "Medium", "Large")
        ];

    private void Icon_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        => BuildHelper.AddIconOnChildren<RadioButton>(Root, Icon?.SelectedIndex ?? 0);
}
