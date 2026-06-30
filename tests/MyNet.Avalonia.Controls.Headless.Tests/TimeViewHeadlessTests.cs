// -----------------------------------------------------------------------
// <copyright file="TimeViewHeadlessTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.Styling;
using Avalonia.Threading;
using FluentAssertions;
using MyNet.Avalonia.Controls.Primitives;
using MyNet.Primitives.Temporal;

namespace MyNet.Avalonia.Controls.Headless.Tests;

public class TimeViewHeadlessTests
{
    [AvaloniaFact]
    public void DigitEntry_TwentyFourHour_12_30_ProducesCorrectSelectedValue()
    {
        var timeView = CreateTimeView(TimeFormat.TwentyFourHour);
        var hour = HeadlessControlHost.FindByName<NumericUpDownTimeComponent>(timeView, "PART_Hour");
        hour.Should().NotBeNull();

        timeView.FocusActiveComponent();
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Input);

        HeadlessControlHost.KeyDown(hour!, Key.D1);
        HeadlessControlHost.KeyDown(hour!, Key.D2);
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Input);

        var minute = HeadlessControlHost.FindByName<NumericUpDownTimeComponent>(timeView, "PART_Minute");
        minute.Should().NotBeNull();

        HeadlessControlHost.KeyDown(minute!, Key.D3);
        HeadlessControlHost.KeyDown(minute!, Key.D0);
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Input);

        timeView.Hour.Should().Be(12);
        timeView.Minute.Should().Be(30);
        timeView.SelectedValue.Should().Be(new TimeSpan(12, 30, 0));
    }

    [AvaloniaFact]
    public void HourChange_TwelveHour_PreservesPmWhenAdjustingHour()
    {
        var timeView = CreateTimeView(TimeFormat.TwelveHour);
        timeView.SelectedValue = new TimeSpan(15, 0, 0);
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Loaded);

        timeView.Hour.Should().Be(3);
        timeView.IsAm.Should().BeFalse();

        timeView.Hour = 4;
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Input);

        timeView.IsAm.Should().BeFalse();
        timeView.SelectedValue.Should().Be(new TimeSpan(16, 0, 0));
    }

    [AvaloniaFact]
    public void HourChange_TwentyFourHour_12_DoesNotResetToMidnight()
    {
        var timeView = CreateTimeView(TimeFormat.TwentyFourHour);

        timeView.Hour = 12;
        timeView.Minute = 0;
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Input);

        timeView.Hour.Should().Be(12);
        timeView.SelectedValue.Should().Be(new TimeSpan(12, 0, 0));
    }

    private static TimeView CreateTimeView(TimeFormat format)
    {
        Application.Current!.TryGetResource(typeof(TimeView), null, out var themeObj).Should().BeTrue();
        var theme = themeObj as ControlTheme;
        theme.Should().NotBeNull();

        var timeView = new TimeView
        {
            TimeFormat = format,
            ShowClock = false,
            Width = 320,
            Height = 80,
            Theme = theme,
        };

        HeadlessControlHost.Show(timeView, new(320, 80));
        Dispatcher.UIThread.RunJobs(DispatcherPriority.Loaded);

        return timeView;
    }
}
