// -----------------------------------------------------------------------
// <copyright file="ClipboardService.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using MyNet.Avalonia.Clipboard;

namespace MyNet.Avalonia.Extended.Clipboard;

/// <summary>
/// Resolves Avalonia <see cref="IClipboard"/> from a <see cref="TopLevel"/> provider.
/// </summary>
public class ClipboardService(Func<TopLevel?> topLevelProvider, IClipboardFeedback? feedback = null) : IClipboardService
{
    /// <inheritdoc />
    public async Task CopyAsync(IAsyncDataTransfer content)
    {
        if (topLevelProvider()?.Clipboard is not { } clipboard)
            return;

        try
        {
            await clipboard.SetDataAsync(content).ConfigureAwait(false);
            feedback?.NotifySuccess();
        }
        catch (Exception)
        {
            feedback?.NotifyError();
        }
    }

    /// <inheritdoc />
    public async Task CopyTextAsync(string text)
    {
        if (string.IsNullOrEmpty(text))
            return;

        if (topLevelProvider()?.Clipboard is not { } clipboard)
            return;

        try
        {
            await clipboard.SetTextAsync(text).ConfigureAwait(false);
            feedback?.NotifySuccess();
        }
        catch (Exception)
        {
            feedback?.NotifyError();
        }
    }
}
