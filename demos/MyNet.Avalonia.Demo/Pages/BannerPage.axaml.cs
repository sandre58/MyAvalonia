// -----------------------------------------------------------------------
// <copyright file="BannerPage.axaml.cs" company="Stéphane ANDRE">
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
internal sealed partial class BannerPage : ContentPage
{
    public BannerPage() => InitializeComponent();

    private void Restore_Click(object? sender, RoutedEventArgs e) => Root.ExecuteOnChildren<Banner>(x => x.IsVisible = true);
}
