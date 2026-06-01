// -----------------------------------------------------------------------
// <copyright file="DialogPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Material.Icons;
using MyNet.Avalonia.Extended.Dialogs;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ViewModels.Base;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.UI.Dialogs.ContentDialogs;
using MyNet.UI.Dialogs.MessageBox;
using MyNet.UI.Locators.Factories;
using MyNet.UI.Notifications;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class DialogPageViewModel : ShowcaseViewModel
{
    public DialogPageViewModel(
        INotificationPublisher notificationPublisher,
        IContentDialogService contentDialogService,
        IMessageBoxFactory messageBoxFactory,
        IViewFactory viewFactory,
        AvaloniaDialogHostOptions hostOptions)
        : base("Dialogs", [new ControlThemeBuilder()])
    {
        WindowGroup = new WindowDialogGroupViewModel(
            notificationPublisher,
            contentDialogService,
            messageBoxFactory,
            viewFactory,
            hostOptions);
        OverlayGroup = new OverlayDialogGroupViewModel(
            notificationPublisher,
            contentDialogService,
            messageBoxFactory,
            viewFactory,
            hostOptions);
    }

    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.DockWindow;

    /// <summary>
    /// Gets the group view model for Window-based dialogs (Window Dialog and Window MessageBox).
    /// </summary>
    public WindowDialogGroupViewModel WindowGroup { get; }

    /// <summary>
    /// Gets the group view model for Overlay-based dialogs (Overlay Dialog, Overlay MessageBox and Overlay DialogBox).
    /// </summary>
    public OverlayDialogGroupViewModel OverlayGroup { get; }

    /// <summary>
    /// Performs cleanup operations when the view model is disposed.
    /// </summary>
    protected override void Cleanup()
    {
        WindowGroup.Dispose();
        OverlayGroup.Dispose();
        base.Cleanup();
    }
}
