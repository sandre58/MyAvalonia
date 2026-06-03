// -----------------------------------------------------------------------
// <copyright file="ControlClassDefinitionTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using FluentAssertions;
using MyNet.Avalonia.Showcase.ThemeBuilder.Definitions;
using MyNet.Avalonia.Theme.Classes;
using MyNet.Avalonia.Theme.Classes.Enums;
using Xunit;

namespace MyNet.Avalonia.Showcase.Tests.ThemeBuilder;

public class ControlClassDefinitionTests
{
    [Fact]
    public void ProvideClasses_WithCssClass_ReturnsFormattedClass()
    {
        var definition = new ControlClassDefinition(CssClass.Variant(ControlVariant.Solid));

        var classes = definition.ProvideClasses(CssClass.Variant(ControlVariant.Solid));

        classes.Should().ContainSingle().Which.Should().Be("variant-solid");
    }

    [Fact]
    public void ProvideClasses_WithNull_ReturnsEmpty()
    {
        var definition = new ControlClassDefinition();

        definition.ProvideClasses(null).Should().BeEmpty();
    }

    [Fact]
    public void ProvideClasses_WithMultipleCssClasses_ReturnsAll()
    {
        var definition = new ControlClassDefinition();
        var value = new[] { CssClass.Variant(ControlVariant.Solid), CssClass.Size(SpacingSize.Md) };

        var classes = definition.ProvideClasses(value);

        classes.Should().BeEquivalentTo(["variant-solid", "size-md"]);
    }
}
