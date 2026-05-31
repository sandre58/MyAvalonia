// -----------------------------------------------------------------------
// <copyright file="CalendarSelectionCoordinatorExpandedTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Avalonia.Controls;
using FluentAssertions;
using MyNet.Avalonia.Controls.Internals;
using Xunit;

namespace MyNet.Avalonia.Controls.Tests.Calendar;

public class CalendarSelectionCoordinatorExpandedTests
{
    [Fact]
    public void ProcessDateSelection_MultipleRangeWithCtrl_TogglesSelection()
    {
        var commands = new RecordingSelectionCommands();
        var coordinator = new CalendarSelectionCoordinator(
            () => CalendarSelectionMode.MultipleRange,
            () => false,
            () => new DateTime(2026, 5, 1),
            _ => true,
            commands);

        coordinator.ProcessDateSelection(new DateTime(2026, 5, 10), shift: false, ctrl: true);

        commands.Singles.Should().ContainSingle().Which.Should().Be(new DateTime(2026, 5, 10));
    }

    [Fact]
    public void ProcessTapRangeSelection_MultipleRangeWithCtrl_AddsTwoTapRange()
    {
        var commands = new RecordingSelectionCommands();
        var coordinator = new CalendarSelectionCoordinator(
            () => CalendarSelectionMode.MultipleRange,
            () => true,
            () => new DateTime(2026, 5, 1),
            _ => true,
            commands);

        coordinator.ProcessTapRangeSelection(new DateTime(2026, 5, 5), ctrl: true);
        coordinator.ProcessTapRangeSelection(new DateTime(2026, 5, 15), ctrl: true);

        commands.Ranges.Should().ContainSingle()
            .Which.Should().Be((new DateTime(2026, 5, 5), new DateTime(2026, 5, 15)));
    }

    [Fact]
    public void BeginPointerSelection_WithShift_KeepsExistingHoverStart()
    {
        var commands = new RecordingSelectionCommands();
        var coordinator = new CalendarSelectionCoordinator(
            () => CalendarSelectionMode.SingleRange,
            () => false,
            () => new DateTime(2026, 5, 1),
            _ => true,
            commands);

        coordinator.ProcessDateSelection(new DateTime(2026, 5, 5), shift: false, ctrl: false);
        coordinator.BeginPointerSelection(new DateTime(2026, 5, 20), shift: true);

        coordinator.HoverStart.Should().Be(new DateTime(2026, 5, 5));
    }

    [Fact]
    public void ProcessDateSelection_SingleRangeWithoutShift_ReplacesSelection()
    {
        var commands = new RecordingSelectionCommands();
        var coordinator = new CalendarSelectionCoordinator(
            () => CalendarSelectionMode.SingleRange,
            () => false,
            () => new DateTime(2026, 5, 1),
            _ => true,
            commands);

        coordinator.ProcessDateSelection(new DateTime(2026, 5, 10), shift: false, ctrl: false);
        coordinator.ProcessDateSelection(new DateTime(2026, 5, 20), shift: false, ctrl: false);

        commands.Singles.Should().Equal(new DateTime(2026, 5, 10), new DateTime(2026, 5, 20));
    }

    private sealed class RecordingSelectionCommands : ICalendarSelectionCommands
    {
        public List<DateTime> Singles { get; } = [];

        public List<(DateTime Start, DateTime End)> Ranges { get; } = [];

        public List<DateTime> Moves { get; } = [];

        public void SetSelection(DateTime date) => Singles.Add(date);

        public void SetSelection(DateTime start, DateTime end) => Ranges.Add((start, end));

        public void AddSelection(DateTime date) => Singles.Add(date);

        public void AddSelection(DateTime start, DateTime end) => Ranges.Add((start, end));

        public void ToggleSelection(DateTime date) => Singles.Add(date);

        public void ChangeSelection(DateTime start, DateTime end, bool isSelected) => Ranges.Add((start, end));

        public bool Contains(DateTime date) => false;

        public void MoveToDate(DateTime date) => Moves.Add(date);
    }
}
