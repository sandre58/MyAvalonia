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

namespace MyNet.Avalonia.Extended.Clipboard;

/// <summary>
/// Resolves Avalonia <see cref="IClipboard"/> from a <see cref="TopLevel"/> provider.
/// </summary>
public sealed class ClipboardService(Func<TopLevel?> topLevelProvider, IClipboardFeedback? feedback = null) : IClipboardService
{
    /// <inheritdoc />
    public Task CopyAsync(IAsyncDataTransfer content)
        => CopyInternalAsync(clipboard => clipboard.SetDataAsync(content));

    /// <inheritdoc />
    public Task CopyTextAsync(string text) => string.IsNullOrEmpty(text) ? Task.CompletedTask : CopyInternalAsync(clipboard => clipboard.SetTextAsync(text));

    private async Task CopyInternalAsync(Func<IClipboard, Task> copy)
    {
        if (topLevelProvider()?.Clipboard is not { } clipboard)
        {
            feedback?.NotifyError();
            return;
        }

        try
        {
            await copy(clipboard).ConfigureAwait(false);
            feedback?.NotifySuccess();
        }
        catch (Exception)
        {
            feedback?.NotifyError();
        }
    }
}
