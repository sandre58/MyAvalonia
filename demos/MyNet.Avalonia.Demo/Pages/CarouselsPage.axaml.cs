// -----------------------------------------------------------------------
// <copyright file="CarouselsPage.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using MyNet.Avalonia.Demo.Helpers;
using MyNet.Avalonia.Extensions;
using MyNet.Utilities;
using MyNet.Utilities.Generator;

namespace MyNet.Avalonia.Demo.Pages;

internal sealed partial class CarouselsPage : AutoBuildPage
{
    public CarouselsPage() => InitializeComponent();

    protected override Control CreateControl(ControlData data)
    {
        var item = new Carousel
        {
            Width = 800,
            Height = 500,
            ItemsSource = RandomGenerator.Int(3, 10).Range().Select(_ =>
            {
                var border = new Border
                {
                    Background = new SolidColorBrush(RandomGenerator.Color().ToColor().GetValueOrDefault()),
                    HorizontalAlignment = global::Avalonia.Layout.HorizontalAlignment.Stretch,
                    VerticalAlignment = global::Avalonia.Layout.VerticalAlignment.Stretch,
                    Padding = new Thickness(60, 10)
                };
                var text = new TextBlock
                {
                    Text = SentenceGenerator.Paragraph(RandomGenerator.Int(10, 30), RandomGenerator.Int(3, 4)),
                    TextWrapping = TextWrapping.Wrap,
                    HorizontalAlignment = global::Avalonia.Layout.HorizontalAlignment.Center,
                    VerticalAlignment = global::Avalonia.Layout.VerticalAlignment.Center
                };
                border.Child = text;

                return border;
            }).ToList()
        };

        return item;
    }

    protected override IEnumerable<ControlThemeData> ProvideThemes()
        => [
            new(),

            new("Full")
        ];

    private void Position_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        => BuildHelper.AddClassesOnChildren<Carousel>(Root, ["Left", "Center", "Right"], Position?.SelectedIndex ?? 0);

    private void Type_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        => BuildHelper.AddClassesOnChildren<Carousel>(Root, ["Dots", "Columnar", "Line"], Type?.SelectedIndex ?? 0);
}
