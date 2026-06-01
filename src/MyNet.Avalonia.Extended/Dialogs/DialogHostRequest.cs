// -----------------------------------------------------------------------
// <copyright file="DialogHostRequest.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using MyNet.Avalonia.Extended.Controls;

namespace MyNet.Avalonia.Extended.Dialogs;

/// <summary>
/// Carries Avalonia-specific presentation settings through <see cref="MyNet.UI.Dialogs.ContentDialogs.DialogOptions.Owner"/>.
/// </summary>
public sealed class DialogHostRequest
{
    /// <summary>
    /// Gets the presentation surface to use.
    /// </summary>
    public DialogPresentationMode Mode { get; init; } = DialogPresentationMode.Overlay;

    /// <summary>
    /// Gets overlay-specific layout options when <see cref="Mode"/> is <see cref="DialogPresentationMode.Overlay"/>.
    /// </summary>
    public OverlayDialogOptions? OverlayOptions { get; init; }

    /// <summary>
    /// Gets the overlay host identifier.
    /// </summary>
    public string? OverlayHostId { get; init; }

    /// <summary>
    /// Gets the owner window when <see cref="Mode"/> is <see cref="DialogPresentationMode.Window"/>.
    /// </summary>
    public Window? WindowOwner { get; init; }
}
