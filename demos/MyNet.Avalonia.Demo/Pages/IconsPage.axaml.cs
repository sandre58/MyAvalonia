// -----------------------------------------------------------------------
// <copyright file="IconsPage.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Demo.ViewModels;

namespace MyNet.Avalonia.Demo.Pages;

internal sealed partial class IconsPage : Page
{
    public IconsPage()
    {
        InitializeComponent();

        DataContext = new IconsViewModel();
    }
}
