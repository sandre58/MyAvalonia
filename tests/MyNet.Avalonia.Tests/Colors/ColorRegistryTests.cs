// -----------------------------------------------------------------------
// <copyright file="ColorRegistryTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using FluentAssertions;
using MyNet.Avalonia.Colors;
using Xunit;

namespace MyNet.Avalonia.Tests.Colors;

public class ColorRegistryTests
{
    [Fact]
    public void Instance_IsSingleton()
    {
        ColorRegistry.Instance.Should().BeSameAs(ColorRegistry.Instance);
    }

    [Fact]
    public void TryResolve_KnownName_ReturnsColor()
    {
        var color = ColorRegistry.Instance.TryResolve("Red");

        color.Should().NotBeNull();
    }
}
