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

namespace MyNet.Avalonia.Controls.Internals.Calendar;

internal static class CalendarDayRangeStateHelper
{
    public static void ClearCommittedRangeState(CalendarDayButton cell)
    {
        cell.IsStartDate = false;
        cell.IsEndDate = false;
        cell.IsInRange = false;
    }

    public static void ClearRangeState(CalendarDayButton cell)
    {
        ClearCommittedRangeState(cell);
        ClearPreviewRangeState(cell);
    }

    public static void ClearPreviewRangeState(CalendarDayButton cell) =>
        cell.SetPreviewRangeState(isPreviewStart: false, isPreviewEnd: false, isPreviewInRange: false);

    public static void ApplyRangeSegmentToCell(CalendarDayButton cell, DateTime date, DateTime rangeStart, DateTime rangeEnd, bool isPreview)
    {
        rangeStart = rangeStart.DiscardTime();
        rangeEnd = rangeEnd.DiscardTime();
        date = date.DiscardTime();

        if (isPreview)
        {
            SetPreviewRangeToCell(cell, date, rangeStart, rangeEnd);
            return;
        }

        if (rangeStart.IsAfter(rangeEnd))
            (rangeStart, rangeEnd) = (rangeEnd, rangeStart);

        if (date.IsBefore(rangeStart) || date.IsAfter(rangeEnd))
            return;

        ClearCommittedRangeState(cell);

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

    public static void SetPreviewRangeToCell(CalendarDayButton cell, DateTime date, DateTime anchor, DateTime previewEnd)
    {
        anchor = anchor.DiscardTime();
        previewEnd = previewEnd.DiscardTime();
        date = date.DiscardTime();

        var isPreviewStart = false;
        var isPreviewEnd = false;
        var isPreviewInRange = false;

        if (anchor == previewEnd)
        {
            if (date == anchor)
            {
                isPreviewStart = true;
                isPreviewEnd = true;
            }
        }
        else
        {
            var rangeStart = anchor.IsBefore(previewEnd) ? anchor : previewEnd;
            var rangeEnd = anchor.IsAfter(previewEnd) ? anchor : previewEnd;

            if (!date.IsBefore(rangeStart) && !date.IsAfter(rangeEnd))
            {
                if (date == rangeStart)
                    isPreviewStart = true;
                else if (date == rangeEnd)
                    isPreviewEnd = true;
                else
                    isPreviewInRange = true;
            }
        }

        cell.SetPreviewRangeState(isPreviewStart, isPreviewEnd, isPreviewInRange);
    }

    public static void ReconcileSingleDayCapDuringPreview(CalendarDayButton cell, DateTime anchor, DateTime previewEnd)
    {
        anchor = anchor.DiscardTime();
        previewEnd = previewEnd.DiscardTime();

        if (anchor == previewEnd)
            return;

        if (!cell.IsStartDate || !cell.IsEndDate)
            return;

        var rangeStart = anchor.IsBefore(previewEnd) ? anchor : previewEnd;
        var rangeEnd = anchor.IsAfter(previewEnd) ? anchor : previewEnd;

        if (anchor == rangeStart)
            cell.IsEndDate = false;
        else if (anchor == rangeEnd)
            cell.IsStartDate = false;
    }

    public static void ApplyPreviewRoleTransition(CalendarDayButton cell, DateTime date, DateTime anchor, DateTime previewEnd) =>
        SetPreviewRangeToCell(cell, date, anchor, previewEnd);

    public static bool CellMatchesCommittedInterval(CalendarDayButton cell, DateTime date, DateTime rangeStart, DateTime rangeEnd)
    {
        rangeStart = rangeStart.DiscardTime();
        rangeEnd = rangeEnd.DiscardTime();
        date = date.DiscardTime();

        if (rangeStart.IsAfter(rangeEnd))
            (rangeStart, rangeEnd) = (rangeEnd, rangeStart);

        if (rangeStart == rangeEnd)
        {
            if (date != rangeStart)
                return !cell.IsStartDate && !cell.IsEndDate && !cell.IsInRange;

            return cell.IsStartDate && cell.IsEndDate && !cell.IsInRange;
        }

        if (date.IsBefore(rangeStart) || date.IsAfter(rangeEnd))
            return !cell.IsStartDate && !cell.IsEndDate && !cell.IsInRange;

        if (date == rangeStart)
            return cell.IsStartDate && !cell.IsEndDate && !cell.IsInRange;

        if (date == rangeEnd)
            return cell.IsEndDate && !cell.IsStartDate && !cell.IsInRange;

        return cell.IsInRange && !cell.IsStartDate && !cell.IsEndDate;
    }

    public static bool CellMatchesPreviewInterval(CalendarDayButton cell, DateTime date, DateTime anchor, DateTime previewEnd)
    {
        anchor = anchor.DiscardTime();
        previewEnd = previewEnd.DiscardTime();
        date = date.DiscardTime();

        if (anchor == previewEnd)
        {
            if (date != anchor)
                return !cell.IsPreviewStartDate && !cell.IsPreviewEndDate && !cell.IsPreviewInRange;

            return cell.IsPreviewStartDate && cell.IsPreviewEndDate && !cell.IsPreviewInRange;
        }

        var rangeStart = anchor.IsBefore(previewEnd) ? anchor : previewEnd;
        var rangeEnd = anchor.IsAfter(previewEnd) ? anchor : previewEnd;

        if (date.IsBefore(rangeStart) || date.IsAfter(rangeEnd))
            return !cell.IsPreviewStartDate && !cell.IsPreviewEndDate && !cell.IsPreviewInRange;

        if (date == rangeStart)
            return cell.IsPreviewStartDate && !cell.IsPreviewEndDate && !cell.IsPreviewInRange;

        if (date == rangeEnd)
            return cell.IsPreviewEndDate && !cell.IsPreviewStartDate && !cell.IsPreviewInRange;

        return cell.IsPreviewInRange && !cell.IsPreviewStartDate && !cell.IsPreviewEndDate;
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
