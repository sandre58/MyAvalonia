// -----------------------------------------------------------------------
// <copyright file="DialogOptionsFactory.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using MyNet.Avalonia.Extended.Controls;
using MyNet.UI.Dialogs.ContentDialogs;

namespace MyNet.Avalonia.Extended.Dialogs;

/// <summary>
/// Factory helpers for Avalonia <see cref="UI.Dialogs.ContentDialogs.DialogOptions"/>.
/// </summary>
public static class DialogOptionsFactory
{
    /// <summary>
    /// Creates options that present the dialog inside an overlay host.
    /// </summary>
    public static DialogOptions ForOverlay(
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
    /// Creates options that present the dialog in a dedicated window.
    /// </summary>
    public static DialogOptions ForWindow(
        IDialog dialog,
        bool isModal = true,
        Window? owner = null,
        WindowDialogOptions? windowOptions = null)
        => new()
        {
            Dialog = dialog,
            IsModal = isModal,
            Title = windowOptions?.Title ?? dialog.Title,
            Owner = new DialogHostRequest
            {
                Mode = DialogPresentationMode.Window,
                WindowOwner = owner,
                WindowOptions = windowOptions
            }
        };

    /// <summary>
    /// Resolves the Avalonia host request from <paramref name="options"/>.
    /// </summary>
    public static DialogHostRequest Resolve(DialogOptions? options)
        => options?.Owner as DialogHostRequest
           ?? new DialogHostRequest();
}
