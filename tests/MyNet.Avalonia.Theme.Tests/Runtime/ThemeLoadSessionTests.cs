// -----------------------------------------------------------------------
// <copyright file="ThemeLoadSessionTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using Avalonia.Media;
using FluentAssertions;
using MyNet.Avalonia.Theme.Runtime;
using MyNet.Avalonia.Theme.Theming.Brushes;
using MyNet.Avalonia.Theme.Theming.Palettes;
using MediaColors = Avalonia.Media.Colors;
using Xunit;

namespace MyNet.Avalonia.Theme.Tests.Runtime;

public class ThemeLoadSessionTests
{
    [Fact]
    public void LoadInitialResources_LoadsXamlThenInjectsPalettes()
    {
        var loader = new RecordingThemeXamlLoader();
        var resources = new ResourceDictionary();
        var injector = new ThemePaletteInjector(
            resources,
            new BrushManager(null, null),
            new ThemeVariantCoordinator(resources),
            () => { },
            (_, _, _, _, _) => new SolidColorBrush(MediaColors.Black));

        var session = new ThemeLoadSession(loader, injector);
        var theme = new MyTheme(null, loader);
        var primary = new ColorShades(MediaColors.Blue);
        var accent = new ColorShades(MediaColors.Orange);

        session.LoadInitialResources(null, theme, primary, accent, ThemeLoadOptions.CoreOnly);

        loader.LoadCount.Should().Be(1);
        loader.LastTarget.Should().BeSameAs(theme);
        resources.Should().ContainKey(MyNet.Avalonia.Theme.Theming.ThemeResourceKeyFactory.Brush("Primary"));
        resources.Should().ContainKey(MyNet.Avalonia.Theme.Theming.ThemeResourceKeyFactory.Brush("Accent"));
    }

    private sealed class RecordingThemeXamlLoader : IThemeXamlLoader
    {
        public int LoadCount { get; private set; }

        public object? LastTarget { get; private set; }

        public void Load(IServiceProvider? serviceProvider, object themeRoot)
        {
            LoadCount++;
            LastTarget = themeRoot;
        }
    }
}
