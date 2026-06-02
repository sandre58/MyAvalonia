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
internal sealed class ThemeVariantCoordinator(Func<ResourceDictionary> getResources)
{
    public void RegisterThemeProvider(ThemeVariantPalette theme)
    {
        ArgumentNullException.ThrowIfNull(theme);
        var resources = getResources();
        var rd = new ResourceDictionary();
        theme.ToResourceDictionary().ForEach(kv => rd.Add(kv.Key, kv.Value));
        resources.ThemeDictionaries[theme.Variant] = rd;
    }

    public ThemeVariantPalette? GetThemePalette()
    {
        var resources = getResources();
        var currentVariant = ResolveActiveThemeVariant();
        return currentVariant is not null && ThemeDictionaryResolver.TryGetThemeDictionary(resources, currentVariant, out var resourceDict)
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
        var resources = getResources();
        var current = ResolveActiveThemeVariant();
        return current is not null && ThemeDictionaryResolver.TryGetThemeDictionary(resources, current, out var themeDictionary)
            ? themeDictionary
            : [];
    }

    public void SyncApplicationThemeVariant(string? themeKey)
    {
        var resources = getResources();
        if (resources.ThemeDictionaries.Count == 0 || string.IsNullOrEmpty(themeKey))
            return;

        Application.Current!.RequestedThemeVariant = resources.ThemeDictionaries.Keys
            .FirstOrDefault(x => x.Key.Equals(themeKey));
    }

    private ThemeVariant? ResolveActiveThemeVariant()
    {
        var resources = getResources();
        if (resources.ThemeDictionaries.Count == 0)
            return null;

        var application = Application.Current;
        var candidates = new[]
        {
            application?.ActualThemeVariant,
            application?.RequestedThemeVariant,
            ThemeVariant.Default,
            ThemeVariant.Light
        };

        foreach (var candidate in candidates)
        {
            if (candidate is not null && resources.ThemeDictionaries.ContainsKey(candidate))
                return candidate;
        }

        return resources.ThemeDictionaries.Keys.FirstOrDefault();
    }
}
