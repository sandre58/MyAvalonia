// -----------------------------------------------------------------------
// <copyright file="PickerInteractionHeadlessTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.Styling;
using Avalonia.Threading;
using Avalonia.VisualTree;
using FluentAssertions;
using MyNet.Avalonia.Controls.Internals;
using MyNet.Avalonia.Controls.Primitives;

namespace MyNet.Avalonia.Controls.Headless.Tests;

public class PickerInteractionHeadlessTests
{
    [AvaloniaFact]
    public void CalendarDatePickerEx_DayClick_ClosesPopupAndCommits()
    {
        var picker = CreateCalendarDatePickerEx();
        var initial = new DateTime(2026, 6, 1);
        picker.SelectedValue = initial;

        picker.IsDropDownOpen = true;
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        var calendar = GetCalendarPreviewer(picker);
        var grid = HeadlessControlHost.FindByName<Grid>(calendar, Calendar.PartMonthGrid);
        grid.Should().NotBeNull();

        var day = CalendarHeadlessTestHelpers.FindDayButton(grid!, new(2026, 6, 10));
        HeadlessControlHost.PointerPress(day);
        HeadlessControlHost.PointerRelease(day);
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        picker.SelectedValue.Should().Be(new(2026, 6, 10));
        picker.IsDropDownOpen.Should().BeFalse();
    }

    [AvaloniaFact]
    public void CalendarDatePickerEx_EscapeWhileOpen_RollsBackAndCloses()
    {
        var picker = CreateCalendarDatePickerEx();
        var committed = new DateTime(2026, 6, 1);
        picker.SelectedValue = committed;

        picker.IsDropDownOpen = true;
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        var calendar = GetCalendarPreviewer(picker);
        var grid = HeadlessControlHost.FindByName<Grid>(calendar, Calendar.PartMonthGrid);
        grid.Should().NotBeNull();

        var day = CalendarHeadlessTestHelpers.FindDayButton(grid!, new(2026, 6, 15));
        HeadlessControlHost.PointerPress(day);
        HeadlessControlHost.PointerRelease(day);
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        picker.SelectedValue.Should().Be(new(2026, 6, 15));

        HeadlessControlHost.KeyDown(picker, Key.Escape);
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        picker.SelectedValue.Should().Be(committed);
        picker.IsDropDownOpen.Should().BeFalse();
    }

    [AvaloniaFact]
    public void DateTimePickerEx_PreviewChange_KeepsPopupOpen()
    {
        var picker = CreateDateTimePickerEx();
        picker.SelectedValue = new DateTime(2026, 6, 1, 10, 0, 0);

        picker.IsDropDownOpen = true;
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        picker.PreviewerView!.SelectedValue = new DateTime(2026, 6, 1, 11, 0, 0);
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        picker.SelectedValue.Should().Be(new(2026, 6, 1, 11, 0, 0));
        picker.IsDropDownOpen.Should().BeTrue();
    }

    [AvaloniaFact]
    public void DateTimePickerEx_EnterWhenClosed_OpensPopup()
    {
        var picker = CreateDateTimePickerEx();
        picker.SelectedValue = new DateTime(2026, 6, 1);

        HeadlessControlHost.KeyDown(picker, Key.Enter);
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        picker.IsDropDownOpen.Should().BeTrue();
    }

    [AvaloniaFact]
    public void CalendarDatePickerEx_EnterOnFocusedDay_ClosesPopupAndCommits()
    {
        var picker = CreateCalendarDatePickerEx();
        picker.SelectedValue = new DateTime(2026, 6, 15);

        picker.IsDropDownOpen = true;
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        var calendar = GetCalendarPreviewer(picker);
        calendar.Focus();
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        for (var i = 0; i < 5; i++)
            HeadlessControlHost.KeyDown(calendar, Key.Right);

        HeadlessControlHost.KeyDown(calendar, Key.Enter);
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        picker.SelectedValue.Should().Be(new(2026, 6, 20));
        picker.IsDropDownOpen.Should().BeFalse();
    }

    [AvaloniaFact]
    public void TimePickerEx_EnterOnHourSpinner_KeepsPopupOpenAndAdvancesToMinute()
    {
        var picker = CreateTimePickerEx();
        picker.SelectedValue = new TimeSpan(9, 0, 0);

        picker.IsDropDownOpen = true;
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        var timeView = picker.TimePreviewer!;
        timeView.FocusComponent(TimeComponent.Hour);
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        var hourComponent = HeadlessControlHost.FindByName<NumericUpDownTimeComponent>(timeView, "PART_Hour")!;
        HeadlessControlHost.KeyDown(hourComponent, Key.Enter);
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        timeView.SelectedComponent.Should().Be(TimeComponent.Minute);
        IsMinuteFocused(timeView).Should().BeTrue();
        IsHourFocused(timeView).Should().BeFalse();
        picker.IsDropDownOpen.Should().BeTrue();
    }

