// -----------------------------------------------------------------------
// <copyright file="ToggleButtonsPage.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using MyNet.Avalonia.Demo.Helpers;
using MyNet.Avalonia.Enums;
using MyNet.Avalonia.Theme.Extensions;
using MyNet.Utilities;
using MyNet.Utilities.Generator;

namespace MyNet.Avalonia.Demo.Pages;

internal sealed partial class ToggleButtonsPage : AutoBuildPage
{
    public ToggleButtonsPage() => InitializeComponent();

    protected override Control CreateControl(ControlData data)
    {
        var item = new ToggleButton
        {
            HorizontalAlignment = global::Avalonia.Layout.HorizontalAlignment.Center,
            Content = data.Theme.ContainsAny("rounded")
                        ? RandomGenerator.Enum<IconData>().ToIcon()
                        : data.Theme.ContainsAny("icon")
                        ? RandomGenerator.Enum<IconData>().ToGeometry()
                        : data.Color.Or(data.Size.OrEmpty()).Or("Default"),
            IsChecked = RandomGenerator.Bool()
        };

        if (data.Theme.NotContainsAny("icon", "rounded"))
            item.Classes.Add("CanAddIcon");

        return item;
    }

    protected override IEnumerable<ControlThemeData> ProvideThemes()
        => [
            new ControlThemeData(defaultStyleDisplay: DefaultStyleDisplay.WithColors)
            .AddLayouts("Circle")
            .AddStyles("Light", "Solid", "Outlined", "Text")
            .AddCartesianStyles("Solid", "Shadow").AddCartesianStyles("Light", "Outlined", "Text")
            .AddDefaultColors()
            .AddSizes("Small", "Medium", "Large"),

            new ControlThemeData("Rounded", DefaultStyleDisplay.WithColors)
            .AddStyles("Light", "Solid", "Outlined", "Text")
            .AddCartesianStyles("Solid", "Shadow").AddCartesianStyles("Light", "Outlined", "Text")
            .AddDefaultColors()
            .AddSizes("Small", "Medium", "Large"),

            new ControlThemeData("Icon", DefaultStyleDisplay.WithColors)
            .AddDefaultColors()
            .AddSizes("ExtraSmall", "Small", "Medium", "Large", "ExtraLarge")
        ];

    private void Icon_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        => BuildHelper.AddIconOnChildren<ToggleButton>(Root, Icon?.SelectedIndex ?? 0, x => x.Classes.Contains("CanAddIcon"));
}
