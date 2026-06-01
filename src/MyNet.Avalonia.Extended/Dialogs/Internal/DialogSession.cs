// -----------------------------------------------------------------------
// <copyright file="DialogSession.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Extended.Controls;

namespace MyNet.Avalonia.Extended.Dialogs.Internal;

public sealed class DialogSession(Action cleanup)
{
    public OverlayDialog? Overlay { get; init; }

    public WindowDialog? Window { get; init; }

    public void CloseVisual(object? result = null)
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

    public void Dispose() => cleanup();
}
