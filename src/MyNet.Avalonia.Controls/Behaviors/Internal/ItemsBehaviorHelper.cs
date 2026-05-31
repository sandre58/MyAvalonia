// -----------------------------------------------------------------------
// <copyright file="ItemsBehaviorHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;

namespace MyNet.Avalonia.Controls.Behaviors.Internal;

internal static class ItemsBehaviorHelper
{
    public static void SortByDisplay<T>(
        List<T> items,
        Func<T, string> displaySelector,
        CompareInfo compareInfo)
        => items.Sort((left, right) =>
            compareInfo.Compare(displaySelector(left), displaySelector(right), CompareOptions.IgnoreCase));

    public static bool RequiresCultureRefresh(bool sortByDisplay, bool includeNullValue, string? nullDisplayResourceKey) =>
        sortByDisplay || (includeNullValue && !string.IsNullOrEmpty(nullDisplayResourceKey));

    public static string ResolveNullDisplay(
        string? nullDisplayText,
        string? nullDisplayResourceKey,
        string? nullDisplayResourceFilename,
        Func<string, string> translateKey,
        Func<string, string, string> translateWithFilename)
        => !string.IsNullOrEmpty(nullDisplayResourceKey)
            ? string.IsNullOrEmpty(nullDisplayResourceFilename)
                ? translateKey(nullDisplayResourceKey)
                : translateWithFilename(nullDisplayResourceKey, nullDisplayResourceFilename)
            : nullDisplayText ?? string.Empty;
}
