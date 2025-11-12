// -----------------------------------------------------------------------
// <copyright file="DialogsViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Windows.Input;
using MyNet.Avalonia.Demo.ViewModels.Dialogs;
using MyNet.Avalonia.Demo.Views.Dialogs;
using MyNet.Avalonia.UI.Controls;
using MyNet.Avalonia.UI.Dialogs;
using MyNet.UI.Commands;
using MyNet.UI.Toasting;
using MyNet.UI.ViewModels.Workspace;

namespace MyNet.Avalonia.Demo.ViewModels;

internal class DialogsViewModel : NavigableWorkspaceViewModel
{
    private readonly WindowDialogService _windowDialogService = new();
    private readonly OverlayDialogService _overlayDialogService = new();

    // WindowDialogService Commands
    public ICommand OpenWindowDialogModalCommand { get; set; }

    public ICommand OpenWindowDialogNonModalCommand { get; set; }

    public ICommand OpenWindowPerfDialogCommand { get; set; }

    // OverlayDialogService Commands
    public ICommand OpenOverlayDialogModalCommand { get; set; }

    public ICommand OpenOverlayDialogNonModalCommand { get; set; }

    public ICommand OpenOverlayPerfDialogCommand { get; set; }

    // Settings Properties
    public bool ShowCloseButton { get; set; } = true;

    public bool CanDragMove { get; set; } = true;

    public bool CanResize { get; set; } = true;

    public bool FullScreen { get; set; }

    public DialogsViewModel()
    {
        // WindowDialogService Commands
        OpenWindowDialogModalCommand = CommandsManager.Create(async () =>
        {
            var vm = new LoginDialogViewModel();
            var view = new LoginDialogView { DataContext = vm };

            var result = await _windowDialogService.ShowModalAsync(view, vm).ConfigureAwait(false);

            ShowToasterResult(vm);
        });

        OpenWindowDialogNonModalCommand = CommandsManager.Create(async () =>
        {
            var vm = new LoginDialogViewModel();
            var view = new LoginDialogView { DataContext = vm };

            await _windowDialogService.ShowAsync(view, vm).ConfigureAwait(false);
        });

        OpenWindowPerfDialogCommand = CommandsManager.Create(async () =>
        {
            using var vm = new PerfDialogViewModel();
            var view = new PerfDialogView { DataContext = vm };

            var result = await _windowDialogService.ShowModalAsync(view, vm).ConfigureAwait(false);
        });

        // OverlayDialogService Commands
        OpenOverlayDialogModalCommand = CommandsManager.Create(async () =>
        {
            var vm = new LoginDialogViewModel();
            var view = new LoginDialogView { DataContext = vm };

            var options = new OverlayDialogOptions
            {
                IsCloseButtonVisible = ShowCloseButton,
                CanDragMove = CanDragMove,
                CanResize = CanResize,
                FullScreen = FullScreen
            };

            var result = await _overlayDialogService.ShowDialogCoreAsync(view, vm, null, options).ConfigureAwait(false);

            ShowToasterResult(vm);
        });

        OpenOverlayDialogNonModalCommand = CommandsManager.Create(async () =>
        {
            var vm = new LoginDialogViewModel();
            var view = new LoginDialogView { DataContext = vm };

            var options = new OverlayDialogOptions
            {
                IsCloseButtonVisible = ShowCloseButton,
                CanDragMove = CanDragMove,
                CanResize = CanResize,
                FullScreen = FullScreen
            };

            _overlayDialogService.Show(view, vm, null, options);

            await System.Threading.Tasks.Task.CompletedTask.ConfigureAwait(false);
        });

        OpenOverlayPerfDialogCommand = CommandsManager.Create(async () =>
        {
            using var vm = new PerfDialogViewModel();
            var view = new PerfDialogView { DataContext = vm };

            var options = new OverlayDialogOptions
            {
                IsCloseButtonVisible = ShowCloseButton,
                CanDragMove = CanDragMove,
                CanResize = CanResize,
                FullScreen = FullScreen
            };

            var result = await _overlayDialogService.ShowDialogCoreAsync(view, vm, null, options).ConfigureAwait(false);
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

        ToasterManager.ShowInformation($"Login : {viewModel.Form.Login} ; Password : {viewModel.Form.Password}");
    }
}
