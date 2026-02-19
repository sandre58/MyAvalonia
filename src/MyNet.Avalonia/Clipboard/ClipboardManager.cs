// -----------------------------------------------------------------------
// <copyright file="ClipboardManager.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Threading.Tasks;
using Avalonia.Input;

namespace MyNet.Avalonia.Clipboard;

public static class ClipboardManager
{
    private static IClipboardService? _clipboardService;

    public static void Initialize(IClipboardService clipboardService) => _clipboardService = clipboardService;

    public static async Task CopyAsync(IAsyncDataTransfer content)
    {
        if (_clipboardService is not { } clipboardService) return;

        await clipboardService.CopyAsync(content).ConfigureAwait(false);
    }

    public static async Task CopyTextAsync(string text)
    {
        if (_clipboardService is not { } clipboardService) return;

        await clipboardService.CopyTextAsync(text).ConfigureAwait(false);
    }
}
