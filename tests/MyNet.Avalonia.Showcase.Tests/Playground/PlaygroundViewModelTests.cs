// -----------------------------------------------------------------------
// <copyright file="PlaygroundViewModelTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using Avalonia.Controls;
using FluentAssertions;
using MyNet.Avalonia.Showcase.Tests.Infrastructure;
using MyNet.Avalonia.Showcase.ThemeBuilder;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.Avalonia.Showcase.ViewModels.Playground.ContentProviders;
using MyNet.Avalonia.Showcase.ViewModels.Playground.Factories;
using MyNet.Avalonia.Showcase.ViewModels.Playground.Options;
using MyNet.Collections;
using Xunit;

namespace MyNet.Avalonia.Showcase.Tests.Playground;

public class PlaygroundViewModelTests
{
    static PlaygroundViewModelTests() => PlaygroundTestHost.EnsureInitialized();

    [Fact]
    public void BuildStyle_includesResolvedThemeClass()
    {
        using var playground = CreatePlayground(ThemeProfiles.TextButton());

        playground.BuildStyle().Classes.Should().Contain("theme-default");
    }

    [Fact]
    public void PreviewCode_includesThemeAttributeWhenThemeKeyIsSet()
    {
        using var playground = CreatePlayground(ThemeProfiles.TextButton("Rounded"));

        playground.PreviewCode.Should().Contain("Theme=\"{StaticResource");
        playground.PreviewCode.Should().Contain("Classes=\"");
    }

    [Fact]
    public void TextProvider_changeUpdatesPreviewCode()
    {
        using var playground = CreatePlayground(ThemeProfiles.TextButton());
        playground.SelectedContentProvider = playground.AvailableContentProviders.OfType<TextProviderViewModel>().First();

        playground.AvailableContentProviders.OfType<TextProviderViewModel>().First().Text = "Hello Playground";

        playground.PreviewCode.Should().Contain("Hello Playground");
    }

    [Fact]
    public void ResetCommand_resetsCustomOptionValues()
    {
        var builder = ThemeProfiles.TextButton()
            .AddProperty(Button.IsDefaultProperty, false, x => x.DisplayName("IsDefault"));
        using var playground = CreatePlayground(builder);
        var option = playground.SelectedTheme!.AvailableOptions.OfType<ValueOptionViewModel>().Single();
        option.Value = true;

        playground.ResetCommand.Execute(null);

        option.Value.Should().Be(false);
    }

    [Fact]
    public void ResetCommand_clearsSelectedVariants()
    {
        using var playground = CreatePlayground(ThemeProfiles.TextButton());
        playground.SelectedVariants.Add(playground.SelectedTheme!.AvailableVariants.First());

        playground.ResetCommand.Execute(null);

        playground.SelectedVariants.Should().BeEmpty();
    }

    [Fact]
    public void SelectedThemeChange_resetsCustomOptionValuesForNewTheme()
    {
        var defaultTheme = ThemeProfiles.TextButton()
            .AddProperty(Button.IsDefaultProperty, false, x => x.DisplayName("IsDefault"));
        var alternateTheme = ThemeProfiles.TextButton("Alt")
            .AddProperty(Button.IsDefaultProperty, true, x => x.DisplayName("IsDefault"));
        using var playground = CreatePlayground(defaultTheme, alternateTheme);
        var option = playground.SelectedTheme!.AvailableOptions.OfType<ValueOptionViewModel>().Single();
        option.Value = false;

        playground.SelectedTheme = playground.AvailableThemes.Last();

        playground.SelectedTheme!.AvailableOptions.OfType<ValueOptionViewModel>().Single().Value.Should().Be(true);
    }

    private static PlaygroundViewModel CreatePlayground(params ControlThemeBuilder[] builders)
    {
        var commands = new TestCommandFactory();
        var themes = builders
            .Select(x => new ControlThemeViewModelFactory(x, commands).Create("Button"))
            .ToList()
            .ToObservableCollection();
        return new("Button", themes, commands);
    }
}
