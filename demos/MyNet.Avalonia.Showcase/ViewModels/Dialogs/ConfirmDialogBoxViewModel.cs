// -----------------------------------------------------------------------
// <copyright file="ConfirmDialogBoxViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Windows.Input;
using MyNet.UI.Commands;
using MyNet.UI.ViewModels.Dialog;

namespace MyNet.Avalonia.Showcase.ViewModels.Dialogs;

/// <summary>
/// Simple confirmation dialog demonstrating a themed <see cref="Extended.Controls.ContentDialog"/> in overlay mode.
/// </summary>
internal sealed class ConfirmDialogBoxViewModel : DialogViewModel<bool>
{
    public ConfirmDialogBoxViewModel(ICommandFactory commands, string message, string? title = null)
        : base(commands)
    {
        Message = message;
        Title = title;
        ConfirmCommand = commands.Create(() => Close(true));
        CancelCommand = commands.Create(Cancel);
    }

    public string Message { get; }

    public ICommand ConfirmCommand { get; }

    public ICommand CancelCommand { get; }

    private void Cancel()
    {
        SetCancelled();
        RequestClose();
    }
}
