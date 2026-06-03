// -----------------------------------------------------------------------
// <copyright file="PlaygroundStyleComposerTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using FluentAssertions;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.Avalonia.Theme.Theming;
using Xunit;

namespace MyNet.Avalonia.Showcase.Tests.ThemeBuilder;

public class PlaygroundStyleComposerTests
{
    [Fact]
    public void ResolveThemeClassName_WithNamedThemeKey_ReturnsInvariantClass()
    {
        var themeKey = ThemeResourceKeyFactory.Theme("Button", "Rounded");

        PlaygroundStyleComposer.ResolveThemeClassName("Button", themeKey).Should().Be("theme-rounded");
    }

    [Fact]
    public void ResolveThemeClassName_WithoutThemeKey_ReturnsThemeDefault()
    {
        PlaygroundStyleComposer.ResolveThemeClassName("Button", null).Should().Be("theme-default");
    }
}
