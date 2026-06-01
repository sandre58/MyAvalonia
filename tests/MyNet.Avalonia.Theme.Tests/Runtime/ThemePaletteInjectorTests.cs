// -----------------------------------------------------------------------
// <copyright file="ThemePaletteInjectorTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using Avalonia.Media;
using FluentAssertions;
using MyNet.Avalonia.Theme.Runtime;
using MyNet.Avalonia.Theme.Theming;
using Xunit;
using MediaColors = Avalonia.Media.Colors;

namespace MyNet.Avalonia.Theme.Tests.Runtime;

public class ThemePaletteInjectorTests
{
    [Fact]
    public void AddOrUpdatePrimaryShades_RegistersColorsAndBrushes()
    {
        var (injector, resources, _) = CreateInjector();

        injector.AddOrUpdatePrimaryShades(new(MediaColors.SteelBlue));

        resources.Should().ContainKey(ThemeResourceKeyFactory.Color("Primary"));
        resources.Should().ContainKey(ThemeResourceKeyFactory.Brush("Primary"));
        resources[ThemeResourceKeyFactory.Brush("Primary")].Should().BeOfType<SolidColorBrush>();
    }

    [Fact]
    public void AddOrUpdatePrimaryShades_InvalidatesResourceCache()
    {
        var (injector, _, wasInvalidated) = CreateInjector();

        injector.AddOrUpdatePrimaryShades(new(MediaColors.Navy));

        wasInvalidated().Should().BeTrue();
    }

    [Fact]
    public void AddOrUpdatePrimaryShades_SecondCall_UpdatesBrushInPlace()
    {
        var (injector, resources, _) = CreateInjector();

        injector.AddOrUpdatePrimaryShades(new(MediaColors.Blue));
        var brush = resources[ThemeResourceKeyFactory.Brush("Primary")].Should().BeOfType<SolidColorBrush>().Subject;

        injector.AddOrUpdatePrimaryShades(new(MediaColors.Crimson));

        brush.Color.Should().Be(MediaColors.Crimson);
    }

    [Fact]
    public void UpdateBrushesFromCurrentTheme_AddsTransparencyBrushes()
    {
        var (injector, resources, wasInvalidated) = CreateInjector();

        injector.UpdateBrushesFromCurrentTheme();

        resources.Should().ContainKey(ThemeResourceKeyFactory.Brush("Transparency"));
        resources.Should().ContainKey(ThemeResourceKeyFactory.Brush("Transparency.Small"));
        wasInvalidated().Should().BeTrue();
    }

    private static (ThemePaletteInjector Injector, ResourceDictionary Resources, Func<bool> WasInvalidated) CreateInjector()
    {
        var resources = new ResourceDictionary();
        var invalidated = false;
        var injector = new ThemePaletteInjector(
            resources,
            new(null, null),
            new(resources),
            () => invalidated = true,
            (_, _, _, _, _) => new SolidColorBrush(MediaColors.Gray));

        return (injector, resources, () => invalidated);
    }
}
