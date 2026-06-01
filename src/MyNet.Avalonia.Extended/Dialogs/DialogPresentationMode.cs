// -----------------------------------------------------------------------
// <copyright file="DialogPresentationMode.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.Avalonia.Extended.Dialogs;

/// <summary>
/// Selects the Avalonia surface used to present a dialog.
/// </summary>
public enum DialogPresentationMode
{
    /// <summary>
    /// Presents inside an <see cref="MyNet.Avalonia.Controls.OverlayDialogHost"/>.
    /// </summary>
    Overlay,

    /// <summary>
    /// Presents inside a modal <see cref="Avalonia.Controls.Window"/>.
    /// </summary>
    Window
}
