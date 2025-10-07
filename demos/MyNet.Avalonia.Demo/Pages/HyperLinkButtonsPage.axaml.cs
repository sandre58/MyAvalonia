// -----------------------------------------------------------------------
// <copyright file="HyperLinkButtonsPage.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Avalonia.Controls;
using MyNet.Avalonia.Demo.Helpers;
using MyNet.Utilities;

namespace MyNet.Avalonia.Demo.Pages;

internal sealed partial class HyperLinkButtonsPage : AutoBuildPage
{
    public HyperLinkButtonsPage() => InitializeComponent();

    protected override Control CreateControl(ControlData data)
    {
        var item = new HyperlinkButton
        {
            HorizontalAlignment = global::Avalonia.Layout.HorizontalAlignment.Center,
            Content = data.Color.Or(data.Size.OrEmpty()).Or("Default")
        };

        return item;
    }

    protected override IEnumerable<ControlThemeData> ProvideThemes()
        => [
            new ControlThemeData(defaultStyleDisplay: DefaultStyleDisplay.WithColors)
            .AddStyles("Text")
            .AddDefaultColors()
        ];

    private void Icon_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        => BuildHelper.AddIconOnChildren<HyperlinkButton>(Root, Icon?.SelectedIndex ?? 0);
}
