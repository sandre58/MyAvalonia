// -----------------------------------------------------------------------
// <copyright file="ClockSelectorPage.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;

using Avalonia.Interactivity;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Extensions;
using PropertyChanged;

namespace MyNet.Avalonia.Demo.Pages;

[DoNotNotify]
internal sealed partial class ClockSelectorPage : ContentPage
{
    public ClockSelectorPage() => InitializeComponent();

    private void Restore_Click(object? sender, RoutedEventArgs e) => Root.ExecuteOnChildren<ClockSelector>(x =>
    {
        x.SelectedValue = null;
        x.SelectedComponent = TimeComponent.Hour;
    });
}
