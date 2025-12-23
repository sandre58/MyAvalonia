// -----------------------------------------------------------------------
// <copyright file="ExpanderPage.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Avalonia.Interactivity;
using MyNet.Avalonia.Extensions;

namespace MyNet.Avalonia.Demo.Pages;

internal sealed partial class ExpanderPage : Page
{
    public ExpanderPage() => InitializeComponent();

    private void Expand_Click(object? sender, RoutedEventArgs e) => this.ExecuteOnChildren<Expander>(x => x.IsExpanded = true);

    private void Collapse_Click(object? sender, RoutedEventArgs e) => this.ExecuteOnChildren<Expander>(x => x.IsExpanded = false);
}
