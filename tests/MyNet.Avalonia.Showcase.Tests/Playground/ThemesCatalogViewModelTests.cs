// -----------------------------------------------------------------------
// <copyright file="ThemesCatalogViewModelTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using FluentAssertions;
using MyNet.Avalonia.Showcase.ThemeBuilder;
using MyNet.Avalonia.Showcase.Tests.Infrastructure;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.Avalonia.Showcase.ViewModels.Playground.Factories;
using MyNet.Avalonia.Theme.Theming.Core;
using MyNet.Collections;
using Xunit;

namespace MyNet.Avalonia.Showcase.Tests.Playground;

public class ThemesCatalogViewModelTests
{
    static ThemesCatalogViewModelTests() => PlaygroundTestHost.EnsureInitialized();

    [Fact]
    public void SelectedTheme_populatesCatalogSections()
    {
        var commands = new TestCommandFactory();
        var themes = new[] { ThemeProfiles.TextButton() }
            .Select(x => new ControlThemeViewModelFactory(x, commands).Create("Button"))
            .ToList()
            .ToObservableCollection();
        using var catalog = new ThemesCatalogViewModel(themes);

        catalog.ShapeItems.Should().NotBeEmpty();
        catalog.VariantItems.Should().NotBeEmpty();
        catalog.SizeItems.Should().NotBeEmpty();
        catalog.RoleItems.Should().NotBeEmpty();
        catalog.RoleItems.Should().OnlyContain(x => x.Definition.Properties.Any(p => p.Value is ThemeRole));
    }

    [Fact]
    public void SelectedTheme_null_clearsCatalogSections()
    {
        var commands = new TestCommandFactory();
        var themes = new[] { ThemeProfiles.TextButton() }
            .Select(x => new ControlThemeViewModelFactory(x, commands).Create("Button"))
            .ToList()
            .ToObservableCollection();
        using var catalog = new ThemesCatalogViewModel(themes);

        catalog.SelectedTheme = null;

        catalog.ShapeItems.Should().BeEmpty();
        catalog.VariantItems.Should().BeEmpty();
        catalog.SizeItems.Should().BeEmpty();
        catalog.RoleItems.Should().BeEmpty();
        catalog.ItemsRoleItems.Should().BeEmpty();
    }
}
