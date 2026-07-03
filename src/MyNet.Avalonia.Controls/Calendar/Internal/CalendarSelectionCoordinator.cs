// -----------------------------------------------------------------------
// <copyright file="CalendarSelectionCoordinator.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using MyNet.Primitives;

namespace MyNet.Avalonia.Controls.Internals.Calendar;

/// <summary>
/// ListBox Extended-style selection rules for calendar day cells (Variant A: first commit sets anchor + selection).
/// </summary>
internal sealed class CalendarSelectionCoordinator(
    Func<CalendarSelectionMode> selectionMode,
    Func<bool> allowTapRangeSelection,
    Func<DateTime> displayDate,
    Func<DateTime, bool> isValidSelection,
    ICalendarSelectionCommands commands)
{
    private DateTime? _anchorDate;

    public DateTime? HoverStart => _anchorDate;

    public DateTime? PointerPressDate { get; private set; }

    public DateTime? PointerDragOriginDate { get; private set; }

    public bool HasPendingRangeAnchor => _anchorDate.HasValue;

    public void ResetHover() => _anchorDate = null;

    /// <summary>Sets the range anchor without changing the committed selection (drag-mode focus tracking).</summary>
    public void SetRangeAnchor(DateTime date)
    {
        if (isValidSelection(date))
            _anchorDate = date;
    }

    /// <summary>Records a pointer press without changing anchor or committed selection.</summary>
    public void RecordPointerPress(DateTime date)
    {
        if (isValidSelection(date))
        {
            PointerPressDate = date;
            PointerDragOriginDate = date;
        }
    }

    public void ClearPointerPress()
    {
        PointerPressDate = null;
        PointerDragOriginDate = null;
    }

    /// <summary>Pointer click release or drag-free commit.</summary>
    public void Commit(DateTime date, bool shift, bool ctrl) => CommitCore(date, shift, ctrl);

    /// <summary>Space / Enter — applies preview state then commits.</summary>
    public void CommitFromKeyboard(DateTime date, bool intervalPreview, bool shift = false, bool ctrl = false)
    {
        if (!isValidSelection(date))
            return;

        NormalizeTapModifiers(ref shift, ref ctrl);

        switch (selectionMode())
        {
            case CalendarSelectionMode.SingleDate:
                commands.SetSelection(date);
                commands.MoveToDate(date);
                break;

            case CalendarSelectionMode.SingleRange:
            case CalendarSelectionMode.MultipleRange:
                if (!_anchorDate.HasValue)
                    CommitCore(date, shift, ctrl);
                else if (intervalPreview)
                    CommitRange(_anchorDate.Value, date, shift, ctrl, clearAnchor: allowTapRangeSelection());
                else
                    CommitCore(date, shift, ctrl);

                break;
        }
    }

    public void BeginPointerSelection(DateTime date, bool shift)
    {
        PointerPressDate = date;

        if (!shift || !_anchorDate.HasValue)
            _anchorDate = date;
    }

    public void CompletePointerSelection(DateTime releaseDate, bool shift, bool ctrl, bool wasDrag)
    {
        if (!isValidSelection(releaseDate))
            return;

        if (wasDrag && PointerPressDate is { } pressDate && pressDate != releaseDate)
        {
            CommitDrag(pressDate, releaseDate, shift, ctrl);
            PointerPressDate = null;
            PointerDragOriginDate = null;
            return;
        }

        Commit(releaseDate, shift, ctrl);
        PointerPressDate = null;
        PointerDragOriginDate = null;
    }

    public void CommitPointerDrag(DateTime pressDate, DateTime farExtent, bool shift, bool ctrl)
    {
        if (!isValidSelection(farExtent) || !isValidSelection(pressDate))
            return;

        var rangeStart = pressDate <= farExtent ? pressDate : farExtent;
        var rangeEnd = pressDate >= farExtent ? pressDate : farExtent;

        if (rangeStart == rangeEnd)
        {
            Commit(rangeStart, shift, ctrl);
            PointerPressDate = null;
            PointerDragOriginDate = null;
            return;
        }

        switch (selectionMode())
        {
            case CalendarSelectionMode.SingleRange:
                if (shift)
                {
                    CommitRange(GetEffectiveAnchor(pressDate), farExtent, shift, ctrl, clearAnchor: false);
                }
                else
                {
                    commands.SetSelection(rangeStart, rangeEnd);
                    _anchorDate = pressDate;
                }

                commands.MoveToDate(farExtent);
                break;

            case CalendarSelectionMode.MultipleRange:
                CommitMultipleRangeDrag(pressDate, farExtent, shift, ctrl);
                commands.MoveToDate(farExtent);
                break;
        }

        PointerPressDate = null;
        PointerDragOriginDate = null;
    }

    private void CommitCore(DateTime date, bool shift, bool ctrl)
    {
        if (!isValidSelection(date))
            return;

        NormalizeTapModifiers(ref shift, ref ctrl);

        switch (selectionMode())
        {
            case CalendarSelectionMode.SingleDate:
                commands.SetSelection(date);
                break;

            case CalendarSelectionMode.SingleRange:
                CommitSingleRangeClick(date, shift, ctrl);
                break;

            case CalendarSelectionMode.MultipleRange:
                CommitMultipleRangeClick(date, shift, ctrl);
                break;
        }

        commands.MoveToDate(date);
    }

    private void CommitSingleRangeClick(DateTime date, bool shift, bool ctrl)
    {
        if (allowTapRangeSelection())
        {
            CommitTapRange(date, shift, ctrl);
            return;
        }

        if (shift)
        {
            CommitRange(GetEffectiveAnchor(), date, shift, ctrl, clearAnchor: false);
            return;
        }

        commands.SetSelection(date);
        _anchorDate = date;
    }

    /// <summary>MultipleRange drag-mode click (ListBox Extended): plain = replace, Ctrl = toggle, Shift = range replace, Ctrl+Shift = add range.</summary>
    private void CommitMultipleRangeClick(DateTime date, bool shift, bool ctrl)
    {
        if (allowTapRangeSelection())
        {
            CommitTapRange(date, shift, ctrl: ctrl);
            return;
        }

        if (ctrl && shift)
        {
            commands.AddSelection(GetEffectiveAnchor(), date);
            return;
        }

        if (ctrl)
        {
            var wasSelected = commands.Contains(date);
            commands.ToggleSelection(date);
            if (!wasSelected)
                _anchorDate = date;

            return;
        }

        if (shift)
        {
            CommitRange(GetEffectiveAnchor(), date, shift, ctrl, clearAnchor: false);
            return;
        }

        commands.SetSelection(date);
        _anchorDate = date;
    }

    /// <summary>Variant A — 1st tap commits start + anchor; 2nd tap commits range. Tap mode ignores Shift; SingleRange tap also ignores Ctrl.</summary>
    private void CommitTapRange(DateTime date, bool shift, bool ctrl)
    {
        if (ctrl && selectionMode() == CalendarSelectionMode.MultipleRange)
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

            return;
        }

        if (!_anchorDate.HasValue)
        {
            commands.SetSelection(date);
            _anchorDate = date;
            return;
        }

        CommitRange(_anchorDate.Value, date, shift: false, ctrl: false, clearAnchor: true);
    }

    private void NormalizeTapModifiers(ref bool shift, ref bool ctrl)
    {
        if (!allowTapRangeSelection())
            return;

        shift = false;

        if (selectionMode() == CalendarSelectionMode.SingleRange)
            ctrl = false;
    }

    private void CommitRange(DateTime anchor, DateTime end, bool shift, bool ctrl, bool clearAnchor)
    {
        var (rangeStart, rangeEnd) = anchor.MinMax(end);

        switch (selectionMode())
        {
            case CalendarSelectionMode.SingleRange:
                commands.SetSelection(rangeStart, rangeEnd);
                break;

            case CalendarSelectionMode.MultipleRange when ctrl && (shift || allowTapRangeSelection()):
                commands.AddSelection(rangeStart, rangeEnd);
                break;

            case CalendarSelectionMode.MultipleRange:
                commands.SetSelection(rangeStart, rangeEnd);
                break;
        }

        if (clearAnchor && !shift)
            _anchorDate = null;
    }

    private void CommitDrag(DateTime pressDate, DateTime releaseDate, bool shift, bool ctrl)
    {
        switch (selectionMode())
        {
            case CalendarSelectionMode.SingleRange:
                CommitSingleRangeDrag(pressDate, releaseDate, shift, ctrl);
                break;

            case CalendarSelectionMode.MultipleRange:
                CommitMultipleRangeDrag(pressDate, releaseDate, shift, ctrl);
                break;
        }

        commands.MoveToDate(releaseDate);
    }

    private void CommitSingleRangeDrag(DateTime pressDate, DateTime releaseDate, bool shift, bool ctrl)
    {
        if (shift)
        {
            CommitRange(GetEffectiveAnchor(pressDate), releaseDate, shift, ctrl, clearAnchor: false);
        }
        else
        {
            var (rangeStart, rangeEnd) = pressDate.MinMax(releaseDate);
            commands.SetSelection(rangeStart, rangeEnd);
            _anchorDate = pressDate;
        }
    }

    /// <summary>MultipleRange drag-mode drag: plain = replace range, Shift = range replace from anchor, Ctrl+Shift = add range; Ctrl alone ignored.</summary>
    private void CommitMultipleRangeDrag(DateTime pressDate, DateTime releaseDate, bool shift, bool ctrl)
    {
        if (ctrl && shift)
        {
            var (rangeStart, rangeEnd) = (_anchorDate ?? pressDate).MinMax(releaseDate);
            commands.AddSelection(rangeStart, rangeEnd);
        }
        else if (shift)
        {
            CommitRange(_anchorDate ?? pressDate, releaseDate, shift, ctrl, clearAnchor: false);
        }
        else
        {
            var (rangeStart, rangeEnd) = pressDate.MinMax(releaseDate);
            commands.SetSelection(rangeStart, rangeEnd);
            _anchorDate = pressDate;
        }
    }

    private DateTime GetEffectiveAnchor(DateTime? fallback = null) =>
        _anchorDate ?? fallback ?? displayDate();
}
