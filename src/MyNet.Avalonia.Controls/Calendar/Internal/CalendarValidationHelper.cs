// -----------------------------------------------------------------------
// <copyright file="CalendarValidationHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using MyNet.Primitives;

namespace MyNet.Avalonia.Controls.Internals.Calendar;

internal static class CalendarValidationHelper
{
    public static bool IsValidFirstDayOfWeek(DayOfWeek day) =>
        day is DayOfWeek.Sunday or DayOfWeek.Monday or DayOfWeek.Tuesday or DayOfWeek.Wednesday or DayOfWeek.Thursday or DayOfWeek.Friday or DayOfWeek.Saturday;

    public static bool IsValidSelectionMode(CalendarSelectionMode mode) =>
        mode is CalendarSelectionMode.SingleDate or CalendarSelectionMode.SingleRange or CalendarSelectionMode.MultipleRange or CalendarSelectionMode.None;

    public static bool IsValidSelection(DateTime date, DateTime rangeStart, DateTime rangeEnd, Func<DateTime, bool> isBlackout) =>
        !isBlackout(date) && date.IsBetween(rangeStart, rangeEnd);
}
