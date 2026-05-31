// -----------------------------------------------------------------------
// <copyright file="ThemeControlsResourceLoader.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MyNet.Avalonia.Theme.Diagnostics;

namespace MyNet.Avalonia.Theme.Controls;

/// <summary>
/// Merges the full control-theme catalog (Foundation, Standard, Custom, and shared families).
/// </summary>
public static class ThemeControlsResourceLoader
{
    public const string CatalogIndex = "avares://MyNet.Avalonia.Theme.Controls/Catalog/_index.axaml";

    private const string DataTemplates = "avares://MyNet.Avalonia.Theme.Controls/Resources/DataTemplates.axaml";

    /// <summary>
    /// Merges data templates and all control themes into <paramref name="themeResources"/>.
    /// </summary>
    public static void Merge(ResourceDictionary themeResources)
    {
        ArgumentNullException.ThrowIfNull(themeResources);

        if (!CanLoadXamlResources())
            return;

        MergeDictionary(themeResources, DataTemplates);
        MergeDictionary(themeResources, CatalogIndex);
    }

    private static bool CanLoadXamlResources() => Application.Current is not null;

    private static void MergeDictionary(ResourceDictionary themeResources, string avaresSource)
    {
        using (PerformanceMonitor.Measure($"[Theme.Controls] Merge {avaresSource}", category: PerformanceCategory.Theme))
        {
            var dictionary = AvaloniaXamlLoader.Load(new Uri(avaresSource)) as ResourceDictionary
                ?? throw new InvalidOperationException($"Theme resource '{avaresSource}' did not resolve to a ResourceDictionary.");

            themeResources.MergedDictionaries.Add(dictionary);
        }
    }
}
