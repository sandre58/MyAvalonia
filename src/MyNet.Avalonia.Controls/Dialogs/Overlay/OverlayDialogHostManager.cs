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
using MyNet.Avalonia.Controls.Dialogs.Overlay.Internal;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// Global registry for <see cref="OverlayDialogHost"/> instances and host resolution used when presenting overlay dialogs.
/// </summary>
/// <remarks>
/// See <c>Dialogs/Overlay/README.md</c> for lookup rules, <see cref="HostId"/>, top-level keys, and automatic host creation.
/// </remarks>
public static class OverlayDialogHostManager
{
    /// <summary>
    /// Default <see cref="OverlayDialogHost.HostId"/> used in showcase and documentation examples.
    /// </summary>
    public const string MainHostId = "main";

    private static readonly ConcurrentDictionary<OverlayDialogHostKey, OverlayDialogHost> Hosts = new();

    /// <summary>
    /// Returns a stable key for the given <see cref="TopLevel"/> used by <see cref="GetHost"/>.
    /// </summary>
    public static int? GetTopLevelKey(TopLevel? topLevel) =>
        topLevel is null ? null : TopLevelIdentity.GetKey(topLevel);

    /// <summary>
    /// Registers a host for the given <paramref name="id"/> and <paramref name="topLevelKey"/>.
    /// </summary>
    public static void Register(OverlayDialogHost host, string? id, int? topLevelKey) =>
        Hosts.AddOrUpdate(new(id, topLevelKey), host, (_, _) => host);

    /// <summary>
    /// Removes a host from the registry.
    /// </summary>
    public static void Unregister(string? id, int? topLevelKey) => Hosts.TryRemove(new(id, topLevelKey), out _);

    /// <summary>
    /// Resolves a registered host, or creates a top-level host on the target window when <paramref name="id"/> is <see langword="null"/>.
    /// </summary>
    /// <param name="id">
    /// Host identifier from <see cref="OverlayDialogHost.HostId"/>. When not <see langword="null"/>, only registered hosts are returned (no auto-creation).
    /// </param>
    /// <param name="topLevelKey">
    /// Stable key from <see cref="GetTopLevelKey"/> for the owning <see cref="TopLevel"/>. Used for exact lookup and window selection.
    /// </param>
    /// <returns>The resolved host, or <see langword="null"/> when no host matches and none can be created.</returns>
    public static OverlayDialogHost? GetHost(string? id, int? topLevelKey)
    {
        if (OverlayDialogHostLookupHelper.TryGetExactMatch(Hosts, id, topLevelKey, out var exactHost)) return exactHost;

        var candidates = OverlayDialogHostLookupHelper.GetMatchingHosts(Hosts, id, topLevelKey);

        if (candidates.Count == 1) return candidates[0];

        if (OverlayDialogHostLookupHelper.ShouldFallbackToSingleTopLevel(id, topLevelKey, candidates.Count))
        {
            var topLevelHosts = Hosts.Values.Where(x => x.IsTopLevel).Distinct().ToList();
            if (topLevelHosts.Count == 1) return topLevelHosts[0];
        }

        return TryCreateTopLevelHost(id, topLevelKey);
    }

    private static OverlayDialogHost? TryCreateTopLevelHost(string? id, int? topLevelKey)
    {
        if (id is not null) return null;

        var window = GetTargetWindow(topLevelKey);
        if (window is null) return null;

        var key = GetTopLevelKey(window);
        var existingHost = window.GetVisualDescendants().OfType<OverlayDialogHost>().FirstOrDefault(x => x.IsTopLevel);
        if (existingHost is not null)
        {
            Register(existingHost, id, key);
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
            Register(host, id, key);
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

        Register(host, id, key);
        return host;
    }

    private static Window? GetTargetWindow(int? topLevelKey)
    {
        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime lifetime) return null;

        if (topLevelKey is not null)
        {
            var matchingWindow = lifetime.Windows.FirstOrDefault(x => GetTopLevelKey(x) == topLevelKey.Value);
            if (matchingWindow is not null) return matchingWindow;
        }

        return lifetime.MainWindow ?? lifetime.Windows.LastOrDefault();
    }
}
