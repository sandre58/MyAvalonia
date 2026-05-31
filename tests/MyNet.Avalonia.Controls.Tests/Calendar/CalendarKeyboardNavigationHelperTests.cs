// -----------------------------------------------------------------------
// <copyright file="CalendarKeyboardNavigationHelperTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Input;
using FluentAssertions;
using MyNet.Avalonia.Controls.Primitives;
using MyNet.Avalonia.Controls.Primitives.Internals;
using Xunit;

namespace MyNet.Avalonia.Controls.Tests.Calendar;

public class CalendarKeyboardNavigationHelperTests
{
    private static readonly MonthContext CurrentMonth = new(5, 2026);
    private static readonly DateTime FocusedDate = new(2026, 5, 15);

    [Fact]
    public void Resolve_ArrowUpInMonthView_MovesOneWeekBack()
    {
        var result = CalendarKeyboardNavigationHelper.Resolve(
            Key.Up,
            new MonthContext(5, 2026),
            FocusedDate,
            CurrentMonth,
            ctrl: false,
            shift: false);

        result.Kind.Should().Be(CalendarNavigationKind.SelectDate);
        result.Date.Should().Be(FocusedDate.AddDays(-7));
    }

    [Fact]
    public void Resolve_PageDownWithoutModifiers_NavigatesNext()
    {
        var result = CalendarKeyboardNavigationHelper.Resolve(
            Key.PageDown,
            new MonthContext(5, 2026),
            FocusedDate,
            CurrentMonth,
            ctrl: false,
            shift: false);

        result.Kind.Should().Be(CalendarNavigationKind.Next);
    }

    [Fact]
    public void Resolve_CtrlPageDownInMonthView_ShowsYearView()
    {
        var result = CalendarKeyboardNavigationHelper.Resolve(
            Key.PageDown,
            new MonthContext(5, 2026),
            FocusedDate,
            CurrentMonth,
            ctrl: true,
            shift: false);

        result.Kind.Should().Be(CalendarNavigationKind.ShowYearView);
    }

    [Fact]
    public void Resolve_HomeInYearView_SelectsBeginningOfYear()
    {
        var result = CalendarKeyboardNavigationHelper.Resolve(
            Key.Home,
            new YearContext(2026),
            FocusedDate,
            CurrentMonth,
            ctrl: false,
            shift: false);

        result.Kind.Should().Be(CalendarNavigationKind.SelectMonthContext);
        result.MonthContext.Should().Be(CurrentMonth.BeginningOfYear());
    }

    [Fact]
    public void Resolve_CtrlPageUpInDecadeView_ShowsYearView()
    {
        var result = CalendarKeyboardNavigationHelper.Resolve(
            Key.PageUp,
            new DecadeContext(2020),
            FocusedDate,
            CurrentMonth,
            ctrl: true,
            shift: false);

        result.Kind.Should().Be(CalendarNavigationKind.ShowYearView);
    }
}
