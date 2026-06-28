// -----------------------------------------------------------------------
// <copyright file="CalendarKeyboardNavigationHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using Avalonia.Input;
using MyNet.Avalonia.Controls.Primitives;
using MyNet.Primitives;
using MyNet.Primitives.Temporal;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls.Internals;
#pragma warning restore IDE0130 // Namespace does not match folder structure

internal static class CalendarKeyboardNavigationHelper
{
    internal const int YearGridColumns = 3;

    public static CalendarNavigationResult Resolve(
        Key key,
        DateContext displayContext,
        DateTime focusedDate,
        MonthContext currentMonth,
        CalendarSelectionMode selectionMode,
        bool allowTapRangeSelection,
        bool ctrl,
        bool shift) =>
        key switch
        {
            Key.Up => ResolveUp(displayContext, focusedDate, currentMonth, selectionMode, allowTapRangeSelection, ctrl, shift),
            Key.Down => ResolveDown(displayContext, focusedDate, currentMonth, selectionMode, allowTapRangeSelection, ctrl, shift),
            Key.Left => ResolveLeft(displayContext, focusedDate, currentMonth, selectionMode, allowTapRangeSelection, ctrl, shift),
            Key.Right => ResolveRight(displayContext, focusedDate, currentMonth, selectionMode, allowTapRangeSelection, ctrl, shift),
            Key.Home => ResolveHome(displayContext, focusedDate, currentMonth, selectionMode, allowTapRangeSelection, ctrl, shift),
            Key.End => ResolveEnd(displayContext, focusedDate, currentMonth, selectionMode, allowTapRangeSelection, ctrl, shift),
            Key.PageDown => ResolvePageDown(displayContext, focusedDate, selectionMode, allowTapRangeSelection, ctrl, shift),
            Key.PageUp => ResolvePageUp(displayContext, focusedDate, selectionMode, allowTapRangeSelection, ctrl, shift),
            _ => default
        };

    private static CalendarNavigationKind GetDateNavigationKind(
        CalendarSelectionMode selectionMode,
        bool allowTapRangeSelection,
        bool ctrl,
        bool shift) =>
        CalendarNavigationKind.MoveFocus;

    private static CalendarNavigationResult ResolveUp(
        DateContext displayContext,
        DateTime focusedDate,
        MonthContext currentMonth,
        CalendarSelectionMode selectionMode,
        bool allowTapRangeSelection,
        bool ctrl,
        bool shift) =>
        displayContext switch
        {
            MonthContext => new(GetDateNavigationKind(selectionMode, allowTapRangeSelection, ctrl, shift), focusedDate.AddDays(-DateTimeHelper.DaysPerWeek)),
            YearContext => new(CalendarNavigationKind.SelectMonthContext, MonthContext: currentMonth.Add(-YearGridColumns)),
            DecadeContext => new(CalendarNavigationKind.SelectMonthContext, MonthContext: currentMonth.AddYears(-YearGridColumns)),
            CenturyContext => new(CalendarNavigationKind.SelectMonthContext, MonthContext: currentMonth.AddDecades(-YearGridColumns)),
            _ => default
        };

    private static CalendarNavigationResult ResolveDown(
        DateContext displayContext,
        DateTime focusedDate,
        MonthContext currentMonth,
        CalendarSelectionMode selectionMode,
        bool allowTapRangeSelection,
        bool ctrl,
        bool shift) =>
        displayContext switch
        {
            MonthContext => new(GetDateNavigationKind(selectionMode, allowTapRangeSelection, ctrl, shift), focusedDate.AddDays(DateTimeHelper.DaysPerWeek)),
            YearContext => new(CalendarNavigationKind.SelectMonthContext, MonthContext: currentMonth.Add(YearGridColumns)),
            DecadeContext => new(CalendarNavigationKind.SelectMonthContext, MonthContext: currentMonth.AddYears(YearGridColumns)),
            CenturyContext => new(CalendarNavigationKind.SelectMonthContext, MonthContext: currentMonth.AddDecades(YearGridColumns)),
            _ => default
        };

    private static CalendarNavigationResult ResolveLeft(
        DateContext displayContext,
        DateTime focusedDate,
        MonthContext currentMonth,
        CalendarSelectionMode selectionMode,
        bool allowTapRangeSelection,
        bool ctrl,
        bool shift) =>
        displayContext switch
        {
            MonthContext => new(GetDateNavigationKind(selectionMode, allowTapRangeSelection, ctrl, shift), focusedDate.AddDays(-1)),
            YearContext => new(CalendarNavigationKind.SelectMonthContext, MonthContext: currentMonth.Add(-1)),
            DecadeContext => new(CalendarNavigationKind.SelectMonthContext, MonthContext: currentMonth.AddYears(-1)),
            CenturyContext => new(CalendarNavigationKind.SelectMonthContext, MonthContext: currentMonth.AddDecades(-1)),
            _ => default
        };

