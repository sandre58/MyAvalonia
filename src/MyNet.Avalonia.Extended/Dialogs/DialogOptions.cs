// -----------------------------------------------------------------------
// <copyright file="DialogOptions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using MyNet.Avalonia.Extended.Controls;
using MyNet.UI.Dialogs.ContentDialogs;

namespace MyNet.Avalonia.Extended.Dialogs;

/// <summary>
/// Factory helpers for Avalonia-specific <see cref="UI.Dialogs.ContentDialogs.DialogOptions"/>.
/// </summary>
public static class DialogOptions
{
    /// <summary>
    /// Creates options that present the dialog inside an overlay host.
    /// </summary>
    public static UI.Dialogs.ContentDialogs.DialogOptions ForOverlay(
        IDialog dialog,
        bool isModal = true,
        OverlayDialogOptions? overlayOptions = null,
        string? hostId = null)
        => new()
        {
            Dialog = dialog,
            IsModal = isModal,
            Title = overlayOptions?.Title ?? dialog.Title,
            CloseOnOverlayClick = overlayOptions?.CanLightDismiss ?? false,
            Owner = new DialogHostRequest
            {
                Mode = DialogPresentationMode.Overlay,
                OverlayOptions = overlayOptions,
                OverlayHostId = hostId
            }
        };

    /// <summary>
    /// Creates options that present the dialog inside a modal window.
    /// </summary>
    public static UI.Dialogs.ContentDialogs.DialogOptions ForWindow(
        IDialog dialog,
        bool isModal = true,
        Window? owner = null)
        => new()
        {
            Dialog = dialog,
            IsModal = isModal,
            Title = dialog.Title,
            Owner = new DialogHostRequest
            {
                Mode = DialogPresentationMode.Window,
                WindowOwner = owner
            }
        };

    /// <summary>
    /// Resolves the Avalonia host request from <paramref name="options"/>.
    /// </summary>
    public static DialogHostRequest Resolve(UI.Dialogs.ContentDialogs.DialogOptions? options)
        => options?.Owner as DialogHostRequest
           ?? new DialogHostRequest();
}
