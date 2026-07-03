// -----------------------------------------------------------------------
// <copyright file="CalendarNavigationResult.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using MyNet.Avalonia.Controls.Primitives;

namespace MyNet.Avalonia.Controls.Internals.Calendar;

internal enum CalendarNavigationKind
{
    None,
    MoveFocus,
    SelectMonthContext,
    Next,
    Previous,
    ShowMonthView,
    ShowYearView,
    ShowDecadeView,
    ShowCenturyView
}

internal readonly record struct CalendarNavigationResult(
    CalendarNavigationKind Kind,
    DateTime? Date = null,
    MonthContext? MonthContext = null);
