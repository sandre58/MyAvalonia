// -----------------------------------------------------------------------
// <copyright file="IconsPage.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;

using MyNet.Avalonia.Demo.ViewModels;
using PropertyChanged;

namespace MyNet.Avalonia.Demo.Pages;

[DoNotNotify]
internal sealed partial class IconsPage : ContentPage
{
    public IconsPage()
    {
        InitializeComponent();

        DataContext = new IconsPageViewModel();
    }
}
