// -----------------------------------------------------------------------
// <copyright file="CalendarMultipleRangeSelectionTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using FluentAssertions;
using MyNet.Avalonia.Controls.Internals;
using MyNet.Primitives;
using Xunit;

namespace MyNet.Avalonia.Controls.Tests.Calendar;

public class CalendarMultipleRangeSelectionTests
{
    [Fact]
    public void ProcessDateSelection_CtrlClickOnSelectedDate_RemovesDate()
    {
        var commands = new RecordingSelectionCommands();
        var coordinator = CreateCoordinator(commands);

        coordinator.Commit(new(2026, 5, 10), shift: false, ctrl: true);
        commands.Contains(new(2026, 5, 10)).Should().BeTrue();

        coordinator.Commit(new(2026, 5, 10), shift: false, ctrl: true);

        commands.Contains(new(2026, 5, 10)).Should().BeFalse();
    }

    [Fact]
    public void ProcessDateSelection_ShiftClicks_KeepsAnchorForSubsequentShift()
    {
        var commands = new RecordingSelectionCommands();
        var coordinator = CreateCoordinator(commands);

        coordinator.Commit(new(2026, 5, 5), shift: false, ctrl: false);
        coordinator.Commit(new(2026, 5, 20), shift: true, ctrl: false);
        coordinator.Commit(new(2026, 5, 25), shift: true, ctrl: false);

        commands.Ranges.Should().HaveCount(2);
        commands.Ranges[1].Should().Be((new(2026, 5, 5), new(2026, 5, 25)));
        coordinator.HoverStart.Should().Be(new(2026, 5, 5));
    }

    [Fact]
    public void PointerSelectionEnd_DragWithoutShift_ReplacesWithRange()
    {
        var commands = new RecordingSelectionCommands();
        var coordinator = CreateCoordinator(commands);

        coordinator.BeginPointerSelection(new(2026, 5, 5), shift: false);
        coordinator.CompletePointerSelection(new(2026, 5, 15), shift: false, ctrl: false, wasDrag: true);

        commands.Ranges.Should().ContainSingle()
            .Which.Should().Be((new(2026, 5, 5), new(2026, 5, 15)));
        coordinator.HoverStart.Should().Be(new(2026, 5, 5));
    }

    [Fact]
    public void CommitFromKeyboard_CtrlWithoutPreview_TogglesSelection()
    {
        var commands = new RecordingSelectionCommands();
        var coordinator = CreateCoordinator(commands);

        coordinator.CommitFromKeyboard(new(2026, 5, 10), intervalPreview: false, ctrl: true);

        commands.Contains(new(2026, 5, 10)).Should().BeTrue();
        coordinator.HoverStart.Should().Be(new(2026, 5, 10));

        coordinator.CommitFromKeyboard(new(2026, 5, 10), intervalPreview: false, ctrl: true);

        commands.Contains(new(2026, 5, 10)).Should().BeFalse();
    }

    [Fact]
    public void CommitFromKeyboard_ShiftIntervalPreview_ReplacesRange()
    {
        var commands = new RecordingSelectionCommands();
        var coordinator = CreateCoordinator(commands);

        coordinator.Commit(new(2026, 5, 5), shift: false, ctrl: false);
        coordinator.CommitFromKeyboard(new(2026, 5, 15), intervalPreview: true, shift: true, ctrl: false);

        commands.Ranges.Should().ContainSingle()
            .Which.Should().Be((new(2026, 5, 5), new(2026, 5, 15)));
        coordinator.HoverStart.Should().Be(new(2026, 5, 5));
    }

    [Fact]
    public void CommitFromKeyboard_CtrlShiftIntervalPreview_AddsRange()
    {
        var commands = new RecordingSelectionCommands();
        var coordinator = CreateCoordinator(commands);

        coordinator.Commit(new(2026, 5, 1), shift: false, ctrl: false);
        coordinator.Commit(new(2026, 5, 7), shift: true, ctrl: false);
        coordinator.CommitFromKeyboard(new(2026, 5, 18), intervalPreview: true, shift: true, ctrl: true);

        commands.Ranges.Should().HaveCount(2);
        commands.Ranges[0].Should().Be((new(2026, 5, 1), new(2026, 5, 7)));
        commands.Ranges[1].Should().Be((new(2026, 5, 1), new(2026, 5, 18)));
    }

