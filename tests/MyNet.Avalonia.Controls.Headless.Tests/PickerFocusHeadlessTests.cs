// -----------------------------------------------------------------------

// <copyright file="PickerFocusHeadlessTests.cs" company="Stéphane ANDRE">

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

using MyNet.Avalonia.Controls.Primitives;


namespace MyNet.Avalonia.Controls.Headless.Tests;



public class PickerFocusHeadlessTests

{

    [AvaloniaFact]

    public void CalendarDatePickerEx_OpenPopup_FocusesDayNotCalendarRoot()

    {

        var picker = CreateCalendarDatePickerEx();

        picker.IsDropDownOpen = true;

        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);



        var calendar = picker.CalendarPreviewer!;

        calendar.IsFocused.Should().BeFalse();

        calendar.GetVisualDescendants().OfType<CalendarDayButton>().Any(x => x.IsFocused).Should().BeTrue();

    }



    [AvaloniaFact]

    public void CalendarDatePickerEx_FocusSelectedDay_DoesNotFocusCalendarRoot()

    {

        var picker = CreateCalendarDatePickerEx();

        picker.IsDropDownOpen = true;

        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);



        var calendar = picker.CalendarPreviewer!;

        calendar.FocusSelectedDay();

        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);



        calendar.IsFocused.Should().BeFalse();

        calendar.GetVisualDescendants().OfType<CalendarDayButton>().Any(x => x.IsFocused).Should().BeTrue();

    }



    [AvaloniaFact]

    public void TimePickerEx_TabFromTextBox_FocusesHour()

    {

        var picker = CreateTimePickerEx();

        picker.IsDropDownOpen = true;

        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);



        var textBox = GetTextBox(picker);

        textBox.Focus();

        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);



        HeadlessControlHost.KeyDown(textBox, Key.Tab);

        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);



        var timeView = picker.TimePreviewer!;

        timeView.SelectedComponent.Should().Be(TimeComponent.Hour);

        IsHourFocused(timeView).Should().BeTrue();

        picker.IsDropDownOpen.Should().BeTrue();

    }



    [AvaloniaFact]

    public void TimePickerEx_TabFromHour_AdvancesToMinuteWithoutReturningToHour()

    {

        var picker = CreateTimePickerEx();

        picker.IsDropDownOpen = true;

        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);



        var timeView = picker.TimePreviewer!;

        timeView.FocusComponent(TimeComponent.Hour);

        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);



        var hourComponent = HeadlessControlHost.FindByName<NumericUpDownTimeComponent>(timeView, "PART_Hour")!;

        HeadlessControlHost.KeyDown(hourComponent, Key.Tab);

        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);



        timeView.SelectedComponent.Should().Be(TimeComponent.Minute);

        IsMinuteFocused(timeView).Should().BeTrue();

        IsHourFocused(timeView).Should().BeFalse();

    }



    [AvaloniaFact]

    public void TimePickerEx_TabFromLastSpinner_ReturnsToTextBoxWithPopupOpen()

    {

        var picker = CreateTimePickerEx();

        picker.IsDropDownOpen = true;

        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);



        var timeView = picker.TimePreviewer!;

        timeView.FocusComponent(TimeComponent.Minute);

        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);



        var minuteComponent = HeadlessControlHost.FindByName<NumericUpDownTimeComponent>(timeView, "PART_Minute")!;

        HeadlessControlHost.KeyDown(minuteComponent, Key.Tab);

        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);



        var textBox = GetTextBox(picker);

        textBox.IsFocused.Should().BeTrue();

        picker.IsDropDownOpen.Should().BeTrue();

    }



    [AvaloniaFact]

    public void DateTimePickerEx_TabFromHour_AdvancesToMinuteOnly()

    {

        var picker = CreateDateTimePickerEx();

        picker.IsDropDownOpen = true;

        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);



        var view = picker.PreviewerView!;

        view.FocusSection(DateTimeViewSection.Time);

        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);



        var timeView = HeadlessControlHost.FindByName<TimeView>(view, DateTimeView.PartTimeView)!;

        var hourComponent = HeadlessControlHost.FindByName<NumericUpDownTimeComponent>(timeView, "PART_Hour")!;

        HeadlessControlHost.KeyDown(hourComponent, Key.Tab);

        Dispatcher.UIThread.RunJobs(DispatcherPriority.Render);



        timeView.SelectedComponent.Should().Be(TimeComponent.Minute);

        IsHourFocused(timeView).Should().BeFalse();

        IsMinuteFocused(timeView).Should().BeTrue();

    }



    private sealed class TestableCalendarDatePickerEx : CalendarDatePickerEx

    {

        public Calendar? CalendarPreviewer => Previewer;

    }



    private sealed class TestableTimePickerEx : TimePickerEx

    {

        public TimeView? TimePreviewer => Previewer;

    }



    private sealed class TestableDateTimePickerEx : DateTimePickerEx

    {

        public DateTimeView? PreviewerView => Previewer;

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

            SelectedValue = new DateTime(2026, 6, 15),

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

            SelectedValue = new TimeSpan(10, 30, 0),

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

            SelectedValue = new DateTime(2026, 6, 15, 10, 30, 0),

            Width = 320,

            Height = 40,

            Theme = theme,

        };



        HeadlessControlHost.Show(picker, new(420, 400));

        Dispatcher.UIThread.RunJobs(DispatcherPriority.Loaded);



        return picker;

    }



    private static TextBox GetTextBox(Control picker)

    {

        var textBox = HeadlessControlHost.FindByName<TextBox>(picker, "PART_TextBox");

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


