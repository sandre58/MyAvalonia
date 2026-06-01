// -----------------------------------------------------------------------
// <copyright file="AvaloniaDialogSessionRegistry.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Concurrent;
using MyNet.UI.Dialogs.ContentDialogs;

namespace MyNet.Avalonia.Extended.Dialogs.Internal;

public sealed class AvaloniaDialogSessionRegistry
{
    private readonly ConcurrentDictionary<IDialog, AvaloniaDialogSession> _sessions = new();

    public AvaloniaDialogSession Register(IDialog dialog, AvaloniaDialogSession session)
    {
        _sessions[dialog] = session;
        return session;
    }

    public bool TryGet(IDialog dialog, out AvaloniaDialogSession session) => _sessions.TryGetValue(dialog, out session!);

    public void Remove(IDialog dialog) => _sessions.TryRemove(dialog, out _);
}
