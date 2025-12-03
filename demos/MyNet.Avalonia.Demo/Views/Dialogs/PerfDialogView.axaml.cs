// -----------------------------------------------------------------------
// <copyright file="PerfDialogView.axaml.cs" company="St�phane ANDRE">
// Copyright (c) St�phane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Markup.Xaml;
using MyNet.Avalonia.Extended.Controls;
using PropertyChanged;

namespace MyNet.Avalonia.Demo.Views.Dialogs;

[DoNotNotify]
public partial class PerfDialogView : ContentDialog
{
    public PerfDialogView() => AvaloniaXamlLoader.Load(this);
}
