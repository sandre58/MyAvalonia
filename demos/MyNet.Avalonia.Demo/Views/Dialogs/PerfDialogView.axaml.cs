// -----------------------------------------------------------------------
// <copyright file="PerfDialogView.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Markup.Xaml;
using MyNet.Avalonia.UI.Controls;
using PropertyChanged;

namespace MyNet.Avalonia.Demo.Views.Dialogs;

[DoNotNotify]
public partial class PerfDialogView : ContentDialog
{
    public PerfDialogView() => AvaloniaXamlLoader.Load(this);
}
