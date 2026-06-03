// -----------------------------------------------------------------------
// <copyright file="MenuFilter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace MyNet.Avalonia.Showcase.ViewModels.Menu;

/// <summary>
/// Filters showcase navigation menu items by localized title.
/// </summary>
internal static class MenuFilter
{
    public static IReadOnlyList<IMenuItemViewModel> Apply(
        IReadOnlyList<IMenuItemViewModel> source,
        string? searchText)
    {
        var filter = searchText?.Trim();
        if (string.IsNullOrEmpty(filter))
            return source;

        var result = new List<IMenuItemViewModel>();
        var sectionIndex = IndexOfSection(source);

        var roots = sectionIndex < 0 ? source : source.Take(sectionIndex).ToList();
        var catalog = sectionIndex < 0 ? [] : source.Skip(sectionIndex + 1).ToList();

        foreach (var item in roots)
        {
            if (item is LazyPageMenuItem lazy && Matches(lazy.Title, filter))
                result.Add(lazy);
        }

        var catalogItems = FilterCatalog(catalog, filter);
        if (catalogItems.Count == 0)
            return result;

        if (sectionIndex >= 0)
            result.Add(source[sectionIndex]);

        result.AddRange(catalogItems);
        return result;
    }

    private static List<IMenuItemViewModel> FilterCatalog(IReadOnlyList<IMenuItemViewModel> catalog, string filter)
    {
        var result = new List<IMenuItemViewModel>();

        foreach (var item in catalog)
        {
            if (item is not PagesGroupViewModel group)
                continue;

            if (Matches(group.Title, filter))
            {
                result.Add(group);
                continue;
            }

            var matchingPages = new List<IMenuItemViewModel>();
            foreach (var page in group.Pages)
            {
                if (page is LazyPageMenuItem lazy && Matches(lazy.Title, filter))
                    matchingPages.Add(lazy);
            }

            if (matchingPages.Count > 0)
                result.Add(new FilteredPagesGroupViewModel(group, matchingPages));
        }

        return result;
    }

    private static int IndexOfSection(IReadOnlyList<IMenuItemViewModel> source)
    {
        for (var i = 0; i < source.Count; i++)
        {
            if (source[i].IsSeparator)
                return i;
        }

        return -1;
    }

    private static bool Matches(string? title, string filter)
        => !string.IsNullOrEmpty(title)
           && title.Contains(filter, StringComparison.CurrentCultureIgnoreCase);
}
