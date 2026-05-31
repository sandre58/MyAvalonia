// -----------------------------------------------------------------------
// <copyright file="CalendarSelectionCoordinator.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls.Primitives.Internals;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public sealed class CalendarSelectionCoordinator(
    Func<CalendarSelectionMode> selectionMode,
    Func<bool> allowTapRangeSelection,
    Func<DateTime> displayDate,
    Func<DateTime, bool> isValidSelection,
    ICalendarSelectionCommands commands)
{
    private DateTime? _hoverStart;

    public DateTime? HoverStart => _hoverStart;

    public void ResetHover() => _hoverStart = null;

    public void ProcessDateSelection(DateTime date, bool shift, bool ctrl)
    {
        if (!isValidSelection(date))
            return;

        if (allowTapRangeSelection())
        {
            ProcessTapRangeSelection(date, ctrl);
            return;
        }

        switch (selectionMode())
        {
            case CalendarSelectionMode.SingleDate:
                commands.SetSelection(date);
                break;

            case CalendarSelectionMode.SingleRange:
                if (shift)
                    commands.SetSelection(_hoverStart ?? displayDate(), date);
                else
                    commands.SetSelection(date);

                break;

            case CalendarSelectionMode.MultipleRange:
                if (ctrl)
                {
                    if (shift)
                    {
                        var startDate = _hoverStart ?? displayDate();
                        commands.ChangeSelection(startDate, date, commands.Contains(startDate));
                    }
                    else
                    {
                        commands.ToggleSelection(date);
                    }
                }
                else if (shift)
                {
                    commands.SetSelection(_hoverStart ?? displayDate(), date);
                }
                else
                {
                    commands.SetSelection(date);
                }

                break;
        }

        if (!shift)
            _hoverStart = date;

        commands.MoveToDate(date);
    }

    public void ProcessTapRangeSelection(DateTime date, bool ctrl)
    {
        switch (selectionMode())
        {
            case CalendarSelectionMode.SingleDate:
                commands.SetSelection(date);
                break;

            case CalendarSelectionMode.SingleRange:
                if (!_hoverStart.HasValue)
                {
                    commands.SetSelection(date);
                    _hoverStart = date;
                }
                else
                {
                    commands.SetSelection(_hoverStart.Value, date);
                    _hoverStart = null;
                }

                break;

            case CalendarSelectionMode.MultipleRange:
                if (ctrl)
                {
                    if (!_hoverStart.HasValue)
                    {
                        commands.AddSelection(date);
                        _hoverStart = date;
                    }
                    else
                    {
                        commands.AddSelection(_hoverStart.Value, date);
                        _hoverStart = null;
                    }
                }
                else if (!_hoverStart.HasValue)
                {
                    commands.SetSelection(date);
                    _hoverStart = date;
                }
                else
                {
                    commands.SetSelection(_hoverStart.Value, date);
                    _hoverStart = null;
                }

                break;
        }

        commands.MoveToDate(date);
    }

    public void BeginPointerSelection(DateTime date, bool shift)
    {
        if (!shift || !_hoverStart.HasValue)
            _hoverStart = date;
    }
}
