// -----------------------------------------------------------------------
// <copyright file="BadgesPage.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Controls.Enums;
using MyNet.Avalonia.Demo.Helpers;
using MyNet.Avalonia.Enums;
using MyNet.Avalonia.Theme.Extensions;
using MyNet.Utilities;
using MyNet.Utilities.Generator;

namespace MyNet.Avalonia.Demo.Pages;

internal sealed partial class BadgesPage : AutoBuildPage
{
    public BadgesPage() => InitializeComponent();

    protected override Control CreateControl(ControlData data)
    {
        var item = new Badge()
        {
            [!Badge.IsRoundedProperty] = IsRounded[!ToggleButton.IsCheckedProperty],
            [!Badge.OffsetXProperty] = OffsetX[!RangeBase.ValueProperty],
            [!Badge.OffsetYProperty] = OffsetY[!RangeBase.ValueProperty],
            Content = new Button
            {
                Width = 100,
                Content = data.Color.Or(data.Size.OrEmpty()).Or("Default"),
            },
            Header = RandomGenerator.Enum<HorizontalAlignment>() switch
            {
                HorizontalAlignment.Left => RandomGenerator.Int(0, 200),
                HorizontalAlignment.Right => RandomGenerator.Enum<IconData>().ToIcon(),
                HorizontalAlignment.Center => data.Color.Or(data.Size.OrEmpty()).Or("Default"),
                HorizontalAlignment.Stretch => RandomGenerator.Int(1000, 9999),
                _ => throw new InvalidOperationException(),
            }
        };

        return item;
    }

    protected override IEnumerable<ControlThemeData> ProvideThemes()
        => [
           new ControlThemeData(defaultStyleDisplay: DefaultStyleDisplay.WithColors)
                                      .AddLayouts("Circle")
                                      .AddStyles("Light", "Outlined", "Shadow")
                                      .AddCartesianStyles("Light", "Outlined")
                                      .AddDefaultColors(false)
                                      .AddSizes("Medium", "Large")
        ];

    private void Position_SelectionChanged(object? sender, SelectionChangedEventArgs e) => BuildHelper.ExecuteOnChildren<Badge>(Root, x => x.CornerPosition = (CornerPosition)Position.SelectedIndex);
}
