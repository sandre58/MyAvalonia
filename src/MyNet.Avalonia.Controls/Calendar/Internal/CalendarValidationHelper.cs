// -----------------------------------------------------------------------
// <copyright file="CalendarValidationHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using MyNet.Primitives;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls.Primitives.Internals;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public static class CalendarValidationHelper
{
    public static bool IsValidFirstDayOfWeek(DayOfWeek day) =>
        day is DayOfWeek.Sunday or DayOfWeek.Monday or DayOfWeek.Tuesday or DayOfWeek.Wednesday or DayOfWeek.Thursday or DayOfWeek.Friday or DayOfWeek.Saturday;

    public static bool IsValidSelectionMode(CalendarSelectionMode mode) =>
        mode is CalendarSelectionMode.SingleDate or CalendarSelectionMode.SingleRange or CalendarSelectionMode.MultipleRange or CalendarSelectionMode.None;

    public static bool IsValidSelection(DateTime date, DateTime rangeStart, DateTime rangeEnd, Func<DateTime, bool> isBlackout) =>
        !isBlackout(date) && date.IsBetween(rangeStart, rangeEnd);
}
