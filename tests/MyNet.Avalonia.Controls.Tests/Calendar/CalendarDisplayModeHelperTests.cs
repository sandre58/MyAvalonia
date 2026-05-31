// -----------------------------------------------------------------------
// <copyright file="CalendarDisplayModeHelperTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using FluentAssertions;
using MyNet.Avalonia.Controls.Primitives;
using MyNet.Avalonia.Controls.Primitives.Internals;
using Xunit;

namespace MyNet.Avalonia.Controls.Tests.Calendar;

public class CalendarDisplayModeHelperTests
{
    [Fact]
    public void GetViewPseudoClasses_ForMonthContext_EnablesMonthOnly()
    {
        var result = CalendarDisplayModeHelper.GetViewPseudoClasses(new MonthContext(5, 2026));

        result.Should().Be((true, false, false, false));
    }

    [Fact]
    public void ToDecadeContext_AlignsToDecadeStart()
    {
        CalendarDisplayModeHelper.ToDecadeContext(new DateTime(2026, 5, 15))
            .StartYear.Should().Be(2020);
    }

    [Fact]
    public void GetHeaderDrillDownAction_FromYearContext_ShowsDecadeView()
    {
        CalendarDisplayModeHelper.GetHeaderDrillDownAction(new YearContext(2026))
            .Should().Be(CalendarNavigationKind.ShowDecadeView);
    }

    [Fact]
    public void GetHeaderDrillDownAction_FromMonthContext_ReturnsNull()
    {
        CalendarDisplayModeHelper.GetHeaderDrillDownAction(new MonthContext(5, 2026))
            .Should().BeNull();
    }
}
