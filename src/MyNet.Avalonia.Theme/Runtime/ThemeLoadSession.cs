// -----------------------------------------------------------------------
// <copyright file="ThemeLoadSession.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
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
    public void LoadInitialResources(
        IServiceProvider? serviceProvider,
        MyTheme theme,
        ColorShades primary,
        ColorShades accent,
        ThemeLoadOptions loadOptions)
    {
        using (PerformanceMonitor.Measure(category: PerformanceCategory.Theme))
            xamlLoader.Load(serviceProvider, theme);

        ThemeResourceModuleLoader.MergeOptionalModules((ResourceDictionary)theme.Resources, loadOptions);

        paletteInjector.AddOrUpdateAccentShades(accent);
        paletteInjector.AddOrUpdatePrimaryShades(primary);
        paletteInjector.UpdateBrushesFromCurrentTheme();
    }
}
