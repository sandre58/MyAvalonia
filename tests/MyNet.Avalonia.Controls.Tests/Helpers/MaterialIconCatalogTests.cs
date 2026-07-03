// -----------------------------------------------------------------------
// <copyright file="MaterialIconCatalogTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using FluentAssertions;
using Material.Icons;
using MyNet.Avalonia.Controls.Icons;
using MyNet.Avalonia.Controls.Tests.Infrastructure;
using Xunit;

namespace MyNet.Avalonia.Controls.Tests.Helpers;

[Collection(GlobalizationTestCollection.Name)]
public class MaterialIconCatalogTests
{
    [Fact]
    public void Groups_ContainsMaterialIconKinds()
    {
        MaterialIconCatalog.Groups.Should().NotBeEmpty();
        MaterialIconCatalog.Groups.Should().OnlyContain(g => !string.IsNullOrEmpty(g.Name));
    }

    [Fact]
    public void Kinds_MatchesGroupKinds() => MaterialIconCatalog.Kinds.Should().BeEquivalentTo(MaterialIconCatalog.Groups.Select(g => g.Kind));

    [Fact]
    public void Groups_ContainsDistinctKinds() => MaterialIconCatalog.Groups.Select(g => g.Kind).Should().OnlyHaveUniqueItems();

    [Fact]
    public void MaterialIconKindGroup_MapsKindAndHumanizesDisplayName()
    {
        var group = MaterialIconCatalog.Groups.First(g => g.Name == nameof(MaterialIconKind.Account));

        group.Kind.Should().Be(MaterialIconKind.Account);
        group.DisplayName.Should().NotBeNullOrWhiteSpace();
        group.DisplayName.Should().NotBe(group.Name);
    }
}
