// -----------------------------------------------------------------------
// <copyright file="CalendarMonthGridHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using MyNet.Avalonia.Controls.Primitives;
using MyNet.Primitives.Temporal;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls.Internals;
#pragma warning restore IDE0130 // Namespace does not match folder structure

internal static class CalendarMonthGridHelper
{
    public static int GetLeadingDayCount(MonthContext monthContext, DayOfWeek firstDayOfWeek)
    {
        var firstDay = monthContext.ToDate();
        var dayOfWeek = firstDay.DayOfWeek;
        var offset = (dayOfWeek - firstDayOfWeek + DateTimeHelper.DaysPerWeek) % DateTimeHelper.DaysPerWeek;
        return offset == 0 ? DateTimeHelper.DaysPerWeek : offset;
    }

    public static int GetDayTitleColumnIndex(int columnIndex, DayOfWeek firstDayOfWeek) =>
        (columnIndex + (int)firstDayOfWeek) % DateTimeHelper.DaysPerWeek;

    public static IEnumerable<CalendarDayCellState> EnumerateDayCells(
        MonthContext monthContext,
        DayOfWeek firstDayOfWeek,
        int dayCellCount)
    {
        var daysBeforeCount = GetLeadingDayCount(monthContext, firstDayOfWeek);
        var date = monthContext.ToDate().AddDays(-daysBeforeCount);

        for (var i = 0; i < dayCellCount; i++)
        {
            var dateContext = new DayContext(date.Day, date.Month, date.Year);
            yield return new(dateContext, date, monthContext.Month != dateContext.Month);
            date = date.AddDays(1);
        }
    }
}
