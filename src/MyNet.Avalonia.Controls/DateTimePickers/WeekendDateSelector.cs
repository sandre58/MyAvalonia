// -----------------------------------------------------------------------
// <copyright file="WeekendDateSelector.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace MyNet.Avalonia.Controls.DateTimePickers;

public class WeekendDateSelector : IDateSelector
{
    public static WeekendDateSelector Instance { get; } = new WeekendDateSelector();

    public bool Match(DateTime? date) => date is not null && (date.Value.DayOfWeek == DayOfWeek.Saturday || date.Value.DayOfWeek == DayOfWeek.Sunday);
}
