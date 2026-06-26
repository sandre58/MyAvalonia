// -----------------------------------------------------------------------
// <copyright file="DownloadBusy.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.UI.Loading.Models;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

/// <summary>
/// Custom <see cref="IBusy"/> implementation used to demonstrate that <c>BusyServiceIndicator</c>
/// can render any busy model through its <c>BusyContentTemplates</c> collection.
/// </summary>
public sealed class DownloadBusy : Busy
{
    /// <summary>
    /// Gets or sets the name of the file being downloaded.
    /// </summary>
    public string? FileName { get; set => SetProperty(ref field, value); }

    /// <summary>
    /// Gets or sets the download progress as a fraction between 0 and 1.
    /// </summary>
    public double Percentage { get; set => SetProperty(ref field, value); }

    /// <summary>
    /// Gets or sets the pre-formatted "received / total" size text.
    /// </summary>
    public string? Sizes { get; set => SetProperty(ref field, value); }

    /// <summary>
    /// Gets or sets the pre-formatted transfer speed text.
    /// </summary>
    public string? Speed { get; set => SetProperty(ref field, value); }
}
