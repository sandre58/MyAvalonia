// -----------------------------------------------------------------------
// <copyright file="ICalendarSelectionCommands.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace MyNet.Avalonia.Controls.Internals.Calendar;

internal interface ICalendarSelectionCommands
{
    void SetSelection(DateTime date);

    void SetSelection(DateTime start, DateTime end);

    void AddSelection(DateTime date);

    void AddSelection(DateTime start, DateTime end);

    void ToggleSelection(DateTime date);

    void ChangeSelection(DateTime start, DateTime end, bool isSelected);

    bool Contains(DateTime date);

    void MoveToDate(DateTime date);
}
