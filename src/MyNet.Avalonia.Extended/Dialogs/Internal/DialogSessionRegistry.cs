// -----------------------------------------------------------------------
// <copyright file="DialogSessionRegistry.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Concurrent;
using MyNet.UI.Dialogs.ContentDialogs;

namespace MyNet.Avalonia.Extended.Dialogs.Internal;

/// <summary>
/// Registry that keeps track of active dialog sessions. This allows the dialog service to manage multiple dialogs and their associated sessions effectively.
/// </summary>
internal sealed class DialogSessionRegistry
{
    private readonly ConcurrentDictionary<IDialog, DialogSession> _sessions = new();

    /// <summary>
    /// Registers a dialog session for the specified dialog. If a session already exists for the dialog, it will be replaced with the new session.
    /// </summary>
    /// <param name="dialog">The dialog for which to register the session.</param>
    /// <param name="session">The dialog session to register.</param>
    /// <returns>The registered dialog session.</returns>
    public DialogSession Register(IDialog dialog, DialogSession session)
    {
        _sessions[dialog] = session;
        return session;
    }

    /// <summary>
    /// Tries to get the dialog session associated with the specified dialog.
    /// </summary>
    /// <param name="dialog">The dialog for which to get the session.</param>
    /// <param name="session">When this method returns, contains the dialog session associated with the specified dialog, if it exists; otherwise, null.</param>
    /// <returns>true if the dialog session was found; otherwise, false.</returns>
    public bool TryGet(IDialog dialog, out DialogSession session) => _sessions.TryGetValue(dialog, out session!);

    /// <summary>
    /// Removes the dialog session associated with the specified dialog.
    /// </summary>
    /// <param name="dialog">The dialog for which to remove the session.</param>
    public void Remove(IDialog dialog) => _sessions.TryRemove(dialog, out _);
}
