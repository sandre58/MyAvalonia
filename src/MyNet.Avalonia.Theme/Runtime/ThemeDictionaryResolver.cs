// -----------------------------------------------------------------------
// <copyright file="ThemeDictionaryResolver.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Styling;

namespace MyNet.Avalonia.Theme.Runtime;

/// <summary>
/// Resolves theme-variant dictionaries declared as <see cref="ResourceDictionary"/> or lazy <see cref="ResourceInclude"/>.
/// </summary>
internal static class ThemeDictionaryResolver
{
    /// <summary>
    /// Resolves the active theme dictionary for the given variant, priming lazy includes when needed.
    /// </summary>
    public static bool TryGetThemeDictionary(
        ResourceDictionary owner,
        ThemeVariant variant,
        out ResourceDictionary dictionary)
    {
        dictionary = [];

        return owner.ThemeDictionaries.TryGetValue(variant, out var raw) && (TryResolveDictionary(raw, out dictionary)
            ? dictionary.Count > 0
            : owner.ThemeDictionaries.TryGetValue(variant, out raw) && TryResolveDictionary(raw, out dictionary) && dictionary.Count > 0);
    }

    private static bool TryResolveDictionary(object raw, out ResourceDictionary dictionary)
    {
        switch (raw)
        {
            case ResourceDictionary resourceDictionary:
                dictionary = resourceDictionary;
                return true;

            case ResourceInclude { Loaded: ResourceDictionary loaded }:
                dictionary = loaded;
                return true;

            case ResourceInclude { Loaded: { } loadedDictionary }:
                dictionary = ToResourceDictionary(loadedDictionary);
                return dictionary.Count > 0;

            case IResourceDictionary resourceDictionary:
                dictionary = ToResourceDictionary(resourceDictionary);
                return dictionary.Count > 0;

            default:
                dictionary = [];
                return false;
        }
    }

    private static ResourceDictionary ToResourceDictionary(IResourceDictionary source)
    {
        var dictionary = new ResourceDictionary();
        foreach (var (key, value) in source)
            dictionary.Add(key, value);

        return dictionary;
    }
}
