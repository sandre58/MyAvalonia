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

[PseudoClasses(PseudoClassName.Pressed, PseudoClassName.Selected, PseudoClassName.StartDate, PseudoClassName.EndDate, PseudoClassName.PreviewStartDate, PseudoClassName.PreviewEndDate, PseudoClassName.InRange, PseudoClassName.Today, PseudoClassName.Blackout, PseudoClassName.Inactive)]
public class CalendarDayButton : CalendarDateButton
{
    public bool IsStartDate
    {
        get;
        set
        {
            field = value;
            PseudoClasses.Set(PseudoClassName.StartDate, value);
        }
    }

    public bool IsEndDate
    {
        get;
        set
        {
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
            PseudoClasses.Set(PseudoClassName.PreviewStartDate, value);
        }
    }

    public bool IsPreviewEndDate
    {
        get;
        set
        {
            field = value;
            PseudoClasses.Set(PseudoClassName.PreviewEndDate, value);
        }
    }

    public bool IsInRange
    {
        get;
        set
        {
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
        PseudoClasses.Set(PseudoClassName.InRange, IsInRange);
    }
}
