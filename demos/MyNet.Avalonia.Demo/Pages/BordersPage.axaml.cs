// -----------------------------------------------------------------------
// <copyright file="BordersPage.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Avalonia.Controls;
using MyNet.Avalonia.Demo.Helpers;
using MyNet.Utilities;

namespace MyNet.Avalonia.Demo.Pages;

internal sealed partial class BordersPage : AutoBuildPage
{
    public BordersPage() => InitializeComponent();

    protected override Control CreateControl(ControlData data)
    {
        var item = new Border
        {
            Height = 150,
            Width = 250
        };

        return item;
    }

    protected override IEnumerable<ControlThemeData> ProvideThemes()
        => [
            new ControlThemeData("Card", DefaultStyleDisplay.WithRoles)
            .AddAllRoles()
            .AddStyles("Light", "Outlined")
            .AddCartesianStyles("Outlined", "Light")
            .AddCartesianStyles("Outlined", "Solid")
            .AddCartesianStyles("Solid", "Shadow Hover")
            .AddCartesianStyles("Outlined", "Shadow Hover"),

            new("Popup")
        ];
}
