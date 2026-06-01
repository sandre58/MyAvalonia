// -----------------------------------------------------------------------
// <copyright file="DialogSessionRegistry.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Concurrent;
using MyNet.UI.Dialogs.ContentDialogs;

namespace MyNet.Avalonia.Extended.Dialogs.Internal;

public sealed class DialogSessionRegistry
{
    private readonly ConcurrentDictionary<IDialog, DialogSession> _sessions = new();

    public DialogSession Register(IDialog dialog, DialogSession session)
    {
        _sessions[dialog] = session;
        return session;
    }

    public bool TryGet(IDialog dialog, out DialogSession session) => _sessions.TryGetValue(dialog, out session!);

    public void Remove(IDialog dialog) => _sessions.TryRemove(dialog, out _);
}
