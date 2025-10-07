// -----------------------------------------------------------------------
// <copyright file="ClipboardManager.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Input;
using MyNet.Avalonia.Commands;

namespace MyNet.Avalonia.Clipboard;

public static class ClipboardManager
{
    private static IClipboardService? _clipboardService;

    public static ICommand? CopyTextCommand { get; set; }

    public static ICommand? CopyCommand { get; set; }

    public static void Initialize(IClipboardService clipboardService)
    {
        _clipboardService = clipboardService;
        CopyTextCommand = ActionCommand.Create<string>(async x => await CopyTextAsync(x).ConfigureAwait(false));
        CopyCommand = ActionCommand.Create<IDataObject>(async x => await CopyAsync(x).ConfigureAwait(false));
    }

    public static async Task CopyAsync(IDataObject content)
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
