// -----------------------------------------------------------------------
// <copyright file="CalendarPreviewSession.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using MyNet.Primitives;

namespace MyNet.Avalonia.Controls.Internals.Calendar;

internal readonly struct CalendarPreviewContext(
    bool isRangeSelectionMode,
    bool allowTapRangeSelection,
    bool isDragRangeSelectionMode,
    bool hasPendingRangeAnchor,
    DateTime? rangeAnchor,
    DateTime? pointerPressDate,
    bool isPointerCommitFrozen,
    bool isPostCommitPreviewSuspended,
    int previewHighlightCount)
{
    public bool IsRangeSelectionMode { get; } = isRangeSelectionMode;

    public bool AllowTapRangeSelection { get; } = allowTapRangeSelection;

    public bool IsDragRangeSelectionMode { get; } = isDragRangeSelectionMode;

    public bool HasPendingRangeAnchor { get; } = hasPendingRangeAnchor;

    public DateTime? RangeAnchor { get; } = rangeAnchor;

    public DateTime? PointerPressDate { get; } = pointerPressDate;

    public bool IsPointerCommitFrozen { get; } = isPointerCommitFrozen;

    public bool IsPostCommitPreviewSuspended { get; } = isPostCommitPreviewSuspended;

    public int PreviewHighlightCount { get; } = previewHighlightCount;
}

/// <summary>
/// Transient preview interval state for calendar range selection (controller, endpoints, cache).
/// Re-entrant gates (commit freeze, post-commit suspend, highlight defer) live on <see cref="Calendar"/> via <see cref="MyNet.Utilities.Suspending.Suspender"/>.
/// </summary>
internal sealed class CalendarPreviewSession
{
    public CalendarPreviewController Controller { get; set; }

    public DateTime? PreviewEndDate { get; set; }

    public DateTime? PointerOverDate { get; set; }

    public bool IntervalPreviewActive { get; set; }

    public bool IsPointerSelecting { get; set; }

    /// <summary>Immutable drag origin set on pointer down; never updated by TryStart.</summary>
    public DateTime? DragOrigin { get; set; }

    /// <summary>Farthest pointer extent during an active drag.</summary>
    public DateTime? DragExtent { get; set; }

    public DateTime? CachedAnchor { get; set; }

    public DateTime? CachedEnd { get; set; }

    public DateTime? SuspendedAtDate { get; set; }

    public void InvalidateCache()
    {
        CachedAnchor = null;
        CachedEnd = null;
    }

    public void ResetController()
    {
        Controller = CalendarPreviewController.None;
        IntervalPreviewActive = false;
    }

    public void ResetForFullClear()
    {
        PreviewEndDate = null;
        PointerOverDate = null;
        IsPointerSelecting = false;
        Controller = CalendarPreviewController.None;
        IntervalPreviewActive = false;
        SuspendedAtDate = null;
        DragOrigin = null;
        DragExtent = null;
    }

    public void ClearVisualState()
    {
        PreviewEndDate = null;
        PointerOverDate = null;
        DragOrigin = null;
        DragExtent = null;
        ResetController();
    }

    public bool ResolveIntervalPreview(CalendarPreviewContext context, bool shiftHeld) =>
        context is { IsRangeSelectionMode: true, HasPendingRangeAnchor: true }
        && (context.AllowTapRangeSelection || shiftHeld);

    public bool ShouldPreviewInterval(CalendarPreviewContext context) => context is { IsRangeSelectionMode: true, IsPostCommitPreviewSuspended: false } && (IsPointerSelecting || Controller == CalendarPreviewController.Drag || (context.HasPendingRangeAnchor && (Controller == CalendarPreviewController.Keyboard && PreviewEndDate.HasValue
        ? IntervalPreviewActive
        : context.AllowTapRangeSelection
            ? PreviewEndDate.HasValue || PointerOverDate.HasValue
            : Controller == CalendarPreviewController.PointerShift || (context.IsPointerCommitFrozen && !IsPointerSelecting && context.PreviewHighlightCount > 0))));

    public bool TryGetPreviewInterval(CalendarPreviewContext context, out DateTime anchor, out DateTime end)
    {
        anchor = default;
        end = default;

        if (context.IsPointerCommitFrozen
            && !IsPointerSelecting
            && CachedAnchor is { } cachedAnchor
            && CachedEnd is { } cachedEnd)
        {
            anchor = cachedAnchor;
            end = cachedEnd;
            return true;
        }

        if (!ShouldPreviewInterval(context))
            return false;

        if (!TryResolvePreviewStart(context, out anchor))
            return false;

        end = Controller == CalendarPreviewController.Keyboard
            ? PreviewEndDate ?? default
            : ResolvePointerPreviewEnd(anchor);
        return end != default;
    }

    private DateTime ResolvePointerPreviewEnd(DateTime anchor)
    {
        var previewEnd = PreviewEndDate;
        var pointerOver = PointerOverDate;

        if (previewEnd is null)
            return pointerOver ?? default;

        if (pointerOver is null)
            return previewEnd.Value;

        anchor = anchor.DiscardTime();
        var preview = previewEnd.Value.DiscardTime();
        var over = pointerOver.Value.DiscardTime();

        return preview >= anchor
            ? over > preview ? over : preview
            : over < preview ? over : preview;
    }

    public static DateTime ExtentFarthestFromAnchor(DateTime anchor, DateTime releaseDate, DateTime previewEnd)
    {
        anchor = anchor.DiscardTime();
        releaseDate = releaseDate.DiscardTime();
        previewEnd = previewEnd.DiscardTime();

        return previewEnd >= anchor
            ? releaseDate > previewEnd ? releaseDate : previewEnd
            : releaseDate < previewEnd ? releaseDate : previewEnd;
    }

    private static bool TryResolvePreviewStart(CalendarPreviewContext context, out DateTime start)
    {
        start = default;

        if (context.IsDragRangeSelectionMode)
        {
            if (context.PointerPressDate is { } press)
            {
                start = press;
                return true;
            }

            if (!context.HasPendingRangeAnchor)
                return false;

            start = context.RangeAnchor!.Value;
            return true;
        }

        if (context.HasPendingRangeAnchor)
        {
            start = context.RangeAnchor!.Value;
            return true;
        }

        return false;
    }
}
