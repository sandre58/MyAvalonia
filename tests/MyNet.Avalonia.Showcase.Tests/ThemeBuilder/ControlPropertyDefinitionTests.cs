// -----------------------------------------------------------------------
// <copyright file="ControlPropertyDefinitionTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;
using FluentAssertions;
using MyNet.Avalonia.Showcase.ThemeBuilder.Definitions;
using MyNet.Avalonia.Theme.Assists;
using MyNet.Avalonia.Theme.Classes.Enums;
using MyNet.Avalonia.Theme.Theming.Core;
using Xunit;

namespace MyNet.Avalonia.Showcase.Tests.ThemeBuilder;

public class ControlPropertyDefinitionTests
{
    private static readonly StyledProperty<ControlVariant> TestVariantProperty =
        AvaloniaProperty.Register<Control, ControlVariant>(nameof(TestVariantProperty), ControlVariant.None);

    [Fact]
    public void ProvideStyleProperty_WhenValueEqualsDefault_ReturnsNull()
    {
        var definition = new ControlPropertyDefinition<ControlVariant>(TestVariantProperty, ControlVariant.Solid);

        definition.ProvideStyleProperty(ControlVariant.Solid).Should().BeNull();
    }

    [Fact]
    public void ProvideStyleProperty_WhenValueDiffersFromDefault_ReturnsStyleProperty()
    {
        var definition = new ControlPropertyDefinition<ControlVariant>(TestVariantProperty, ControlVariant.Solid);

        var styleProperty = definition.ProvideStyleProperty(ControlVariant.Light);

        styleProperty.Should().NotBeNull();
        styleProperty!.Property.Should().Be(TestVariantProperty);
        styleProperty.Value.Should().Be(ControlVariant.Light);
    }

    [Fact]
    public void ProvideStyleProperty_WithFlagEnumCollection_CombinesValues()
    {
        var definition = new ControlPropertyDefinition<ControlVariant>(TestVariantProperty, ControlVariant.None);
        var combined = new[] { ControlVariant.Solid, ControlVariant.Light };

        var styleProperty = definition.ProvideStyleProperty(combined);

        styleProperty.Should().NotBeNull();
        styleProperty!.Value.Should().Be(ControlVariant.Solid | ControlVariant.Light);
    }

    [Fact]
    public void ProvideStyleProperty_WithThemeRole_UsesPropertyName()
    {
        var definition = new ControlPropertyDefinition<ThemeRole>(ThemeAssist.RoleProperty, ThemeRole.Default);

        var styleProperty = definition.ProvideStyleProperty(ThemeRole.Primary);

        styleProperty.Should().NotBeNull();
        styleProperty!.XamlKey.Should().Be(ThemeAssist.RoleProperty.Name);
    }
}
