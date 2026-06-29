// -----------------------------------------------------------------------
// <copyright file="CalendarHeadlessTests.Helpers.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using MyNet.Avalonia.Controls.Primitives;

namespace MyNet.Avalonia.Controls.Headless.Tests;

public partial class CalendarHeadlessTests
{
    private static RangeCalendarContext ShowRangeCalendar(
        CalendarSelectionMode mode,
        bool allowTap,
        DateTime? displayDate = null) =>
        CalendarHeadlessTestHelpers.ShowRangeCalendar(mode, allowTap, displayDate);

    private static Calendar CreateCalendar(DateTime displayDate) =>
        CalendarHeadlessTestHelpers.CreateCalendar(displayDate);

    private static CalendarDayButton FindDayButton(Grid grid, DateTime date) =>
        CalendarHeadlessTestHelpers.FindDayButton(grid, date);

    private static void EstablishDragModeAnchor(Calendar calendar, DateTime anchorDate) =>
        CalendarHeadlessTestHelpers.EstablishDragModeAnchor(calendar, anchorDate);

    private static void DragSelectRange(Grid grid, DateTime from, DateTime to, KeyModifiers modifiers = default) =>
        CalendarHeadlessTestHelpers.DragSelectRange(grid, from, to, modifiers);

    private static void AssertPreviewRange(Grid grid, DateTime start, DateTime end) =>
        CalendarHeadlessTestHelpers.AssertPreviewRange(grid, start, end);

    private static void AssertCommittedRange(Grid grid, DateTime start, DateTime end, bool middleCellsNotSelected = true) =>
        CalendarHeadlessTestHelpers.AssertCommittedRange(grid, start, end, middleCellsNotSelected);

    private static void AssertPreviewStableOverFrames(Grid grid, Action assert, int frameCount = 3) =>
        CalendarHeadlessTestHelpers.AssertPreviewStableOverFrames(grid, assert, frameCount);

    private static void SimulateVerticalGridGap(Grid grid, CalendarDayButton upperButton, CalendarDayButton lowerButton, bool leftButtonPressed = false) =>
        CalendarHeadlessTestHelpers.SimulateVerticalGridGap(grid, upperButton, lowerButton, leftButtonPressed);

    private static Point GetVerticalGapPosition(Grid grid, CalendarDayButton upperButton, CalendarDayButton lowerButton) =>
        CalendarHeadlessTestHelpers.GetVerticalGapPosition(grid, upperButton, lowerButton);
}
