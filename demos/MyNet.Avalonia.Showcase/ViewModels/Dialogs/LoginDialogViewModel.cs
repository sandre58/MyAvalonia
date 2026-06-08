// -----------------------------------------------------------------------
// <copyright file="LoginDialogViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Windows.Input;
using MyNet.Avalonia.Showcase.ViewModels.Samples;
using MyNet.UI.Commands;
using MyNet.UI.ViewModels.Dialog;

namespace MyNet.Avalonia.Showcase.ViewModels.Dialogs;

internal sealed class LoginDialogViewModel : DialogViewModel<LoginResult>
{
    public LoginDialogViewModel(ICommandFactory commandFactory)
        : base(commandFactory)
    {
        Form = new(commandFactory);

        ValidateCommand = commandFactory.Create(() =>
        {
            if (!Form.Submit())
            {
                return;
            }

            Close(new(Form.Login, Form.Password));
        });
    }

    public ICommand ValidateCommand { get; }

    public FormViewModel Form { get; }

    public bool CanResize { get; set; } = true;
}

internal record LoginResult(string Login, string Password);
