// -----------------------------------------------------------------------
// <copyright file="OverlayContentDialog.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Controls;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Extended.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// Overlay shell that raises a typed close result for content dialogs.
/// </summary>
internal sealed class OverlayContentDialog : OverlayDialog
{
    public override void Close() => CloseWithResult(null);

    public void CloseWithResult(object? result) => OnElementClosing(this, result);
}
