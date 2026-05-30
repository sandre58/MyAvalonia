// -----------------------------------------------------------------------
// <copyright file="WindowDialogOptions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Extended.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// Options for configuring a WindowDialog presentation.
/// </summary>
public class WindowDialogOptions
{
    internal static WindowDialogOptions Default { get; } = new();

    /// <summary>
    /// Gets or sets a value indicating whether drag move is enabled.
    /// </summary>
    public bool CanDragMove { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the dialog can be resized.
    /// </summary>
    public bool CanResize { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the dialog appears in the taskbar.
    /// </summary>
    public bool ShowInTaskbar { get; set; } = true;

    /// <summary>
    /// Gets or sets the startup location of the dialog window.
    /// </summary>
    public WindowStartupLocation StartupLocation { get; set; } = WindowStartupLocation.CenterOwner;

    /// <summary>
    /// Gets or sets the initial position of the dialog window (only when StartupLocation is Manual).
    /// </summary>
    public PixelPoint? Position { get; set; }

    /// <summary>
    /// Gets or sets the desired width of the dialog.
    /// </summary>
    public double? Width { get; set; }

    /// <summary>
    /// Gets or sets the desired height of the dialog.
    /// </summary>
    public double? Height { get; set; }

    /// <summary>
    /// Gets or sets the minimum width of the dialog.
    /// </summary>
    public double? MinWidth { get; set; }

    /// <summary>
    /// Gets or sets the minimum height of the dialog.
    /// </summary>
    public double? MinHeight { get; set; }

    /// <summary>
    /// Gets or sets a space-separated list of CSS style classes to apply.
    /// </summary>
    public string? StyleClass { get; set; }

    /// <summary>
    /// Gets or sets the window title.
    /// </summary>
    public string? Title { get; set; }
}


