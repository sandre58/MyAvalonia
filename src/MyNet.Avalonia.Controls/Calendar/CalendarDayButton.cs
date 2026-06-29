// -----------------------------------------------------------------------
// <copyright file="CalendarDayButton.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Avalonia.Controls.Metadata;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls.Primitives;
#pragma warning restore IDE0130 // Namespace does not match folder structure

[PseudoClasses(PseudoClassName.Pressed, PseudoClassName.Selected, PseudoClassName.StartDate, PseudoClassName.EndDate, PseudoClassName.PreviewStartDate, PseudoClassName.PreviewEndDate, PseudoClassName.PreviewInRange, PseudoClassName.InRange, PseudoClassName.Today, PseudoClassName.Blackout, PseudoClassName.Inactive)]
public class CalendarDayButton : CalendarDateButton
{
    private bool _suppressPreviewPseudoClassUpdates;

    internal void SetPreviewRangeState(bool isPreviewStart, bool isPreviewEnd, bool isPreviewInRange)
    {
        if (IsPreviewStartDate == isPreviewStart
            && IsPreviewEndDate == isPreviewEnd
            && IsPreviewInRange == isPreviewInRange)
        {
            return;
        }

        _suppressPreviewPseudoClassUpdates = true;

        if (isPreviewInRange && !IsPreviewInRange)
        {
            IsPreviewInRange = true;
            PseudoClasses.Set(PseudoClassName.PreviewInRange, true);
        }

        if (isPreviewStart && !IsPreviewStartDate)
        {
            IsPreviewStartDate = true;
            PseudoClasses.Set(PseudoClassName.PreviewStartDate, true);
        }

        if (isPreviewEnd && !IsPreviewEndDate)
        {
            IsPreviewEndDate = true;
            PseudoClasses.Set(PseudoClassName.PreviewEndDate, true);
        }

        if (!isPreviewStart && IsPreviewStartDate)
        {
            IsPreviewStartDate = false;
            PseudoClasses.Set(PseudoClassName.PreviewStartDate, false);
        }

        if (!isPreviewEnd && IsPreviewEndDate)
        {
            IsPreviewEndDate = false;
            PseudoClasses.Set(PseudoClassName.PreviewEndDate, false);
        }

        if (!isPreviewInRange && IsPreviewInRange)
        {
            IsPreviewInRange = false;
            PseudoClasses.Set(PseudoClassName.PreviewInRange, false);
        }

        _suppressPreviewPseudoClassUpdates = false;
    }

    public bool IsStartDate
    {
        get;
        set
        {
            if (field == value)
                return;

            field = value;
            PseudoClasses.Set(PseudoClassName.StartDate, value);
        }
    }

    public bool IsEndDate
    {
        get;
        set
        {
            if (field == value)
                return;

            field = value;
            PseudoClasses.Set(PseudoClassName.EndDate, value);
        }
    }

    public bool IsPreviewStartDate
    {
        get;
        set
        {
            field = value;
            if (!_suppressPreviewPseudoClassUpdates)
                PseudoClasses.Set(PseudoClassName.PreviewStartDate, value);
        }
    }

    public bool IsPreviewEndDate
    {
        get;
        set
        {
            field = value;
            if (!_suppressPreviewPseudoClassUpdates)
                PseudoClasses.Set(PseudoClassName.PreviewEndDate, value);
        }
    }

    public bool IsPreviewInRange
    {
        get;
        set
        {
            field = value;
            if (!_suppressPreviewPseudoClassUpdates)
                PseudoClasses.Set(PseudoClassName.PreviewInRange, value);
        }
    }

    public bool IsInRange
    {
        get;
        set
        {
            if (field == value)
                return;

            field = value;
            PseudoClasses.Set(PseudoClassName.InRange, value);
        }
    }

    public bool IsBlackout
    {
        get;
        set
        {
            field = value;
            PseudoClasses.Set(PseudoClassName.Blackout, value);
        }
    }

    protected override void SetPseudoClasses()
    {
        base.SetPseudoClasses();

        PseudoClasses.Set(PseudoClassName.Pressed, IsPressed);
        PseudoClasses.Set(PseudoClassName.Disabled, !IsEnabled);
        PseudoClasses.Set(PseudoClassName.Blackout, IsBlackout);
        PseudoClasses.Set(PseudoClassName.StartDate, IsStartDate);
        PseudoClasses.Set(PseudoClassName.EndDate, IsEndDate);
        PseudoClasses.Set(PseudoClassName.PreviewEndDate, IsPreviewEndDate);
        PseudoClasses.Set(PseudoClassName.PreviewStartDate, IsPreviewStartDate);
        PseudoClasses.Set(PseudoClassName.PreviewInRange, IsPreviewInRange);
        PseudoClasses.Set(PseudoClassName.InRange, IsInRange);
    }
}
