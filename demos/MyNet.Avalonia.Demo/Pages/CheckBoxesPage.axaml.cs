// -----------------------------------------------------------------------
// <copyright file="CheckBoxesPage.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Avalonia.Controls;
using MyNet.Avalonia.Demo.Helpers;
using MyNet.Utilities;
using MyNet.Utilities.Generator;

namespace MyNet.Avalonia.Demo.Pages;

internal sealed partial class CheckBoxesPage : AutoBuildPage
{
    public CheckBoxesPage() => InitializeComponent();

    protected override IEnumerable<ControlThemeData> ProvideThemes()
        => [
            new ControlThemeData(defaultStyleDisplay: DefaultStyleDisplay.WithColors)
            .AddLayouts("Circle", "Alternate")
            .AddDefaultColors(false)
            .AddSizes("Small", "Medium", "Large")
        ];

    protected override Control CreateControl(ControlData data)
    {
        var item = new CheckBox
        {
            HorizontalAlignment = global::Avalonia.Layout.HorizontalAlignment.Center,
            Content = data.Color.Or(data.Size.OrEmpty()).Or("Default"),
            IsChecked = true
        };

        return item;
    }

    private void Icon_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    => BuildHelper.AddIconOnChildren<CheckBox>(Root, Icon?.SelectedIndex ?? 0);

    private void Check_Click(object? sender, global::Avalonia.Interactivity.RoutedEventArgs e) => BuildHelper.ExecuteOnChildren<CheckBox>(Root, x => x.IsChecked = true);

    private void Uncheck_Click(object? sender, global::Avalonia.Interactivity.RoutedEventArgs e) => BuildHelper.ExecuteOnChildren<CheckBox>(Root, x => x.IsChecked = false);

    private void Random_Click(object? sender, global::Avalonia.Interactivity.RoutedEventArgs e) => BuildHelper.ExecuteOnChildren<CheckBox>(Root, x => x.IsChecked = RandomGenerator.ListItem([true, false, (bool?)null]));
}
