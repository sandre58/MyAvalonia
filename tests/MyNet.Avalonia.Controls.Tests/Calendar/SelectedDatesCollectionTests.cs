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
    public void Set_SameDayRange_SelectsSingleDate()
    {
        var calendar = new Controls.Calendar { SelectionMode = CalendarSelectionMode.SingleRange };
        var date = new DateTime(2026, 6, 15);

        var act = () => calendar.SelectedDates.Set(date, date);

        act.Should().NotThrow();
        calendar.SelectedDates.Should().ContainSingle().Which.Should().Be(date);
    }
}
