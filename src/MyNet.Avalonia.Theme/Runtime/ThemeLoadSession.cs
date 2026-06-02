// -----------------------------------------------------------------------
// <copyright file="ThemeLoadSession.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using MyNet.Avalonia.Theme.Diagnostics;
using MyNet.Avalonia.Theme.Theming.Palettes;

namespace MyNet.Avalonia.Theme.Runtime;

/// <summary>
/// Orchestrates initial XAML load and first-time palette injection for <see cref="MyTheme"/>.
/// </summary>
internal sealed class ThemeLoadSession(
    IThemeXamlLoader xamlLoader,
    ThemePaletteInjector paletteInjector)
{
    public void LoadBaseResources(
        IServiceProvider? serviceProvider,
        MyTheme theme,
        ColorShades primary,
        ColorShades accent)
    {
        using (PerformanceMonitor.Measure(category: PerformanceCategory.Theme))
            xamlLoader.Load(serviceProvider, theme);

        paletteInjector.AddOrUpdateAccentShades(accent);
        paletteInjector.AddOrUpdatePrimaryShades(primary);
    }

    public void ApplyVariantBrushes() => paletteInjector.UpdateBrushesFromCurrentTheme();
}
