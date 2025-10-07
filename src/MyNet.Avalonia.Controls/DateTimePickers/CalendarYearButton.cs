// -----------------------------------------------------------------------
// <copyright file="CalendarYearButton.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Mixins;
using Avalonia.Input;
using Avalonia.Interactivity;
using MyNet.Avalonia.Controls.DateTimePickers;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

[PseudoClasses(PseudoClassName.Range, PseudoClassName.Selected)]
public class CalendarYearButton : Button
{
    public static readonly RoutedEvent<CalendarYearButtonEventArgs> ItemSelectedEvent =
        RoutedEvent.Register<CalendarYearButton, CalendarYearButtonEventArgs>(
            nameof(ItemSelected), RoutingStrategies.Bubble);

    static CalendarYearButton() => PressedMixin.Attach<CalendarYearButton>();

    internal CalendarContext CalendarContext { get; set; } = new();

    internal CalendarViewMode Mode { get; private set; }

    public event EventHandler<CalendarDayButtonEventArgs> ItemSelected
    {
        add => AddHandler(ItemSelectedEvent, value);
        remove => RemoveHandler(ItemSelectedEvent, value);
    }

    internal void SetContext(CalendarViewMode mode, CalendarContext context)
    {
        CalendarContext = context.Clone();
        Mode = mode;
        Content = Mode switch
        {
            CalendarViewMode.Year => CultureInfo.CurrentCulture.DateTimeFormat
                                .AbbreviatedMonthNames[CalendarContext.Month - 1 ?? 0],
            CalendarViewMode.Decade => CalendarContext.Year is <= 0 or > 9999
                                ? null
                                : CalendarContext.Year?.ToString(CultureInfo.CurrentCulture),
            CalendarViewMode.Century => CalendarContext.EndYear <= 0 || CalendarContext.StartYear > 9999
                                ? null
                                : CalendarContext.StartYear + "-" + CalendarContext.EndYear,
            CalendarViewMode.Month => null,
            _ => throw new InvalidOperationException(),
        };
        IsEnabled = Content != null;
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);
        RaiseEvent(new CalendarYearButtonEventArgs(Mode, CalendarContext.Clone())
        { RoutedEvent = ItemSelectedEvent, Source = this });
    }
}
