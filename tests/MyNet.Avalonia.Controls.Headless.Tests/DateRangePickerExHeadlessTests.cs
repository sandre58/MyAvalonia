// -----------------------------------------------------------------------
// <copyright file="DateRangePickerExHeadlessTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Styling;
using Avalonia.Threading;
using FluentAssertions;
using MyNet.Avalonia.Controls.Primitives;
using MyNet.Primitives;

namespace MyNet.Avalonia.Controls.Headless.Tests;

public class DateRangePickerExHeadlessTests
{
    private sealed class TestableDateRangePickerEx : DateRangePickerEx
    {
        public Calendar? CalendarPreviewer => Previewer;
    }

    [AvaloniaFact]
    public void ClosePopup_WithPendingFirstTap_RestoresCommittedValue()
    {
        var picker = CreatePicker();
        var committed = new DateTime(2026, 6, 8).ToPeriod(new DateTime(2026, 6, 11));
        picker.SelectedValue = committed;

        picker.IsDropDownOpen = true;
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        var calendar = GetPreviewer(picker);
        var grid = HeadlessControlHost.FindByName<Grid>(calendar, Calendar.PartMonthGrid);
        grid.Should().NotBeNull();

        HeadlessControlHost.PointerPress(FindDayButton(grid!, new(2026, 6, 14)));
        HeadlessControlHost.PointerRelease(FindDayButton(grid!, new(2026, 6, 14)));
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        calendar.HasPendingRangeSelection.Should().BeTrue();

        picker.IsDropDownOpen = false;
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        picker.SelectedValue.Should().Be(committed);
    }

    [AvaloniaFact]
    public void ReopenPopup_AfterUncommittedFirstTap_HasNoPendingAnchor()
    {
        var picker = CreatePicker();
        picker.SelectedValue = new DateTime(2026, 6, 8).ToPeriod(new DateTime(2026, 6, 11));

        picker.IsDropDownOpen = true;
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        var calendar = GetPreviewer(picker);
        var grid = HeadlessControlHost.FindByName<Grid>(calendar, Calendar.PartMonthGrid);
        grid.Should().NotBeNull();

        HeadlessControlHost.PointerPress(FindDayButton(grid!, new(2026, 6, 14)));
        HeadlessControlHost.PointerRelease(FindDayButton(grid!, new(2026, 6, 14)));
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        picker.IsDropDownOpen = false;
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        picker.IsDropDownOpen = true;
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        GetPreviewer(picker).HasPendingRangeSelection.Should().BeFalse();
    }

    [AvaloniaFact]
    public void CancelPendingRangeSelection_ClearsTapModeAnchor()
    {
        var calendar = CalendarHeadlessTestHelpers.CreateCalendar(new(2026, 6, 15));
        calendar.SelectionMode = CalendarSelectionMode.SingleRange;
        calendar.AllowTapRangeSelection = true;
        HeadlessControlHost.Show(calendar, new(420, 360));

        var grid = HeadlessControlHost.FindByName<Grid>(calendar, Calendar.PartMonthGrid);
        grid.Should().NotBeNull();

        HeadlessControlHost.PointerPress(FindDayButton(grid!, new(2026, 6, 14)));
        HeadlessControlHost.PointerRelease(FindDayButton(grid!, new(2026, 6, 14)));
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        calendar.HasPendingRangeSelection.Should().BeTrue();

        calendar.CancelPendingRangeSelection();

        calendar.HasPendingRangeSelection.Should().BeFalse();
    }

    private static TestableDateRangePickerEx CreatePicker()
    {
        HeadlessTestApp.EnsureGlobalizationServices();
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

        Application.Current!.TryGetResource(typeof(DateRangePickerEx), null, out var themeObj).Should().BeTrue();
        var theme = themeObj as ControlTheme;
        theme.Should().NotBeNull();

        var picker = new TestableDateRangePickerEx
        {
            DisplayFormat = "yyyy-MM-dd",
            DisplayDate = new(2026, 6, 15),
            AutoCommit = false,
            Width = 320,
            Height = 40,
            Theme = theme,
        };

        HeadlessControlHost.Show(picker, new(420, 360));
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Loaded);

        return picker;
    }

    private static Calendar GetPreviewer(TestableDateRangePickerEx picker)
    {
        picker.CalendarPreviewer.Should().NotBeNull();
        return picker.CalendarPreviewer!;
    }

    private static CalendarDayButton FindDayButton(Grid grid, DateTime date) =>
        CalendarHeadlessTestHelpers.FindDayButton(grid, date);
}
