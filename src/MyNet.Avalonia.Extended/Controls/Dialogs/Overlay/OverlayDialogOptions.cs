// -----------------------------------------------------------------------
// <copyright file="OverlayDialogOptions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Controls.Enums;
using MyNet.UI.Dialogs.MessageBox;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Extended.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// Defines options for configuring the appearance and behavior of an overlay dialog, including layout, size, and message box settings.
/// </summary>
public class OverlayDialogOptions
{
    internal static OverlayDialogOptions Default { get; } = new();

    /// <summary>
    /// Gets or sets a value indicating whether the dialog should be displayed in full screen mode. When set to true, the dialog will cover the entire host area and ignore anchor and offset settings.
    /// </summary>
    public bool FullScreen { get; set; }

    /// <summary>
    /// Gets or sets the horizontal anchor position of the dialog. Only used when <see cref="FullScreen"/> is false.
    /// </summary>
    public HorizontalPosition HorizontalAnchor { get; set; } = HorizontalPosition.Center;

    /// <summary>
    /// Gets or sets the vertical anchor position of the dialog. Only used when <see cref="FullScreen"/> is false.
    /// </summary>
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
    ///     Gets or sets the desired width of the dialog.
    /// </summary>
    public double? Width { get; set; }

    /// <summary>
    ///     Gets or sets the desired height of the dialog.
    /// </summary>
    public double? Height { get; set; }

    /// <summary>
    ///     Gets or sets the minimum width of the dialog.
    /// </summary>
    public double? MinWidth { get; set; }

    /// <summary>
    ///     Gets or sets the minimum height of the dialog.
    /// </summary>
    public double? MinHeight { get; set; }

    /// <summary>
    ///     Gets or sets the maximum width of the dialog.
    /// </summary>
    public double? MaxWidth { get; set; }

    /// <summary>
    ///     Gets or sets the maximum height of the dialog.
    /// </summary>
    public double? MaxHeight { get; set; }

    /// <summary>
    /// Gets or sets severity for <see cref="OverlayMessageBox"/> when not <see cref="MessageSeverity.Custom"/>.
    /// </summary>
    public MessageSeverity Severity { get; set; } = MessageSeverity.Custom;

    /// <summary>
    /// Gets or sets buttons for <see cref="OverlayMessageBox"/> when not the default <see cref="MessageBoxResultOption.OkCancel"/>.
    /// </summary>
    public MessageBoxResultOption Buttons { get; set; } = MessageBoxResultOption.OkCancel;

    /// <summary>
    /// Gets or sets the overlay title when provided (content and message box shells).
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    ///     Gets or sets only works for ContentDialogControl.
    /// </summary>
    public bool? IsCloseButtonVisible { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the dialog can be dismissed by clicking outside of it (light dismiss). When set to true, clicking outside the dialog will close it. Only applicable when <see cref="IsModal"/> is false.
    /// </summary>
    public bool CanLightDismiss { get; set; }

    /// <summary>
    /// Gets or sets the stable top-level key from <see cref="MyNet.Avalonia.Controls.OverlayDialogHostManager.GetTopLevelKey"/>.
    /// Used to target a specific window when several hosts share the same <c>HostId</c>.
    /// </summary>
    public int? TopLevelKey { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the overlay dialog chrome can be dragged by its title area.
    /// </summary>
    public bool CanDragMove { get; set; } = true;

    /// <summary>
    /// Gets or sets the style class applied to the dialog's root element, allowing for custom styling of the overlay dialog.
    /// </summary>
    public string? StyleClass { get; set; }
}
