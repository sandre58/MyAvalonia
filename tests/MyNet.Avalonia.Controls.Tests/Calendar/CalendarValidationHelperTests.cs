// -----------------------------------------------------------------------
// <copyright file="CalendarValidationHelperTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using FluentAssertions;
using MyNet.Avalonia.Controls.Primitives.Internals;
using Xunit;

namespace MyNet.Avalonia.Controls.Tests.Calendar;

public class CalendarValidationHelperTests
{
    [Theory]
    [InlineData(DayOfWeek.Monday, true)]
    [InlineData(DayOfWeek.Sunday, true)]
    [InlineData((DayOfWeek)7, false)]
    public void IsValidFirstDayOfWeek_ValidatesEnumValues(DayOfWeek day, bool expected)
    {
        CalendarValidationHelper.IsValidFirstDayOfWeek(day).Should().Be(expected);
    }

    [Theory]
    [InlineData(CalendarSelectionMode.SingleDate, true)]
    [InlineData(CalendarSelectionMode.SingleRange, true)]
    [InlineData(CalendarSelectionMode.MultipleRange, true)]
    [InlineData(CalendarSelectionMode.None, true)]
    [InlineData((CalendarSelectionMode)99, false)]
    public void IsValidSelectionMode_ValidatesEnumValues(CalendarSelectionMode mode, bool expected)
    {
        CalendarValidationHelper.IsValidSelectionMode(mode).Should().Be(expected);
    }

    [Fact]
    public void IsValidSelection_RejectsBlackoutDates()
    {
        var date = new DateTime(2026, 5, 15);
        var start = new DateTime(2026, 5, 1);
        var end = new DateTime(2026, 5, 31);

        CalendarValidationHelper.IsValidSelection(date, start, end, d => d == date).Should().BeFalse();
    }

    [Fact]
    public void IsValidSelection_RejectsDatesOutsideRange()
    {
        var start = new DateTime(2026, 5, 1);
        var end = new DateTime(2026, 5, 31);

        CalendarValidationHelper.IsValidSelection(new DateTime(2026, 6, 1), start, end, _ => false).Should().BeFalse();
        CalendarValidationHelper.IsValidSelection(new DateTime(2026, 5, 15), start, end, _ => false).Should().BeTrue();
    }
}
