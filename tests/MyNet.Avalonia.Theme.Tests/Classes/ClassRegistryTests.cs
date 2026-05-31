// -----------------------------------------------------------------------
// <copyright file="ClassRegistryTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using FluentAssertions;
using MyNet.Avalonia.Theme.Classes;
using MyNet.Avalonia.Theme.Classes.Registry;
using Xunit;

namespace MyNet.Avalonia.Theme.Tests.Classes;

public class ClassRegistryTests
{
    static ClassRegistryTests()
    {
        ClassesBootstrapper.Initialize();
    }

    [Fact]
    public void ContainsRegisteredClass_IsCaseInsensitive()
    {
        ClassRegistry.ContainsRegisteredClass("VARIANT-SOLID").Should().BeTrue();
        ClassRegistry.ContainsRegisteredClass("variant-solid").Should().BeTrue();
    }

    [Fact]
    public void ContainsRegisteredClass_UnknownClass_ReturnsFalse()
    {
        ClassRegistry.ContainsRegisteredClass("not-a-real-utility-class").Should().BeFalse();
    }

    [Fact]
    public void Bootstrapper_RegistersUtilityClasses()
    {
        ClassRegistry.RegisteredClassCount.Should().BeGreaterThan(50);
    }
}
