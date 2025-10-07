// -----------------------------------------------------------------------
// <copyright file="OverlayDialogOptions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Controls.Enums;
using MyNet.UI.Dialogs.MessageBox;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.UI.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public class OverlayDialogOptions
{
    internal static OverlayDialogOptions Default { get; } = new();

    public bool FullScreen { get; set; }

    public HorizontalPosition HorizontalAnchor { get; set; } = HorizontalPosition.Center;

    public VerticalPosition VerticalAnchor { get; set; } = VerticalPosition.Center;

    /// <summary>
    ///     Gets or sets this attribute is only used when HorizontalAnchor is not Center.
    /// </summary>
    public double? HorizontalOffset { get; set; }

    /// <summary>
    ///     Gets or sets this attribute is only used when VerticalAnchor is not Center.
    /// </summary>
    public double? VerticalOffset { get; set; }

    /// <summary>
    ///     Gets or sets only works for DefaultDialogControl.
    /// </summary>
    public MessageSeverity Severity { get; set; } = MessageSeverity.Custom;

    /// <summary>
    ///     Gets or sets only works for DefaultDialogControl.
    /// </summary>
    public MessageBoxResultOption Buttons { get; set; } = MessageBoxResultOption.OkCancel;

    /// <summary>
    ///     Gets or sets only works for DefaultDialogControl.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    ///     Gets or sets only works for CustomDialogControl.
    /// </summary>
    public bool? IsCloseButtonVisible { get; set; } = true;

    public bool CanLightDismiss { get; set; }

    public bool CanDragMove { get; set; } = true;

    /// <summary>
    ///     Gets or sets the hash code of the top level dialog host. This is used to identify the dialog host if there are multiple dialog
    ///     hosts with the same id. If this is not provided, the dialog will be added to the first dialog host with the same
    ///     id.
    /// </summary>
    public int? TopLevelHashCode { get; set; }

    public bool CanResize { get; set; }

    public string? StyleClass { get; set; }
}
