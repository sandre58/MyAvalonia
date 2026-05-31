// -----------------------------------------------------------------------
// <copyright file="ICalendarSelectionCommands.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls.Primitives.Internals;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public interface ICalendarSelectionCommands
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
