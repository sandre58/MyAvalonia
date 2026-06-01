// -----------------------------------------------------------------------
// <copyright file="AvaloniaContentOverlayDialog.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Controls;

namespace MyNet.Avalonia.Extended.Dialogs.Internal;

/// <summary>
/// Overlay shell that raises a typed close result for content dialogs.
/// </summary>
internal sealed class AvaloniaContentOverlayDialog : OverlayDialog
{
    public override void Close() => CloseWithResult(null);

    public void CloseWithResult(object? result) => OnElementClosing(this, result);
}
