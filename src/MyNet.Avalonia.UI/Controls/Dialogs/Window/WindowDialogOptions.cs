// -----------------------------------------------------------------------
// <copyright file="WindowDialogOptions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.UI.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public class WindowDialogOptions
{
    internal static WindowDialogOptions Default { get; } = new WindowDialogOptions();

    public WindowStartupLocation StartupLocation { get; set; } = WindowStartupLocation.CenterOwner;

    public PixelPoint? Position { get; set; }

    public string? Title { get; set; }

    public bool IsCloseButtonVisible { get; set; } = true;

    public bool ShowInTaskBar { get; set; } = true;

    public bool CanDragMove { get; set; } = true;

    public bool CanResize { get; set; }

    public string? Classes { get; set; }
}
