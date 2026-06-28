// -----------------------------------------------------------------------
// <copyright file="SelectedDatesCollectionTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using FluentAssertions;
using Xunit;

namespace MyNet.Avalonia.Controls.Tests.Calendar;

public class SelectedDatesCollectionTests
{
    [Fact]
    public void Remove_MiddleOfRange_RemovesOnlyThatDate()
    {
        var calendar = new Controls.Calendar { SelectionMode = CalendarSelectionMode.MultipleRange };
        calendar.SelectedDates.Set(new DateTime(2026, 6, 10), new DateTime(2026, 6, 14));

        calendar.SelectedDates.Remove(new DateTime(2026, 6, 12));

        calendar.SelectedDates.Should().BeEquivalentTo([
            new DateTime(2026, 6, 10),
            new DateTime(2026, 6, 11),
            new DateTime(2026, 6, 13),
            new DateTime(2026, 6, 14),
        ]);
    }

    [Fact]
    public void SelectedDateChange_MultipleRange_DoesNotReplaceExistingSelection()
    {
        var calendar = new Controls.Calendar { SelectionMode = CalendarSelectionMode.MultipleRange };
        calendar.SelectedDates.Set(new DateTime(2026, 6, 10), new DateTime(2026, 6, 14));

        calendar.SelectedDate = new DateTime(2026, 6, 12);

        calendar.SelectedDates.Should().BeEquivalentTo([
            new DateTime(2026, 6, 10),
            new DateTime(2026, 6, 11),
            new DateTime(2026, 6, 12),
            new DateTime(2026, 6, 13),
            new DateTime(2026, 6, 14),
        ]);
    }

    [Fact]
    public void Set_SameDayRange_SelectsSingleDate()
    {
        var calendar = new Controls.Calendar { SelectionMode = CalendarSelectionMode.SingleRange };
        var date = new DateTime(2026, 6, 15);

        var act = () => calendar.SelectedDates.Set(date, date);

        act.Should().NotThrow();
        calendar.SelectedDates.Should().ContainSingle().Which.Should().Be(date);
    }
}
