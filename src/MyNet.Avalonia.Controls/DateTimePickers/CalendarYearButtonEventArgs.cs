// -----------------------------------------------------------------------
// <copyright file="CalendarYearButtonEventArgs.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Interactivity;
using MyNet.Avalonia.Controls.DateTimePickers;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public class CalendarYearButtonEventArgs : RoutedEventArgs
{
    internal CalendarContext Context { get; }

    internal CalendarViewMode Mode { get; }

    internal CalendarYearButtonEventArgs(CalendarViewMode mode, CalendarContext context)
    {
        Context = context;
        Mode = mode;
    }
}
