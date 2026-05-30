// -----------------------------------------------------------------------
// <copyright file="IClipboardService.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Threading.Tasks;
using Avalonia.Input;

namespace MyNet.Avalonia.Clipboard;

/// <summary>
/// Platform clipboard copy operations for Avalonia hosts.
/// </summary>
public interface IClipboardService
{
    /// <summary>
    /// Copies rich clipboard content.
    /// </summary>
    Task CopyAsync(IAsyncDataTransfer content);

    /// <summary>
    /// Copies plain text to the clipboard.
    /// </summary>
    Task CopyTextAsync(string text);
}
