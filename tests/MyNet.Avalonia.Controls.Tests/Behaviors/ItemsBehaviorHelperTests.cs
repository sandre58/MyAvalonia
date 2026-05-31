// -----------------------------------------------------------------------
// <copyright file="ItemsBehaviorHelperTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Globalization;
using FluentAssertions;
using MyNet.Avalonia.Controls.Behaviors.Internal;
using Xunit;

namespace MyNet.Avalonia.Controls.Tests.Behaviors;

public class ItemsBehaviorHelperTests
{
    [Fact]
    public void SortByDisplay_OrdersCaseInsensitively()
    {
        var items = new List<string> { "zebra", "Alpha", "beta" };
        var compareInfo = CultureInfo.InvariantCulture.CompareInfo;

        ItemsBehaviorHelper.SortByDisplay(items, x => x, compareInfo);

        items.Should().Equal("Alpha", "beta", "zebra");
    }

    [Theory]
    [InlineData(true, false, null, true)]
    [InlineData(false, true, "Key", true)]
    [InlineData(false, true, "", false)]
    [InlineData(false, false, null, false)]
    public void RequiresCultureRefresh_DetectsSortOrResourceNullLabel(bool sort, bool includeNull, string? key, bool expected)
    {
        ItemsBehaviorHelper.RequiresCultureRefresh(sort, includeNull, key).Should().Be(expected);
    }

    [Fact]
    public void ResolveNullDisplay_UsesPlainTextWhenNoResourceKey()
    {
        ItemsBehaviorHelper.ResolveNullDisplay("(none)", null, null, _ => "x", (_, _) => "y")
            .Should().Be("(none)");
    }

    [Fact]
    public void ResolveNullDisplay_UsesResourceTranslation()
    {
        ItemsBehaviorHelper.ResolveNullDisplay(null, "NullLabel", null, key => $"[{key}]", (_, _) => "y")
            .Should().Be("[NullLabel]");
    }

    [Fact]
    public void ResolveNullDisplay_UsesFilenameWhenProvided()
    {
        ItemsBehaviorHelper.ResolveNullDisplay(null, "NullLabel", "Messages", _ => "x", (_, filename) => filename)
            .Should().Be("Messages");
    }
}
