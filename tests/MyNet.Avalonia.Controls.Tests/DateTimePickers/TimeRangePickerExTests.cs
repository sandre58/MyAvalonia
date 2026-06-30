// -----------------------------------------------------------------------
// <copyright file="TimeRangePickerExTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using System.Reflection;
using Avalonia.Controls;
using Avalonia.Interactivity;
using FluentAssertions;
using MyNet.Avalonia.Controls.Internals;
using MyNet.Primitives;
using Xunit;

namespace MyNet.Avalonia.Controls.Tests.DateTimePickers;

public class TimeRangePickerExTests
{
    [Fact]
    public void SelectedValue_SyncsStartTimeAndEndTime()
    {
        var picker = CreatePicker();
        var start = new TimeSpan(9, 0, 0);
        var end = new TimeSpan(17, 30, 0);

        picker.SelectedValue = TimeRangeHelper.BuildPeriod(start, end, DateTime.Today).Period;

        picker.StartTime.Should().Be(start);
        picker.EndTime.Should().Be(end);
    }

    [Fact]
    public void StartTimeAndEndTime_UpdateSelectedValue()
    {
        var picker = CreatePicker();

        picker.StartTime = new TimeSpan(8, 0, 0);
        picker.EndTime = new TimeSpan(12, 0, 0);

        picker.SelectedValue.Should().NotBeNull();
        picker.StartTime.Should().Be(new TimeSpan(8, 0, 0));
        picker.EndTime.Should().Be(new TimeSpan(12, 0, 0));
    }

    [Fact]
    public void StartTimeAfterEndTime_NormalizesSelectedValue()
    {
        var picker = CreatePicker();

        picker.StartTime = new TimeSpan(18, 0, 0);
        picker.EndTime = new TimeSpan(10, 0, 0);

        picker.StartTime.Should().Be(new TimeSpan(10, 0, 0));
        picker.EndTime.Should().Be(new TimeSpan(18, 0, 0));
    }

    [Fact]
    public void SelectedValue_UpdatesTextWithRangeSeparator()
    {
        var picker = CreatePicker();
        picker.DisplayFormat = @"HH\:mm";
        picker.RangeSeparator = " -> ";
        picker.SelectedValue = TimeRangeHelper.BuildPeriod(new TimeSpan(9, 0, 0), new TimeSpan(17, 0, 0), DateTime.Today).Period;

        picker.Text.Should().Be("09:00 -> 17:00");
    }

    [Fact]
    public void AllowOvernight_PreservesOvernightRange()
    {
        var picker = CreatePicker();
        picker.AllowOvernight = true;
        picker.SelectedValue = TimeRangeHelper.BuildPeriod(
            new TimeSpan(22, 0, 0),
            new TimeSpan(2, 0, 0),
            DateTime.Today,
            allowOvernight: true,
            TimeRangeInvalidBehavior.Swap).Period;

        picker.StartTime.Should().Be(new TimeSpan(22, 0, 0));
        picker.EndTime.Should().Be(new TimeSpan(2, 0, 0));
        TimeRangeHelper.SpansOvernight(picker.SelectedValue!).Should().BeTrue();
    }

    [Fact]
    public void ShowOvernightIndicator_AppendsSuffix()
    {
        var picker = CreatePicker();
        picker.AllowOvernight = true;
        picker.ShowOvernightIndicator = true;
        picker.DisplayFormat = @"hh\:mm";
        picker.SelectedValue = TimeRangeHelper.BuildPeriod(
            new TimeSpan(22, 0, 0),
            new TimeSpan(2, 0, 0),
            DateTime.Today,
            allowOvernight: true,
            TimeRangeInvalidBehavior.Swap).Period;

        picker.Text.Should().Contain("(+1)");
    }

