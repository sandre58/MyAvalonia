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

    [Fact]
    public void SetRange_RaisesSingleCollectionChanged()
    {
        var calendar = new Controls.Calendar { SelectionMode = CalendarSelectionMode.SingleRange };
        var changeCount = 0;
        calendar.SelectedDates.CollectionChanged += (_, _) => changeCount++;

        calendar.SelectedDates.Set(new DateTime(2026, 6, 10), new DateTime(2026, 6, 20));

        changeCount.Should().Be(1);
        calendar.SelectedDates.Should().HaveCount(11);
    }

    [Fact]
    public void SetRange_ReplacingRange_RaisesSingleCollectionChanged()
    {
        var calendar = new Controls.Calendar { SelectionMode = CalendarSelectionMode.SingleRange };
        calendar.SelectedDates.Set(new DateTime(2026, 6, 1), new DateTime(2026, 6, 5));

        var changeCount = 0;
        calendar.SelectedDates.CollectionChanged += (_, _) => changeCount++;

        calendar.SelectedDates.Set(new DateTime(2026, 6, 10), new DateTime(2026, 6, 15));

        changeCount.Should().Be(1);
        calendar.SelectedDates.Should().HaveCount(6);
    }

    [Fact]
    public void SetRange_DoesNotChangeDisplayDateContext_WhenSameMonth()
    {
        var calendar = new Controls.Calendar
        {
            SelectionMode = CalendarSelectionMode.MultipleRange,
            DisplayDate = new DateTime(2026, 6, 15),
        };
        var contextBefore = calendar.DisplayDateContext;

        calendar.SelectedDates.Set(new DateTime(2026, 6, 10), new DateTime(2026, 6, 14));

        calendar.DisplayDateContext.Should().Be(contextBefore);
        calendar.SelectedDate.Should().Be(new DateTime(2026, 6, 10));
    }

    [Fact]
    public void Set_ReverseRangeOrder_IncludesBothEnds()
    {
        var calendar = new Controls.Calendar { SelectionMode = CalendarSelectionMode.SingleRange };

        calendar.SelectedDates.Set(new DateTime(2026, 6, 14), new DateTime(2026, 6, 9));

        calendar.SelectedDates.Should().Contain(new DateTime(2026, 6, 9));
        calendar.SelectedDates.Should().Contain(new DateTime(2026, 6, 14));
        calendar.SelectedDates.Should().HaveCount(6);
    }

    [Fact]
    public void Set_ReverseRangeOrder_WithOverlappingPriorSelection_IncludesBothEnds()
    {
        var calendar = new Controls.Calendar { SelectionMode = CalendarSelectionMode.SingleRange };

        calendar.SelectedDates.Set(new DateTime(2026, 6, 8), new DateTime(2026, 6, 11));
        calendar.SelectedDates.Set(new DateTime(2026, 6, 14), new DateTime(2026, 6, 9));

        calendar.SelectedDates.Should().Contain(new DateTime(2026, 6, 9));
        calendar.SelectedDates.Should().Contain(new DateTime(2026, 6, 14));
        calendar.SelectedDates.Should().HaveCount(6);
    }
}
