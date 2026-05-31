// -----------------------------------------------------------------------
// <copyright file="ThemeVariantCoordinator.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Styling;
using MyNet.Avalonia.Theme.Theming;
using MyNet.Avalonia.Theme.Theming.Palettes;
using MyNet.Collections;
using MyNet.Primitives;

namespace MyNet.Avalonia.Theme.Runtime;

/// <summary>
/// Manages <see cref="ResourceDictionary.ThemeDictionaries"/> and application theme variant selection.
/// </summary>
internal sealed class ThemeVariantCoordinator(ResourceDictionary resources)
{
    public void RegisterThemeProvider(ThemeVariantPalette theme)
    {
        ArgumentNullException.ThrowIfNull(theme);
        var rd = new ResourceDictionary();
        theme.ToResourceDictionary().ForEach(kv => rd.Add(kv.Key, kv.Value));
        resources.ThemeDictionaries[theme.Variant] = rd;
    }

    public ThemeVariantPalette? GetThemePalette()
    {
        var currentVariant = Application.Current?.ActualThemeVariant ?? ThemeVariant.Default;
        return resources.ThemeDictionaries.TryGetValue(currentVariant, out var rd) && rd is ResourceDictionary resourceDict
            ? ThemeVariantPalette.FromResourceDictionary(
                currentVariant,
                resourceDict.ToDictionary(
                    x => x.Key.ToString().OrEmpty().Replace(
                        ThemeResourceKeyFactory.Pattern(ThemeResourceKeyFactory.ColorKey).FormatWithInvariant(string.Empty),
                        string.Empty,
                        StringComparison.OrdinalIgnoreCase),
                    x => x.Value!))
            : null;
    }

    public ResourceDictionary GetActiveThemeDictionary()
    {
        var current = Application.Current?.ActualThemeVariant ?? ThemeVariant.Default;
        return resources.ThemeDictionaries.TryGetValue(current, out var rd) && rd is ResourceDictionary resources1
            ? resources1
            : [];
    }

    public void SyncApplicationThemeVariant(string? themeKey)
    {
        if (resources.ThemeDictionaries.Count == 0 || string.IsNullOrEmpty(themeKey))
            return;

        Application.Current!.RequestedThemeVariant = resources.ThemeDictionaries.Keys
            .FirstOrDefault(x => x.Key.Equals(themeKey));
    }
}
