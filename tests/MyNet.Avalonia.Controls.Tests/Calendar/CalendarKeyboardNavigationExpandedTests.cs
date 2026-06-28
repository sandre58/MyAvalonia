// -----------------------------------------------------------------------
// <copyright file="CalendarKeyboardNavigationExpandedTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using Avalonia.Input;
using FluentAssertions;
using MyNet.Avalonia.Controls.Internals;
using MyNet.Avalonia.Controls.Primitives;
using MyNet.Primitives;
using Xunit;

namespace MyNet.Avalonia.Controls.Tests.Calendar;

public class CalendarKeyboardNavigationExpandedTests
{
    private static readonly MonthContext CurrentMonth = new(5, 2026);
    private static readonly DateTime FocusedDate = new(2026, 5, 15);

    [Theory]
    [InlineData(Key.Left)]
    [InlineData(Key.Right)]
    [InlineData(Key.Up)]
    [InlineData(Key.Down)]
    public void Resolve_ArrowKeysInMonthView_MoveFocus(Key key)
    {
        var result = CalendarKeyboardNavigationHelper.Resolve(
            key,
            new MonthContext(5, 2026),
            FocusedDate,
            CurrentMonth,
            CalendarSelectionMode.SingleDate,
            allowTapRangeSelection: false,
            ctrl: false,
            shift: false);

        result.Kind.Should().Be(CalendarNavigationKind.MoveFocus);
        result.Date.Should().NotBeNull();
    }

    [Fact]
    public void Resolve_EndInMonthView_SelectsEndOfMonth()
    {
        var result = CalendarKeyboardNavigationHelper.Resolve(
            Key.End,
            new MonthContext(5, 2026),
            FocusedDate,
            CurrentMonth,
            CalendarSelectionMode.SingleDate,
            allowTapRangeSelection: false,
            ctrl: false,
            shift: false);

        result.Kind.Should().Be(CalendarNavigationKind.MoveFocus);
        result.Date.Should().Be(FocusedDate.EndOfMonth());
    }

    [Fact]
    public void Resolve_ShiftPageDownInMonthView_AddsOneMonth()
    {
        var result = CalendarKeyboardNavigationHelper.Resolve(
            Key.PageDown,
            new MonthContext(5, 2026),
            FocusedDate,
            CurrentMonth,
            CalendarSelectionMode.SingleDate,
            allowTapRangeSelection: false,
            ctrl: false,
            shift: true);

        result.Kind.Should().Be(CalendarNavigationKind.MoveFocus);
        result.Date.Should().Be(FocusedDate.AddMonths(1));
    }

    [Fact]
    public void Resolve_LeftInYearView_SelectsPreviousMonthContext()
    {
        var result = CalendarKeyboardNavigationHelper.Resolve(
            Key.Left,
            new YearContext(2026),
            FocusedDate,
            CurrentMonth,
            CalendarSelectionMode.SingleDate,
            allowTapRangeSelection: false,
            ctrl: false,
            shift: false);

        result.MonthContext.Should().Be(CurrentMonth.Add(-1));
    }

    [Fact]
    public void Resolve_CenturyPageUp_ShowsDecadeView()
    {
        var result = CalendarKeyboardNavigationHelper.Resolve(
            Key.PageUp,
            new CenturyContext(2000),
            FocusedDate,
            CurrentMonth,
            CalendarSelectionMode.SingleDate,
            allowTapRangeSelection: false,
            ctrl: true,
            shift: false);

        result.Kind.Should().Be(CalendarNavigationKind.ShowDecadeView);
    }

    [Fact]
    public void Resolve_Enter_IsHandledByCalendarNotHelper()
    {
        var result = CalendarKeyboardNavigationHelper.Resolve(
            Key.Enter,
            new MonthContext(5, 2026),
            FocusedDate,
            CurrentMonth,
            CalendarSelectionMode.MultipleRange,
            allowTapRangeSelection: false,
            ctrl: false,
            shift: false);

        result.Kind.Should().Be(CalendarNavigationKind.None);
    }
}
