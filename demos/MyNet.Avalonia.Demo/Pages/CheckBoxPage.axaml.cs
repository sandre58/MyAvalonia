// -----------------------------------------------------------------------
// <copyright file="CheckBoxPage.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using MyNet.Avalonia.Extensions;
using MyNet.Utilities.Generator;

namespace MyNet.Avalonia.Demo.Pages;

internal sealed partial class CheckBoxPage : Page
{
    public CheckBoxPage() => InitializeComponent();

    private void Check_Click(object? sender, global::Avalonia.Interactivity.RoutedEventArgs e) => this.ExecuteOnChildren<CheckBox>(x => x.IsChecked = true);

    private void Uncheck_Click(object? sender, global::Avalonia.Interactivity.RoutedEventArgs e) => this.ExecuteOnChildren<CheckBox>(x => x.IsChecked = false);

    private void Random_Click(object? sender, global::Avalonia.Interactivity.RoutedEventArgs e) => this.ExecuteOnChildren<CheckBox>(x => x.IsChecked = RandomGenerator.ListItem([true, false, (bool?)null]));
}
