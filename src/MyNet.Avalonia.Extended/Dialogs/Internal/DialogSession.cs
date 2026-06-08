// -----------------------------------------------------------------------
// <copyright file="DialogSession.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Extended.Controls;
using MyNet.Avalonia.Threading;

namespace MyNet.Avalonia.Extended.Dialogs.Internal;

/// <summary>
/// Represents an active dialog session, which may include an overlay dialog and/or a window dialog. This class provides methods to close the visual representation of the dialog and to dispose of the session when it is no longer needed.
/// </summary>
/// <param name="cleanup">The action to perform when disposing of the session.</param>
/// <param name="uiThread">The UI thread dispatcher used to safely close visuals.</param>
internal sealed class DialogSession(Action cleanup, IUiThreadDispatcher uiThread)
{
    /// <summary>
    /// Gets the overlay dialog associated with this session, if any. The overlay dialog is typically used to display content on top of the main application window, such as a modal dialog or a message box.
    /// </summary>
    public OverlayDialog? Overlay { get; init; }

    /// <summary>
    /// Gets the window dialog associated with this session, if any. The window dialog is typically used to display a separate window for the dialog content, which can be moved and resized independently of the main application window.
    /// </summary>
    public WindowDialog? Window { get; init; }

    /// <summary>
    /// Closes the visual representation of the dialog session, including both the overlay and window dialogs if they are present. If a result is provided, it will be passed to the window dialog when closing it. This method ensures that all visual elements associated with the dialog session are properly closed and cleaned up.
    /// </summary>
    /// <param name="result">The result to pass to the window dialog when closing it, if applicable.</param>
    public void CloseVisual(object? result = null) => uiThread.Post(() => CloseVisualCore(result));

    /// <summary>
    /// Core method that performs the actual closing of the visual elements associated with the dialog session. This method is intended to be called on the UI thread to ensure that all UI operations are performed safely. It checks for the presence of both the overlay and window dialogs and closes them accordingly, passing any provided result to the window dialog if it exists.
    /// </summary>
    /// <param name="result">The result to pass to the window dialog when closing it, if applicable.</param>
    private void CloseVisualCore(object? result)
    {
        switch (Overlay)
        {
            case ContentOverlayDialog contentOverlay:
                contentOverlay.CloseWithResult(result);
                return;
            case OverlayMessageBox messageBox:
                messageBox.Close();
                return;
        }

        Overlay?.Close();

        if (Window is not null)
        {
            if (result is not null)
                Window.CloseWithResult(result);
            else
                Window.Close();
        }
    }

    /// <summary>
    /// Disposes of the dialog session by invoking the cleanup action provided during construction. This method should be called when the dialog session is no longer needed to ensure that any resources associated with the session are properly released. The cleanup action is responsible for removing the session from any registries or collections that track active sessions, as well as performing any additional cleanup tasks necessary to maintain the integrity of the application state.
    /// </summary>
    public void Dispose() => cleanup();
}
