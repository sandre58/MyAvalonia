// -----------------------------------------------------------------------
// <copyright file="OverlayDialogHostLookupHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls.Internals;
#pragma warning restore IDE0130 // Namespace does not match folder structure

internal static class OverlayDialogHostLookupHelper
{
    public static bool MatchesFilter(OverlayDialogHostKey key, string? id, int? hash) =>
        (id is null || key.Id == id) && (hash is null || key.Hash == hash);

    public static bool TryGetExactMatch<T>(
        IReadOnlyDictionary<OverlayDialogHostKey, T?> hosts,
        string? id,
        int? hash,
        out T? host)
    {
        if (hash is null)
        {
            host = default!;
            return false;
        }

        return hosts.TryGetValue(new(id, hash), out host);
    }

    public static IReadOnlyList<T> GetMatchingHosts<T>(
        IEnumerable<KeyValuePair<OverlayDialogHostKey, T>> hosts,
        string? id,
        int? hash) =>
        [.. hosts.Where(x => MatchesFilter(x.Key, id, hash))
            .Select(x => x.Value)
            .Distinct()];

    public static bool ShouldFallbackToSingleTopLevel(string? id, int? hash, int candidateCount) =>
        candidateCount != 1 && id is null && hash is null;
}
