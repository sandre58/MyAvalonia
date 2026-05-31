// -----------------------------------------------------------------------
// <copyright file="SelectedDatesHelperTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using FluentAssertions;
using MyNet.Avalonia.Controls.Internals;
using Xunit;

namespace MyNet.Avalonia.Controls.Tests.Calendar;

public class SelectedDatesHelperTests
{
    [Fact]
    public void EnumerateDateRange_Forward_IncludesBothEnds()
    {
        var dates = SelectedDatesHelper.EnumerateDateRange(new DateTime(2026, 5, 10), new DateTime(2026, 5, 12)).ToList();

        dates.Should().Equal(new DateTime(2026, 5, 10), new DateTime(2026, 5, 11), new DateTime(2026, 5, 12));
    }

    [Fact]
    public void EnumerateDateRange_Reverse_IncludesBothEnds()
    {
        var dates = SelectedDatesHelper.EnumerateDateRange(new DateTime(2026, 5, 12), new DateTime(2026, 5, 10)).ToList();

        dates.Should().Equal(new DateTime(2026, 5, 12), new DateTime(2026, 5, 11), new DateTime(2026, 5, 10));
    }

    [Fact]
    public void EnumerateDateRange_SingleDay_ReturnsOneDate()
    {
        SelectedDatesHelper.EnumerateDateRange(new DateTime(2026, 5, 15), new DateTime(2026, 5, 15))
            .Should().ContainSingle().Which.Should().Be(new DateTime(2026, 5, 15));
    }
}
