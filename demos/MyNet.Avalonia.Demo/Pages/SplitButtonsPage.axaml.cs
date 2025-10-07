// -----------------------------------------------------------------------
// <copyright file="SplitButtonsPage.axaml.cs" company="Stéphane ANDRE">
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
using MyNet.Utilities;

namespace MyNet.Avalonia.Demo.Pages;

internal sealed partial class SplitButtonsPage : AutoBuildPage
{
    public SplitButtonsPage() => InitializeComponent();

    protected override Control CreateControl(ControlData data)
    {
        var item = new SplitButton
        {
            HorizontalAlignment = global::Avalonia.Layout.HorizontalAlignment.Center,
            Content = data.Color.Or(data.Size.OrEmpty()).Or("Default"),
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
            .AddCustomControls(CreateCustomControls())
        ];

    private static Func<Control[]> CreateCustomControls() => () =>
    {
        var result = new List<Control>
        {
            new SplitButton
                {
                    HorizontalAlignment = global::Avalonia.Layout.HorizontalAlignment.Center,
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
        => BuildHelper.AddIconOnChildren<SplitButton>(Root, Icon?.SelectedIndex ?? 0);

    private void DropDownPlacement_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        => BuildHelper.AddClassesOnChildren<SplitButton>(Root, ["Left", "Right", "Top", "Bottom"], DropDownPlacement?.SelectedIndex ?? 0);
}
