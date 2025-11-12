// -----------------------------------------------------------------------
// <copyright file="CalendarDateChangedEventArgs.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Interactivity;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public class CalendarDateChangedEventArgs(DateTime? removedDate, DateTime? addedDate) : RoutedEventArgs
{
    public DateTime? RemovedDate { get; private set; } = removedDate;

    public DateTime? AddedDate { get; private set; } = addedDate;
}
