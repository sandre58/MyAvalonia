// -----------------------------------------------------------------------
// <copyright file="ThemeVariantCoordinatorTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Avalonia.Styling;
using FluentAssertions;
using MyNet.Avalonia.Theme.Runtime;
using Xunit;

namespace MyNet.Avalonia.Theme.Tests.Runtime;

public class ThemeVariantCoordinatorTests
{
    [Fact]
    public void GetActiveThemeDictionary_WithoutApplication_ReturnsEmpty()
    {
        var coordinator = new ThemeVariantCoordinator(() => []);

        coordinator.GetActiveThemeDictionary().Should().BeEmpty();
    }

    [Fact]
    public void GetActiveThemeDictionary_WithoutApplication_UsesLightFallbackWhenAvailable()
    {
        var resources = new ResourceDictionary();
        var lightDictionary = new ResourceDictionary { ["Color.Primary"] = 1 };
        resources.ThemeDictionaries[ThemeVariant.Light] = lightDictionary;
        var coordinator = new ThemeVariantCoordinator(() => resources);

        coordinator.GetActiveThemeDictionary().Should().BeSameAs(lightDictionary);
    }

    [Fact]
    public void RegisterThemeProvider_AddsThemeDictionaryEntry()
    {
        var resources = new ResourceDictionary();
        var variant = new ThemeVariant("CoordinatorTest", ThemeVariant.Dark);
        var inner = new ResourceDictionary { ["TestKey"] = 42 };

        resources.ThemeDictionaries[variant] = inner;

        resources.ThemeDictionaries.Should().ContainKey(variant);
        resources.ThemeDictionaries[variant].Should().BeSameAs(inner);
    }
}
