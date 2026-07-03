// -----------------------------------------------------------------------
// <copyright file="OverlayDialogHostLookupHelperTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using FluentAssertions;
using MyNet.Avalonia.Controls.Dialogs.Overlay.Internal;
using Xunit;

namespace MyNet.Avalonia.Controls.Tests.Dialogs;

public class OverlayDialogHostLookupHelperTests
{
    [Theory]
    [InlineData("main", 1, "main", 1, true)]
    [InlineData("main", 1, "other", 1, false)]
    [InlineData("main", 1, "main", 2, false)]
    [InlineData(null, null, "main", null, false)]
    public void MatchesFilter_RespectsIdAndHash(string? keyId, int? keyHash, string? id, int? hash, bool expected) => OverlayDialogHostLookupHelper.MatchesFilter(new(keyId, keyHash), id, hash)
        .Should().Be(expected);

    [Fact]
    public void TryGetExactMatch_ReturnsHostWhenHashMatches()
    {
        var hosts = new Dictionary<OverlayDialogHostKey, string>
        {
            [new("main", 42)] = "host-a"
        };

        OverlayDialogHostLookupHelper.TryGetExactMatch(hosts, "main", 42, out var host).Should().BeTrue();
        host.Should().Be("host-a");
    }

    [Fact]
    public void GetMatchingHosts_ReturnsDistinctValues()
    {
        var hosts = new Dictionary<OverlayDialogHostKey, string>
        {
            [new("a", 1)] = "host",
            [new("b", 2)] = "host",
            [new("c", 3)] = "other"
        };

        OverlayDialogHostLookupHelper.GetMatchingHosts(hosts, null, null)
            .Should().BeEquivalentTo("host", "other");
    }

    [Theory]
    [InlineData(null, null, 2, true)]
    [InlineData("id", null, 2, false)]
    [InlineData(null, null, 1, false)]
    public void ShouldFallbackToSingleTopLevel_DetectsAmbiguousLookup(string? id, int? hash, int candidateCount, bool expected) => OverlayDialogHostLookupHelper.ShouldFallbackToSingleTopLevel(id, hash, candidateCount).Should().Be(expected);
}
