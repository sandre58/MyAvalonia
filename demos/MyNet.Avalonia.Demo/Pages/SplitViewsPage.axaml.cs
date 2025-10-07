// -----------------------------------------------------------------------
// <copyright file="SplitViewsPage.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using MyNet.Avalonia.Demo.Helpers;
using MyNet.Avalonia.Demo.Views.Samples;

namespace MyNet.Avalonia.Demo.Pages;

internal sealed partial class SplitViewsPage : AutoBuildPage
{
    public SplitViewsPage() => InitializeComponent();

    protected override Control CreateControl(ControlData data)
    {
        var item = new SplitView
        {
            Content = new ExtraLargeContent1(),
            Pane = new LargeContent1 { Width = double.NaN },
            [!SplitView.CompactPaneLengthProperty] = CompactPaneLength[!RangeBase.ValueProperty],
            [!SplitView.DisplayModeProperty] = DisplayMode[!SelectingItemsControl.SelectedValueProperty],
            [!SplitView.IsPaneOpenProperty] = IsPaneOpen[!ToggleButton.IsCheckedProperty],
            [!SplitView.OpenPaneLengthProperty] = OpenPaneLength[!RangeBase.ValueProperty],
            [!SplitView.PanePlacementProperty] = Placement[!SelectingItemsControl.SelectedValueProperty],
            [!SplitView.UseLightDismissOverlayModeProperty] = UseOverlay[!ToggleButton.IsCheckedProperty],
            Height = 500,
            Width = 600
        };

        return item;
    }

    protected override IEnumerable<ControlThemeData> ProvideThemes()
        => [
            new ControlThemeData()
            .AddStyles("Solid", "Outlined", "Transparent")
            .AddCartesianStyles("Solid", "Shadow")
            .AddThemeColors().AddColors(Color.Dark)
        ];
}
