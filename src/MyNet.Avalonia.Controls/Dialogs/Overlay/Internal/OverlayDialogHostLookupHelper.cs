// -----------------------------------------------------------------------
// <copyright file="OverlayDialogHostLookupHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

namespace MyNet.Avalonia.Controls.Dialogs.Overlay.Internal;

internal static class OverlayDialogHostLookupHelper
{
    public static bool MatchesFilter(OverlayDialogHostKey key, string? id, int? topLevelKey) =>
        (id is null || key.Id == id) && (topLevelKey is null || key.TopLevelKey == topLevelKey);

    public static bool TryGetExactMatch<T>(
        IReadOnlyDictionary<OverlayDialogHostKey, T?> hosts,
        string? id,
        int? topLevelKey,
        out T? host)
    {
        if (topLevelKey is null)
        {
            host = default!;
            return false;
        }

        return hosts.TryGetValue(new(id, topLevelKey), out host);
    }

    public static IReadOnlyList<T> GetMatchingHosts<T>(
        IEnumerable<KeyValuePair<OverlayDialogHostKey, T>> hosts,
        string? id,
        int? topLevelKey) =>
        [.. hosts.Where(x => MatchesFilter(x.Key, id, topLevelKey))
            .Select(x => x.Value)
            .Distinct()];

    public static bool ShouldFallbackToSingleTopLevel(string? id, int? topLevelKey, int candidateCount) =>
        candidateCount != 1 && id is null && topLevelKey is null;
}
