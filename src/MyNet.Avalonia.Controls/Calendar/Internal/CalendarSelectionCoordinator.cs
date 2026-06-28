// -----------------------------------------------------------------------
// <copyright file="CalendarSelectionCoordinator.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls.Internals;
#pragma warning restore IDE0130 // Namespace does not match folder structure

internal sealed class CalendarSelectionCoordinator(
    Func<CalendarSelectionMode> selectionMode,
    Func<bool> allowTapRangeSelection,
    Func<DateTime> displayDate,
    Func<DateTime, bool> isValidSelection,
    ICalendarSelectionCommands commands)
{
    private DateTime? _anchorDate;
    private DateTime? _pointerPressDate;

    public DateTime? HoverStart => _anchorDate;

    public DateTime? PointerPressDate => _pointerPressDate;

    public bool HasPendingRangeAnchor => _anchorDate.HasValue;

    public void ResetHover() => _anchorDate = null;

    public void CommitPendingKeyboardRange(DateTime endDate)
    {
        if (!isValidSelection(endDate) || !_anchorDate.HasValue)
            return;

        if (allowTapRangeSelection())
        {
            ProcessTapRangeSelection(endDate, ctrl: false, shift: false);
            return;
        }

        var anchor = _anchorDate.Value;

        switch (selectionMode())
        {
            case CalendarSelectionMode.SingleRange:
            case CalendarSelectionMode.MultipleRange:
                commands.SetSelection(anchor, endDate);
                break;
        }

        _anchorDate = null;
        commands.MoveToDate(endDate);
    }

    public void CommitKeyboardAnchor(DateTime date)
    {
        if (!isValidSelection(date))
            return;

        if (allowTapRangeSelection())
        {
            ProcessTapRangeSelection(date, ctrl: false, shift: false);
            return;
        }

        switch (selectionMode())
        {
            case CalendarSelectionMode.SingleRange:
                ProcessSingleRangeSelection(date, shift: false);
                break;

            case CalendarSelectionMode.MultipleRange:
                ProcessMultipleRangeSelection(date, shift: false, ctrl: false);
                break;
        }
    }

    public void ProcessDateSelection(DateTime date, bool shift, bool ctrl)
    {
        if (!isValidSelection(date))
            return;

        if (selectionMode() == CalendarSelectionMode.MultipleRange && ctrl)
        {
            if (allowTapRangeSelection())
            {
                ProcessTapRangeSelection(date, ctrl: true, shift);
                return;
            }

            ProcessMultipleRangeSelection(date, shift, ctrl);
            commands.MoveToDate(date);
            return;
        }

        if (allowTapRangeSelection())
        {
            ProcessTapRangeSelection(date, ctrl, shift);
            return;
        }

        switch (selectionMode())
        {
            case CalendarSelectionMode.SingleDate:
                commands.SetSelection(date);
                break;

            case CalendarSelectionMode.SingleRange:
                ProcessSingleRangeSelection(date, shift);
                break;

            case CalendarSelectionMode.MultipleRange:
                ProcessMultipleRangeSelection(date, shift, ctrl);
                break;
        }

        commands.MoveToDate(date);
    }

    public void PointerSelectionEnd(DateTime releaseDate, bool shift, bool ctrl)
    {
        if (!isValidSelection(releaseDate))
            return;

        var pressDate = _pointerPressDate;
        var wasDrag = pressDate.HasValue
            && pressDate.Value != releaseDate
            && !allowTapRangeSelection();

        if (wasDrag)
        {
            var press = pressDate!.Value;

            switch (selectionMode())
            {
                case CalendarSelectionMode.SingleRange:
                    if (shift)
                    {
                        commands.SetSelection(GetEffectiveAnchor(press), releaseDate);
                    }
                    else
                    {
                        commands.SetSelection(press, releaseDate);
                        _anchorDate = press;
                    }

                    break;

                case CalendarSelectionMode.MultipleRange:
                    ProcessMultipleRangeDragEnd(press, releaseDate, shift, ctrl);
                    break;
            }

            commands.MoveToDate(releaseDate);
        }
        else
        {
            ProcessDateSelection(releaseDate, shift, ctrl);
        }

        _pointerPressDate = null;
    }

    public void ProcessTapRangeSelection(DateTime date, bool ctrl, bool shift = false)
    {
        switch (selectionMode())
        {
            case CalendarSelectionMode.SingleDate:
                commands.SetSelection(date);
                break;

            case CalendarSelectionMode.SingleRange:
                if (shift && _anchorDate.HasValue)
                {
                    commands.SetSelection(_anchorDate.Value, date);
                }
                else if (!_anchorDate.HasValue)
                {
                    commands.SetSelection(date);
                    _anchorDate = date;
                }
                else
                {
                    commands.SetSelection(_anchorDate.Value, date);
                    _anchorDate = null;
                }

                break;

            case CalendarSelectionMode.MultipleRange:
                if (ctrl)
                {
                    if (!_anchorDate.HasValue)
                    {
                        commands.AddSelection(date);
                        _anchorDate = date;
                    }
                    else
                    {
                        commands.AddSelection(_anchorDate.Value, date);
                        _anchorDate = null;
                    }
                }
                else if (shift && _anchorDate.HasValue)
                {
                    commands.SetSelection(_anchorDate.Value, date);
                }
                else if (!_anchorDate.HasValue)
                {
                    commands.SetSelection(date);
                    _anchorDate = date;
                }
                else
                {
                    commands.SetSelection(_anchorDate.Value, date);
                    _anchorDate = null;
                }

                break;
        }

        commands.MoveToDate(date);
    }

    public void BeginPointerSelection(DateTime date, bool shift)
    {
        _pointerPressDate = date;

        if (!shift || !_anchorDate.HasValue)
            _anchorDate = date;
    }

    private void ProcessSingleRangeSelection(DateTime date, bool shift)
    {
        if (shift)
        {
            commands.SetSelection(GetEffectiveAnchor(), date);
        }
        else
        {
            commands.SetSelection(date);
            _anchorDate = date;
        }
    }

    private DateTime GetEffectiveAnchor(DateTime? fallback = null) =>
        _anchorDate ?? fallback ?? displayDate();

    private void ProcessMultipleRangeSelection(DateTime date, bool shift, bool ctrl)
    {
        if (ctrl && shift)
        {
            commands.AddSelection(_anchorDate ?? displayDate(), date);
        }
        else if (ctrl)
        {
            var wasSelected = commands.Contains(date);
            commands.ToggleSelection(date);
            if (!wasSelected)
            {
                _anchorDate = date;
            }
        }
        else if (shift)
        {
            commands.SetSelection(_anchorDate ?? displayDate(), date);
        }
        else
        {
            commands.SetSelection(date);
            _anchorDate = date;
        }
    }

    private void ProcessMultipleRangeDragEnd(DateTime pressDate, DateTime releaseDate, bool shift, bool ctrl)
    {
        if (ctrl && shift)
        {
            commands.AddSelection(_anchorDate ?? pressDate, releaseDate);
        }
        else if (shift)
        {
            commands.SetSelection(_anchorDate ?? pressDate, releaseDate);
        }
        else
        {
            commands.SetSelection(pressDate, releaseDate);
            _anchorDate = pressDate;
        }
    }
}
