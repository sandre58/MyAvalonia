// -----------------------------------------------------------------------
// <copyright file="CalendarBlackoutDatesHelperTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using FluentAssertions;
using MyNet.Avalonia.Controls.Internals;
using Xunit;

namespace MyNet.Avalonia.Controls.Tests.Calendar;

public class CalendarBlackoutDatesHelperTests
{
    [Fact]
    public void NormalizeRange_OrdersAscending()
    {
        var (start, end) = CalendarBlackoutDatesHelper.NormalizeRange(
            new(2026, 5, 20, 14, 30, 0),
            new(2026, 5, 10, 8, 0, 0));

        start.Should().Be(new(2026, 5, 10));
        end.Should().Be(new(2026, 5, 20));
    }

    [Fact]
    public void NormalizeRange_DiscardsTimeComponent()
    {
        var (start, end) = CalendarBlackoutDatesHelper.NormalizeRange(
            new(2026, 1, 1, 23, 59, 59),
            new(2026, 1, 2, 0, 0, 1));

        start.Should().Be(new(2026, 1, 1));
        end.Should().Be(new(2026, 1, 2));
    }
}
