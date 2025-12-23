// -----------------------------------------------------------------------
// <copyright file="BannerPage.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Avalonia.Interactivity;
using MyNet.Avalonia.Extensions;

namespace MyNet.Avalonia.Demo.Pages;

internal sealed partial class BannerPage : Page
{
    public BannerPage() => InitializeComponent();

    private void Restore_Click(object? sender, RoutedEventArgs e) => this.ExecuteOnChildren<CheckBox>(x => x.IsVisible = true);
}
