// -----------------------------------------------------------------------
// <copyright file="OverlayDialogHostManager.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.UI.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

internal static class OverlayDialogHostManager
{
    private static readonly ConcurrentDictionary<HostKey, OverlayDialogHost> Hosts = new();

    public static void Register(OverlayDialogHost host, string? id, int? hash)
    {
        Debug.WriteLine("Count: " + Hosts.Count);
        _ = Hosts.TryAdd(new HostKey(id, hash), host);
    }

    public static void Unregister(string? id, int? hash) => Hosts.TryRemove(new HostKey(id, hash), out _);

    public static OverlayDialogHost? GetHost(string? id, int? hash)
    {
        HostKey? key = hash is null ? Hosts.Keys.FirstOrDefault(k => k.Id == id) : Hosts.Keys.FirstOrDefault(k => k.Id == id && k.Hash == hash);
        return Hosts.GetValueOrDefault(key.Value);
    }

    internal record struct HostKey(string? Id, int? Hash);
}
