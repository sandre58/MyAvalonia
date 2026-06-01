// -----------------------------------------------------------------------
// <copyright file="CalendarDateRangeHelperExpandedTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using FluentAssertions;
using MyNet.Avalonia.Controls.Internals;
using Xunit;

namespace MyNet.Avalonia.Controls.Tests.Calendar;

public class CalendarDateRangeHelperExpandedTests
{
    [Fact]
    public void GetSelectedMin_And_Max_WithEmptyCollection_ReturnNull()
    {
        CalendarDateRangeHelper.GetSelectedMin([]).Should().BeNull();
        CalendarDateRangeHelper.GetSelectedMax([]).Should().BeNull();
    }

    [Fact]
    public void ResolveDisplayDateStartChange_WhenSelectedBeforeNewStart_AdjustsStart()
    {
        var adjustment = CalendarDateRangeHelper.ResolveDisplayDateStartChange(
            new(2026, 6, 1),
            new(2026, 12, 31),
            new(2026, 5, 1),
            new DateTime(2026, 5, 15),
            null);

        adjustment.Should().NotBeNull();
        adjustment.Value.DisplayDateStart.Should().Be(new(2026, 5, 15));
    }

    [Fact]
    public void ResolveDisplayDateStartChange_WhenStartAfterDisplayDate_UpdatesDisplayDate()
    {
        var adjustment = CalendarDateRangeHelper.ResolveDisplayDateStartChange(
            new(2026, 8, 1),
            new(2026, 12, 31),
            new(2026, 5, 1),
            null,
            null);

        adjustment.Should().NotBeNull();
        adjustment.Value.DisplayDate.Should().Be(new(2026, 8, 1));
        adjustment.Value.RequiresRefresh.Should().BeTrue();
    }

    [Fact]
    public void ResolveDisplayDateEndChange_WhenEndBeforeDisplayDate_UpdatesDisplayDate()
    {
        var adjustment = CalendarDateRangeHelper.ResolveDisplayDateEndChange(
            new(2026, 2, 28),
            new(2026, 1, 1),
            new(2026, 5, 1),
            null);

        adjustment.Should().NotBeNull();
        adjustment.Value.DisplayDate.Should().Be(new(2026, 2, 28));
    }

    [Fact]
    public void ResolveDisplayDateEndChange_WhenEndBeforeRangeStart_ClampsEndToStart()
    {
        var adjustment = CalendarDateRangeHelper.ResolveDisplayDateEndChange(
            new(2025, 12, 31),
            new(2026, 1, 1),
            new(2026, 5, 1),
            null);

        adjustment.Should().NotBeNull();
        adjustment.Value.DisplayDateEnd.Should().Be(new(2026, 1, 1));
    }
}
