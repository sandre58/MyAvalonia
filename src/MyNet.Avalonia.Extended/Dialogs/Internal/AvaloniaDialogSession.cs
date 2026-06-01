// -----------------------------------------------------------------------
// <copyright file="AvaloniaDialogSession.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using MyNet.Avalonia.Controls.Primitives;
using MyNet.Avalonia.Extended.Controls;

namespace MyNet.Avalonia.Extended.Dialogs.Internal;

public sealed class AvaloniaDialogSession(Action cleanup)
{
    public OverlayDialog? Overlay { get; init; }

    public WindowDialog? Window { get; init; }

    public void CloseVisual(object? result = null)
    {
        if (Overlay is AvaloniaContentOverlayDialog contentOverlay)
        {
            contentOverlay.CloseWithResult(result);
            return;
        }

        if (Overlay is OverlayMessageBox messageBox)
        {
            messageBox.Close();
            return;
        }

        Overlay?.Close();

        if (Window is not null)
        {
            if (result is not null)
                Window.Close(result);
            else
                Window.Close();
        }
    }

    public void Dispose()
    {
        cleanup();
    }
}
