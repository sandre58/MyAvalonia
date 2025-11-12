// -----------------------------------------------------------------------
// <copyright file="CalendarDayButton.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Avalonia.Controls.Metadata;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

[PseudoClasses(PseudoClassName.Pressed, PseudoClassName.Selected, PseudoClassName.StartDate, PseudoClassName.EndDate, PseudoClassName.PreviewStartDate, PseudoClassName.PreviewEndDate, PseudoClassName.InRange, PseudoClassName.Today, PseudoClassName.Blackout, PseudoClassName.Inactive)]
public class CalendarDayButton : CalendarDateButton
{
    private bool _ignoringMouseOverState;

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

    internal void IgnoreMouseOverState()
    {
        // TODO: Investigate whether this needs to be done by changing the
        // state everytime we change any state, or if it can be done once
        // to properly reset the control.
        _ignoringMouseOverState = false;

        // If the button thinks it's in the MouseOver state (which can
        // happen when a Popup is closed before the button can change state)
        // we will override the state so it shows up as normal.
        if (IsPointerOver)
        {
            _ignoringMouseOverState = true;
            SetPseudoClasses();
        }
    }

    protected override void SetPseudoClasses()
    {
        base.SetPseudoClasses();

        if (_ignoringMouseOverState)
        {
            PseudoClasses.Set(PseudoClassName.Pressed, IsPressed);
            PseudoClasses.Set(PseudoClassName.Disabled, !IsEnabled);
        }

        PseudoClasses.Set(PseudoClassName.Blackout, IsBlackout);
        PseudoClasses.Set(PseudoClassName.StartDate, IsStartDate);
        PseudoClasses.Set(PseudoClassName.EndDate, IsEndDate);
        PseudoClasses.Set(PseudoClassName.PreviewEndDate, IsPreviewEndDate);
        PseudoClasses.Set(PseudoClassName.PreviewStartDate, IsPreviewStartDate);
        PseudoClasses.Set(PseudoClassName.InRange, IsInRange);
    }
}
