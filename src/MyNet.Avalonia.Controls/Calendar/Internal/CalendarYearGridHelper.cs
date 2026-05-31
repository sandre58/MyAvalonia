// -----------------------------------------------------------------------
// <copyright file="CalendarYearGridHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using MyNet.Primitives;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls.Primitives.Internals;
#pragma warning restore IDE0130 // Namespace does not match folder structure

internal static class CalendarYearGridHelper
{
    internal const int CellCount = 12;

    public static IReadOnlyList<CalendarYearCellState> BuildCells(DateContext displayContext, MonthContext currentMonth)
    {
        var selectedDate = currentMonth.ToDate();
        return displayContext switch
        {
            YearContext yearContext => BuildYearCells(yearContext, selectedDate),
            DecadeContext decadeContext => BuildDecadeCells(decadeContext, selectedDate),
            CenturyContext centuryContext => BuildCenturyCells(centuryContext, selectedDate),
            _ => [],
        };
    }

    private static CalendarYearCellState[] BuildYearCells(YearContext yearContext, DateTime selectedDate)
    {
        var cells = new CalendarYearCellState[CellCount];
        for (var i = 0; i < CellCount; i++)
        {
            var dateContext = new MonthContext(i + 1, yearContext.Year);
            cells[i] = new(
                i,
                dateContext,
                dateContext.ToDate(),
                yearContext.Year != dateContext.Year,
                dateContext.IsSimilar(selectedDate));
        }

        return cells;
    }

    private static CalendarYearCellState[] BuildDecadeCells(DecadeContext decadeContext, DateTime selectedDate)
    {
        var cells = new CalendarYearCellState[CellCount];
        for (var i = 0; i < CellCount; i++)
        {
            var dateContext = new YearContext(decadeContext.StartYear - 1 + i);
            cells[i] = new(
                i,
                dateContext,
                dateContext.ToDate(),
                decadeContext.StartYear != dateContext.Year.DecadeStart(),
                dateContext.IsSimilar(selectedDate));
        }

        return cells;
    }

    private static CalendarYearCellState[] BuildCenturyCells(CenturyContext centuryContext, DateTime selectedDate)
    {
        var cells = new CalendarYearCellState[CellCount];
        for (var i = 0; i < CellCount; i++)
        {
            var dateContext = new DecadeContext(centuryContext.StartYear - 10 + (i * 10));
            var centuryStart = dateContext.StartYear.Century().Start.GetValueOrDefault().Value;
            cells[i] = new(
                i,
                dateContext,
                dateContext.ToDate(),
                centuryContext.StartYear != centuryStart,
                dateContext.IsSimilar(selectedDate));
        }

        return cells;
    }
}
