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

    public static TimeSpan GetPeriodEndTime(Period period) => period.End!.Value.Value.TimeOfDay;

    public static bool SpansOvernight(Period period)
    {
        var start = period.Start!.Value.Value;
        var end = period.End!.Value.Value;
        return end.Date > start.Date;
    }

    public static TimeRangeBuildResult BuildPeriod(
        TimeSpan start,
        TimeSpan end,
        DateTime referenceDate,
        bool allowOvernight,
        TimeRangeInvalidBehavior invalidBehavior)
    {
        referenceDate = referenceDate.DiscardTime();

        if (!allowOvernight && end < start)
        {
            if (invalidBehavior == TimeRangeInvalidBehavior.ReportError)
                return new(null, IsValid: false, ShouldReportError: true);

            (start, end) = (end, start);
        }

        var startDateTime = referenceDate.Add(start);
        var endDateTime = allowOvernight && end <= start
            ? referenceDate.AddDays(1).Add(end)
            : referenceDate.Add(end);

        return new(startDateTime.ToPeriod(endDateTime), IsValid: true, ShouldReportError: false);
    }

    public static TimeRangeBuildResult BuildPeriod(
        TimeSpan start,
        TimeSpan end,
        DateTime referenceDate) =>
        BuildPeriod(start, end, referenceDate, allowOvernight: false, TimeRangeInvalidBehavior.Swap);

    public static (TimeSpan Start, TimeSpan End) NormalizeTimes(TimeSpan start, TimeSpan end)
    {
        if (end < start)
            (start, end) = (end, start);

        return (start, end);
    }
}
