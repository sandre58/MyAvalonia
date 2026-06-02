// -----------------------------------------------------------------------
// <copyright file="ThemePaletteInjector.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using MyNet.Avalonia.Theme.Diagnostics;
using MyNet.Avalonia.Theme.Theming;
using MyNet.Avalonia.Theme.Theming.Brushes;
using MyNet.Avalonia.Theme.Theming.Palettes;
using MyNet.Collections;
using MyNet.Primitives;

namespace MyNet.Avalonia.Theme.Runtime;

/// <summary>
/// Injects brand palettes and variant colors into theme resources and synchronizes <see cref="BrushManager"/> entries.
/// </summary>
internal sealed class ThemePaletteInjector(
    Func<ResourceDictionary> getResources,
    BrushManager brushManager,
    ThemeVariantCoordinator variants,
    Action invalidateResourceCache,
    Func<string, string?, bool, double?, double?, IBrush> resolveBrush)
{
    private static readonly HashSet<string> SemanticColorKeys = new(StringComparer.OrdinalIgnoreCase)
    {
        nameof(ThemeVariantPalette.Success),
        nameof(ThemeVariantPalette.Error),
        nameof(ThemeVariantPalette.Warning),
        nameof(ThemeVariantPalette.Information),
        nameof(ThemeVariantPalette.Neutral)
    };

    public void AddOrUpdatePrimaryShades(ColorShades primary)
    {
        using (PerformanceMonitor.Measure("AddOrUpdatePrimaryShades", category: PerformanceCategory.Theme))
            AddOrUpdateColorShades(primary, "Primary");

        invalidateResourceCache();
    }

    public void AddOrUpdateAccentShades(ColorShades accent)
    {
        using (PerformanceMonitor.Measure("AddOrUpdateAccentShades", category: PerformanceCategory.Theme))
            AddOrUpdateColorShades(accent, "Accent");

        invalidateResourceCache();
    }

    public void UpdateBrushesFromCurrentTheme()
    {
        const string transparencyKey = "Transparency";
        const string transparencySmallKey = "Transparency.Small";
        var resources = getResources();

        using (PerformanceMonitor.Measure("UpdateBrushesFromCurrentTheme", category: PerformanceCategory.Theme))
        {
            var count = 0;
            var activeTheme = variants.GetActiveThemeDictionary();

            foreach (var (key, obj) in activeTheme)
            {
                if (obj is not Color color)
                    continue;

                var colorKey = key.ToString()?.Replace(
                    ThemeResourceKeyFactory.Pattern(ThemeResourceKeyFactory.ColorKey).FormatWithInvariant(string.Empty),
                    string.Empty,
                    StringComparison.OrdinalIgnoreCase);

                if (string.IsNullOrEmpty(colorKey))
                    continue;

                var contrastedColor = GetContrastedColorForKey(colorKey, activeTheme);

                if (SemanticColorKeys.Contains(colorKey))
                {
                    var shades = new ColorShades(color, contrastedColor);
                    shades.ToResourceDictionary(colorKey).ForEach(x =>
                    {
                        AddOrUpdateBrush(x.Key, x.Value, shades.Foreground);
                        count++;
                    });
                }
                else
                {
                    AddOrUpdateBrush(colorKey, color, contrastedColor);
                    count++;
                }
            }

            if (!resources.ContainsKey(ThemeResourceKeyFactory.Brush(transparencyKey)))
                resources.Add(ThemeResourceKeyFactory.Brush(transparencyKey), CreateTransparencyBrush(20));

            if (!resources.ContainsKey(ThemeResourceKeyFactory.Brush(transparencySmallKey)))
                resources.Add(ThemeResourceKeyFactory.Brush(transparencySmallKey), CreateTransparencyBrush(8));

            PerformanceMonitor.Debug($"UpdateBrushesFromCurrentTheme processed {count + 3} brushes", category: PerformanceCategory.Theme);
        }

        invalidateResourceCache();
    }

    private void AddOrUpdateColorShades(ColorShades shades, string name)
    {
        using (PerformanceMonitor.Measure(category: PerformanceCategory.Theme))
        {
            var count = 0;

            foreach (var (key, color) in shades.ToResourceDictionary(name))
            {
                AddOrUpdateColorAndBrush(
                    key,
                    color,
                    !key.Contains(nameof(ColorShades.Foreground), StringComparison.OrdinalIgnoreCase) ? shades.Foreground : null);
                count++;
            }

            PerformanceMonitor.Debug($"AddOrUpdateColorShades({name}) processed {count} shades", category: PerformanceCategory.Theme);
        }
    }

    private void AddOrUpdateColorAndBrush(string colorKey, Color newColor, Color? contrastedColor)
    {
        AddOrUpdateColor(colorKey, newColor);
        AddOrUpdateBrush(colorKey, newColor, contrastedColor);
    }

    private void AddOrUpdateColor(string key, Color color)
    {
        var resources = getResources();
        resources[ThemeResourceKeyFactory.Color(key)] = color;
    }

    private void AddOrUpdateBrush(string key, Color color, Color? contrastedColor)
    {
        var resources = getResources();
        var fullBrushKey = ThemeResourceKeyFactory.Brush(key);
        var brush = brushManager.Register(fullBrushKey, color, contrastedColor);
        resources[fullBrushKey] = brush;
    }

    private static Color? GetContrastedColorForKey(string colorKey, ResourceDictionary themeDictionary)
    {
        var contrastedColorKey = ThemeResourceKeyFactory.ContrastedColor(colorKey);

        return contrastedColorKey is null
            ? null
            : themeDictionary.TryGetResource(contrastedColorKey, null, out var obj) && obj is Color color
                ? color
                : null;
    }

    private VisualBrush CreateTransparencyBrush(double size) => new(new Image
    {
        Height = size,
        Width = size,
        Source = new DrawingImage
        {
            Drawing = new DrawingGroup
            {
                Children =
                [
                    new GeometryDrawing { Brush = Brushes.Transparent, Geometry = PathGeometry.Parse("M0,0 L2,0 2,2, 0,2Z") },
                    new GeometryDrawing
                    {
                        Brush = resolveBrush("Foreground.Primary", nameof(Classes.Enums.Opacity.Scrim), false, null, null),
                        Geometry = PathGeometry.Parse("M0,1 L2,1 2,2, 1,2 1,0 0,0Z")
                    }
                ]
            }
        }
    })
    {
        DestinationRect = new(0, 0, size, size, RelativeUnit.Absolute),
        Stretch = Stretch.Uniform,
        TileMode = TileMode.Tile
    };
}
