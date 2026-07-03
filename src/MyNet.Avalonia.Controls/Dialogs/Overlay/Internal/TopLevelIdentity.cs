// -----------------------------------------------------------------------
// <copyright file="TopLevelIdentity.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;
using System.Threading;
using Avalonia.Controls;

namespace MyNet.Avalonia.Controls.Dialogs.Overlay.Internal;

internal static class TopLevelIdentity
{
    private static int _nextKey = 1;

    private static readonly ConditionalWeakTable<TopLevel, KeyHolder> Keys = [];

    public static int GetKey(TopLevel topLevel)
    {
        if (Keys.TryGetValue(topLevel, out var holder))
            return holder.Key;

        var key = Interlocked.Increment(ref _nextKey);
        Keys.Add(topLevel, new(key));
        return key;
    }

    private sealed class KeyHolder(int key)
    {
        public int Key { get; } = key;
    }
}
