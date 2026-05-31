// -----------------------------------------------------------------------
// <copyright file="CalendarKeyboardNavigationHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
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
        bool ctrl,
        bool shift) =>
        key switch
        {
            Key.Up => ResolveUp(displayContext, focusedDate, currentMonth),
            Key.Down => ResolveDown(displayContext, focusedDate, currentMonth),
            Key.Left => ResolveLeft(displayContext, focusedDate, currentMonth),
            Key.Right => ResolveRight(displayContext, focusedDate, currentMonth),
            Key.Home => ResolveHome(displayContext, focusedDate, currentMonth),
            Key.End => ResolveEnd(displayContext, focusedDate, currentMonth),
            Key.PageDown => ResolvePageDown(displayContext, focusedDate, ctrl, shift),
            Key.PageUp => ResolvePageUp(displayContext, focusedDate, ctrl, shift),
            _ => default
        };

    private static CalendarNavigationResult ResolveUp(DateContext displayContext, DateTime focusedDate, MonthContext currentMonth) =>
        displayContext switch
        {
            MonthContext => new(CalendarNavigationKind.SelectDate, focusedDate.AddDays(-DateTimeHelper.DaysPerWeek)),
            YearContext => new(CalendarNavigationKind.SelectMonthContext, MonthContext: currentMonth.Add(-YearGridColumns)),
            DecadeContext => new(CalendarNavigationKind.SelectMonthContext, MonthContext: currentMonth.AddYears(-YearGridColumns)),
            CenturyContext => new(CalendarNavigationKind.SelectMonthContext, MonthContext: currentMonth.AddDecades(-YearGridColumns)),
            _ => default
        };

    private static CalendarNavigationResult ResolveDown(DateContext displayContext, DateTime focusedDate, MonthContext currentMonth) =>
        displayContext switch
        {
            MonthContext => new(CalendarNavigationKind.SelectDate, focusedDate.AddDays(DateTimeHelper.DaysPerWeek)),
            YearContext => new(CalendarNavigationKind.SelectMonthContext, MonthContext: currentMonth.Add(YearGridColumns)),
            DecadeContext => new(CalendarNavigationKind.SelectMonthContext, MonthContext: currentMonth.AddYears(YearGridColumns)),
            CenturyContext => new(CalendarNavigationKind.SelectMonthContext, MonthContext: currentMonth.AddDecades(YearGridColumns)),
            _ => default
        };

    private static CalendarNavigationResult ResolveLeft(DateContext displayContext, DateTime focusedDate, MonthContext currentMonth) =>
        displayContext switch
        {
            MonthContext => new(CalendarNavigationKind.SelectDate, focusedDate.AddDays(-1)),
            YearContext => new(CalendarNavigationKind.SelectMonthContext, MonthContext: currentMonth.Add(-1)),
            DecadeContext => new(CalendarNavigationKind.SelectMonthContext, MonthContext: currentMonth.AddYears(-1)),
            CenturyContext => new(CalendarNavigationKind.SelectMonthContext, MonthContext: currentMonth.AddDecades(-1)),
            _ => default
        };

    private static CalendarNavigationResult ResolveRight(DateContext displayContext, DateTime focusedDate, MonthContext currentMonth) =>
        displayContext switch
        {
            MonthContext => new(CalendarNavigationKind.SelectDate, focusedDate.AddDays(1)),
            YearContext => new(CalendarNavigationKind.SelectMonthContext, MonthContext: currentMonth.Add(1)),
            DecadeContext => new(CalendarNavigationKind.SelectMonthContext, MonthContext: currentMonth.AddYears(1)),
            CenturyContext => new(CalendarNavigationKind.SelectMonthContext, MonthContext: currentMonth.AddDecades(1)),
            _ => default
        };

    private static CalendarNavigationResult ResolveHome(DateContext displayContext, DateTime focusedDate, MonthContext currentMonth) =>
        displayContext switch
        {
            MonthContext => new(CalendarNavigationKind.SelectDate, focusedDate.BeginningOfMonth()),
            YearContext => new(CalendarNavigationKind.SelectMonthContext, MonthContext: currentMonth.BeginningOfYear()),
            DecadeContext => new(CalendarNavigationKind.SelectMonthContext, MonthContext: currentMonth.BeginningOfDecade()),
            CenturyContext => new(CalendarNavigationKind.SelectMonthContext, MonthContext: currentMonth.BeginningOfCentury()),
            _ => default
        };

    private static CalendarNavigationResult ResolveEnd(DateContext displayContext, DateTime focusedDate, MonthContext currentMonth) =>
        displayContext switch
        {
            MonthContext => new(CalendarNavigationKind.SelectDate, focusedDate.EndOfMonth()),
            YearContext => new(CalendarNavigationKind.SelectMonthContext, MonthContext: currentMonth.EndOfYear()),
            DecadeContext => new(CalendarNavigationKind.SelectMonthContext, MonthContext: currentMonth.EndOfDecade()),
            CenturyContext => new(CalendarNavigationKind.SelectMonthContext, MonthContext: currentMonth.EndOfCentury()),
            _ => default
        };

    private static CalendarNavigationResult ResolvePageDown(
        DateContext displayContext,
        DateTime focusedDate,
        bool ctrl,
        bool shift)
        => !ctrl && !shift
            ? new(CalendarNavigationKind.Next)
            : displayContext switch
            {
                MonthContext when ctrl => new(CalendarNavigationKind.ShowYearView),
                MonthContext when shift => new(CalendarNavigationKind.SelectDate, focusedDate.AddMonths(1)),
                YearContext when ctrl => new(CalendarNavigationKind.ShowDecadeView),
                DecadeContext when ctrl => new(CalendarNavigationKind.ShowCenturyView),
                _ => default
            };

    private static CalendarNavigationResult ResolvePageUp(
        DateContext displayContext,
        DateTime focusedDate,
        bool ctrl,
        bool shift)
        => !ctrl && !shift
            ? new(CalendarNavigationKind.Previous)
            : displayContext switch
            {
                MonthContext when shift => new(CalendarNavigationKind.SelectDate, focusedDate.AddMonths(-1)),
                YearContext when ctrl => new(CalendarNavigationKind.ShowMonthView),
                DecadeContext when ctrl => new(CalendarNavigationKind.ShowYearView),
                CenturyContext when ctrl => new(CalendarNavigationKind.ShowDecadeView),
                _ => default
            };
}
