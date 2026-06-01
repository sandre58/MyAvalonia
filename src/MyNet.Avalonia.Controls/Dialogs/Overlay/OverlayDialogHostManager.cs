// -----------------------------------------------------------------------
// <copyright file="OverlayDialogHostManager.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Concurrent;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Layout;
using Avalonia.VisualTree;
using MyNet.Avalonia.Controls.Internals;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// Global registry for <see cref="OverlayDialogHost"/> instances and host resolution used when presenting overlay dialogs.
/// </summary>
/// <remarks>
/// See <c>Dialogs/Overlay/README.md</c> for lookup rules, <see cref="HostId"/>, <c>TopLevelHashCode</c>, and automatic host creation.
/// </remarks>
public static class OverlayDialogHostManager
{
    private static readonly ConcurrentDictionary<OverlayDialogHostKey, OverlayDialogHost> Hosts = new();

    /// <summary>
    /// Registers a host for the given <paramref name="id"/> and top-level <paramref name="hash"/>.
    /// </summary>
    public static void Register(OverlayDialogHost host, string? id, int? hash) => Hosts.AddOrUpdate(new(id, hash), host, (_, _) => host);

    /// <summary>
    /// Removes a host from the registry.
    /// </summary>
    public static void Unregister(string? id, int? hash) => Hosts.TryRemove(new(id, hash), out _);

    /// <summary>
    /// Resolves a registered host, or creates a top-level host on the target window when <paramref name="id"/> is <see langword="null"/>.
    /// </summary>
    /// <param name="id">
    /// Host identifier from <see cref="OverlayDialogHost.HostId"/>. When not <see langword="null"/>, only registered hosts are returned (no auto-creation).
    /// </param>
    /// <param name="hash">
    /// Hash of the owning <see cref="Avalonia.Controls.TopLevel"/> (typically <c>GetHashCode()</c>). Used for exact lookup and window selection.
    /// </param>
    /// <returns>The resolved host, or <see langword="null"/> when no host matches and none can be created.</returns>
    public static OverlayDialogHost? GetHost(string? id, int? hash)
    {
        if (OverlayDialogHostLookupHelper.TryGetExactMatch(Hosts, id, hash, out var exactHost)) return exactHost;

        var candidates = OverlayDialogHostLookupHelper.GetMatchingHosts(Hosts, id, hash);

        if (candidates.Count == 1) return candidates[0];

        if (OverlayDialogHostLookupHelper.ShouldFallbackToSingleTopLevel(id, hash, candidates.Count))
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
}
