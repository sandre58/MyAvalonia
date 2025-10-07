// -----------------------------------------------------------------------
// <copyright file="CalendarDayButton.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Mixins;
using Avalonia.Input;
using Avalonia.Interactivity;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

[PseudoClasses(PseudoClassName.Pressed, PseudoClassName.Selected, PseudoClassName.StartDate, PseudoClassName.EndDate, PseudoClassName.PreviewStartDate, PseudoClassName.PreviewEndDate, PseudoClassName.InRange, PseudoClassName.Today, PseudoClassName.Blackout, PseudoClassName.Inactive)]
public class CalendarDayButton : Button
{
    private static readonly HashSet<string> AvailablePseudoClasses =
    [
        PseudoClassName.Selected, PseudoClassName.StartDate, PseudoClassName.EndDate, PseudoClassName.PreviewStartDate,  PseudoClassName.PreviewEndDate, PseudoClassName.InRange
    ];

    public static readonly RoutedEvent<CalendarDayButtonEventArgs> DateSelectedEvent =
        RoutedEvent.Register<CalendarDayButton, CalendarDayButtonEventArgs>(
            nameof(DateSelected), RoutingStrategies.Bubble);

    public static readonly RoutedEvent<CalendarDayButtonEventArgs> DatePreviewedEvent =
        RoutedEvent.Register<CalendarDayButton, CalendarDayButtonEventArgs>(
            nameof(DatePreviewed), RoutingStrategies.Bubble);

    static CalendarDayButton() => PressedMixin.Attach<CalendarDayButton>();

    public bool IsToday
    {
        get;
        set
        {
            field = value;
            PseudoClasses.Set(PseudoClassName.Today, value);
        }
    }

    public bool IsStartDate
    {
        get;
        set
        {
            field = value;
            SetPseudoClass(PseudoClassName.StartDate, value);
        }
    }

    public bool IsEndDate
    {
        get;
        set
        {
            field = value;
            SetPseudoClass(PseudoClassName.EndDate, value);
        }
    }

    public bool IsPreviewStartDate
    {
        get;
        set
        {
            field = value;
            SetPseudoClass(PseudoClassName.PreviewStartDate, value);
        }
    }

    public bool IsPreviewEndDate
    {
        get;
        set
        {
            field = value;
            SetPseudoClass(PseudoClassName.PreviewEndDate, value);
        }
    }

    public bool IsInRange
    {
        get;
        set
        {
            field = value;
            SetPseudoClass(PseudoClassName.InRange, value);
        }
    }

    public bool IsSelected
    {
        get;
        set
        {
            field = value;
            SetPseudoClass(PseudoClassName.Selected, value);
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

    public bool IsNotCurrentMonth
    {
        get;
        set
        {
            field = value;
            PseudoClasses.Set(PseudoClassName.Inactive, value);
        }
    }

    public event EventHandler<CalendarDayButtonEventArgs> DateSelected
    {
        add => AddHandler(DateSelectedEvent, value);
        remove => RemoveHandler(DateSelectedEvent, value);
    }

    public event EventHandler<CalendarDayButtonEventArgs> DatePreviewed
    {
        add => AddHandler(DatePreviewedEvent, value);
        remove => RemoveHandler(DatePreviewedEvent, value);
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);
        if (DataContext is DateTime d)
            RaiseEvent(new CalendarDayButtonEventArgs(d) { RoutedEvent = DateSelectedEvent, Source = this });
    }

    protected override void OnPointerEntered(PointerEventArgs e)
    {
        base.OnPointerEntered(e);
        if (DataContext is DateTime d)
            RaiseEvent(new CalendarDayButtonEventArgs(d) { RoutedEvent = DatePreviewedEvent, Source = this });
    }

    internal void ResetSelection()
    {
        foreach (var pc in AvailablePseudoClasses)
        {
            PseudoClasses.Set(pc, false);
        }
    }

    private void SetPseudoClass(string s, bool value)
    {
        if (AvailablePseudoClasses.Contains(s) && value)
        {
            foreach (var pc in AvailablePseudoClasses)
            {
                PseudoClasses.Set(pc, false);
            }
        }

        PseudoClasses.Set(s, value);
    }
}
