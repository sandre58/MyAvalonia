// -----------------------------------------------------------------------
// <copyright file="TimeSelectorBaseTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using FluentAssertions;
using MyNet.Primitives.Temporal;
using Xunit;
using TimeViewControl = MyNet.Avalonia.Controls.TimeView;

namespace MyNet.Avalonia.Controls.Tests.TimeView;

public class TimeSelectorBaseTests
{
    [Fact]
    public void HourChange_TwentyFourHour_12_DoesNotResetToMidnight()
    {
        var timeView = new TimeViewControl
        {
            TimeFormat = TimeFormat.TwentyFourHour,
        };

        timeView.Hour = 12;
        timeView.Minute = 0;

        timeView.Hour.Should().Be(12);
        timeView.SelectedValue.Should().Be(new TimeSpan(12, 0, 0));
    }

    [Fact]
    public void HourChange_TwentyFourHour_12_30_ProducesCorrectSelectedValue()
    {
        var timeView = new TimeViewControl
        {
            TimeFormat = TimeFormat.TwentyFourHour,
        };

        timeView.Hour = 12;
        timeView.Minute = 30;

        timeView.SelectedValue.Should().Be(new TimeSpan(12, 30, 0));
    }

    [Fact]
    public void HourChange_TwelveHour_PreservesPmWhenAdjustingHour()
    {
        var timeView = new TimeViewControl
        {
            TimeFormat = TimeFormat.TwelveHour,
            SelectedValue = new TimeSpan(15, 0, 0),
        };

        timeView.Hour.Should().Be(3);
        timeView.IsAm.Should().BeFalse();

        timeView.Hour = 4;

        timeView.IsAm.Should().BeFalse();
        timeView.SelectedValue.Should().Be(new TimeSpan(16, 0, 0));
    }

    [Fact]
    public void HourChange_TwelveHour_12Pm_ProducesNoon()
    {
        var timeView = new TimeViewControl
        {
            TimeFormat = TimeFormat.TwelveHour,
            IsAm = false,
        };

        timeView.Hour = 12;
        timeView.Minute = 0;

        timeView.SelectedValue.Should().Be(new TimeSpan(12, 0, 0));
    }
}
