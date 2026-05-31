// -----------------------------------------------------------------------
// <copyright file="ThemeResourceModuleLoader.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MyNet.Avalonia.Theme.Diagnostics;

namespace MyNet.Avalonia.Theme.Runtime;

/// <summary>
/// Merges optional control-theme modules into the theme resource dictionary.
/// </summary>
internal static class ThemeResourceModuleLoader
{
    public static void MergeOptionalModules(ResourceDictionary themeResources, ThemeLoadOptions options)
    {
        ArgumentNullException.ThrowIfNull(themeResources);
        ArgumentNullException.ThrowIfNull(options);

        if (options.IncludeColorPicker)
            MergeModule(themeResources, ThemeAssetCatalog.ColorPickerModule);

        if (options.IncludeDataGrid)
            MergeModule(themeResources, ThemeAssetCatalog.DataGridModule);

        if (options.IncludeExtendedDateTime)
            MergeModule(themeResources, ThemeAssetCatalog.ExtendedDateTimeModule);
    }

    private static void MergeModule(ResourceDictionary themeResources, string avaresSource)
    {
        using (PerformanceMonitor.Measure($"[Theme] Merge module {avaresSource}", category: PerformanceCategory.Theme))
        {
            var module = AvaloniaXamlLoader.Load(new Uri(avaresSource)) as ResourceDictionary
                ?? throw new InvalidOperationException($"Theme module '{avaresSource}' did not resolve to a ResourceDictionary.");

            themeResources.MergedDictionaries.Add(module);
        }
    }
}