    [Fact]
    public void InvalidRangeBehavior_ReportError_DoesNotCommitInvalidRange()
    {
        var picker = CreatePicker();
        picker.InvalidRangeBehavior = TimeRangeInvalidBehavior.ReportError;

        picker.StartTime = new TimeSpan(17, 0, 0);
        picker.EndTime = new TimeSpan(9, 0, 0);

        picker.SelectedValue.Should().BeNull();
        picker.StartTime.Should().Be(new TimeSpan(17, 0, 0));
        picker.EndTime.Should().Be(new TimeSpan(9, 0, 0));
        picker.GetValue(DataValidationErrors.ErrorsProperty).Should().NotBeNull();
    }

    [Fact]
    public void IncrementValue_ShiftsEntirePeriod()
    {
        var picker = CreatePicker();
        var start = new TimeSpan(9, 0, 0);
        var end = new TimeSpan(10, 0, 0);
        picker.SelectedValue = TimeRangeHelper.BuildPeriod(start, end, DateTime.Today).Period;

        picker.Increment(30);

        picker.StartTime.Should().Be(start.Add(TimeSpan.FromMinutes(30)));
        picker.EndTime.Should().Be(end.Add(TimeSpan.FromMinutes(30)));
    }

    [Fact]
    public void TryUpdateSelectedValue_KeepsActiveBoundaryOnEnd()
    {
        var view = new TimeRangeView();
        view.StartTime = new TimeSpan(9, 0, 0);
        view.EndTime = new TimeSpan(17, 0, 0);
        view.SwitchBoundary(TimeRangeBoundary.End);

        view.EndTime = new TimeSpan(18, 0, 0);

        view.ActiveBoundary.Should().Be(TimeRangeBoundary.End);
    }

    [Fact]
    public void SwitchToEnd_SeedsEndTimeAndKeepsEndBoundary()
    {
        var view = new TimeRangeView();
        view.StartTime = new TimeSpan(9, 0, 0);

        view.SwitchToEnd(autoAdvance: true);

        view.ActiveBoundary.Should().Be(TimeRangeBoundary.End);
        view.EndTime.Should().Be(new TimeSpan(10, 0, 0));
    }

    [Fact]
    public void SwitchToEnd_DoesNotSetSelectedValueDuringFlyoutEdit()
    {
        var view = new TimeRangeView();
        view.StartTime = new TimeSpan(9, 0, 0);

        view.SwitchToEnd(autoAdvance: true);

        view.ActiveBoundary.Should().Be(TimeRangeBoundary.End);
        view.EndTime.Should().Be(new TimeSpan(10, 0, 0));
        view.SelectedValue.Should().BeNull();
    }

    [Fact]
    public void CompleteRequested_IsRaisedWhenEndBoundaryCompletesWithStartDefined()
    {
        var view = new TimeRangeView();
        var raised = false;
        view.CompleteRequested += (_, _) => raised = true;
        view.StartTime = new TimeSpan(9, 0, 0);
        view.SwitchBoundary(TimeRangeBoundary.End);
        view.EndTime = new TimeSpan(17, 0, 0);

        view.GetType().GetMethod("OnTimeViewInputCompleted", BindingFlags.NonPublic | BindingFlags.Instance)!
            .Invoke(view, [null, new RoutedEventArgs()]);

        raised.Should().BeTrue();
    }

    [Fact]
    public void InputCompleted_OnEndWithoutStart_ReturnsToStartBoundary()
    {
        var view = new TimeRangeView();
        view.SwitchBoundary(TimeRangeBoundary.End);
        view.EndTime = new TimeSpan(17, 0, 0);

        view.GetType().GetMethod("OnTimeViewInputCompleted", BindingFlags.NonPublic | BindingFlags.Instance)!
            .Invoke(view, [null, new RoutedEventArgs()]);

        view.ActiveBoundary.Should().Be(TimeRangeBoundary.Start);
    }

    private static TimeRangePickerEx CreatePicker()
    {
        var picker = new TimeRangePickerEx { DisplayFormat = @"hh\:mm" };
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        return picker;
    }
}
