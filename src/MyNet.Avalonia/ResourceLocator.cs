// -----------------------------------------------------------------------
// <copyright file="ResourceLocator.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Media;
using Avalonia.Styling;
using MyNet.Avalonia.Resources;
using MyNet.Utilities;
using MyNet.Utilities.Localization;
using MyNet.Utilities.Logging;

namespace MyNet.Avalonia;

public static class ResourceLocator
{
    private static bool _isInitialized;

    public static Dictionary<Color, string> ColorResourcesDictionary { get; private set; } = [];

    public static void Initialize()
    {
        if (_isInitialized) return;

        // Common Resources
        TranslationService.RegisterResources(nameof(ColorResources), ColorResources.ResourceManager);

        Humanizer.ResourceLocator.Initialize();

        GlobalizationService.Current.CultureChanged += (_, _) => FillColorResourcesDictionary();
        _isInitialized = true;
    }

    public static T? TryGetResource<T>(string resourceKey, ThemeVariant? themeVariant = null) => (Application.Current?.TryGetResource(resourceKey, themeVariant ?? Application.Current.ActualThemeVariant, out var resource) ?? false) ? (T?)resource : default;

    public static T GetResource<T>(string resourceKey, ThemeVariant? themeVariant = null) => TryGetResource<T>(resourceKey, themeVariant) ?? throw new InvalidOperationException($"Resource {resourceKey} not found");

    private static void FillColorResourcesDictionary()
    {
        ColorResourcesDictionary = [];
        var resourceSet = ColorResources.ResourceManager.GetResourceSet(GlobalizationService.Current.Culture, true, true);

        if (resourceSet is null) return;

        foreach (var entry in resourceSet.OfType<DictionaryEntry>())
        {
            try
            {
                if (Color.TryParse(entry.Key.ToString().OrEmpty(), out var color))
                {
                    ColorResourcesDictionary.Add(color, entry.Value!.ToString()!);
                }
            }
            catch (Exception)
            {
                LogManager.Warning($"{entry.Key} is not a valid color key");
            }
        }
    }
}
