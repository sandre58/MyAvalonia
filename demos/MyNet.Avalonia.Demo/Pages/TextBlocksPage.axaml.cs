// -----------------------------------------------------------------------
// <copyright file="TextBlocksPage.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Avalonia.Controls;
using MyNet.Avalonia.Demo.Helpers;
using MyNet.Utilities;

namespace MyNet.Avalonia.Demo.Pages;

internal sealed partial class TextBlocksPage : AutoBuildPage
{
    public TextBlocksPage() => InitializeComponent();

    protected override Control CreateControl(ControlData data)
    {
        var item = new TextBlock { HorizontalAlignment = global::Avalonia.Layout.HorizontalAlignment.Center, Text = data.Color.Or(data.Size.OrEmpty()).Or("Default") };

        return item;
    }

    protected override IEnumerable<ControlThemeData> ProvideThemes()
        => [
            new ControlThemeData(defaultStyleDisplay: DefaultStyleDisplay.WithColors)
            .AddStyles("Secondary", "Tertiary", "Underline", "Delete", "Disablable")
            .AddDefaultColors()
            .AddAllSizes()
        ];
}
