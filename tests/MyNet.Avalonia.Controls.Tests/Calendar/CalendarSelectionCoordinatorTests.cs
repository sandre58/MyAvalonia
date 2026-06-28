// -----------------------------------------------------------------------
// <copyright file="CalendarSelectionCoordinatorTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Avalonia.Controls;
using FluentAssertions;
using MyNet.Avalonia.Controls.Internals;
using MyNet.Primitives;
using Xunit;

namespace MyNet.Avalonia.Controls.Tests.Calendar;

public class CalendarSelectionCoordinatorTests
{
    [Fact]
    public void ProcessDateSelection_SingleDate_SelectsDate()
    {
        var commands = new RecordingSelectionCommands();
        var coordinator = new CalendarSelectionCoordinator(
            () => CalendarSelectionMode.SingleDate,
            () => false,
            () => new(2026, 5, 1),
            _ => true,
            commands);

        coordinator.ProcessDateSelection(new(2026, 5, 10), shift: false, ctrl: false);

        commands.Singles.Should().ContainSingle().Which.Should().Be(new(2026, 5, 10));
        commands.Moves.Should().ContainSingle().Which.Should().Be(new(2026, 5, 10));
        coordinator.HoverStart.Should().BeNull();
    }

    [Fact]
    public void ProcessDateSelection_SingleRangeWithShift_UsesHoverStart()
    {
        var commands = new RecordingSelectionCommands();
        var coordinator = new CalendarSelectionCoordinator(
            () => CalendarSelectionMode.SingleRange,
            () => false,
            () => new(2026, 5, 1),
            _ => true,
            commands);

        coordinator.ProcessDateSelection(new(2026, 5, 5), shift: false, ctrl: false);
        coordinator.ProcessDateSelection(new(2026, 5, 15), shift: true, ctrl: false);

        commands.Ranges.Should().ContainSingle()
            .Which.Should().Be((new(2026, 5, 5), new(2026, 5, 15)));
        coordinator.HoverStart.Should().Be(new(2026, 5, 5));
    }

    [Fact]
    public void ProcessDateSelection_SingleRangeWithRepeatedShift_UpdatesRangeEnd()
    {
        var commands = new RecordingSelectionCommands();
        var coordinator = new CalendarSelectionCoordinator(
            () => CalendarSelectionMode.SingleRange,
            () => false,
            () => new(2026, 5, 1),
            _ => true,
            commands);

        coordinator.ProcessDateSelection(new(2026, 5, 5), shift: false, ctrl: false);
        coordinator.ProcessDateSelection(new(2026, 5, 15), shift: true, ctrl: false);
        coordinator.ProcessDateSelection(new(2026, 5, 20), shift: true, ctrl: false);

        commands.Ranges.Should().HaveCount(2);
        commands.Ranges[1].Should().Be((new(2026, 5, 5), new(2026, 5, 20)));
        coordinator.HoverStart.Should().Be(new(2026, 5, 5));
    }

    [Fact]
    public void ResetHover_ClearsHoverStart()
    {
        var commands = new RecordingSelectionCommands();
        var coordinator = new CalendarSelectionCoordinator(
            () => CalendarSelectionMode.SingleDate,
            () => false,
            () => new(2026, 5, 1),
            _ => true,
            commands);

        coordinator.ProcessDateSelection(new(2026, 5, 10), shift: false, ctrl: false);
        coordinator.ResetHover();

        coordinator.HoverStart.Should().BeNull();
    }

    [Fact]
    public void ProcessTapRangeSelection_SingleRange_CompletesOnSecondTap()
    {
        var commands = new RecordingSelectionCommands();
        var coordinator = new CalendarSelectionCoordinator(
            () => CalendarSelectionMode.SingleRange,
            () => true,
            () => new(2026, 5, 1),
            _ => true,
            commands);

        coordinator.ProcessTapRangeSelection(new(2026, 5, 10), ctrl: false);
        coordinator.ProcessTapRangeSelection(new(2026, 5, 20), ctrl: false);

        commands.Ranges.Should().ContainSingle()
            .Which.Should().Be((new(2026, 5, 10), new(2026, 5, 20)));
        coordinator.HoverStart.Should().BeNull();
    }

    [Fact]
    public void ProcessTapRangeSelection_SingleRangeWithShift_KeepsAnchor()
    {
        var commands = new RecordingSelectionCommands();
        var coordinator = new CalendarSelectionCoordinator(
            () => CalendarSelectionMode.SingleRange,
            () => true,
            () => new(2026, 5, 1),
            _ => true,
            commands);

        coordinator.ProcessTapRangeSelection(new(2026, 5, 10), ctrl: false);
        coordinator.ProcessTapRangeSelection(new(2026, 5, 20), ctrl: false, shift: true);
        coordinator.ProcessTapRangeSelection(new(2026, 5, 25), ctrl: false, shift: true);

        commands.Ranges.Should().HaveCount(2);
        commands.Ranges[1].Should().Be((new(2026, 5, 10), new(2026, 5, 25)));
        coordinator.HoverStart.Should().Be(new(2026, 5, 10));
    }

