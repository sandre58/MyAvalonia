// -----------------------------------------------------------------------
// <copyright file="LoginDialogViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.UI.ViewModels.Dialogs;

namespace MyNet.Avalonia.Demo.ViewModels.Dialogs;

internal sealed class LoginDialogViewModel : DialogViewModel
{
    public FormsViewModel Form { get; set; } = new();
}
