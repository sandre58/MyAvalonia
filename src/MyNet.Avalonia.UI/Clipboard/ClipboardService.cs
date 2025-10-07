// -----------------------------------------------------------------------
// <copyright file="ClipboardService.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Input.Platform;
using MyNet.Avalonia.Clipboard;
using MyNet.UI.Resources;
using MyNet.UI.Toasting;

namespace MyNet.Avalonia.UI.Clipboard;

public class ClipboardService(Func<TopLevel?> topLevel) : IClipboardService
{
    private readonly Lazy<IClipboard?> _clipboard = new(() => topLevel()?.Clipboard);

    public async Task CopyAsync(IAsyncDataTransfer content)
    {
        if (_clipboard.Value is not { } clipboard) return;

        try
        {
            await clipboard.SetDataAsync(content).ConfigureAwait(false);
            ToasterManager.ShowInformation(MessageResources.CopyInClipBoardSuccess);
        }
        catch (Exception)
        {
            ToasterManager.ShowError(MessageResources.CopyInClipBoardError);
        }
    }

    public async Task CopyTextAsync(string text)
    {
        if (_clipboard.Value is not { } clipboard) return;

        try
        {
            await clipboard.SetTextAsync(text).ConfigureAwait(false);
            ToasterManager.ShowInformation(MessageResources.CopyInClipBoardSuccess);
        }
        catch (Exception)
        {
            ToasterManager.ShowError(MessageResources.CopyInClipBoardError);
        }
    }
}