    [Fact]
    public void ProcessDateSelection_SingleRangeTapWithShift_KeepsAnchor()
    {
        var commands = new RecordingSelectionCommands();
        var coordinator = new CalendarSelectionCoordinator(
            () => CalendarSelectionMode.SingleRange,
            () => true,
            () => new(2026, 5, 1),
            _ => true,
            commands);

        coordinator.ProcessDateSelection(new(2026, 5, 10), shift: false, ctrl: false);
        coordinator.ProcessDateSelection(new(2026, 5, 20), shift: true, ctrl: false);
        coordinator.ProcessDateSelection(new(2026, 5, 25), shift: true, ctrl: false);

        commands.Ranges.Should().HaveCount(2);
        commands.Ranges[1].Should().Be((new(2026, 5, 10), new(2026, 5, 25)));
        coordinator.HoverStart.Should().Be(new(2026, 5, 10));
    }

    [Fact]
    public void ProcessDateSelection_MultipleRangeWithCtrl_AddsRangeFromAnchor()
    {
        var commands = new RecordingSelectionCommands();
        var coordinator = new CalendarSelectionCoordinator(
            () => CalendarSelectionMode.MultipleRange,
            () => false,
            () => new(2026, 5, 1),
            _ => true,
            commands);

        coordinator.ProcessDateSelection(new(2026, 5, 10), shift: false, ctrl: true);
        coordinator.ProcessDateSelection(new(2026, 5, 20), shift: true, ctrl: true);

        commands.Singles.Should().ContainSingle().Which.Should().Be(new(2026, 5, 10));
        commands.Ranges.Should().ContainSingle()
            .Which.Should().Be((new(2026, 5, 10), new(2026, 5, 20)));
        coordinator.HoverStart.Should().Be(new(2026, 5, 10));
    }

    [Fact]
    public void ProcessDateSelection_MultipleRangeWithAllowTapAndCtrl_CompletesIntervalWithoutShift()
    {
        var commands = new RecordingSelectionCommands();
        var coordinator = new CalendarSelectionCoordinator(
            () => CalendarSelectionMode.MultipleRange,
            () => true,
            () => new(2026, 5, 1),
            _ => true,
            commands);

        coordinator.ProcessDateSelection(new(2026, 5, 5), shift: false, ctrl: true);
        coordinator.ProcessDateSelection(new(2026, 5, 15), shift: false, ctrl: true);

        commands.Ranges.Should().ContainSingle()
            .Which.Should().Be((new(2026, 5, 5), new(2026, 5, 15)));
        coordinator.HoverStart.Should().BeNull();
    }

    [Fact]
    public void ProcessDateSelection_MultipleRangeWithShiftThenCtrlAnchor_CompletesIntervalWithCtrlShift()
    {
        var commands = new RecordingSelectionCommands();
        var coordinator = new CalendarSelectionCoordinator(
            () => CalendarSelectionMode.MultipleRange,
            () => false,
            () => new(2026, 5, 1),
            _ => true,
            commands);

        coordinator.ProcessDateSelection(new(2026, 5, 1), shift: false, ctrl: false);
        coordinator.ProcessDateSelection(new(2026, 5, 7), shift: true, ctrl: false);
        coordinator.ProcessDateSelection(new(2026, 5, 12), shift: false, ctrl: true);
        coordinator.ProcessDateSelection(new(2026, 5, 18), shift: true, ctrl: true);

        commands.Ranges.Should().HaveCount(2);
        commands.Ranges[0].Should().Be((new(2026, 5, 1), new(2026, 5, 7)));
        commands.Ranges[1].Should().Be((new(2026, 5, 12), new(2026, 5, 18)));
        coordinator.HoverStart.Should().Be(new(2026, 5, 12));
    }

    [Fact]
    public void ProcessDateSelection_IgnoresInvalidDates()
    {
        var commands = new RecordingSelectionCommands();
        var coordinator = new CalendarSelectionCoordinator(
            () => CalendarSelectionMode.SingleDate,
            () => false,
            () => new(2026, 5, 1),
            _ => false,
            commands);

        coordinator.ProcessDateSelection(new(2026, 5, 10), shift: false, ctrl: false);

        commands.Singles.Should().BeEmpty();
        commands.Moves.Should().BeEmpty();
    }

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
