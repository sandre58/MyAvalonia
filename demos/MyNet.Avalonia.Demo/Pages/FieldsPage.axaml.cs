// -----------------------------------------------------------------------
// <copyright file="FieldsPage.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using MyNet.Avalonia.Extensions;
using MyNet.Utilities;

namespace MyNet.Avalonia.Demo.Pages;

internal sealed partial class FieldsPage : Page
{
    public FieldsPage() => InitializeComponent();

    private void IsPassword_IsCheckedChanged(object? sender, global::Avalonia.Interactivity.RoutedEventArgs e)
        => this.ExecuteOnChildren<TextBox>(x => x.PasswordChar = IsPassword.IsChecked.IsTrue() ? '*' : '\0');

}
