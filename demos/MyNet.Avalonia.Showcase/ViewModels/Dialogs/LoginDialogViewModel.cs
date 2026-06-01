// -----------------------------------------------------------------------
// <copyright file="LoginDialogViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Showcase.ViewModels.Samples;
using MyNet.UI.ViewModels.Dialog;

namespace MyNet.Avalonia.Showcase.ViewModels.Dialogs;

internal sealed class LoginDialogViewModel : DialogViewModel
{
    public FormViewModel Form { get; set; } = new();
}
