// -----------------------------------------------------------------------
// <copyright file="ThemeResourceStoreTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Styling;
using FluentAssertions;
using MyNet.Avalonia.Theme.Runtime;
using Xunit;

namespace MyNet.Avalonia.Theme.Tests.Runtime;

public class ThemeResourceStoreTests
{
    [Fact]
    public void EnsureLoaded_InvokesLoadOnce()
    {
        var store = new ThemeResourceStore();
        var calls = 0;

        store.EnsureLoaded(() => calls++);
        store.EnsureLoaded(() => calls++);

        store.IsLoaded.Should().BeTrue();
        calls.Should().Be(1);
    }

    [Fact]
    public void TryGetResource_CachesSuccessfulLookup()
    {
        var store = new ThemeResourceStore();
        var lookups = 0;
        const string key = "cache-hit";

        store.TryGetResource(
            key,
            ThemeVariant.Dark,
            () => { },
            (_, _) =>
            {
                lookups++;
                return (true, "value");
            },
            out var first).Should().BeTrue();

        first.Should().Be("value");
        lookups.Should().Be(1);

        store.TryGetResource(
            key,
            ThemeVariant.Dark,
            () => throw new InvalidOperationException("Should not load again"),
            (_, _) => (true, "other"),
            out var second).Should().BeTrue();

        second.Should().Be("value");
        lookups.Should().Be(1);
    }

    [Fact]
    public void TryGetResource_CachesMisses()
    {
        var store = new ThemeResourceStore();
        var storeMarkedLoaded = false;

        store.TryGetResource(
            "missing",
            null,
            () => storeMarkedLoaded = true,
            (_, _) => (false, null),
            out _).Should().BeFalse();

        storeMarkedLoaded.Should().BeTrue();

        store.TryGetResource(
            "missing",
            null,
            () => throw new InvalidOperationException(),
            (_, _) => (true, "unexpected"),
            out _).Should().BeFalse();
    }

    [Fact]
    public void Invalidate_ForcesLookupOnNextAccess()
    {
        var store = new ThemeResourceStore();
        var lookups = 0;

        store.TryGetResource(
            "key",
            ThemeVariant.Light,
            () => { },
            (_, _) =>
            {
                lookups++;
                return (true, lookups);
            },
            out var first);

        first.Should().Be(1);

        store.Invalidate();

        store.TryGetResource(
            "key",
            ThemeVariant.Light,
            () => { },
            (_, _) =>
            {
                lookups++;
                return (true, lookups);
            },
            out var second);

        second.Should().Be(2);
        lookups.Should().Be(2);
    }

    [Fact]
    public void TryGetResource_ThemeVariantChange_ClearsCache()
    {
        var store = new ThemeResourceStore();

        store.TryGetResource("key", ThemeVariant.Dark, () => { }, (_, _) => (true, "dark"), out var dark);
        dark.Should().Be("dark");

        store.TryGetResource("key", ThemeVariant.Light, () => { }, (_, _) => (true, "light"), out var light);
        light.Should().Be("light");
    }
}
