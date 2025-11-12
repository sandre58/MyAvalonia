// -----------------------------------------------------------------------
// <copyright file="CalendarDateButton.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Mixins;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using MyNet.Avalonia.Controls.DateTimePickers;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

[PseudoClasses(PseudoClassName.Range, PseudoClassName.Selected, PseudoClassName.Inactive, PseudoClassName.Today)]
public class CalendarDateButton : Button
{
    public static readonly RoutedEvent<CalendarDateButtonEventArgs> DateSelectedEvent = RoutedEvent.Register<CalendarYearButton, CalendarDateButtonEventArgs>(nameof(DateSelected), RoutingStrategies.Bubble);

    public static readonly RoutedEvent<CalendarDateButtonEventArgs> DatePreviewedEvent = RoutedEvent.Register<CalendarDayButton, CalendarDateButtonEventArgs>(nameof(DatePreviewed), RoutingStrategies.Bubble);

    static CalendarDateButton() => PressedMixin.Attach<CalendarDateButton>();

    public DateContext? DateContext { get; private set; }

    internal Calendar? Owner { get; set; }

    internal int Index { get; set; }

    public event EventHandler<CalendarDateButtonEventArgs> DateSelected
    {
        add => AddHandler(DateSelectedEvent, value);
        remove => RemoveHandler(DateSelectedEvent, value);
    }

    public event EventHandler<CalendarDateButtonEventArgs> DatePreviewed
    {
        add => AddHandler(DatePreviewedEvent, value);
        remove => RemoveHandler(DatePreviewedEvent, value);
    }

    public bool IsToday
    {
        get;
        set
        {
            field = value;
            PseudoClasses.Set(PseudoClassName.Today, value);
        }
    }

    public bool IsInactive
    {
        get;
        set
        {
            field = value;
            PseudoClasses.Set(PseudoClassName.Inactive, value);
        }
    }

    public bool IsSelected
    {
        get;
        set
        {
            field = value;
            PseudoClasses.Set(PseudoClassName.Selected, value);
        }
    }

    internal void SetContext(DateContext context)
    {
        DateContext = context;
        DataContext = context.ToDate();
        Content = context.ToString();
        IsToday = context.IsSimilar(DateTime.Today);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e) => SetPseudoClasses();

    protected virtual void SetPseudoClasses()
    {
        PseudoClasses.Set(PseudoClassName.Selected, IsSelected);
        PseudoClasses.Set(PseudoClassName.Inactive, IsInactive);
        PseudoClasses.Set(PseudoClassName.Today, IsToday);
    }
}
