// -----------------------------------------------------------------------
// <copyright file="CalendarViewMode.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

internal enum CalendarViewMode
{
    // Show days in current month.
    Month,

    // Show Months in current year.
    Year,

    // The button represents 1 years.
    Decade,

    // The button represents 10 years.
    Century,
}
