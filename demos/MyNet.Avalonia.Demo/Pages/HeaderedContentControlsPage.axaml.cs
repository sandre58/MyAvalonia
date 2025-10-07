// -----------------------------------------------------------------------
// <copyright file="HeaderedContentControlsPage.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using MyNet.Avalonia.Controls.Assists;
using MyNet.Avalonia.Demo.Helpers;
using MyNet.Avalonia.Enums;
using MyNet.Avalonia.Theme.Extensions;
using MyNet.Utilities;
using MyNet.Utilities.Generator;

namespace MyNet.Avalonia.Demo.Pages;

internal sealed partial class HeaderedContentControlsPage : AutoBuildPage
{
    public HeaderedContentControlsPage() => InitializeComponent();

    protected override Control CreateControl(ControlData data)
    {
        var item = new HeaderedContentControl
        {
            Content = new TextBlock
            {
                Text = SentenceGenerator.Paragraph(RandomGenerator.Int(10, 30), RandomGenerator.Int(3, 4)),
                TextWrapping = TextWrapping.Wrap,
                TextAlignment = TextAlignment.Justify
            },
            Header = data.Color.Or(data.Size.OrEmpty()).Or("Default"),
            MaxWidth = 300
        };

        IconAssist.SetIcon(item, RandomGenerator.Enum<IconData>().ToIcon());

        return item;
    }

    protected override IEnumerable<ControlThemeData> ProvideThemes()
        => [
            new ControlThemeData()
            .AddStyles("Light", "Solid", "Outlined", "Headered", "Labelled", "Labelled Watermark", "Labelled Regular")
            .AddCartesianStyles("Solid", "Shadow")
            .AddCartesianStyles("Headered", "HeaderShadow")
            .AddCartesianStyles("Solid", "Outlined")
            .AddCartesianStyles("Light", "Outlined", "Headered")
            .AddAllColors()
        ];

    private void Layout_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        => BuildHelper.AddClassesOnChildren<TemplatedControl>(Root, ["Left", "Top", "Right", "Bottom"], Layout?.SelectedIndex ?? 0);
}
