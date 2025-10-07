// -----------------------------------------------------------------------
// <copyright file="ExpandersPage.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Media;
using MyNet.Avalonia.Controls.Assists;
using MyNet.Avalonia.Demo.Helpers;
using MyNet.Avalonia.Enums;
using MyNet.Avalonia.Theme.Extensions;
using MyNet.Utilities;
using MyNet.Utilities.Generator;

namespace MyNet.Avalonia.Demo.Pages;

internal sealed partial class ExpandersPage : AutoBuildPage
{
    public ExpandersPage() => InitializeComponent();

    protected override Control CreateControl(ControlData data)
    {
        var item = new Expander
        {
            Content = new TextBlock
            {
                Text = SentenceGenerator.Paragraph(RandomGenerator.Int(10, 30), RandomGenerator.Int(3, 4)),
                TextWrapping = TextWrapping.Wrap,
                TextAlignment = TextAlignment.Justify
            },
            Header = data.Color.Or(data.Size.OrEmpty()).Or("Default"),
            IsExpanded = RandomGenerator.Bool(),
            VerticalAlignment = global::Avalonia.Layout.VerticalAlignment.Top,
            HorizontalAlignment = global::Avalonia.Layout.HorizontalAlignment.Left,
            Width = 300,
            MaxWidth = 300
        };

        IconAssist.SetIcon(item, RandomGenerator.Enum<IconData>().ToIcon());

        return item;
    }

    protected override IEnumerable<ControlThemeData> ProvideThemes()
        => [
            new ControlThemeData()
            .AddStyles("Light", "Solid", "Outlined", "Headered", "Labelled")
            .AddCartesianStyles("Solid", "Shadow")
            .AddCartesianStyles("Headered", "HeaderShadow")
            .AddCartesianStyles("Solid", "Outlined")
            .AddCartesianStyles("Light", "Outlined", "Headered")
            .AddCartesianStyles("Labelled", "Centered")
            .AddAllColors(),

            new ControlThemeData("Button")
            .AddLayouts("Circle")
            .AddStyles("Light", "Solid", "Outlined", "Text")
            .AddCartesianStyles("Solid", "Shadow").AddCartesianStyles("Light", "Outlined", "Text")
            .AddAllColors()
            .AddSizes("Small", "Medium", "Large")
        ];

    private void Direction_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        => BuildHelper.ExecuteOnChildren<Expander>(Root, x =>
        {
            switch (Direction.SelectedIndex)
            {
                case 0:

                    x.ExpandDirection = ExpandDirection.Down;
                    x.VerticalAlignment = global::Avalonia.Layout.VerticalAlignment.Top;
                    x.HorizontalAlignment = global::Avalonia.Layout.HorizontalAlignment.Left;
                    x.Width = 300;
                    x.Height = double.NaN;
                    break;

                case 1:
                    x.ExpandDirection = ExpandDirection.Up;
                    x.VerticalAlignment = global::Avalonia.Layout.VerticalAlignment.Bottom;
                    x.HorizontalAlignment = global::Avalonia.Layout.HorizontalAlignment.Left;
                    x.Width = 300;
                    x.Height = double.NaN;
                    break;

                case 2:
                    x.ExpandDirection = ExpandDirection.Left;
                    x.VerticalAlignment = global::Avalonia.Layout.VerticalAlignment.Top;
                    x.HorizontalAlignment = global::Avalonia.Layout.HorizontalAlignment.Right;
                    x.Width = double.NaN;
                    x.Height = 300;
                    break;

                case 3:
                    x.ExpandDirection = ExpandDirection.Right;
                    x.VerticalAlignment = global::Avalonia.Layout.VerticalAlignment.Top;
                    x.HorizontalAlignment = global::Avalonia.Layout.HorizontalAlignment.Left;
                    x.Width = double.NaN;
                    x.Height = 300;
                    break;
            }
        });

    private void Expand_Click(object? sender, global::Avalonia.Interactivity.RoutedEventArgs e) => BuildHelper.ExecuteOnChildren<Expander>(Root, x => x.IsExpanded = true);

    private void Collapse_Click(object? sender, global::Avalonia.Interactivity.RoutedEventArgs e) => BuildHelper.ExecuteOnChildren<Expander>(Root, x => x.IsExpanded = false);
}
