// -----------------------------------------------------------------------
// <copyright file="CalendarsPage.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using MyNet.Avalonia.Demo.Helpers;

namespace MyNet.Avalonia.Demo.Pages;

internal sealed partial class CalendarsPage : AutoBuildPage
{
    public CalendarsPage() => InitializeComponent();

    protected override Control CreateControl(ControlData data)
    {
        var item = new Calendar
        {
            [!Calendar.SelectionModeProperty] = SelectionMode[!SelectingItemsControl.SelectedValueProperty]
        };

        return item;
    }

    protected override IEnumerable<ControlThemeData> ProvideThemes()
        => [
            new ControlThemeData(defaultStyleDisplay: DefaultStyleDisplay.WithColors)
            .AddStyles("Shadow")
            .AddColors(Color.Accent)
        ];
}
