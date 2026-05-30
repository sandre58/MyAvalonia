// -----------------------------------------------------------------------
// <copyright file="ColorRegistry.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia.Media;
using MyNet.Avalonia;
using MyNet.Avalonia.Resources;
using MyNet.Globalization.Facade;

namespace MyNet.Avalonia.Colors;

/// <summary>
/// Culture-invariant color-to-resource-key registry. Localized labels are resolved through the translation catalog at read time.
/// </summary>
public sealed class ColorRegistry : IColorRegistry
{
    /// <summary>
    /// Translation catalog key for <see cref="ColorResources"/>.
    /// </summary>
    public const string ResourceName = nameof(ColorResources);

    /// <summary>
    /// Gets shared instance registered in DI and used by extension methods.
    /// </summary>
    public static ColorRegistry Instance { get; } = new();

    private readonly FrozenDictionary<Color, string> _colorToKey;
    private readonly FrozenDictionary<string, Color> _invariantNameToColor;

    private ColorRegistry()
    {
        var colorToKey = new Dictionary<Color, string>();
        var invariantNameToColor = new Dictionary<string, Color>(StringComparer.OrdinalIgnoreCase);

        var neutralCulture = ColorResources.Culture ?? CultureInfo.GetCultureInfo("fr");
        var resourceSet = ColorResources.ResourceManager.GetResourceSet(neutralCulture, true, true);

        if (resourceSet is not null)
        {
            foreach (var key in from DictionaryEntry entry in resourceSet select entry.Key.ToString())
            {
                if (string.IsNullOrEmpty(key) || !Color.TryParse(key, out var color))
                    continue;

                colorToKey[color] = key;
                invariantNameToColor[key] = color;
            }
        }

        _colorToKey = colorToKey.ToFrozenDictionary();
        _invariantNameToColor = invariantNameToColor.ToFrozenDictionary(StringComparer.OrdinalIgnoreCase);
    }

    /// <inheritdoc />
    public string? GetDisplayName(Color color, CultureInfo? culture = null)
        => !_colorToKey.TryGetValue(color, out var key) ? null : key.Translate(ResourceName, culture.OrContext());

    /// <inheritdoc />
    public Color? TryResolve(string name, CultureInfo? culture = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            return null;

        if (_invariantNameToColor.TryGetValue(name, out var invariantMatch))
            return invariantMatch;

        var effectiveCulture = culture.OrContext();
        foreach (var (color, key) in _colorToKey)
        {
            if (string.Equals(key.Translate(ResourceName, effectiveCulture), name, StringComparison.OrdinalIgnoreCase))
                return color;
        }

        return null;
    }
}
