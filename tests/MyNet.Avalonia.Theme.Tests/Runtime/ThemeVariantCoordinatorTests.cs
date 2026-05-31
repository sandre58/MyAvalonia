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
        var coordinator = new ThemeVariantCoordinator(new ResourceDictionary());

        coordinator.GetActiveThemeDictionary().Should().BeEmpty();
    }

    [Fact]
    public void RegisterThemeProvider_AddsThemeDictionaryEntry()
    {
        var resources = new ResourceDictionary();
        var coordinator = new ThemeVariantCoordinator(resources);
        var variant = new ThemeVariant("CoordinatorTest", ThemeVariant.Dark);
        var inner = new ResourceDictionary { ["TestKey"] = 42 };

        resources.ThemeDictionaries[variant] = inner;

        resources.ThemeDictionaries.Should().ContainKey(variant);
        resources.ThemeDictionaries[variant].Should().BeSameAs(inner);
    }
}
