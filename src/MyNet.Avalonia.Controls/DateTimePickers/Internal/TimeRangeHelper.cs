// -----------------------------------------------------------------------
// <copyright file="TimeRangeHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using MyNet.Primitives;
using MyNet.Primitives.Intervals;
using MyNet.Primitives.Temporal;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls.Internals;
#pragma warning restore IDE0130 // Namespace does not match folder structure

internal static class TimeRangeHelper
{
    public static TimeSpan GetPeriodStartTime(Period period) => period.Start!.Value.Value.TimeOfDay;

    public static TimeSpan GetPeriodEndTime(Period period)
    {
        var start = GetPeriodStartTime(period);
        var end = period.End!.Value.Value.TimeOfDay;

        if (end > start && end - start < TimeSpan.FromMinutes(1))
            return start;

        return end;
    }

    public static bool SpansOvernight(Period period)
    {
        var start = period.Start!.Value.Value;
        var end = period.End!.Value.Value;
        return end.Date > start.Date;
    }

    /// <summary>
    /// When same-day ranges are required, aligns the non-edited boundary to the edited one if <paramref name="end"/> &lt; <paramref name="start"/>.
    /// </summary>
    public static (TimeSpan Start, TimeSpan End) CoerceSameDayRange(
        TimeSpan start,
        TimeSpan end,
        TimeRangeBoundary editedBoundary)
    {
        if (end >= start)
            return (start, end);

        return editedBoundary == TimeRangeBoundary.End
            ? (end, end)
            : (start, start);
    }

    public static TimeRangeBuildResult BuildPeriod(
        TimeSpan start,
        TimeSpan end,
        DateTime referenceDate,
        bool allowOvernight)
    {
        referenceDate = referenceDate.DiscardTime();

        var startDateTime = referenceDate.Add(start);
        var endDateTime = allowOvernight && end <= start
            ? referenceDate.AddDays(1).Add(end)
            : referenceDate.Add(end);

        if (!allowOvernight && end == start)
            endDateTime = startDateTime.AddTicks(1);

        return new(startDateTime.ToPeriod(endDateTime), IsValid: true);
    }

    public static TimeRangeBuildResult BuildPeriod(
        TimeSpan start,
        TimeSpan end,
        DateTime referenceDate) =>
        BuildPeriod(start, end, referenceDate, allowOvernight: false);
}
