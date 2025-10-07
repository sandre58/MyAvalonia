// -----------------------------------------------------------------------
// <copyright file="DropDownButtonsPage.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using MyNet.Avalonia.Controls.Assists;
using MyNet.Avalonia.Demo.Helpers;
using MyNet.Avalonia.Demo.Views.Samples;
using MyNet.Avalonia.Enums;
using MyNet.Avalonia.Theme.Extensions;
using MyNet.Utilities;
using MyNet.Utilities.Generator;

namespace MyNet.Avalonia.Demo.Pages;

internal sealed partial class DropDownButtonsPage : AutoBuildPage
{
    public DropDownButtonsPage() => InitializeComponent();

    protected override Control CreateControl(ControlData data)
    {
        var item = new DropDownButton
        {
            HorizontalAlignment = global::Avalonia.Layout.HorizontalAlignment.Center,
            Content = data.Theme.ContainsAny("rounded")
                        ? RandomGenerator.Enum<IconData>().ToIcon()
                        : data.Color.Or(data.Size.OrEmpty()).Or("Default"),
            Flyout = new MenuFlyout
            {
                ItemsSource = MenuHelper.RandomizeMenuItems(1, 3, 5, 3)
            },
            [!FlyoutAssist.PlacementProperty] = PopupPlacement[!SelectingItemsControl.SelectedValueProperty]
        };

        if (data.Theme.NotContainsAny("rounded"))
            item.Classes.Add("CanAddIcon");

        return item;
    }

    protected override IEnumerable<ControlThemeData> ProvideThemes()
        => [
            new ControlThemeData()
            .AddLayouts("Circle")
            .AddStyles("Light", "Solid", "Outlined", "Text")
            .AddCartesianStyles("Solid", "Shadow").AddCartesianStyles("Light", "Outlined", "Text")
            .AddDefaultColors()
            .AddSizes("Small", "Medium", "Large")
            .AddCustomControls(CreateCustomControls()),

            new ControlThemeData("Rounded")
            .AddStyles("Light", "Solid", "Outlined", "Text")
            .AddCartesianStyles("Solid", "Shadow").AddCartesianStyles("Light", "Outlined", "Text")
            .AddDefaultColors()
            .AddSizes("Small", "Medium", "Large")
        ];

    private static Func<Control[]> CreateCustomControls() => () =>
    {
        var result = new List<Control>
        {
            new DropDownButton
                {
                    Content = "Custom Flyout",
                    Flyout = new Flyout
                    {
                        Content = new LargeContent1()
                    }
                }
        };
        return [.. result];
    };

    private void Icon_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        => BuildHelper.AddIconOnChildren<DropDownButton>(Root, Icon?.SelectedIndex ?? 0, x => x.Classes.Contains("CanAddIcon"));

    private void DropDownPlacement_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        => BuildHelper.AddClassesOnChildren<DropDownButton>(Root, ["Left", "Right", "Top", "Bottom"], DropDownPlacement?.SelectedIndex ?? 0);
}
