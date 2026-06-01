// -----------------------------------------------------------------------
// <copyright file="PerfDialogViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.ObjectModel;
using System.Linq;
using MyNet.UI.ViewModels.Dialog;

namespace MyNet.Avalonia.Showcase.ViewModels.Dialogs;

internal sealed class PerfDialogViewModel : DialogViewModel
{
    public ObservableCollection<int>? List { get; } = new(Enumerable.Range(1, 1000));
}
