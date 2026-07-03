// -----------------------------------------------------------------------
// <copyright file="TimeRangeHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using MyNet.Primitives;
using MyNet.Primitives.Intervals;

namespace MyNet.Avalonia.Controls.DateTimePickers.Internal;

internal static class TimeRangeHelper
{
    public static TimeSpan GetPeriodStartTime(Period period) => period.StartTimeOfDay;

    public static TimeSpan GetPeriodEndTime(Period period) => period.EndTimeOfDay();

    public static bool SpansOvernight(Period period) => period.SpansMidnight;

    public static (TimeSpan Start, TimeSpan End) CoerceSameDayRange(
        TimeSpan start,
        TimeSpan end,
        TimeRangeBoundary editedBoundary) =>
        start.CoerceSameDayRange(end, editedBoundary == TimeRangeBoundary.End);

    public static TimeRangeBuildResult BuildPeriod(
        TimeSpan start,
        TimeSpan end,
        DateTime referenceDate,
        bool allowOvernight) =>
        new(start.ToPeriodFromTimeOfDay(end, referenceDate, allowOvernight), IsValid: true);

    public static TimeRangeBuildResult BuildPeriod(
        TimeSpan start,
        TimeSpan end,
        DateTime referenceDate) =>
        BuildPeriod(start, end, referenceDate, allowOvernight: false);
}
