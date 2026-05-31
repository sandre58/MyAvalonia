// -----------------------------------------------------------------------
// <copyright file="CalendarDisplayModeHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using MyNet.Avalonia.Controls.Primitives;
using MyNet.Primitives;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls.Internals;
#pragma warning restore IDE0130 // Namespace does not match folder structure

internal static class CalendarDisplayModeHelper
{
    public static MonthContext ToMonthContext(DateTime displayDate) => new(displayDate.Month, displayDate.Year);

    public static YearContext ToYearContext(DateTime displayDate) => new(displayDate.Year);

    public static DecadeContext ToDecadeContext(DateTime displayDate) => new(displayDate.Year.DecadeStart());

    public static CenturyContext ToCenturyContext(DateTime displayDate) =>
        new(displayDate.Year.Century().Start.GetValueOrDefault().Value);

    public static (bool Month, bool Year, bool Decade, bool Century) GetViewPseudoClasses(DateContext context) =>
        context switch
        {
            MonthContext => (true, false, false, false),
            YearContext => (false, true, false, false),
            DecadeContext => (false, false, true, false),
            CenturyContext => (false, false, false, true),
            _ => (false, false, false, false)
        };

    public static CalendarNavigationKind? GetHeaderDrillDownAction(DateContext context) =>
        context switch
        {
            YearContext => CalendarNavigationKind.ShowDecadeView,
            DecadeContext => CalendarNavigationKind.ShowCenturyView,
            _ => null
        };
}
