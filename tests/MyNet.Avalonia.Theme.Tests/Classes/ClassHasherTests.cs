// -----------------------------------------------------------------------
// <copyright file="ClassHasherTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using FluentAssertions;
using MyNet.Avalonia.Theme.Classes.Engine;
using Xunit;

namespace MyNet.Avalonia.Theme.Tests.Classes;

public class ClassHasherTests
{
    [Fact]
    public void Hash_SameClassesDifferentOrder_ProducesSameHash()
    {
        var first = ClassHasher.Hash(["variant-solid", "size-md", "gap-sm"]);
        var second = ClassHasher.Hash(["gap-sm", "variant-solid", "size-md"]);

        second.Should().Be(first);
    }

    [Fact]
    public void Hash_DifferentClasses_ProducesDifferentHash()
    {
        var first = ClassHasher.Hash(["variant-solid"]);
        var second = ClassHasher.Hash(["variant-outlined"]);

        second.Should().NotBe(first);
    }

    [Fact]
    public void Hash_EmptyCollection_IsStable()
    {
        var first = ClassHasher.Hash([]);
        var second = ClassHasher.Hash([]);

        second.Should().Be(first);
    }
}
