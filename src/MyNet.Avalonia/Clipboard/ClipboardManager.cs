// -----------------------------------------------------------------------
// <copyright file="ClipboardManager.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Avalonia.Input;

namespace MyNet.Avalonia.Clipboard;

/// <summary>
/// Static facade used by Theme XAML commands to access the registered clipboard service.
/// </summary>
public static class ClipboardManager
{
    private static IClipboardService? _clipboardService;

    /// <summary>
    /// Connects the static facade to the DI-registered clipboard service.
    /// </summary>
    public static void Configure(IClipboardService clipboardService)
        => _clipboardService = clipboardService ?? throw new ArgumentNullException(nameof(clipboardService));

    /// <summary>
    /// Copies rich clipboard content through the registered service.
    /// </summary>
    public static async Task CopyAsync(IAsyncDataTransfer content)
    {
        EnsureInitialized();
        await _clipboardService!.CopyAsync(content).ConfigureAwait(false);
    }

    /// <summary>
    /// Copies plain text through the registered service.
    /// </summary>
    public static async Task CopyTextAsync(string text)
    {
        EnsureInitialized();
        await _clipboardService!.CopyTextAsync(text).ConfigureAwait(false);
    }

    private static void EnsureInitialized()
    {
        if (_clipboardService is null)
        {
            throw new InvalidOperationException(
                "Clipboard is not initialized. Register IClipboardService and call UseAvaloniaClipboard() on the built IServiceProvider.");
        }
    }
}
