// -----------------------------------------------------------------------
// <copyright file="PerfDialogViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.ObjectModel;
using MyNet.UI.ViewModels.Dialogs;
using MyNet.Utilities.Helpers;

namespace MyNet.Avalonia.Demo.ViewModels.Dialogs;

internal class PerfDialogViewModel : DialogViewModel
{
    public ObservableCollection<int>? List { get; } = new(EnumerableHelper.Range(1, 1000, 1));
}
