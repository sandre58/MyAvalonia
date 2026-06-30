// -----------------------------------------------------------------------
// <copyright file="CalendarHeadlessTestHelpers.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Threading;
using FluentAssertions;
using MyNet.Avalonia.Controls.Primitives;

namespace MyNet.Avalonia.Controls.Headless.Tests;

internal static class CalendarHeadlessTestHelpers
{
    internal const int DefaultWidth = 420;
    internal const int DefaultHeight = 360;
    internal static readonly DateTime DefaultDisplayDate = new(2026, 5, 15);

    internal static Calendar CreateCalendar(DateTime displayDate)
    {
        HeadlessTestApp.EnsureGlobalizationServices();

        return new()
        {
            DisplayDate = displayDate,
            SelectionMode = CalendarSelectionMode.SingleDate
        };
    }

    internal static RangeCalendarContext ShowRangeCalendar(
        CalendarSelectionMode mode,
        bool allowTap,
        DateTime? displayDate = null)
    {
        var calendar = CreateCalendar(displayDate ?? DefaultDisplayDate);
        calendar.SelectionMode = mode;
        calendar.AllowTapRangeSelection = allowTap;
        HeadlessControlHost.Show(calendar, new(DefaultWidth, DefaultHeight));

        var grid = HeadlessControlHost.FindByName<Grid>(calendar, Calendar.PartMonthGrid);
        grid.Should().NotBeNull();

        return new(calendar, grid!);
    }

    internal static CalendarDayButton FindDayButton(Grid grid, DateTime date) =>
        grid.Children.OfType<CalendarDayButton>().Single(x => x.DataContext is DateTime d && d == date);

    internal static void EstablishDragModeAnchor(Calendar calendar, DateTime anchorDate)
    {
        calendar.MoveToDate(anchorDate.AddDays(-1));
        HeadlessControlHost.KeyDown(calendar, Key.Right);
    }

    internal static void DragSelectRange(Grid grid, DateTime from, DateTime to, KeyModifiers modifiers = default)
    {
        HeadlessControlHost.PointerPress(FindDayButton(grid, from), modifiers);
        HeadlessControlHost.PointerMove(FindDayButton(grid, to), modifiers);
        HeadlessControlHost.PointerRelease(FindDayButton(grid, to), modifiers);
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);
    }

    internal static void AssertPreviewRange(Grid grid, DateTime start, DateTime end)
    {
        var startButton = FindDayButton(grid, start);
        var endButton = FindDayButton(grid, end);
        startButton.IsPreviewStartDate.Should().BeTrue();
        endButton.IsPreviewEndDate.Should().BeTrue();

        if (start == end)
            return;

        var rangeStart = start < end ? start : end;
        var rangeEnd = start < end ? end : start;

        for (var date = rangeStart.AddDays(1); date < rangeEnd; date = date.AddDays(1))
            FindDayButton(grid, date).IsPreviewInRange.Should().BeTrue();
    }

    internal static void AssertCommittedRange(Grid grid, DateTime start, DateTime end, bool middleCellsNotSelected = true)
    {
        var startButton = FindDayButton(grid, start);
        var endButton = FindDayButton(grid, end);
        startButton.IsStartDate.Should().BeTrue();
        endButton.IsEndDate.Should().BeTrue();

        if (start == end)
            return;

        var rangeStart = start < end ? start : end;
        var rangeEnd = start < end ? end : start;

        for (var date = rangeStart.AddDays(1); date < rangeEnd; date = date.AddDays(1))
        {
            var middleButton = FindDayButton(grid, date);
            middleButton.IsInRange.Should().BeTrue();
            if (middleCellsNotSelected)
                middleButton.IsSelected.Should().BeFalse();
        }

        if (middleCellsNotSelected)
        {
            startButton.IsSelected.Should().BeFalse();
            endButton.IsSelected.Should().BeFalse();
        }
    }

    internal static void AssertPreviewStableOverFrames(Grid grid, Action assert, int frameCount = 3)
    {
        for (var i = 0; i < frameCount; i++)
        {
            Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);
            assert();
        }
    }

    internal static void SimulateVerticalGridGap(Grid grid, CalendarDayButton upperButton, CalendarDayButton lowerButton, bool leftButtonPressed = false)
    {
        var gapPosition = GetVerticalGapPosition(grid, upperButton, lowerButton);

        HeadlessControlHost.PointerExitedAt(upperButton, new(4, 4), leftButtonPressed: leftButtonPressed);
        HeadlessControlHost.PointerExitedAt(grid, gapPosition, leftButtonPressed: leftButtonPressed);
        HeadlessControlHost.PointerMoveAt(grid, gapPosition, leftButtonPressed: leftButtonPressed);
    }

    internal static Point GetVerticalGapPosition(Grid grid, CalendarDayButton upperButton, CalendarDayButton lowerButton)
    {
        var upperOrigin = upperButton.TranslatePoint(new(0, 0), grid) ?? default;
        var lowerOrigin = lowerButton.TranslatePoint(new(0, 0), grid) ?? default;
        var gapY = (upperOrigin.Y + upperButton.Bounds.Height + lowerOrigin.Y) / 2;
        var gapX = upperOrigin.X + (upperButton.Bounds.Width / 2);
        return new(gapX, gapY);
    }
}
