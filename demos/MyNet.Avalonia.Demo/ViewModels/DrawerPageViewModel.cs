// -----------------------------------------------------------------------
// <copyright file="DrawerPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Threading.Tasks;
using System.Windows.Input;
using MyNet.Avalonia.Demo.ViewModels.Dialogs;
using MyNet.Avalonia.Extended.Controls;
using MyNet.Avalonia.Extended.Dialogs;
using MyNet.Avalonia.Theme.Classes.Enums;
using MyNet.UI.Commands;

namespace MyNet.Avalonia.Demo.ViewModels
{
    internal sealed class DrawerPageViewModel : PageViewModel
    {
        public ICommand OpenCommand { get; set; }

        public DrawerPageViewModel() => OpenCommand = CommandsManager.Create<string>(async _ => await ShowAsync().ConfigureAwait(false));

        /// <inheritdoc/>
        public override IconData Icon => IconData.ViewSplitVertical;

        private async Task ShowAsync()
        {
            var options = new DrawerOptions
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
