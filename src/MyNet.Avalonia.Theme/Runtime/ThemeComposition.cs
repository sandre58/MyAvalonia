// -----------------------------------------------------------------------
// <copyright file="ThemeComposition.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Avalonia.Controls;

namespace MyNet.Avalonia.Theme.Runtime;

/// <summary>
/// Extension point for satellite theme assemblies (for example <c>MyNet.Avalonia.Theme.Controls</c>).
/// </summary>
public static class ThemeComposition
{
    private static readonly List<Action<ResourceDictionary>> CatalogMergers = [];

    /// <summary>
    /// Registers a callback that merges control-theme resources into the active theme dictionary.
    /// </summary>
    /// <param name="mergeCatalog">Merge implementation.</param>
    public static void RegisterCatalogMerger(Action<ResourceDictionary> mergeCatalog)
    {
        ArgumentNullException.ThrowIfNull(mergeCatalog);
        lock (CatalogMergers)
            CatalogMergers.Add(mergeCatalog);
    }

    internal static void MergeRegisteredCatalogs(ResourceDictionary themeResources)
    {
        Action<ResourceDictionary>[] mergers;
        lock (CatalogMergers)
            mergers = [.. CatalogMergers];

        foreach (var merge in mergers)
            merge(themeResources);
    }
}
