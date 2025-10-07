// -----------------------------------------------------------------------
// <copyright file="ClocksPage.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Demo.Helpers;

namespace MyNet.Avalonia.Demo.Pages;

internal sealed partial class ClocksPage : AutoBuildPage
{
    public ClocksPage() => InitializeComponent();

    protected override Control CreateControl(ControlData data)
    {
        var item = new Clock
        {
            Height = 150,
            Width = 150,
            [!Clock.IsSmoothProperty] = IsSmooth[!ToggleButton.IsCheckedProperty],
            [!Clock.ShowHourHandProperty] = ShowHourHands[!ListBoxItem.IsSelectedProperty],
            [!Clock.ShowHourTicksProperty] = ShowHourTicks[!ListBoxItem.IsSelectedProperty],
            [!Clock.ShowMinuteHandProperty] = ShowMinuteHands[!ListBoxItem.IsSelectedProperty],
            [!Clock.ShowMinuteTicksProperty] = ShowMinuteTicks[!ListBoxItem.IsSelectedProperty],
            [!Clock.ShowSecondHandProperty] = ShowSecondHands[!ListBoxItem.IsSelectedProperty],
            [!Clock.LiveUpdateProperty] = LiveUpdate[!ToggleButton.IsCheckedProperty],
        };

        return item;
    }

    protected override IEnumerable<ControlThemeData> ProvideThemes()
        => [
            new ControlThemeData(defaultStyleDisplay: DefaultStyleDisplay.WithColors)
                                .AddStyles("Solid", "Outlined")
                                .AddCartesianStyles("Solid", "Outlined", "Shadow")
                                .AddThemeColors()
        ];
}
