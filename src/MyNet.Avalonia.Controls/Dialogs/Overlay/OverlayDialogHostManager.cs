// -----------------------------------------------------------------------
// <copyright file="OverlayDialogHostManager.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Layout;
using Avalonia.VisualTree;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public static class OverlayDialogHostManager
{
    private static readonly ConcurrentDictionary<HostKey, OverlayDialogHost> Hosts = new();

    public static void Register(OverlayDialogHost host, string? id, int? hash)
    {
        Debug.WriteLine("Count: " + Hosts.Count);
        Hosts.AddOrUpdate(new(id, hash), host, (_, _) => host);
    }

    public static void Unregister(string? id, int? hash) => Hosts.TryRemove(new(id, hash), out _);

    public static OverlayDialogHost? GetHost(string? id, int? hash)
    {
        if (hash is not null && Hosts.TryGetValue(new(id, hash), out var exactHost)) return exactHost;

        var candidates = Hosts.Where(x => (id is null || x.Key.Id == id) && (hash is null || x.Key.Hash == hash))
                              .Select(x => x.Value)
                              .Distinct()
                              .ToList();

        if (candidates.Count == 1) return candidates[0];

        if (id is null && hash is null)
        {
            var topLevelHosts = Hosts.Values.Where(x => x.IsTopLevel).Distinct().ToList();
            if (topLevelHosts.Count == 1) return topLevelHosts[0];
        }

        return TryCreateTopLevelHost(id, hash);
    }

    private static OverlayDialogHost? TryCreateTopLevelHost(string? id, int? hash)
    {
        if (id is not null) return null;

        var window = GetTargetWindow(hash);
        if (window is null) return null;

        var existingHost = window.GetVisualDescendants().OfType<OverlayDialogHost>().FirstOrDefault(x => x.IsTopLevel);
        if (existingHost is not null)
        {
            Register(existingHost, id, hash ?? window.GetHashCode());
            return existingHost;
        }

        var host = new OverlayDialogHost
        {
            IsTopLevel = true,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            ZIndex = int.MaxValue
        };

        if (window.Content is Panel panelRoot)
        {
            panelRoot.Children.Add(host);
            Register(host, id, hash ?? window.GetHashCode());
            return host;
        }

        var originalContent = window.Content;
        window.Content = null;

        var root = new Grid();
        if (originalContent is Control control)
        {
            root.Children.Add(control);
        }
        else if (originalContent is not null)
        {
            root.Children.Add(new ContentControl { Content = originalContent });
        }

        root.Children.Add(host);
        window.Content = root;

        Register(host, id, hash ?? window.GetHashCode());
        return host;
    }

    private static Window? GetTargetWindow(int? hash)
    {
        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime lifetime) return null;

        if (hash is not null)
        {
            var matchingWindow = lifetime.Windows.FirstOrDefault(x => x.GetHashCode() == hash.Value);
            if (matchingWindow is not null) return matchingWindow;
        }

        return lifetime.MainWindow ?? lifetime.Windows.LastOrDefault();
    }

    internal record struct HostKey(string? Id, int? Hash);
}