    private static CalendarNavigationResult ResolveRight(
        DateContext displayContext,
        DateTime focusedDate,
        MonthContext currentMonth,
        CalendarSelectionMode selectionMode,
        bool allowTapRangeSelection,
        bool ctrl,
        bool shift) =>
        displayContext switch
        {
            MonthContext => new(GetDateNavigationKind(selectionMode, allowTapRangeSelection, ctrl, shift), focusedDate.AddDays(1)),
            YearContext => new(CalendarNavigationKind.SelectMonthContext, MonthContext: currentMonth.Add(1)),
            DecadeContext => new(CalendarNavigationKind.SelectMonthContext, MonthContext: currentMonth.AddYears(1)),
            CenturyContext => new(CalendarNavigationKind.SelectMonthContext, MonthContext: currentMonth.AddDecades(1)),
            _ => default
        };

    private static CalendarNavigationResult ResolveHome(
        DateContext displayContext,
        DateTime focusedDate,
        MonthContext currentMonth,
        CalendarSelectionMode selectionMode,
        bool allowTapRangeSelection,
        bool ctrl,
        bool shift) =>
        displayContext switch
        {
            MonthContext => new(GetDateNavigationKind(selectionMode, allowTapRangeSelection, ctrl, shift), focusedDate.BeginningOfMonth()),
            YearContext => new(CalendarNavigationKind.SelectMonthContext, MonthContext: currentMonth.BeginningOfYear()),
            DecadeContext => new(CalendarNavigationKind.SelectMonthContext, MonthContext: currentMonth.BeginningOfDecade()),
            CenturyContext => new(CalendarNavigationKind.SelectMonthContext, MonthContext: currentMonth.BeginningOfCentury()),
            _ => default
        };

    private static CalendarNavigationResult ResolveEnd(
        DateContext displayContext,
        DateTime focusedDate,
        MonthContext currentMonth,
        CalendarSelectionMode selectionMode,
        bool allowTapRangeSelection,
        bool ctrl,
        bool shift) =>
        displayContext switch
        {
            MonthContext => new(GetDateNavigationKind(selectionMode, allowTapRangeSelection, ctrl, shift), focusedDate.EndOfMonth()),
            YearContext => new(CalendarNavigationKind.SelectMonthContext, MonthContext: currentMonth.EndOfYear()),
            DecadeContext => new(CalendarNavigationKind.SelectMonthContext, MonthContext: currentMonth.EndOfDecade()),
            CenturyContext => new(CalendarNavigationKind.SelectMonthContext, MonthContext: currentMonth.EndOfCentury()),
            _ => default
        };

    private static CalendarNavigationResult ResolvePageDown(
        DateContext displayContext,
        DateTime focusedDate,
        CalendarSelectionMode selectionMode,
        bool allowTapRangeSelection,
        bool ctrl,
        bool shift)
        => !ctrl && !shift
            ? new(CalendarNavigationKind.Next)
            : displayContext switch
            {
                MonthContext when ctrl => new(CalendarNavigationKind.ShowYearView),
                MonthContext when shift => new(
                    GetDateNavigationKind(selectionMode, allowTapRangeSelection, ctrl, shift),
                    focusedDate.AddMonths(1)),
                YearContext when ctrl => new(CalendarNavigationKind.ShowDecadeView),
                DecadeContext when ctrl => new(CalendarNavigationKind.ShowCenturyView),
                _ => default
            };

    private static CalendarNavigationResult ResolvePageUp(
        DateContext displayContext,
        DateTime focusedDate,
        CalendarSelectionMode selectionMode,
        bool allowTapRangeSelection,
        bool ctrl,
        bool shift)
        => !ctrl && !shift
            ? new(CalendarNavigationKind.Previous)
            : displayContext switch
            {
                MonthContext when shift => new(
                    GetDateNavigationKind(selectionMode, allowTapRangeSelection, ctrl, shift),
                    focusedDate.AddMonths(-1)),
                YearContext when ctrl => new(CalendarNavigationKind.ShowMonthView),
                DecadeContext when ctrl => new(CalendarNavigationKind.ShowYearView),
                CenturyContext when ctrl => new(CalendarNavigationKind.ShowDecadeView),
                _ => default
            };
}
