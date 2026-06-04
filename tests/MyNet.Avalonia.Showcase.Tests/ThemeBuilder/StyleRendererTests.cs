// -----------------------------------------------------------------------
// <copyright file="StyleRendererTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using FluentAssertions;
using MyNet.Avalonia.Showcase.ThemeBuilder.Rendering;
using Xunit;

namespace MyNet.Avalonia.Showcase.Tests.ThemeBuilder;

public class StyleRendererTests
{
    [Fact]
    public void Apply_ReplacesPreviouslyAppliedClasses()
    {
        using var renderer = new StyleRenderer();
        var control = new Button();

        renderer.Apply(control, new() { Classes = ["variant-solid", "size-md"] });
        control.Classes.Should().BeEquivalentTo("variant-solid", "size-md");

        renderer.Apply(control, new() { Classes = ["variant-outlined"] });
        control.Classes.Should().BeEquivalentTo("variant-outlined");
    }

    [Fact]
    public void Apply_IgnoresNullOrEmptyClassNames()
    {
        using var renderer = new StyleRenderer();
        var control = new Button();

        renderer.Apply(control, new() { Classes = ["variant-solid", string.Empty, null!] });

        control.Classes.Should().ContainSingle().Which.Should().Be("variant-solid");
    }

    [Fact]
    public void Apply_ClearsThemeWhenConfigurationHasNoTheme()
    {
        using var renderer = new StyleRenderer();
        var control = new Button { Theme = new() };

        renderer.Apply(control, new());

        control.Theme.Should().BeNull();
    }
}
