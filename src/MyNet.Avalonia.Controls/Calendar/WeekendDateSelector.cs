// -----------------------------------------------------------------------
// <copyright file="WeekendDateSelector.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls.Primitives;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public class WeekendDateSelector : IDateSelector
{
    public static WeekendDateSelector Instance { get; } = new WeekendDateSelector();

    public bool Match(DateTime? date) => date?.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
}
