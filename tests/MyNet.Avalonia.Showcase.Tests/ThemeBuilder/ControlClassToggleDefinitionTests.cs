// -----------------------------------------------------------------------
// <copyright file="ControlClassToggleDefinitionTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using FluentAssertions;
using MyNet.Avalonia.Showcase.ThemeBuilder.Definitions;
using MyNet.Avalonia.Theme.Classes;
using Xunit;

namespace MyNet.Avalonia.Showcase.Tests.ThemeBuilder;

public class ControlClassToggleDefinitionTests
{
    [Fact]
    public void ProvideClasses_WhenEnabled_ReturnsClass()
    {
        var definition = new ControlClassToggleDefinition(CssClass.ShadowControl);

        definition.ProvideClasses(true).Should().ContainSingle().Which.Should().Be("shadow-control");
    }

    [Fact]
    public void ProvideClasses_WhenDisabled_ReturnsEmpty()
    {
        var definition = new ControlClassToggleDefinition(CssClass.ShadowControl);

        definition.ProvideClasses(false).Should().BeEmpty();
    }
}