    [Fact]
    public void PointerSelectionEnd_CtrlShiftDrag_AddsRangeToExistingSelection()
    {
        var commands = new RecordingSelectionCommands();
        var coordinator = CreateCoordinator(commands);

        coordinator.Commit(new(2026, 5, 1), shift: false, ctrl: false);
        coordinator.Commit(new(2026, 5, 7), shift: true, ctrl: false);
        coordinator.Commit(new(2026, 5, 12), shift: false, ctrl: true);
        coordinator.BeginPointerSelection(new(2026, 5, 12), shift: false);
        coordinator.CompletePointerSelection(new(2026, 5, 18), shift: true, ctrl: true, wasDrag: true);

        commands.Ranges.Should().HaveCount(2);
        commands.Ranges[0].Should().Be((new(2026, 5, 1), new(2026, 5, 7)));
        commands.Ranges[1].Should().Be((new(2026, 5, 12), new(2026, 5, 18)));
    }

    [Fact]
    public void Commit_PlainClickOnMiddleOfRange_ReplacesWithSingleDate()
    {
        var commands = new RecordingSelectionCommands();
        var coordinator = CreateCoordinator(commands);

        coordinator.BeginPointerSelection(new(2026, 5, 10), shift: false);
        coordinator.CompletePointerSelection(new(2026, 5, 14), shift: false, ctrl: false, wasDrag: true);

        coordinator.Commit(new(2026, 5, 12), shift: false, ctrl: false);

        commands.Contains(new(2026, 5, 12)).Should().BeTrue();
        commands.Contains(new(2026, 5, 10)).Should().BeFalse();
        commands.Contains(new(2026, 5, 11)).Should().BeFalse();
        commands.Contains(new(2026, 5, 13)).Should().BeFalse();
        commands.Contains(new(2026, 5, 14)).Should().BeFalse();
    }

    [Fact]
    public void Commit_PlainClickOnOnlySelectedDate_KeepsSingleSelection()
    {
        var commands = new RecordingSelectionCommands();
        var coordinator = CreateCoordinator(commands);

        coordinator.Commit(new(2026, 5, 12), shift: false, ctrl: false);
        coordinator.Commit(new(2026, 5, 12), shift: false, ctrl: false);

        commands.Singles.Should().HaveCount(2);
        commands.Contains(new(2026, 5, 12)).Should().BeTrue();
    }

    private static CalendarSelectionCoordinator CreateCoordinator(RecordingSelectionCommands commands) =>
        new(
            () => CalendarSelectionMode.MultipleRange,
            () => false,
            () => new(2026, 5, 1),
            _ => true,
            commands);

    private sealed class RecordingSelectionCommands : ICalendarSelectionCommands
    {
        private readonly HashSet<DateTime> _dates = [];

        public List<DateTime> Singles { get; } = [];

        public List<(DateTime Start, DateTime End)> Ranges { get; } = [];

        public List<DateTime> Moves { get; } = [];

        public void SetSelection(DateTime date)
        {
            Singles.Add(date);
            _dates.Clear();
            _dates.Add(date.DiscardTime());
        }

        public void SetSelection(DateTime start, DateTime end)
        {
            Ranges.Add((start, end));
            _dates.Clear();
            foreach (var date in SelectedDatesHelper.EnumerateDateRange(start, end))
                _dates.Add(date.DiscardTime());
        }

        public void AddSelection(DateTime date)
        {
            Singles.Add(date);
            _dates.Add(date.DiscardTime());
        }

        public void AddSelection(DateTime start, DateTime end)
        {
            Ranges.Add((start, end));
            foreach (var date in SelectedDatesHelper.EnumerateDateRange(start, end))
                _dates.Add(date.DiscardTime());
        }

        public void ToggleSelection(DateTime date)
        {
            var normalized = date.DiscardTime();
            if (_dates.Remove(normalized))
                Singles.Add(date);
            else
            {
                Singles.Add(date);
                _dates.Add(normalized);
            }
        }

        public void ChangeSelection(DateTime start, DateTime end, bool isSelected) => Ranges.Add((start, end));

        public bool Contains(DateTime date) => _dates.Contains(date.DiscardTime());

        public void MoveToDate(DateTime date) => Moves.Add(date);
    }
}
