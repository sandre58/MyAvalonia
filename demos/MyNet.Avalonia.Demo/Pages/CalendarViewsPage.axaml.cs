// -----------------------------------------------------------------------
// <copyright file="CalendarViewsPage.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Avalonia.Controls;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Demo.Helpers;

namespace MyNet.Avalonia.Demo.Pages;

internal sealed partial class CalendarViewsPage : AutoBuildPage
{
    public CalendarViewsPage() => InitializeComponent();

    protected override Control CreateControl(ControlData data)
    {
        var item = new CalendarView
        {
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
