// -----------------------------------------------------------------------
// <copyright file="ClockSelectorPage.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Interactivity;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Extensions;

namespace MyNet.Avalonia.Demo.Pages;

internal sealed partial class ClockSelectorPage : Page
{
    public ClockSelectorPage() => InitializeComponent();

    private void Restore_Click(object? sender, RoutedEventArgs e) => Root.ExecuteOnChildren<ClockSelector>(x =>
    {
        x.SelectedValue = null;
        x.SelectedComponent = TimeComponent.Hour;
    });
}
