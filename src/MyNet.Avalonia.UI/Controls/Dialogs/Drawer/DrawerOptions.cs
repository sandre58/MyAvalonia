// -----------------------------------------------------------------------
// <copyright file="DrawerOptions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Controls.Enums;
using MyNet.UI.Dialogs.MessageBox;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.UI.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public class DrawerOptions
{
    internal static DrawerOptions Default => new();

    public Position Position { get; set; } = Position.Right;

    public bool CanLightDismiss { get; set; } = true;

    public bool? IsCloseButtonVisible { get; set; } = true;

    public double? MinWidth { get; set; }

    public double? MinHeight { get; set; }

    public double? MaxWidth { get; set; }

    public double? MaxHeight { get; set; }

    public MessageBoxResultOption Buttons { get; set; } = MessageBoxResultOption.OkCancel;

    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the hash code of the top level dialog host. This is used to identify the dialog host if there are multiple dialog hosts with the same id. If this is not provided, the dialog will be added to the first dialog host with the same id.
    /// </summary>
    public int? TopLevelHashCode { get; set; }

    public bool CanResize { get; set; }

    public string? StyleClass { get; set; }
}
