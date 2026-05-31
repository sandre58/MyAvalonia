// -----------------------------------------------------------------------
// <copyright file="CalendarNavigationResult.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using MyNet.Avalonia.Controls.Primitives;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls.Internals;
#pragma warning restore IDE0130 // Namespace does not match folder structure

internal enum CalendarNavigationKind
{
    None,
    SelectDate,
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
