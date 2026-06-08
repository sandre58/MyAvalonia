// -----------------------------------------------------------------------
// <copyright file="PerfDialogView.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Markup.Xaml;
using MyNet.Avalonia.Controls;

namespace MyNet.Avalonia.Showcase.Views.Dialogs;

public partial class PerfDialogView : ContentDialog
{
    public PerfDialogView() => AvaloniaXamlLoader.Load(this);
}
