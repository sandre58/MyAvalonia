// -----------------------------------------------------------------------
// <copyright file="OverlayDialogLayerChangeEventArgs.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Interactivity;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.UI.Controls.Primitives;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public class OverlayDialogLayerChangeEventArgs : RoutedEventArgs
{
    public OverlayDialogLayerChangeType ChangeType { get; }

    public OverlayDialogLayerChangeEventArgs(OverlayDialogLayerChangeType type) => ChangeType = type;

    public OverlayDialogLayerChangeEventArgs(RoutedEvent routedEvent, OverlayDialogLayerChangeType type)
        : base(routedEvent) => ChangeType = type;
}
