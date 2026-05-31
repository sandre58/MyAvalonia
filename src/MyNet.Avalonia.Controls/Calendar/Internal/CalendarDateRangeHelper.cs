// -----------------------------------------------------------------------
// <copyright file="CalendarDateRangeHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using MyNet.Primitives;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls.Primitives.Internals;
#pragma warning restore IDE0130 // Namespace does not match folder structure

internal static class CalendarDateRangeHelper
{
    public static DateTime GetRangeStart(DateTime? displayDateStart) => displayDateStart ?? DateTime.MinValue;

    public static DateTime GetRangeEnd(DateTime? displayDateEnd) => displayDateEnd ?? DateTime.MaxValue;

    public static DateTime ClampToRange(DateTime date, DateTime rangeStart, DateTime rangeEnd) => date.IsBefore(rangeStart) ? rangeStart : date.IsAfter(rangeEnd) ? rangeEnd : date;

    public static DateTime? GetSelectedMin(IReadOnlyList<DateTime> selectedDates)
    {
        if (selectedDates.Count == 0)
            return null;

        var selectedMin = selectedDates[0];
        for (var i = 1; i < selectedDates.Count; i++)
        {
            if (selectedDates[i].IsBefore(selectedMin))
                selectedMin = selectedDates[i];
        }

        return selectedMin;
    }

    public static DateTime? GetSelectedMax(IReadOnlyList<DateTime> selectedDates)
    {
        if (selectedDates.Count == 0)
            return null;

        var selectedMax = selectedDates[0];
        for (var i = 1; i < selectedDates.Count; i++)
        {
            if (selectedDates[i].IsAfter(selectedMax))
                selectedMax = selectedDates[i];
        }

        return selectedMax;
    }

    public static DisplayDateRangeAdjustment? ResolveDisplayDateStartChange(
        DateTime newStart,
        DateTime rangeEnd,
        DateTime displayDate,
        DateTime? selectedMin,
        DateTime? currentDisplayDateStart)
        => selectedMin.HasValue && selectedMin.Value.IsBefore(newStart)
            ? new() { DisplayDateStart = selectedMin.Value }
            : newStart.IsAfter(rangeEnd)
                ? new() { DisplayDateEnd = currentDisplayDateStart ?? newStart }
                : newStart.IsAfter(displayDate) ? new DisplayDateRangeAdjustment { DisplayDate = newStart, RequiresRefresh = true } : new DisplayDateRangeAdjustment { RequiresRefresh = true };

    public static DisplayDateRangeAdjustment? ResolveDisplayDateEndChange(
        DateTime newEnd,
        DateTime rangeStart,
        DateTime displayDate,
        DateTime? selectedMax)
        => selectedMax.HasValue && selectedMax.Value.IsAfter(newEnd)
            ? new() { DisplayDateEnd = selectedMax.Value }
            : newEnd.IsBefore(rangeStart)
                ? new() { DisplayDateEnd = rangeStart }
                : newEnd.IsBefore(displayDate) ? new DisplayDateRangeAdjustment { DisplayDate = newEnd, RequiresRefresh = true } : new DisplayDateRangeAdjustment { RequiresRefresh = true };
}

internal readonly record struct DisplayDateRangeAdjustment
{
    public DateTime? DisplayDateStart { get; init; }

    public DateTime? DisplayDateEnd { get; init; }

    public DateTime? DisplayDate { get; init; }

    public bool RequiresRefresh { get; init; }
}
