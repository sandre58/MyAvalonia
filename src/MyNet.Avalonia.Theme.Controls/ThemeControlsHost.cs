// -----------------------------------------------------------------------
// <copyright file="ThemeControlsHost.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MyNet.Avalonia.Theme.Controls.Classes;
using MyNet.Avalonia.Theme.Diagnostics;
using MyNet.Avalonia.Theme.Runtime;

namespace MyNet.Avalonia.Theme.Controls;

/// <summary>
/// Entry point for wiring control themes into <see cref="MyNet.Avalonia.Theme.MyTheme"/>.
/// Call <see cref="Register"/> once at application startup, before <c>MyTheme</c> loads resources
/// (typically in <c>App.Initialize</c>, before <c>AvaloniaXamlLoader.Load</c>).
/// </summary>
public static class ThemeControlsHost
{
    private const string CatalogIndex = "avares://MyNet.Avalonia.Theme.Controls/Catalog/_index.axaml";

    private const string DataTemplates = "avares://MyNet.Avalonia.Theme.Controls/Resources/DataTemplates.axaml";

    private static int _registered;

    private static bool _utilityClassesInitialized;

    /// <summary>
    /// Registers utility classes and hooks control-theme XAML merge into <see cref="ThemeComposition"/>.
    /// Safe to call multiple times.
    /// </summary>
    public static void Register()
    {
        if (Interlocked.CompareExchange(ref _registered, 1, 0) != 0)
            return;

        RegisterUtilityClasses();
        ThemeComposition.RegisterCatalogMerger(MergeCatalog);
    }

    private static void RegisterUtilityClasses()
    {
        if (_utilityClassesInitialized)
            return;

        _utilityClassesInitialized = true;

        IconClassRegistry.Register();
        LayoutClassRegistry.Register();
    }

    private static void MergeCatalog(ResourceDictionary themeResources)
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
            var dictionary = AvaloniaXamlLoader.Load(new(avaresSource)) as ResourceDictionary
                ?? throw new InvalidOperationException($"Theme resource '{avaresSource}' did not resolve to a ResourceDictionary.");

            themeResources.MergedDictionaries.Add(dictionary);
        }
    }
}
