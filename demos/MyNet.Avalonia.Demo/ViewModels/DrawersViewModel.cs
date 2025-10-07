// -----------------------------------------------------------------------
// <copyright file="DrawersViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Threading.Tasks;
using System.Windows.Input;
using MyNet.Avalonia.Demo.ViewModels.Dialogs;
using MyNet.Avalonia.UI.Controls;
using MyNet.Avalonia.UI.Dialogs;
using MyNet.UI.Commands;
using MyNet.UI.ViewModels.Workspace;

namespace MyNet.Avalonia.Demo.ViewModels
{
    internal class DrawersViewModel : NavigableWorkspaceViewModel
    {
        public ICommand OpenCommand { get; set; }

        public DrawersViewModel() => OpenCommand = CommandsManager.Create<string>(async x => await ShowAsync().ConfigureAwait(false));

        private async Task ShowAsync()
        {
            var options = new DrawerOptions()
            {
                //FullScreen = FullScreen,
                //HorizontalAnchor = HorizontalAnchor,
                //VerticalAnchor = VerticalAnchor,
                //HorizontalOffset = HorizontalOffset,
                //VerticalOffset = VerticalOffset,
                //Mode = Mode,
                //Buttons = Button,
                //Title = Title,
                //CanLightDismiss = CanLightDismiss,
                //CanDragMove = CanDragMove,
                //IsCloseButtonVisible = IsCloseButtonVisible,
                //CanResize = CanResize,
                //Classes = Classes,
            };
            //string? dialogHostId = IsLocal ? DialogDemoViewModel.LocalHost : null;
            //if (IsModal)
            //{
            //    await OverlayDialog.ShowModal<DefaultDemoDialog, DefaultDemoDialogViewModel>(new DefaultDemoDialogViewModel(), dialogHostId, options: options);
            //}
            //else
            //{
            //await DrawerManager.ShowAsync(new LoginDialogViewModel(), options: options).ConfigureAwait(false);
            DrawerManager.ShowAsync(new LoginDialogViewModel(), options: options);
            //}
        }
    }
}
