// -----------------------------------------------------------------------
// <copyright file="CalendarDisplayContextHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using MyNet.Avalonia.Controls.Primitives;
using MyNet.Primitives;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls.Primitives.Internals;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public static class CalendarDisplayContextHelper
{
    public static DateContext CoerceDisplayDateContext(DateContext value) => value switch
    {
        DecadeContext decadeContext => decadeContext.StartYear % 10 == 0 ? decadeContext : new(decadeContext.StartYear.DecadeStart()),
        CenturyContext centuryContext => centuryContext.StartYear % 100 == 0 ? centuryContext : new(centuryContext.StartYear.CenturyStart()),
        _ => value
    };

    public static DateTime GetFocusedDate(DateTime? lastSelectedDate, DateContext displayDateContext, DateTime today) => lastSelectedDate.HasValue && displayDateContext.IsSimilar(lastSelectedDate.Value)
        ? lastSelectedDate.Value
        : displayDateContext.IsSimilar(today) ? today : displayDateContext.ToDate();
}
