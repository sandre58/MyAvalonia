// -----------------------------------------------------------------------
// <copyright file="DialogPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Material.Icons;
using MyNet.Avalonia.Extended.Dialogs;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.UI.Commands;
using MyNet.UI.Dialogs.ContentDialogs;
using MyNet.UI.Dialogs.MessageBox;
using MyNet.UI.Notifications;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class DialogPageViewModel(
    INotificationPublisher notificationPublisher,
    IContentDialogService contentDialogService,
    IMessageBoxFactory messageBoxFactory,
    ICommandFactory commands,
    DialogHostOptions hostOptions)
    : ShowcaseViewModel("Dialogs", commands, [new()])
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.DockWindow;

    /// <summary>
    /// Gets the group view model for Window-based dialogs (Window Dialog and Window MessageBox).
    /// </summary>
    public WindowDialogGroupViewModel WindowGroup { get; } = new(
        notificationPublisher,
        contentDialogService,
        messageBoxFactory,
        commands);

    /// <summary>
    /// Gets the group view model for Overlay-based dialogs (Overlay Dialog, Overlay MessageBox and Overlay DialogBox).
    /// </summary>
    public OverlayDialogGroupViewModel OverlayGroup { get; } = new(
        notificationPublisher,
        contentDialogService,
        messageBoxFactory,
        commands,
        hostOptions);

    /// <summary>
    /// Performs cleanup operations when the view model is disposed.
    /// </summary>
    protected override void DisposeManagedResources()
    {
        WindowGroup.Dispose();
        OverlayGroup.Dispose();
        base.DisposeManagedResources();
    }
}
