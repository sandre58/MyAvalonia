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
            () => new DateTime(2026, 5, 1),
            _ => true,
            commands);

        coordinator.ProcessDateSelection(new DateTime(2026, 5, 10), shift: false, ctrl: false);

        commands.Singles.Should().ContainSingle().Which.Should().Be(new DateTime(2026, 5, 10));
        commands.Moves.Should().ContainSingle().Which.Should().Be(new DateTime(2026, 5, 10));
        coordinator.HoverStart.Should().Be(new DateTime(2026, 5, 10));
    }

    [Fact]
    public void ProcessDateSelection_SingleRangeWithShift_UsesHoverStart()
    {
        var commands = new RecordingSelectionCommands();
        var coordinator = new CalendarSelectionCoordinator(
            () => CalendarSelectionMode.SingleRange,
            () => false,
            () => new DateTime(2026, 5, 1),
            _ => true,
            commands);

        coordinator.ProcessDateSelection(new DateTime(2026, 5, 5), shift: false, ctrl: false);
        coordinator.ProcessDateSelection(new DateTime(2026, 5, 15), shift: true, ctrl: false);

        commands.Ranges.Should().ContainSingle()
            .Which.Should().Be((new DateTime(2026, 5, 5), new DateTime(2026, 5, 15)));
    }

    [Fact]
    public void ResetHover_ClearsHoverStart()
    {
        var commands = new RecordingSelectionCommands();
        var coordinator = new CalendarSelectionCoordinator(
            () => CalendarSelectionMode.SingleDate,
            () => false,
            () => new DateTime(2026, 5, 1),
            _ => true,
            commands);

        coordinator.ProcessDateSelection(new DateTime(2026, 5, 10), shift: false, ctrl: false);
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
            () => new DateTime(2026, 5, 1),
            _ => true,
            commands);

        coordinator.ProcessTapRangeSelection(new DateTime(2026, 5, 10), ctrl: false);
        coordinator.ProcessTapRangeSelection(new DateTime(2026, 5, 20), ctrl: false);

        commands.Ranges.Should().ContainSingle()
            .Which.Should().Be((new DateTime(2026, 5, 10), new DateTime(2026, 5, 20)));
        coordinator.HoverStart.Should().BeNull();
    }

    [Fact]
    public void ProcessDateSelection_IgnoresInvalidDates()
    {
        var commands = new RecordingSelectionCommands();
        var coordinator = new CalendarSelectionCoordinator(
            () => CalendarSelectionMode.SingleDate,
            () => false,
            () => new DateTime(2026, 5, 1),
            _ => false,
            commands);

        coordinator.ProcessDateSelection(new DateTime(2026, 5, 10), shift: false, ctrl: false);

        commands.Singles.Should().BeEmpty();
        commands.Moves.Should().BeEmpty();
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
