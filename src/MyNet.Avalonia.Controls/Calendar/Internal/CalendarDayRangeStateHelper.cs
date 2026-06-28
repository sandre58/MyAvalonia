// -----------------------------------------------------------------------
// <copyright file="CalendarDayRangeStateHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using MyNet.Avalonia.Controls.Primitives;
using MyNet.Primitives;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls.Internals;
#pragma warning restore IDE0130 // Namespace does not match folder structure

internal static class CalendarDayRangeStateHelper
{
    public static void ClearRangeState(CalendarDayButton cell)
    {
        cell.IsStartDate = false;
        cell.IsEndDate = false;
        ClearPreviewRangeState(cell);
        cell.IsInRange = false;
    }

    public static void ClearPreviewRangeState(CalendarDayButton cell)
    {
        cell.IsPreviewStartDate = false;
        cell.IsPreviewEndDate = false;
        cell.IsPreviewInRange = false;
    }

    public static void ApplyRangeSegmentToCell(CalendarDayButton cell, DateTime date, DateTime rangeStart, DateTime rangeEnd, bool isPreview)
    {
        rangeStart = rangeStart.DiscardTime();
        rangeEnd = rangeEnd.DiscardTime();
        date = date.DiscardTime();

        if (isPreview)
        {
            ApplyPreviewRangeToCell(cell, date, rangeStart, rangeEnd);
            return;
        }

        if (rangeStart.IsAfter(rangeEnd))
            (rangeStart, rangeEnd) = (rangeEnd, rangeStart);

        if (date.IsBefore(rangeStart) || date.IsAfter(rangeEnd))
            return;

        if (rangeStart == rangeEnd)
        {
            cell.IsStartDate = true;
            cell.IsEndDate = true;
            return;
        }

        if (date == rangeStart)
            cell.IsStartDate = true;
        else if (date == rangeEnd)
            cell.IsEndDate = true;
        else
            cell.IsInRange = true;
    }

    public static void ApplyPreviewRangeToCell(CalendarDayButton cell, DateTime date, DateTime anchor, DateTime previewEnd)
    {
        anchor = anchor.DiscardTime();
        previewEnd = previewEnd.DiscardTime();
        date = date.DiscardTime();

        if (anchor == previewEnd)
        {
            if (date == anchor)
            {
                cell.IsPreviewStartDate = true;
                cell.IsPreviewEndDate = true;
            }

            return;
        }

        var rangeStart = anchor.IsBefore(previewEnd) ? anchor : previewEnd;
        var rangeEnd = anchor.IsAfter(previewEnd) ? anchor : previewEnd;

        if (date.IsBefore(rangeStart) || date.IsAfter(rangeEnd))
            return;

        if (date == rangeStart)
            cell.IsPreviewStartDate = true;
        else if (date == rangeEnd)
            cell.IsPreviewEndDate = true;
        else
            cell.IsPreviewInRange = true;
    }

    public static IEnumerable<(DateTime Start, DateTime End)> EnumerateConsecutiveRanges(IReadOnlyList<DateTime> dates)
    {
        if (dates.Count == 0)
            yield break;

        var sorted = dates.Select(x => x.DiscardTime()).OrderBy(x => x).ToList();
        var start = sorted[0];
        var end = sorted[0];

        for (var i = 1; i < sorted.Count; i++)
        {
            var current = sorted[i];
            if (current == end.AddDays(1))
            {
                end = current;
            }
            else
            {
                yield return (start, end);
                start = current;
                end = current;
            }
        }

        yield return (start, end);
    }
}
