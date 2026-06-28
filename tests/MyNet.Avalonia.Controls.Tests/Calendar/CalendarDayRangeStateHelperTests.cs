// -----------------------------------------------------------------------
// <copyright file="CalendarDayRangeStateHelperTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using FluentAssertions;
using MyNet.Avalonia.Controls.Internals;
using MyNet.Avalonia.Controls.Primitives;
using Xunit;

namespace MyNet.Avalonia.Controls.Tests.Calendar;

public class CalendarDayRangeStateHelperTests
{
    [Fact]
    public void EnumerateConsecutiveRanges_SplitsDisjointSegments()
    {
        var dates = new[]
        {
            new DateTime(2026, 5, 10),
            new DateTime(2026, 5, 11),
            new DateTime(2026, 5, 20),
            new DateTime(2026, 5, 21),
            new DateTime(2026, 5, 22),
        };

        CalendarDayRangeStateHelper.EnumerateConsecutiveRanges(dates)
            .Should().Equal(
                (new DateTime(2026, 5, 10), new DateTime(2026, 5, 11)),
                (new DateTime(2026, 5, 20), new DateTime(2026, 5, 22)));
    }

    [Fact]
    public void ApplyRangeSegmentToCell_PreviewRange_SetsExpectedStates()
    {
        var start = new CalendarDayButton();
        var middle = new CalendarDayButton();
        var end = new CalendarDayButton();
        var rangeStart = new DateTime(2026, 6, 10);
        var rangeEnd = new DateTime(2026, 6, 12);

        CalendarDayRangeStateHelper.ApplyRangeSegmentToCell(start, rangeStart, rangeStart, rangeEnd, isPreview: true);
        CalendarDayRangeStateHelper.ApplyRangeSegmentToCell(middle, rangeStart.AddDays(1), rangeStart, rangeEnd, isPreview: true);
        CalendarDayRangeStateHelper.ApplyRangeSegmentToCell(end, rangeEnd, rangeStart, rangeEnd, isPreview: true);

        start.IsPreviewStartDate.Should().BeTrue();
        start.IsPreviewEndDate.Should().BeFalse();
        middle.IsPreviewInRange.Should().BeTrue();
        end.IsPreviewEndDate.Should().BeTrue();
    }

    [Fact]
    public void ApplyPreviewRangeToCell_ReversedHover_UsesChronologicalCaps()
    {
        var start = new CalendarDayButton();
        var end = new CalendarDayButton();
        var anchor = new DateTime(2026, 6, 14);
        var previewEnd = new DateTime(2026, 6, 10);

        CalendarDayRangeStateHelper.ApplyPreviewRangeToCell(start, previewEnd, anchor, previewEnd);
        CalendarDayRangeStateHelper.ApplyPreviewRangeToCell(end, anchor, anchor, previewEnd);

        start.IsPreviewStartDate.Should().BeTrue();
        start.IsPreviewEndDate.Should().BeFalse();
        end.IsPreviewEndDate.Should().BeTrue();
        end.IsPreviewStartDate.Should().BeFalse();
    }

    [Fact]
    public void SetPreviewRangeToCell_ExtendedRange_PromotesPreviousEndToInRange()
    {
        var cell = new CalendarDayButton();
        var anchor = new DateTime(2026, 6, 10);
        var previousEnd = new DateTime(2026, 6, 12);
        var newEnd = new DateTime(2026, 6, 13);

        CalendarDayRangeStateHelper.SetPreviewRangeToCell(cell, previousEnd, anchor, previousEnd);
        cell.IsPreviewEndDate.Should().BeTrue();
        cell.IsPreviewInRange.Should().BeFalse();

        CalendarDayRangeStateHelper.SetPreviewRangeToCell(cell, previousEnd, anchor, newEnd);

        cell.IsPreviewEndDate.Should().BeFalse();
        cell.IsPreviewInRange.Should().BeTrue();
        cell.IsPreviewStartDate.Should().BeFalse();
    }

    [Fact]
    public void ApplyRangeSegmentToCell_CommittedSingleDay_SetsStartAndEnd()
    {
        var cell = new CalendarDayButton();
        var date = new DateTime(2026, 7, 4);

        CalendarDayRangeStateHelper.ApplyRangeSegmentToCell(cell, date, date, date, isPreview: false);

        cell.IsStartDate.Should().BeTrue();
        cell.IsEndDate.Should().BeTrue();
        cell.IsInRange.Should().BeFalse();
    }

    [Fact]
    public void ClearRangeState_ResetsAllRangeFlags()
    {
        var cell = new CalendarDayButton
        {
            IsStartDate = true,
            IsEndDate = true,
            IsPreviewStartDate = true,
            IsPreviewEndDate = true,
            IsInRange = true,
        };

        CalendarDayRangeStateHelper.ClearRangeState(cell);

        cell.IsStartDate.Should().BeFalse();
        cell.IsEndDate.Should().BeFalse();
        cell.IsPreviewStartDate.Should().BeFalse();
        cell.IsPreviewEndDate.Should().BeFalse();
        cell.IsInRange.Should().BeFalse();
        cell.IsPreviewInRange.Should().BeFalse();
    }

    [Fact]
    public void ClearCommittedRangeState_ResetsOnlyCommittedFlags()
    {
        var cell = new CalendarDayButton
        {
            IsStartDate = true,
            IsEndDate = true,
            IsPreviewStartDate = true,
            IsPreviewEndDate = true,
            IsInRange = true,
        };

        CalendarDayRangeStateHelper.ClearCommittedRangeState(cell);

        cell.IsStartDate.Should().BeFalse();
        cell.IsEndDate.Should().BeFalse();
        cell.IsInRange.Should().BeFalse();
        cell.IsPreviewStartDate.Should().BeTrue();
        cell.IsPreviewEndDate.Should().BeTrue();
    }

    [Fact]
    public void CellMatchesCommittedInterval_SingleDay_ReturnsTrueWhenCorrect()
    {
        var cell = new CalendarDayButton();
        var date = new DateTime(2026, 6, 15);

        CalendarDayRangeStateHelper.ApplyRangeSegmentToCell(cell, date, date, date, isPreview: false);

        CalendarDayRangeStateHelper.CellMatchesCommittedInterval(cell, date, date, date).Should().BeTrue();
    }
}
