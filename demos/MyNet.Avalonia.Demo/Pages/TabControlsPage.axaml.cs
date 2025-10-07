// -----------------------------------------------------------------------
// <copyright file="TabControlsPage.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using MyNet.Avalonia.Controls.Assists;
using MyNet.Avalonia.Demo.Helpers;
using MyNet.Avalonia.Demo.Views.Samples;
using MyNet.Avalonia.Enums;
using MyNet.Avalonia.Extensions;
using MyNet.Avalonia.Theme.Extensions;
using MyNet.Utilities;
using MyNet.Utilities.Generator;
using MyNet.Utilities.Helpers;

namespace MyNet.Avalonia.Demo.Pages;

internal sealed partial class TabControlsPage : AutoBuildPage
{
    public TabControlsPage() => InitializeComponent();

    protected override Control CreateControl(ControlData data)
    {
        var item = new TabControl
        {
            MaxWidth = 500
        };

        EnumerableHelper.Iteration(4, x =>
        {
            var tabItem = new TabItem
            {
                Content = new ScrollViewer { HorizontalScrollBarVisibility = global::Avalonia.Controls.Primitives.ScrollBarVisibility.Auto, Content = new LargeContent1 { BorderBrush = new SolidColorBrush(RandomGenerator.Color().ToColor() ?? Colors.White) } },
                Header = $"Tab {x}"
            };

            if (data.Theme.NotContainsAny("Indicator"))
                tabItem.Classes.Add("CanAddIcon");
            else
                IconAssist.SetIcon(tabItem, RandomGenerator.Enum<IconData>().ToIcon(20));

            _ = item.Items.Add(tabItem);
        });

        if (data is { Layout: "Header Inverse", Color: null })
            item.AddClasses("Primary");

        return item;
    }

    protected override IEnumerable<ControlThemeData> ProvideThemes()
        => [
            new ControlThemeData(defaultStyleDisplay: DefaultStyleDisplay.WithColors)
            .AddLayouts("Header", "Header Inverse")
            .AddStyles("Solid", "Light", "Outlined")
            .AddCartesianStyles("Solid", "Light", "Outlined")
            .AddCartesianStyles("Circle", "Solid")
            .AddThemeColors(false)
            .AddCustomControls(() =>
            {
                var item = new TabControl
                {
                    MaxWidth = 500
                };

                EnumerableHelper.Iteration(20, x =>
                {
                    var tabItem = new TabItem
                    {
                        Content = new ScrollViewer { HorizontalScrollBarVisibility = global::Avalonia.Controls.Primitives.ScrollBarVisibility.Auto, Content = new LargeContent1 { BorderBrush = new SolidColorBrush(RandomGenerator.Color().ToColor() ?? Colors.White) } },
                        Header = $"Tab {x}"
                    };

                    tabItem.Classes.Add("CanAddIcon");

                    _ = item.Items.Add(tabItem);
                });

                return [item];
            }),

            new ControlThemeData("Indicator", defaultStyleDisplay: DefaultStyleDisplay.WithColors)
            .AddThemeColors(false)
        ];

    private void Layout_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        => BuildHelper.ExecuteOnChildren<TabControl>(Root, x => x.TabStripPlacement = (Dock)Layout.SelectedIndex);

    private void Icon_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        => BuildHelper.AddIconOnChildren<TabItem>(Root, Icon?.SelectedIndex ?? 0, x => x.Classes.Contains("CanAddIcon"));

    private void Uniform_IsCheckedChanged(object? sender, RoutedEventArgs e)
        => BuildHelper.AddClassesOnChildren<TabControl>(Root, [string.Empty, "Uniform"], Convert.ToInt32(Uniform.IsChecked, CultureInfo.CurrentCulture));
}
