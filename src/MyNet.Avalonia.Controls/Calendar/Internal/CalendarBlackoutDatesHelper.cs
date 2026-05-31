// -----------------------------------------------------------------------
// <copyright file="CalendarBlackoutDatesHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using MyNet.Primitives;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls.Internals;
#pragma warning restore IDE0130 // Namespace does not match folder structure

internal static class CalendarBlackoutDatesHelper
{
    public static (DateTime Start, DateTime End) NormalizeRange(DateTime start, DateTime end) =>
        start.IsBefore(end)
            ? (start.DiscardTime(), end.DiscardTime())
            : (end.DiscardTime(), start.DiscardTime());
}
