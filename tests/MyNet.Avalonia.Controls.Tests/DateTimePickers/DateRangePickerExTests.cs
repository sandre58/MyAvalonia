// -----------------------------------------------------------------------
// <copyright file="DateRangePickerExTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using FluentAssertions;
using MyNet.Avalonia.Controls.DateTimePickers.Internal;
using MyNet.Avalonia.Controls.Internals.Calendar;
using MyNet.Primitives;
using Xunit;

namespace MyNet.Avalonia.Controls.Tests.DateTimePickers;

public class DateRangePickerExTests
{
    [Fact]
    public void SelectedValue_SyncsStartDateAndEndDate()
    {
        var picker = CreatePicker();
        var start = new DateTime(2026, 3, 10);
        var end = new DateTime(2026, 3, 20);

        picker.SelectedValue = start.ToPeriod(end);

        picker.StartDate.Should().Be(start);
        picker.EndDate.Should().Be(end);
    }

    [Fact]
    public void StartDateAndEndDate_UpdateSelectedValue()
    {
        var picker = CreatePicker();
        var start = new DateTime(2026, 4, 1);
        var end = new DateTime(2026, 4, 15);

        picker.StartDate = start;
        picker.EndDate = end;

        picker.SelectedValue.Should().NotBeNull();
        picker.StartDate.Should().Be(start);
        picker.EndDate.Should().Be(end);
    }

    [Fact]
    public void StartDateAfterEndDate_NormalizesSelectedValue()
    {
        var picker = CreatePicker();

        picker.StartDate = new DateTime(2026, 5, 20);
        picker.EndDate = new DateTime(2026, 5, 10);

        picker.StartDate.Should().Be(new(2026, 5, 10));
        picker.EndDate.Should().Be(new(2026, 5, 20));
    }

    [Fact]
    public void SelectedValue_UpdatesTextWithRangeSeparator()
    {
        var picker = CreatePicker();
        picker.DisplayFormat = "yyyy-MM-dd";
        picker.RangeSeparator = " -> ";
        picker.SelectedValue = new DateTime(2026, 6, 1).ToPeriod(new DateTime(2026, 6, 7));

        picker.Text.Should().Be("2026-06-01 -> 2026-06-07");
    }

    [Fact]
    public void IncrementValue_ShiftsEntirePeriod()
    {
        var picker = CreatePicker();
        var start = new DateTime(2026, 7, 1);
        var end = new DateTime(2026, 7, 5);
        picker.SelectedValue = start.ToPeriod(end);

        picker.Increment(2);

        picker.StartDate.Should().Be(start.AddDays(2));
        picker.EndDate.Should().Be(end.AddDays(2));
    }

    [Fact]
    public void Clear_ResetsSelectedValue()
    {
        var picker = CreatePicker();
        picker.SelectedValue = new DateTime(2026, 8, 1).ToPeriod(new DateTime(2026, 8, 3));

        picker.Clear();

        picker.SelectedValue.Should().BeNull();
        picker.IsEmpty().Should().BeTrue();
    }

    [Fact]
    public void SelectedValue_SingleDayRange_SyncsStartAndEndDate()
    {
        var picker = CreatePicker();
        var date = new DateTime(2026, 9, 10);

        picker.SelectedValue = CalendarDateRangeHelper.ToDateRangePeriod(date, date);

        picker.StartDate.Should().Be(date);
        picker.EndDate.Should().Be(date);
        picker.Text.Should().Be("2026-09-10 – 2026-09-10");
    }

    private static DateRangePickerEx CreatePicker()
    {
        var picker = new DateRangePickerEx { DisplayFormat = "yyyy-MM-dd" };
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        return picker;
    }
}