    [AvaloniaFact]
    public void TimePickerEx_EnterOnTextBoxWhileOpen_CommitsWithoutClosing()
    {
        var picker = CreateTimePickerEx();
        picker.SelectedValue = new TimeSpan(9, 0, 0);

        picker.IsDropDownOpen = true;
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        picker.TimePreviewer!.SelectedValue = new TimeSpan(10, 30, 0);
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        var textBox = GetTextBox(picker);
        textBox.Focus();
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        HeadlessControlHost.KeyDown(textBox, Key.Enter);
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        picker.SelectedValue.Should().Be(new TimeSpan(10, 30, 0));
        picker.IsDropDownOpen.Should().BeTrue();
    }

    [AvaloniaFact]
    public void TimePickerEx_F4OnTextBox_TogglesPopupWithoutDoubleToggle()
    {
        var picker = CreateTimePickerEx();
        picker.SelectedValue = new TimeSpan(9, 0, 0);

        var textBox = GetTextBox(picker);
        textBox.Focus();
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        HeadlessControlHost.KeyDown(textBox, Key.F4);
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        picker.IsDropDownOpen.Should().BeTrue();

        HeadlessControlHost.KeyDown(textBox, Key.F4);
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        picker.IsDropDownOpen.Should().BeFalse();
    }

    [AvaloniaFact]
    public void TimePickerEx_EscapeOnPreviewerWhileOpen_RollsBackAndCloses()
    {
        var picker = CreateTimePickerEx();
        var committed = new TimeSpan(9, 0, 0);
        picker.SelectedValue = committed;

        picker.IsDropDownOpen = true;
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        picker.TimePreviewer!.SelectedValue = new TimeSpan(11, 30, 0);
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        picker.TimePreviewer!.Focus();
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        HeadlessControlHost.KeyDown(picker.TimePreviewer!, Key.Escape);
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);

        picker.SelectedValue.Should().Be(committed);
        picker.IsDropDownOpen.Should().BeFalse();
    }

    private sealed class TestableCalendarDatePickerEx : CalendarDatePickerEx
    {
        public Calendar? CalendarPreviewer => Previewer;
    }

    private sealed class TestableDateTimePickerEx : DateTimePickerEx
    {
        public DateTimeView? PreviewerView => Previewer;
    }

    private sealed class TestableTimePickerEx : TimePickerEx
    {
        public TimeView? TimePreviewer => Previewer;
    }

    private static TestableCalendarDatePickerEx CreateCalendarDatePickerEx()
    {
        HeadlessTestApp.EnsureGlobalizationServices();
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

        Application.Current!.TryGetResource(typeof(CalendarDatePickerEx), null, out var themeObj).Should().BeTrue();
        var theme = themeObj as ControlTheme;
        theme.Should().NotBeNull();

        var picker = new TestableCalendarDatePickerEx
        {
            DisplayFormat = "yyyy-MM-dd",
            DisplayDate = new(2026, 6, 15),
            Width = 320,
            Height = 40,
            Theme = theme,
        };

        HeadlessControlHost.Show(picker, new(420, 360));
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Loaded);

        return picker;
    }

    private static TestableDateTimePickerEx CreateDateTimePickerEx()
    {
        HeadlessTestApp.EnsureGlobalizationServices();
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

        Application.Current!.TryGetResource(typeof(DateTimePickerEx), null, out var themeObj).Should().BeTrue();
        var theme = themeObj as ControlTheme;
        theme.Should().NotBeNull();

        var picker = new TestableDateTimePickerEx
        {
            DisplayDate = new(2026, 6, 15),
            Width = 320,
            Height = 40,
            Theme = theme,
        };

        HeadlessControlHost.Show(picker, new(420, 360));
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Loaded);

        return picker;
    }

    private static TestableTimePickerEx CreateTimePickerEx()
    {
        HeadlessTestApp.EnsureGlobalizationServices();
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

        Application.Current!.TryGetResource(typeof(TimePickerEx), null, out var themeObj).Should().BeTrue();
        var theme = themeObj as ControlTheme;
        theme.Should().NotBeNull();

        var picker = new TestableTimePickerEx
        {
            DisplayFormat = @"hh\:mm",
            Width = 320,
            Height = 40,
            Theme = theme,
        };

        HeadlessControlHost.Show(picker, new(420, 360));
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Loaded);

        return picker;
    }

    private static Calendar GetCalendarPreviewer(TestableCalendarDatePickerEx picker)
    {
        picker.CalendarPreviewer.Should().NotBeNull();
        return picker.CalendarPreviewer!;
    }

    private static TextBox GetTextBox(TimePickerEx picker)
    {
        var textBox = HeadlessControlHost.FindByName<TextBox>(picker, TextPicker<TimeSpan?, TimeView>.PartTextBox);
        textBox.Should().NotBeNull();
        return textBox!;
    }

    private static bool IsHourFocused(TimeView timeView)
    {
        var hour = HeadlessControlHost.FindByName<NumericUpDownTimeComponent>(timeView, "PART_Hour");
        return hour is not null && (hour.IsFocused || hour.GetVisualDescendants().OfType<TextBox>().Any(x => x.IsFocused));
    }

    private static bool IsMinuteFocused(TimeView timeView)
    {
        var minute = HeadlessControlHost.FindByName<NumericUpDownTimeComponent>(timeView, "PART_Minute");
        return minute is not null && (minute.IsFocused || minute.GetVisualDescendants().OfType<TextBox>().Any(x => x.IsFocused));
    }
}
