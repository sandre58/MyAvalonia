// -----------------------------------------------------------------------
// <copyright file="ProgressBarsPage.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using MyNet.Avalonia.Demo.Helpers;
using MyNet.Avalonia.Extensions;
using MyNet.Utilities;

namespace MyNet.Avalonia.Demo.Pages;

internal sealed partial class ProgressBarsPage : AutoBuildPage
{
    public ProgressBarsPage() => InitializeComponent();

    protected override Control CreateControl(ControlData data)
    {
        var item = new ProgressBar
        {
            [!RangeBase.ValueProperty] = Value[!RangeBase.ValueProperty],
            [!ProgressBar.OrientationProperty] = Orientation[!SelectingItemsControl.SelectedValueProperty],
            [!ProgressBar.IsIndeterminateProperty] = IsIndeterminate[!ToggleButton.IsCheckedProperty]
        };

        if (!data.Theme.ContainsAny("Circular"))
        {
            item.Width = 250;
        }
        else
        {
            item.AddClasses("Circular");
        }

        return item;
    }

    protected override IEnumerable<ControlThemeData> ProvideThemes()
        => [
            new ControlThemeData(defaultStyleDisplay: DefaultStyleDisplay.WithColors)
            .AddLayouts("Circle")
            .AddStyles("Shadow")
            .AddDefaultColors(false)
            .AddSizes("Small", "Medium", "Large"),

            new ControlThemeData("Circular", defaultStyleDisplay: DefaultStyleDisplay.WithColors)
            .AddDefaultColors(false)
            .AddSizes("Small", "Medium", "Large")
        ];

    private void Orientation_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        => BuildHelper.ExecuteOnChildren<ProgressBar>(Root, x =>
        {
            if (x.Classes.Contains("Circular")) return;

            switch (Orientation.SelectedIndex)
            {
                case 0:
                    x.Width = 250;
                    x.Height = double.NaN;
                    break;

                case 1:
                    x.Height = 250;
                    x.Width = double.NaN;
                    break;
            }
        });

    private void ValuePosition_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    => BuildHelper.ExecuteOnChildren<ProgressBar>(Root, x =>
    {
        _ = x.Classes.Remove("Left");
        _ = x.Classes.Remove("Right");
        switch (ValuePosition.SelectedIndex)
        {
            case 0:
                x.ShowProgressText = false;
                break;

            case 1:
                x.ShowProgressText = true;
                x.AddClasses("Left");
                break;

            case 2:
                x.ShowProgressText = true;
                break;

            case 3:
                x.ShowProgressText = true;
                x.AddClasses("Right");
                break;
        }
    });
}
