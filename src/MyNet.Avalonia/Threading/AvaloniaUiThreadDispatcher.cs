// -----------------------------------------------------------------------
// <copyright file="AvaloniaUiThreadDispatcher.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Avalonia.Threading;

namespace MyNet.Avalonia.Threading;

/// <summary>
/// Implements the <see cref="IUiThreadDispatcher"/> interface using Avalonia's UI thread dispatcher.
/// </summary>
public sealed class AvaloniaUiThreadDispatcher : IUiThreadDispatcher
{
    /// <inheritdoc/>
    public async Task InvokeAsync(Func<Task> action)
    {
        if (Dispatcher.UIThread.CheckAccess())
            await action().ConfigureAwait(true);
        else
            await Dispatcher.UIThread.InvokeAsync(action).ConfigureAwait(true);
    }

    /// <inheritdoc/>
    public void Post(Action action)
    {
        if (Dispatcher.UIThread.CheckAccess())
            action();
        else
            Dispatcher.UIThread.Post(action);
    }
}
