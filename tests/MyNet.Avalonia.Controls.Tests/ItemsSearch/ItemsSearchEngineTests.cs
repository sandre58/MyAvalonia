// -----------------------------------------------------------------------
// <copyright file="ItemsSearchEngineTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Data;
using FluentAssertions;
using MyNet.Avalonia.Controls.Behaviors;
using MyNet.Avalonia.Controls.Icons;
using MyNet.Avalonia.Controls.Internal;
using MyNet.Avalonia.Controls.Tests.Infrastructure;
using MyNet.Geography;
using Xunit;

namespace MyNet.Avalonia.Controls.Tests.ItemsSearch;

[Collection(GlobalizationTestCollection.Name)]
public class ItemsSearchEngineTests
{
    private readonly GlobalizationTestFixture _globalization;

    public ItemsSearchEngineTests(GlobalizationTestFixture globalization) => _globalization = globalization;

    [Theory]
    [InlineData("be", "Beta", ItemsSearchFilterMode.Contains, false, true)]
    [InlineData("be", "Alpha", ItemsSearchFilterMode.Contains, false, false)]
    [InlineData("al", "Beta", ItemsSearchFilterMode.StartsWith, false, false)]
    [InlineData("Be", "Beta", ItemsSearchFilterMode.StartsWith, false, true)]
    [InlineData("beta", "Beta", ItemsSearchFilterMode.Equals, false, true)]
    [InlineData("BETA", "Beta", ItemsSearchFilterMode.Equals, false, true)]
    [InlineData("BETA", "Beta", ItemsSearchFilterMode.Equals, true, false)]
    public void IsMatch_RespectsFilterModeAndCase(
        string query,
        string itemText,
        ItemsSearchFilterMode mode,
        bool caseSensitive,
        bool expected)
    {
        ItemsSearchEngine.IsMatch(query, itemText, mode, caseSensitive).Should().Be(expected);
    }

    [Theory]
    [InlineData(null, 0, true)]
    [InlineData("", 2, true)]
    [InlineData("a", 2, false)]
    [InlineData("ab", 2, true)]
    public void ShouldApplyFilter_RespectsMinimumLength(string? text, int minimumLength, bool expected) =>
        ItemsSearchEngine.ShouldApplyFilter(text, minimumLength).Should().Be(expected);

    [Fact]
    public void GetItemText_UsesDisplayMemberBindingWhenDefined()
    {
        var control = new ListBox
        {
            DisplayMemberBinding = new Binding("Length"),
        };

        ItemsSearchEngine.GetItemText(control, "hello").Should().Be("5");
    }

    [Fact]
    public void GetItemText_FallsBackToRegisteredDisplayType()
    {
        var control = new ListBox();

        ItemsSearchEngine.GetItemText(control, 42).Should().Be("42");
    }

    [Fact]
    public void GetItemText_UsesSearchMemberPathWhenDefined()
    {
        var control = new ListBox();
        ItemsSearchBehavior.SetSearchMemberPath(control, "Name");

        ItemsSearchEngine.GetItemText(control, new SearchMemberStub("Espagne")).Should().Be("Espagne");
    }

    [Fact]
    public void GetItemText_SearchMemberPathTakesPrecedenceOverDisplayMemberBinding()
    {
        var control = new ListBox
        {
            DisplayMemberBinding = new Binding("Code"),
        };
        ItemsSearchBehavior.SetSearchMemberPath(control, "Name");

        ItemsSearchEngine.GetItemText(control, new SearchMemberStub("Espagne", "ES")).Should().Be("Espagne");
    }

    [Fact]
    public void GetItemText_Country_UsesLocalizedDisplayText()
    {
        _globalization.SetFrenchCulture();

        ItemsSearchEngine.GetItemText(new ListBox(), Country.Spain).Should().Be("Espagne");
    }

    [Fact]
    public void GetItemText_Country_SearchMemberPathName_UsesLocalizedDisplayText()
    {
        _globalization.SetFrenchCulture();
        var control = new ListBox();
        ItemsSearchBehavior.SetSearchMemberPath(control, "Name");

        ItemsSearchEngine.GetItemText(control, Country.Spain).Should().Be("Espagne");
    }

    [Fact]
    public void GetItemText_Country_SearchMemberPathAlpha2_UsesReflectionBinding()
    {
        var control = new ListBox();
        ItemsSearchBehavior.SetSearchMemberPath(control, "Alpha2");

        ItemsSearchEngine.GetItemText(control, Country.Spain).Should().Be(Country.Spain.Alpha2);
    }

    [Fact]
    public void GetItemText_MaterialIconKindGroup_UsesDisplayNameWithoutBinding()
    {
        var control = new ListBox();
        ItemsSearchBehavior.SetSearchMemberPath(control, "DisplayName");
        var group = MaterialIconCatalog.Groups.First(x => x.Name == "Home");

        ItemsSearchEngine.GetItemText(control, group).Should().Be(group.DisplayName);
    }

    [Theory]
    [InlineData("fr-FR", "spain", false)]
    [InlineData("en-US", "spain", true)]
    [InlineData("en-US", "espagne", false)]
    public void IsMatch_CountryUsesLocalizedDisplayText(string cultureName, string query, bool expected)
    {
        _globalization.SetCulture(CultureInfo.GetCultureInfo(cultureName));
        var control = new ListBox();
        var displayText = ItemsSearchEngine.GetItemText(control, Country.Spain);

        ItemsSearchEngine.IsMatch(query, displayText, ItemsSearchFilterMode.Contains, false).Should().Be(expected);
    }

    private sealed record SearchMemberStub(string Name, string Code = "");
}
