// -----------------------------------------------------------------------
// <copyright file="DialogsViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Windows.Input;
using MyNet.Avalonia.Demo.Services;
using MyNet.Avalonia.Demo.ViewModels.Dialogs;
using MyNet.UI.Commands;
using MyNet.UI.Toasting;
using MyNet.UI.ViewModels.Workspace;

namespace MyNet.Avalonia.Demo.ViewModels;

internal class DialogsViewModel : NavigableWorkspaceViewModel
{
    public ICommand OpenContentDialogCommand { get; set; }

    public ICommand OpenCustomNonDialogCommand { get; set; }

    public ICommand OpenPerfNonDialogCommand { get; set; }

    public ICommand OpenPerfDialogCommand { get; set; }

    public DialogsViewModel()
    {
        OpenContentDialogCommand = CommandsManager.Create(async () =>
        {
            var vm = new LoginDialogViewModel();
            await WindowDialogManager.ShowDialogAsync(vm).ConfigureAwait(false);

            ShowToasterResult(vm);
        });

        OpenCustomNonDialogCommand = CommandsManager.Create(async () =>
        {
            var vm = new LoginDialogViewModel();
            await WindowDialogManager.ShowAsync(vm, x => ShowToasterResult(x)).ConfigureAwait(false);
        });

        OpenPerfNonDialogCommand = CommandsManager.Create(async () =>
        {
            using var vm = new PerfDialogViewModel();
            await WindowDialogManager.ShowAsync(vm).ConfigureAwait(false);
        });

        OpenPerfDialogCommand = CommandsManager.Create(async () =>
        {
            using var vm = new PerfDialogViewModel();
            await WindowDialogManager.ShowDialogAsync(vm).ConfigureAwait(false);
        });
    }

    private static void ShowToasterResult(LoginDialogViewModel viewModel)
    {
        if (!viewModel.DialogResult.HasValue)
            ToasterManager.ShowWarning("No result.");
        else if (viewModel.DialogResult.Value)
            ToasterManager.ShowSuccess("Dialog has been validated.");
        else
            ToasterManager.ShowError("Dialog has been cancelled");

        ToasterManager.ShowInformation($"Login : {viewModel.Login} ; Password : {viewModel.Password}");
    }
}
