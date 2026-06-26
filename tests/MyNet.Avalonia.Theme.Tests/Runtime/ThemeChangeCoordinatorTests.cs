// -----------------------------------------------------------------------
// <copyright file="ThemeChangeCoordinatorTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using FluentAssertions;
using MyNet.Avalonia.Theme.Runtime;
using Xunit;

namespace MyNet.Avalonia.Theme.Tests.Runtime;

public class ThemeChangeCoordinatorTests
{
    [Fact]
    public void NotifyChange_WhenDeferred_IsSuppressedUntilScopeEnds()
    {
        var version = 0;
        var coordinator = new ThemeChangeCoordinator(new(), () => version++);

        using (coordinator.Defer())
        {
            coordinator.NotifyChange();
            version.Should().Be(0);
        }

        version.Should().Be(1);
    }

    [Fact]
    public void NotifyChange_AfterDeferredScope_IncrementsThemeVersion()
    {
        var version = 0;
        var coordinator = new ThemeChangeCoordinator(new(), () => version++);

        using (coordinator.Defer())
        {
            coordinator.NotifyChange();
        }

        coordinator.NotifyChange();
        version.Should().Be(2);
    }
}
