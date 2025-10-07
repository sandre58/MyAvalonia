// -----------------------------------------------------------------------
// <copyright file="MainWindow.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.UI.Controls;
using PropertyChanged;

namespace MyNet.Avalonia.Demo.Views;

[DoNotNotify]
public partial class MainWindow : ExtendedWindow
{
    public MainWindow() => InitializeComponent();
}
