// -----------------------------------------------------------------------
// <copyright file="LoginDialogView.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Extended.Controls;
using PropertyChanged;

namespace MyNet.Avalonia.Showcase.Views.Dialogs;

[DoNotNotify]
public partial class LoginDialogView : ContentDialog
{
    public LoginDialogView() => InitializeComponent();
}
