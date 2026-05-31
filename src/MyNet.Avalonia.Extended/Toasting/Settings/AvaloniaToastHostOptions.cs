// -----------------------------------------------------------------------
// <copyright file="AvaloniaToastHostOptions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace MyNet.Avalonia.Extended.Toasting.Settings;

/// <summary>
/// Visual layout options for the Avalonia toast host.
/// </summary>
public sealed class AvaloniaToastHostOptions
{
    /// <summary>
    /// Gets the default host options.
    /// </summary>
    public static AvaloniaToastHostOptions Default { get; } = new();

    /// <summary>
    /// Gets or sets toast placement on the host window.
    /// </summary>
    public AvaloniaToastPosition Position { get; set; } = AvaloniaToastPosition.BottomRight;

    /// <summary>
    /// Gets or sets the maximum number of toast cards shown at once by the native manager.
    /// </summary>
    public int MaxItems { get; set; } = 3;

    /// <summary>
    /// Gets or sets the preferred toast content width in device-independent pixels.
    /// </summary>
    public double Width { get; set; } = 300;

    /// <summary>
    /// Gets or sets the horizontal margin offset from the window edge.
    /// </summary>
    public double OffsetX { get; set; } = 10;

    /// <summary>
    /// Gets or sets the vertical margin offset from the window edge.
    /// </summary>
    public double OffsetY { get; set; } = 10;
}
