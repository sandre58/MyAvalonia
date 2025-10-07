// -----------------------------------------------------------------------
// <copyright file="BannersPage.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Controls.Assists;
using MyNet.Avalonia.Controls.Enums;
using MyNet.Avalonia.Demo.Helpers;
using MyNet.Avalonia.Enums;
using MyNet.Avalonia.Theme.Extensions;
using MyNet.Utilities;
using MyNet.Utilities.Generator;

namespace MyNet.Avalonia.Demo.Pages;

internal sealed partial class BannersPage : AutoBuildPage
{
    public BannersPage() => InitializeComponent();

    protected override Control CreateControl(ControlData data)
    {
        var item = new Banner
        {
            [!Banner.CanCloseProperty] = ShowClearButton[!ToggleButton.IsCheckedProperty],
            Content = new TextBlock
            {
                Text = SentenceGenerator.Paragraph(RandomGenerator.Int(8, 12), RandomGenerator.Int(1, 2)),
                TextWrapping = TextWrapping.Wrap
            },
            Header = data.Color.Or(data.Size.OrEmpty()).Or("Default"),
            MaxWidth = 500
        };

        IconAssist.SetIcon(item, RandomGenerator.Enum<IconData>().ToIcon());

        return item;
    }

    protected override IEnumerable<ControlThemeData> ProvideThemes()
        => [
            new ControlThemeData()
            .AddStyles("Light", "Solid", "Outlined")
            .AddCartesianStyles("Solid", "Shadow")
            .AddCartesianStyles("Light", "Outlined")
            .AddDefaultColors()
            .AddCustomControls([.. Enum.GetValues<Severity>().Except([Severity.Custom]).Select(x => new Banner
            {
                Severity = x,
                Content = new TextBlock
                {
                    Text = SentenceGenerator.Paragraph(RandomGenerator.Int(8, 12), RandomGenerator.Int(1, 2)),
                    TextWrapping = TextWrapping.Wrap
                },
            })])
        ];

    private void Button_Click(object? sender, global::Avalonia.Interactivity.RoutedEventArgs e) => BuildHelper.ExecuteOnChildren<Banner>(Root, x => x.IsVisible = true);
}
